﻿using System;
using SpiceSharp.Attributes;
using SpiceSharp.Behaviors;
using SpiceSharp.Components.Semiconductors;
using SpiceSharp.Simulations;
using SpiceSharp.Algebra;

namespace SpiceSharp.Components.DiodeBehaviors
{
    /// <summary>
    /// DC biasing behavior for a <see cref="Diode" />.
    /// </summary>
    public class BiasingBehavior : TemperatureBehavior, IBiasingBehavior, IConvergenceBehavior
    {
        private readonly IIterationSimulationState _iteration;

        /// <summary>
        /// The variables used by the behavior.
        /// </summary>
        protected readonly DiodeVariables<double> Variables;
        
        /// <summary>
        /// Gets the matrix elements.
        /// </summary>
        /// <value>
        /// The matrix elements.
        /// </value>
        protected ElementSet<double> Elements { get; private set; }

        /// <summary>
        /// Gets the voltage.
        /// </summary>
        [ParameterName("v"), ParameterName("vd"), ParameterInfo("Diode voltage")]
        public double Voltage => LocalVoltage * Parameters.SeriesMultiplier;

        /// <summary>
        /// Gets the current.
        /// </summary>
        [ParameterName("i"), ParameterName("id"), ParameterInfo("Diode current")]
        public double Current => LocalCurrent * Parameters.ParallelMultiplier;

        /// <summary>
        /// Gets the small-signal conductance.
        /// </summary>
        [ParameterName("gd"), ParameterInfo("Small-signal conductance")]
        public double Conductance => LocalConductance * Parameters.ParallelMultiplier;

        /// <summary>
        /// Gets the power dissipated.
        /// </summary>
        [ParameterName("p"), ParameterName("pd"), ParameterInfo("Power")]
        public double Power => Current * Voltage;

        /// <summary>
        /// The voltage across a single diode (not including parallel or series multipliers).
        /// </summary>
        protected double LocalVoltage;

        /// <summary>
        /// The current through a single diode (not including parallel or series multipliers).
        /// </summary>
        protected double LocalCurrent;

        /// <summary>
        /// The conductance through a single diode (not including paralle or series multipliers).
        /// </summary>
        protected double LocalConductance;

        /// <summary>
        /// Initializes a new instance of the <see cref="BiasingBehavior"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="context">The context.</param>
        public BiasingBehavior(string name, IComponentBindingContext context) : base(name, context) 
        {
            context.Nodes.CheckNodes(2);

            var state = context.GetState<IBiasingSimulationState>();
            _iteration = context.GetState<IIterationSimulationState>();

            Variables = new DiodeVariables<double>(name, state, context);
            Elements = new ElementSet<double>(state.Solver,
                Variables.GetMatrixLocations(state.Map),
                Variables.GetRhsIndicies(state.Map));
        }

        /// <summary>
        /// Loads the Y-matrix and Rhs-vector.
        /// </summary>
        protected virtual void Load()
        {
            double cd, gd;

            // Get the current voltage across (one diode).
            Initialize(out double vd, out bool check);

            /* 
             * this routine loads diodes for dc and transient analyses.
             */
            var csat = TempSaturationCurrent * Parameters.Area;
            var gspr = ModelTemperature.Conductance * Parameters.Area;

            // compute dc current and derivatives
            if (vd >= -3 * Vte)
            {
                // Forward bias
                var evd = Math.Exp(vd / Vte);
                cd = csat * (evd - 1) + BiasingParameters.Gmin * vd;
                gd = csat * evd / Vte + BiasingParameters.Gmin;
            }
            else if (!ModelParameters.BreakdownVoltage.Given || vd >= -TempBreakdownVoltage)
            {
                // Reverse bias
                var arg = 3 * Vte / (vd * Math.E);
                arg = arg * arg * arg;
                cd = -csat * (1 + arg) + BiasingParameters.Gmin * vd;
                gd = csat * 3 * arg / vd + BiasingParameters.Gmin;
            }
            else
            {
                // Reverse breakdown
                var evrev = Math.Exp(-(TempBreakdownVoltage + vd) / Vte);
                cd = -csat * evrev + BiasingParameters.Gmin * vd;
                gd = csat * evrev / Vte + BiasingParameters.Gmin;
            }

            // Check convergence
            if (_iteration.Mode != IterationModes.Fix || !Parameters.Off)
            {
                if (check)
                    _iteration.IsConvergent = false;
            }

            // Store for next time
            LocalVoltage = vd;
            LocalCurrent = cd;
            LocalConductance = gd;

            var m = Parameters.ParallelMultiplier;
            var n = Parameters.SeriesMultiplier;

            var cdeq = cd - gd * vd;
            gd *= m / n;
            gspr *= m / n;
            cdeq *= m;
            Elements.Add(
                // Y-matrix
                gspr, gd, gd + gspr, -gd, -gd, -gspr, -gspr,
                // RHS vector
                cdeq, -cdeq);
        }

        /// <summary>
        /// Loads the Y-matrix and Rhs-vector.
        /// </summary>
        void IBiasingBehavior.Load() => Load();

        /// <summary>
        /// Initialize the device based on the current iteration state.
        /// </summary>
        protected void Initialize(out double vd, out bool check)
        {
            check = false;
            if (_iteration.Mode == IterationModes.Junction)
            {
                vd = Parameters.Off ? 0.0 : TempVCritical;
            }
            else if (_iteration.Mode == IterationModes.Fix && Parameters.Off)
            {
                vd = 0.0;
            }
            else
            {
                // Get voltage over the diodes (without series resistance).
                vd = (Variables.PosPrime.Value - Variables.Negative.Value) / Parameters.SeriesMultiplier;

                // Limit new junction voltage.
                if (!double.IsNaN(ModelParameters.BreakdownVoltage) && vd < Math.Min(0, -TempBreakdownVoltage + 10 * Vte))
                {
                    var vdtemp = -(vd + TempBreakdownVoltage);
                    vdtemp = Semiconductor.LimitJunction(vdtemp, -(LocalVoltage + TempBreakdownVoltage), Vte, TempVCritical, ref check);
                    vd = -(vdtemp + TempBreakdownVoltage);
                }
                else
                {
                    vd = Semiconductor.LimitJunction(vd, LocalVoltage, Vte, TempVCritical, ref check);
                }
            }
        }

        /// <summary>
        /// Check convergence for the diode
        /// </summary>
        /// <returns></returns>
        bool IConvergenceBehavior.IsConvergent()
        {
            var vd = (Variables.PosPrime.Value - Variables.Negative.Value) / Parameters.SeriesMultiplier;

            var delvd = vd - LocalVoltage;
            var cdhat = LocalCurrent + LocalConductance * delvd;
            var cd = LocalCurrent;

            // check convergence
            var tol = BiasingParameters.RelativeTolerance * Math.Max(Math.Abs(cdhat), Math.Abs(cd)) + BiasingParameters.AbsoluteTolerance;
            if (Math.Abs(cdhat - cd) > tol)
            {
                _iteration.IsConvergent = false;
                return false;
            }
            return true;
        }
    }
}
