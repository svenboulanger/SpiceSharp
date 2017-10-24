﻿using System;
using System.Collections.Generic;
using SpiceSharp.Circuits;
using SpiceSharp.Diagnostics;
using System.Numerics;
using SpiceSharp.Parameters;
using SpiceSharp.Behaviours;

namespace SpiceSharp.Simulations
{
    /// <summary>
    /// Frequency-domain analysis (AC analysis)
    /// </summary>
    public class AC : Simulation<AC>
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

        private List<CircuitObjectBehaviourLoad> loadbehaviours;
        private List<CircuitObjectBehaviourAcLoad> acloadbehaviours;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The name of the simulation</param>
        /// <param name="config">The configuration</param>
        public AC(string name) : base(name)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The name of the simulation</param>
        /// <param name="type">The simulation type: lin, oct or dec</param>
        /// <param name="n">The number of steps</param>
        /// <param name="start">The starting frequency</param>
        /// <param name="stop">The stopping frequency</param>
        public AC(string name, string type, int n, double start, double stop) : base(name)
        {
            switch (type.ToLower())
            {
                case "dec": StepType = StepTypes.Decade; break;
                case "oct": StepType = StepTypes.Octave; break;
                case "lin": StepType = StepTypes.Linear; break;
            }
            NumberSteps = n;
            StartFreq = start;
            StopFreq = stop;
        }

        /// <summary>
        /// Initialize the AC analysis
        /// </summary>
        /// <param name="ckt"></param>
        public override void Initialize(Circuit ckt)
        {
            base.Initialize(ckt);

            // Get the behaviours necessary for the AC analysis
            loadbehaviours = Behaviours.Behaviours.CreateBehaviours<CircuitObjectBehaviourLoad>(ckt);
            acloadbehaviours = Behaviours.Behaviours.CreateBehaviours<CircuitObjectBehaviourAcLoad>(ckt);
        }

        /// <summary>
        /// Execute
        /// </summary>
        protected override void Execute()
        {
            var ckt = Circuit;

            var state = ckt.State;
            var cstate = state.Complex;
            var config = CurrentConfig;

            double freq = 0.0, freqdelta = 0.0;
            int n = 0;

            // Calculate the step
            switch (StepType)
            {
                case StepTypes.Decade:
                    freqdelta = Math.Exp(Math.Log(10.0) / NumberSteps);
                    n = (int)Math.Floor(Math.Log(StopFreq / StartFreq) / Math.Log(freqdelta) + 0.25) + 1;
                    break;

                case StepTypes.Octave:
                    freqdelta = Math.Exp(Math.Log(2.0) / NumberSteps);
                    n = (int)Math.Floor(Math.Log(StopFreq / StartFreq) / Math.Log(freqdelta) + 0.25) + 1;
                    break;

                case StepTypes.Linear:
                    if (NumberSteps > 1)
                    {
                        freqdelta = (StopFreq - StartFreq) / (NumberSteps - 1);
                        n = NumberSteps;
                    }
                    else
                    {
                        freqdelta = double.PositiveInfinity;
                        n = 1;
                    }
                    break;

                default:
                    throw new CircuitException("Invalid step type");
            }

            // Calculate the operating point
            state.Initialize(ckt);
            state.Complex.Laplace = 0.0;
            state.Domain = CircuitState.DomainTypes.Frequency;
            state.UseIC = false;
            state.UseDC = true;
            state.UseSmallSignal = false;
            state.Gmin = config.Gmin;
            Initialize(ckt);
            ckt.Op(loadbehaviours, config, config.DcMaxIterations);

            // Load all in order to calculate the AC info for all devices
            state.UseDC = false;
            state.UseSmallSignal = true;
            foreach (var behaviour in loadbehaviours)
                behaviour.Execute(ckt);

            // Export operating point if requested
            if (config.KeepOpInfo)
                Export(ckt);

            // Calculate the AC solution
            state.UseDC = false;
            freq = StartFreq;

            // Sweep the frequency
            for (int i = 0; i < n; i++)
            {
                // Calculate the current frequency
                state.Complex.Laplace = new Complex(0.0, 2.0 * Circuit.CONSTPI * freq);

                // Solve
                ckt.AcIterate(acloadbehaviours, config);

                // Export the timepoint
                Export(ckt);

                // Increment the frequency
                switch (StepType)
                {
                    case StepTypes.Decade:
                    case StepTypes.Octave:
                        freq = freq * freqdelta;
                        break;

                    case StepTypes.Linear:
                        freq = StartFreq + i * freqdelta;
                        break;
                }
            }

            // Finalize the export
            Finalize(ckt);
        }
    }
}