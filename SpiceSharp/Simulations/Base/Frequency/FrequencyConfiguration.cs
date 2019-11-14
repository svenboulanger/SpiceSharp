﻿using SpiceSharp.Algebra;
using SpiceSharp.Attributes;
using System.Numerics;

namespace SpiceSharp.Simulations
{
    /// <summary>
    /// A configuration for a <see cref="FrequencySimulation" />.
    /// </summary>
    /// <seealso cref="SpiceSharp.ParameterSet" />
    public class FrequencyConfiguration : ParameterSet
    {
        /// <summary>
        /// Gets or sets a value indicating whether the operation point should be exported.
        /// </summary>
        public bool KeepOpInfo { get; set; } = false;

        /// <summary>
        /// Gets or sets the absolute threshold for choosing pivots.
        /// </summary>
        public double AbsolutePivotThreshold { get; set; }

        /// <summary>
        /// Gets or sets the relative threshold for choosing pivots.
        /// </summary>
        public double RelativePivotThreshold { get; set; } = 1e-3;

        /// <summary>
        /// Gets or sets the frequency sweep.
        /// </summary>
        public Sweep<double> FrequencySweep { get; set; }

        /// <summary>
        /// Gets or sets the solver used to solve equations. If <c>null</c>, a default solver will be used.
        /// </summary>
        /// <value>
        /// The solver.
        /// </value>
        [ParameterName("complex.solver"), ParameterInfo("The solver used to solve equations.")]
        public ISparseSolver<Complex> Solver { get; set; }

        /// <summary>
        /// Gets or sets the mapper used to map <see cref="Variable"/> to equation indices. If <c>null</c>,
        /// a default mapper will be used.
        /// </summary>
        /// <value>
        /// The map.
        /// </value>
        [ParameterName("complex.map"), ParameterInfo("The mapper used to map variables to node indices.")]
        public IVariableMap Map { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrequencyConfiguration"/> class.
        /// </summary>
        public FrequencyConfiguration()
        {
            // Default frequency-sweep
            FrequencySweep = new DecadeSweep(1, 100, 10);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrequencyConfiguration"/> class.
        /// </summary>
        /// <param name="frequencySweep">The frequency sweep.</param>
        public FrequencyConfiguration(Sweep<double> frequencySweep)
        {
            FrequencySweep = frequencySweep;
        }
    }
}
