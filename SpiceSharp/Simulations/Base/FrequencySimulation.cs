﻿using System;
using System.Numerics;
using System.Collections.Generic;
using SpiceSharp.Circuits;
using SpiceSharp.Attributes;
using SpiceSharp.Diagnostics;
using SpiceSharp.Sparse;
using SpiceSharp.Behaviors;

namespace SpiceSharp.Simulations
{
    /// <summary>
    /// A base class for frequency-dependent analysis
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class FrequencySimulation : BaseSimulation
    {
        /// <summary>
        /// Enumerations
        /// </summary>
        public enum StepTypes { Decade, Octave, Linear };

        /// <summary>
        /// Gets or sets the number of steps
        /// </summary>
        [SpiceName("steps"), SpiceName("n"), SpiceInfo("The number of steps")]
        public double Steps
        {
            get => NumberSteps;
            set => NumberSteps = (int)(Math.Round(value) + 0.1);
        }
        public int NumberSteps { get; set; } = 10;

        /// <summary>
        /// Gets or sets the starting frequency
        /// </summary>
        [SpiceName("start"), SpiceInfo("Starting frequency")]
        public double StartFreq { get; set; } = 1.0;

        /// <summary>
        /// Gets or sets the stopping frequency
        /// </summary>
        [SpiceName("stop"), SpiceInfo("Stopping frequency")]
        public double StopFreq { get; set; } = 1.0e3;

        /// <summary>
        /// Gets or sets the step type (string version)
        /// </summary>
        [SpiceName("type"), SpiceInfo("The step type")]
        public string _StepType
        {
            get
            {
                switch (StepType)
                {
                    case StepTypes.Linear: return "lin";
                    case StepTypes.Octave: return "oct";
                    case StepTypes.Decade: return "dec";
                }
                return null;
            }
            set
            {
                switch (value.ToLower())
                {
                    case "linear":
                    case "lin": StepType = StepTypes.Linear; break;
                    case "octave":
                    case "oct": StepType = StepTypes.Octave; break;
                    case "decade":
                    case "dec": StepType = StepTypes.Decade; break;
                    default:
                        throw new CircuitException($"Invalid step type {value}");
                }
            }
        }

        /// <summary>
        /// Gets or sets the type of step used
        /// </summary>
        public StepTypes StepType { get; set; } = StepTypes.Decade;

        /// <summary>
        /// Private variables
        /// </summary>
        protected List<AcBehavior> acbehaviors = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name</param>
        public FrequencySimulation(Identifier name) : base(name)
        {
        }

        /// <summary>
        /// Setup the simulation
        /// </summary>
        protected override void Setup()
        {
            base.Setup();

            // Get behaviors
            acbehaviors = SetupBehaviors<AcBehavior>();

            // Setup AC behaviors
            var matrix = State.Matrix;
            foreach (var behavior in acbehaviors)
                behavior.GetMatrixPointers(matrix);
        }

        /// <summary>
        /// Unsetup the simulation
        /// </summary>
        protected override void Unsetup()
        {
            // Remove references
            foreach (var behavior in acbehaviors)
                behavior.Unsetup();
            acbehaviors.Clear();
            acbehaviors = null;

            base.Unsetup();
        }

        /// <summary>
        /// Calculate the AC solution
        /// </summary>
        /// <param name="ckt">Circuit</param>
        protected void AcIterate(Circuit ckt)
        {
            var state = State;
            var matrix = state.Matrix;
            matrix.Complex = true;

            // Initialize the circuit
            if (!state.Initialized)
                state.Initialize(ckt);

            retry:
            state.IsCon = true;

            // Load AC
            state.Clear();
            foreach (var behavior in acbehaviors)
                behavior.Load(this);

            if (state.Sparse.HasFlag(State.SparseFlags.NIACSHOULDREORDER))
            {
                var error = matrix.Reorder(state.PivotAbsTol, state.PivotRelTol);
                state.Sparse &= ~State.SparseFlags.NIACSHOULDREORDER;
                if (error != SparseError.Okay)
                    throw new CircuitException("Sparse matrix exception: " + SparseUtilities.ErrorMessage(state.Matrix, "AC"));
            }
            else
            {
                var error = matrix.Factor();
                if (error != 0)
                {
                    if (error == SparseError.Singular)
                    {
                        state.Sparse |= State.SparseFlags.NIACSHOULDREORDER;
                        goto retry;
                    }
                    throw new CircuitException("Sparse matrix exception: " + SparseUtilities.ErrorMessage(state.Matrix, "AC"));
                }
            }

            // Solve
            matrix.Solve(state.Rhs, state.iRhs);

            // Reset values
            state.Rhs[0] = 0.0;
            state.iRhs[0] = 0.0;
            state.Solution[0] = 0.0;
            state.iSolution[0] = 0.0;

            // Store them in the solution
            state.StoreSolution(true);
        }

        /// <summary>
        /// Create an export method for this type of simulation
        /// The simulation will determine which export method is returned if multiple behaviors implement a export method by the same name
        /// </summary>
        /// <param name="name">The identifier of the entity</param>
        /// <param name="property">The parameter name</param>
        /// <returns></returns>
        public override Func<State, double> CreateExport(Identifier name, string property)
        {
            var eb = pool.GetEntityBehaviors(name) ?? throw new CircuitException($"{Name}: Could not find behaviors of {name}");

            // Most logical place to look for frequency analysis: AC behaviors
            var export = eb.Get<AcBehavior>()?.CreateExport(property);

            // Next most logical place is the LoadBehavior
            if (export == null)
                export = eb.Get<LoadBehavior>()?.CreateExport(property);
            return export;
        }

        /// <summary>
        /// Create an export method
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="property">Property</param>
        /// <returns></returns>
        public Func<State, Complex> CreateAcExport(Identifier name, string property)
        {
            var eb = pool.GetEntityBehaviors(name) ?? throw new CircuitException($"{Name}: Could not find behaviors of {name}");

            // Only AC behaviors implement these export methods
            return eb.Get<AcBehavior>()?.CreateAcExport(property);
        }

        /// <summary>
        /// Create an export method for this type of simulation 
        /// </summary>
        /// <param name="pos">Positive voltage</param>
        /// <param name="neg">Negative voltage</param>
        /// <returns></returns>
        public virtual Func<State, Complex> CreateAcVoltageExport(Identifier pos, Identifier neg = null)
        {
            int node = Circuit.Nodes[pos].Index;
            if (neg == null)
                return (State state) => new Complex(state.Solution[node], state.iSolution[node]);
            int refnode = Circuit.Nodes[neg].Index;
            return (State state) => new Complex(state.Solution[node] - state.Solution[refnode], state.iSolution[node] - state.iSolution[refnode]);
        }
    }
}