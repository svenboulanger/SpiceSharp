﻿using SpiceSharp.Attributes;
using SpiceSharp.Circuits;

namespace SpiceSharp.Simulations
{
    /// <summary>
    /// Configuration for a <see cref="Noise"/> analysis
    /// </summary>
    public class NoiseConfiguration : Parameters
    {
        /// <summary>
        /// Gets or sets the noise output node
        /// </summary>
        [SpiceName("output"), SpiceInfo("Noise output summation node")]
        public Identifier Output { get; set; } = null;

        /// <summary>
        /// Gets or sets the noise output reference node
        /// </summary>
        [SpiceName("outputref"), SpiceInfo("Noise output reference node")]
        public Identifier OutputRef { get; set; } = null;

        /// <summary>
        /// Gets or sets the name of the AC source used as input reference
        /// </summary>
        [SpiceName("input"), SpiceInfo("Name of the AC source used as input reference")]
        public Identifier Input { get; set; } = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public NoiseConfiguration()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="output">Output</param>
        /// <param name="reference">Output reference</param>
        /// <param name="input">Input</param>
        public NoiseConfiguration(Identifier output, Identifier reference, Identifier input)
        {
            Output = output;
            OutputRef = reference;
            Input = input;
        }
    }
}