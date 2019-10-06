﻿using SpiceSharp.Simulations;

namespace SpiceSharp.Entities.ParallelLoaderBehaviors
{
    /// <summary>
    /// Noise preparer for a <see cref="ParallelLoader"/>
    /// </summary>
    /// <seealso cref="IParallelPreparer" />
    public class NoisePreparer : IParallelPreparer
    {
        /// <summary>
        /// Prepares the specified simulation for parallel loading.
        /// </summary>
        /// <param name="simulation">The simulation.</param>
        public void Prepare(ISimulation simulation)
        {
            var psim = (ParallelSimulation)simulation;
            psim.States.Add(psim.Parent.States.GetValue<INoiseSimulationState>());
        }

        /// <summary>
        /// Restores the specified simulation from parallel loading.
        /// </summary>
        /// <param name="simulation">The simulation.</param>
        public void Restore(ISimulation simulation)
        {
        }
    }
}