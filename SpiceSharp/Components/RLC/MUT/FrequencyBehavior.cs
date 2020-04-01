﻿using SpiceSharp.Behaviors;
using SpiceSharp.Simulations;
using System.Numerics;
using SpiceSharp.Algebra;

namespace SpiceSharp.Components.MutualInductanceBehaviors
{
    /// <summary>
    /// AC behavior for <see cref="MutualInductance"/>
    /// </summary>
    public class FrequencyBehavior : TemperatureBehavior, IFrequencyBehavior
    {
        private readonly IVariable<Complex> _branch1, _branch2;
        private readonly ElementSet<Complex> _elements;
        private readonly IComplexSimulationState _complex;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="FrequencyBehavior"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="context">The context.</param>
        public FrequencyBehavior(string name, MutualInductanceBindingContext context) : base(name, context) 
        {
            _complex = context.GetState<IComplexSimulationState>();
            _branch1 = context.Inductor1Behaviors.GetValue<IBranchedBehavior<Complex>>().Branch;
            _branch2 = context.Inductor2Behaviors.GetValue<IBranchedBehavior<Complex>>().Branch;

            var br1 = _complex.Map[_branch1];
            var br2 = _complex.Map[_branch2];
            _elements = new ElementSet<Complex>(_complex.Solver,
                new MatrixLocation(br1, br2),
                new MatrixLocation(br2, br1));
        }

        /// <summary>
        /// Initializes the parameters.
        /// </summary>
        void IFrequencyBehavior.InitializeParameters()
        {
        }

        /// <summary>
        /// Execute behavior for AC analysis
        /// </summary>
        void IFrequencyBehavior.Load()
        {
            var value = _complex.Laplace * Factor;
            _elements.Add(-value, -value);
        }
    }
}
