﻿using System;
using SpiceSharp.Behaviors;
using SpiceSharp.Simulations;

namespace SpiceSharp.Components.NoiseSources
{
    /// <summary>
    /// A class that represents a noise generator
    /// </summary>
    public abstract class NoiseGenerator
    {
        /// <summary>
        /// Gets the name of the noise generator
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the calculated noise density
        /// </summary>
        public double Noise { get; private set; }

        /// <summary>
        /// Gets the log of the calculated noise density
        /// </summary>
        public double LogNoise { get; private set; }

        /// <summary>
        /// Integrated output noise
        /// </summary>
        public double TotalOutputNoise { get; private set; }

        /// <summary>
        /// Integrated input noise
        /// </summary>
        public double TotalInputNoise { get; private set; }

        /// <summary>
        /// Gets the nodes this noise generator is connected to
        /// </summary>
        public NodeCollection Nodes { get; private set; }

        /// <summary>
        /// Private variables
        /// </summary>
        private readonly int[] _pins;


        /// <summary>
        /// Gets the biasing simulation state.
        /// </summary>
        /// <value>
        /// The biasing simulation state.
        /// </value>
        protected BiasingSimulationState BiasingState { get; private set; }

        /// <summary>
        /// Gets the noise simulation state.
        /// </summary>
        /// <value>
        /// The noise simulation state.
        /// </value>
        protected NoiseSimulationState NoiseState { get; private set; }

        /// <summary>
        /// Gets the complex simulation state.
        /// </summary>
        /// <value>
        /// The complex simulation state.
        /// </value>
        protected ComplexSimulationState ComplexState { get; private set; }

        /// <summary>
        /// Creates a new instance of the <see cref="NoiseGenerator"/> class.
        /// </summary>
        /// <param name="name">Name of the noise source</param>
        /// <param name="pins">Pins</param>
        protected NoiseGenerator(string name, params int[] pins)
        {
            Name = name;
            _pins = pins;
            Nodes = null;
        }

        /// <summary>
        /// Connect the noise generator in the circuit
        /// </summary>
        /// <param name="context">The binding context.</param>
        /// <param name="nodes">The nodes the generator can connect to.</param>
        public virtual void Bind(BindingContext context, params int[] nodes)
        {
            // Get the nodes
            var mapped = new int[nodes.Length];
            for (var i = 0; i < _pins.Length; i++)
            {
                if (_pins[i] >= nodes.Length)
                    throw new CircuitException("Not enough pins to find node {0}".FormatString(_pins[i]));
                mapped[i] = nodes[_pins[i]];
            }
            Nodes = new NodeCollection(mapped);

            BiasingState = context.States.Get<BiasingSimulationState>();
            ComplexState = context.States.Get<ComplexSimulationState>();
            NoiseState = context.States.Get<NoiseSimulationState>();
        }

        /// <summary>
        /// Unsetup the noise generator.
        /// </summary>
        public virtual void Unsetup()
        {
            Nodes = null;
        }

        /// <summary>
        /// Set the values for evaluating the noise generator
        /// </summary>
        /// <param name="coefficients">Coefficients</param>
        public abstract void SetCoefficients(params double[] coefficients);

        /// <summary>
        /// Evaluates the specified cstate.
        /// </summary>
        public virtual void Evaluate()
        {
            // Calculate the noise
            Noise = CalculateNoise();
            var lnNdens = Math.Log(Math.Max(Noise, 1e-38));

            // Initialize the integrated noise if we just started
            if (NoiseState.DeltaFrequency.Equals(0.0))
            {
                LogNoise = lnNdens;
                TotalOutputNoise = 0.0;
                TotalInputNoise = 0.0;
            }
            else
            {
                // Integrate the output noise
                var tempOnoise = NoiseState.Integrate(Noise, lnNdens, LogNoise);
                var tempInoise = NoiseState.Integrate(Noise * NoiseState.GainInverseSquared, lnNdens + NoiseState.LogInverseGain, LogNoise + NoiseState.LogInverseGain);
                LogNoise = lnNdens;

                // Add integrated quantity
                TotalOutputNoise += tempOnoise;
                TotalInputNoise += tempInoise;
            }
        }

        /// <summary>
        /// Calculates the noise contributions.
        /// </summary>
        /// <returns></returns>
        protected abstract double CalculateNoise();
    }
}
