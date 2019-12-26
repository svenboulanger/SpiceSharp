﻿using SpiceSharp.Algebra;
using SpiceSharp.Behaviors;
using SpiceSharp.Components.Distributed;
using SpiceSharp.Simulations;

namespace SpiceSharp.Components.LosslessTransmissionLineBehaviors
{
    /// <summary>
    /// Transient behavior for a <see cref="LosslessTransmissionLine" />.
    /// </summary>
    public class TimeBehavior : BiasingBehavior, ITimeBehavior
    {
        /// <summary>
        /// Gets the delayed signals.
        /// </summary>
        public DelayedSignal Signals { get; private set; }

        /// <summary>
        /// Gets the transient vector elements.
        /// </summary>
        /// <value>
        /// The transient vector elements.
        /// </value>
        protected ElementSet<double> TransientElements { get; private set; }

        private int _pos1, _neg1, _pos2, _neg2, _br1, _br2;
        private ITimeSimulationState _time;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeBehavior" /> class.
        /// </summary>
        /// <param name="name">The name of the behavior.</param>
        /// <param name="context">The context.</param>
        public TimeBehavior(string name, ComponentBindingContext context)
            : base(name, context)
        {
            _time = context.GetState<ITimeSimulationState>();
            _pos1 = BiasingState.Map[context.Nodes[0]];
            _neg1 = BiasingState.Map[context.Nodes[1]];
            _pos2 = BiasingState.Map[context.Nodes[2]];
            _neg2 = BiasingState.Map[context.Nodes[3]];
            _br1 = BiasingState.Map[Branch1];
            _br2 = BiasingState.Map[Branch2];
            TransientElements = new ElementSet<double>(BiasingState.Solver, null, new[] { _br1, _br2 });
            Signals = new DelayedSignal(2, BaseParameters.Delay);
        }

        /// <summary>
        /// Initialize the states.
        /// </summary>
        void ITimeBehavior.InitializeStates()
        {
            var sol = BiasingState.ThrowIfNotBound(this).Solution;

            // Calculate the inputs
            var input1 = sol[_pos2] - sol[_neg2] + BaseParameters.Impedance * sol[_br2];
            var input2 = sol[_pos1] - sol[_neg1] + BaseParameters.Impedance * sol[_br1];
            Signals.SetProbedValues(input1, input2);
        }

        /// <summary>
        /// Load the Y-matrix and Rhs-vector.
        /// </summary>
        void IBiasingBehavior.Load()
        {
            var y = BaseParameters.Admittance;
            if (_time.UseDc)
            {
                Elements.Add(
                    y, -y, -y, y, 1, 0, -1, -1,
                    y, -y, -y, y, 1, 0, -1, 0,
                    1, -1, 1, 1, 1
                    );
            }
            else
            {
                Elements.Add(
                    y, -y, -y, y, 1, 1, -1, -1,
                    y, -y, -y, y, 1, 1, -1, -1
                    );
            }

            var sol = BiasingState.Solution;

            // Calculate inputs
            var input1 = sol[_pos2] - sol[_neg2] + BaseParameters.Impedance * sol[_br2];
            var input2 = sol[_pos1] - sol[_neg1] + BaseParameters.Impedance * sol[_br1];
            Signals.SetProbedValues(input1, input2);

            // Update the branch equations
            TransientElements.Add(Signals.Values[0], Signals.Values[1]);
        }
    }
}