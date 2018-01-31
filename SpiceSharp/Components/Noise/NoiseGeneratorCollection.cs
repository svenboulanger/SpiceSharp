﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace SpiceSharp.Components.NoiseSources
{
    /// <summary>
    /// Collection of noise generators
    /// </summary>
    public class NoiseGeneratorCollection : IReadOnlyCollection<NoiseGenerator>
    {
        /// <summary>
        /// Generators
        /// </summary>
        List<NoiseGenerator> generators = new List<NoiseGenerator>();

        /// <summary>
        /// Get a noise generator
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns></returns>
        public NoiseGenerator this[int index]
        {
            get
            {
                return generators[index];
            }
        }

        /// <summary>
        /// Get the number of noise generators
        /// </summary>
        public int Count { get => generators.Count; }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="generators">Generators</param>
        public NoiseGeneratorCollection(IEnumerable<NoiseGenerator> generators)
        {
            if (generators == null)
                throw new ArgumentNullException(nameof(generators));

            foreach (var generator in generators)
                this.generators.Add(generator);
        }

        /// <summary>
        /// Get enumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<NoiseGenerator> GetEnumerator() => generators.GetEnumerator();

        /// <summary>
        /// Get enumerator
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator() => generators.GetEnumerator();
    }
}