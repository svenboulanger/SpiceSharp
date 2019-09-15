﻿using SpiceSharp.Algebra;
using System;

namespace SpiceSharp.Simulations
{
    /// <summary>
    /// A simulation state for simulations using real numbers.
    /// </summary>
    /// <seealso cref="SimulationState" />
    public class BiasingSimulationState : SimulationState
    {
        /// <summary>
        /// Gets or sets the initialization flag.
        /// </summary>
        public InitializationModes Init { get; set; }

        /// <summary>
        /// Gets or sets the flag for ignoring time-related effects. If true, each device should assume the circuit is not moving in time.
        /// </summary>
        public bool UseDc { get; set; }

        /// <summary>
        /// Gets or sets the flag for using initial conditions. If true, the operating point will not be calculated, and initial conditions will be used instead.
        /// </summary>
        public bool UseIc { get; set; }

        /// <summary>
        /// The current source factor.
        /// This parameter is changed when doing source stepping for aiding convergence.
        /// </summary>
        /// <remarks>
        /// In source stepping, all sources are considered to be at 0 which has typically only one single solution (all nodes and
        /// currents are 0V and 0A). By increasing the source factor in small steps, it is possible to progressively reach a solution
        /// without having non-convergence.
        /// </remarks>
        public double SourceFactor { get; set; } = 1.0;

        /// <summary>
        /// Gets or sets the a conductance that is shunted with PN junctions to aid convergence.
        /// </summary>
        public double Gmin { get; set; } = 1e-12;

        /// <summary>
        /// Is the current iteration convergent?
        /// This parameter is used to communicate convergence.
        /// </summary>
        public bool IsConvergent { get; set; } = true;

        /// <summary>
        /// The current temperature for this circuit in Kelvin.
        /// </summary>
        public double Temperature { get; set; } = 300.15;

        /// <summary>
        /// The nominal temperature for the circuit in Kelvin.
        /// Used by models as the default temperature where the parameters were measured.
        /// </summary>
        public double NominalTemperature { get; set; } = 300.15;

        /// <summary>
        /// Gets the solution vector.
        /// </summary>
        public IVector<double> Solution { get; protected set; }

        /// <summary>
        /// Gets the previous solution vector.
        /// </summary>
        /// <remarks>
        /// This vector is needed for determining convergence.
        /// </remarks>
        public IVector<double> OldSolution { get; protected set; }

        /// <summary>
        /// Gets the sparse solver.
        /// </summary>
        /// <value>
        /// The solver.
        /// </value>
        public ISolver<double> Solver
        {
            get => _solver;
            set
            {
                _solver = value ?? throw new ArgumentNullException();
            }
        }
        private ISolver<double> _solver;

        /// <summary>
        /// Initializes a new instance of the <see cref="BiasingSimulationState"/> class.
        /// </summary>
        public BiasingSimulationState()
        {
            Solver = new SparseRealSolver<SparseMatrix<double>, SparseVector<double>>(
                new SparseMatrix<double>(),
                new SparseVector<double>());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BiasingSimulationState"/> class.
        /// </summary>
        /// <param name="solver">The solver.</param>
        public BiasingSimulationState(ISolver<double> solver)
        {
            Solver = solver.ThrowIfNull(nameof(solver));
        }

        /// <summary>
        /// Sets up the simulation state.
        /// </summary>
        /// <param name="nodes">The unknown variables for which the state is used.</param>
        public override void Setup(IVariableSet nodes)
        {
            nodes.ThrowIfNull(nameof(nodes));

            // Initialize all matrices
            Solution = new DenseVector<double>(Solver.Size);
            OldSolution = new DenseVector<double>(Solver.Size);
            Solver.Reset();

            // Initialize all states and parameters
            Init = InitializationModes.None;
            UseDc = true;
            UseIc = false;

            base.Setup(nodes);
        }

        /// <summary>
        /// Unsetup the state.
        /// </summary>
        public override void Unsetup()
        {
            Solution = null;
            OldSolution = null;
            base.Unsetup();
        }

        /// <summary>
        /// Stores the solution.
        /// </summary>
        public void StoreSolution()
        {
            var tmp = OldSolution;
            OldSolution = Solution;
            Solution = tmp;
        }
    }
}
