﻿using System;
using System.Collections.Generic;
using System.Numerics;
using SpiceSharp.Behaviors;
using SpiceSharp.Entities;
using SpiceSharp.ParameterSets;

namespace SpiceSharp.Simulations
{
    /// <summary>
    /// A class that implements a noise analysis.
    /// </summary>
    /// <seealso cref="FrequencySimulation" />
    public partial class Noise : FrequencySimulation,
        INoiseSimulation,
        IParameterized<NoiseParameters>
    {
        private NoiseSimulationState _state;
        private BehaviorList<INoiseBehavior> _noiseBehaviors;

        /// <summary>
        /// Gets the noise parameters.
        /// </summary>
        /// <value>
        /// The noise parameters.
        /// </value>
        public NoiseParameters NoiseParameters { get; } = new NoiseParameters();

        NoiseParameters IParameterized<NoiseParameters>.Parameters => NoiseParameters;
        INoiseSimulationState IStateful<INoiseSimulationState>.State => _state;

        /// <summary>
        /// Initializes a new instance of the <see cref="Noise"/> class.
        /// </summary>
        /// <param name="name">The name of the simulation.</param>
        public Noise(string name) 
            : base(name)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Noise"/> class.
        /// </summary>
        /// <param name="name">The name of the simulation.</param>
        /// <param name="output">The output node name.</param>
        /// <param name="frequencySweep">The frequency points.</param>
        public Noise(string name, string output, IEnumerable<double> frequencySweep) 
            : base(name, frequencySweep)
        {
            NoiseParameters.Output = output;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Noise"/> class.
        /// </summary>
        /// <param name="name">The name of the simulation.</param>
        /// <param name="output">The output node name.</param>
        /// <param name="reference">The reference output node name.</param>
        /// <param name="frequencySweep">The frequency points.</param>
        public Noise(string name, string output, string reference, IEnumerable<double> frequencySweep) 
            : base(name, frequencySweep)
        {
            NoiseParameters.Output = output;
            NoiseParameters.OutputRef = reference;
        }

        /// <summary>
        /// Set up the simulation.
        /// </summary>
        /// <param name="entities">The circuit that will be used.</param>
        protected override void Setup(IEntityCollection entities)
        {
            entities.ThrowIfNull(nameof(entities));

            // Get behaviors, parameters and states
            _state = new NoiseSimulationState(Name);
            base.Setup(entities);

            // Cache local variables
            _noiseBehaviors = EntityBehaviors.GetBehaviorList<INoiseBehavior>();
        }

        /// <summary>
        /// Destroys the simulation.
        /// </summary>
        protected override void Unsetup()
        {
            // Remove references
            _noiseBehaviors = null;
            base.Unsetup();
        }

        /// <summary>
        /// Executes the simulation.
        /// </summary>
        protected override void Execute()
        {
            base.Execute();
            var cstate = (ComplexSimulationState)GetState<IComplexSimulationState>();

            var noiseconfig = NoiseParameters;
            var exportargs = new ExportDataEventArgs(this);

            // Find the output nodes
            var posOutNode = noiseconfig.Output != null ? cstate.Map[cstate.GetSharedVariable(noiseconfig.Output)] : 0;
            var negOutNode = noiseconfig.OutputRef != null ? cstate.Map[cstate.GetSharedVariable(noiseconfig.OutputRef)] : 0;

            // Initialize
            var freq = FrequencyParameters.Frequencies.GetEnumerator();
            if (!freq.MoveNext())
                return;
            cstate.Laplace = 0;
            Op(BiasingParameters.DcMaxIterations);

            // Initialize all devices for small-signal analysis and reset all noise contributions
            InitializeAcParameters();
            foreach (var behavior in _noiseBehaviors)
                behavior.Initialize();

            // Loop through noise figures
            do
            {
                // First compute the AC gain
                cstate.Laplace = new Complex(0.0, 2.0 * Math.PI * freq.Current);
                AcIterate();

                var val = cstate.Solution[posOutNode] - cstate.Solution[negOutNode];
                var inverseGainSquared = 1.0 / Math.Max(val.Real * val.Real + val.Imaginary * val.Imaginary, 1e-20);
                _state.SetCurrentPoint(new NoisePoint(freq.Current, inverseGainSquared));

                // Solve the adjoint system
                NzIterate(posOutNode, negOutNode);

                // Now we use the adjoint system to calculate the noise
                // contributions of each generator in the circuit
                foreach (var behavior in _noiseBehaviors)
                {
                    behavior.Compute();
                    _state.Add(behavior);
                }

                // Export the data
                OnExport(exportargs);
            }
            while (freq.MoveNext());
        }

        /// <summary>
        /// Calculate the solution for <see cref="Noise" /> analysis
        /// </summary>
        /// <param name="posDrive">The positive driving node index.</param>
        /// <param name="negDrive">The negative driving node index.</param>
        /// <remarks>
        /// This routine solves the adjoint system. It assumes that the matrix has
        /// already been loaded by a call to AcIterate, so it only alters the right
        /// hand side vector. The unit-valued current excitation is applied between
        /// nodes posDrive and negDrive.
        /// </remarks>
        private void NzIterate(int posDrive, int negDrive)
        {
            var cstate = GetState<IComplexSimulationState>();
            var solver = cstate.Solver;

            // Clear out the right hand side vector
            solver.Precondition((matrix, rhs) => {
                rhs.Reset();
            });

            // Apply unit current excitation
            solver.GetElement(posDrive).Add(1.0);
            solver.GetElement(negDrive).Subtract(1.0);

            solver.SolveTransposed(cstate.Solution);
            cstate.Solution[0] = 0.0;
        }
    }
}
