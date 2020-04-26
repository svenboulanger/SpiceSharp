﻿using SpiceSharp.Behaviors;
using SpiceSharp.Simulations;
using SpiceSharp.Algebra;

namespace SpiceSharp.Components
{
    public partial class VoltageDelay
    {
        /// <summary>
        /// Biasing behavior for a <see cref="VoltageDelay" />.
        /// </summary>
        /// <seealso cref="Behavior"/>
        /// <seealso cref="IBiasingBehavior"/>
        /// <seealso cref="IBranchedBehavior{T}"/>
        /// <seealso cref="IParameterized{P}"/>
        /// <seealso cref="VoltageDelayParameters"/>
        protected class BiasingBehavior : Behavior, 
            IBiasingBehavior, 
            IBranchedBehavior<double>,
            IParameterized<VoltageDelayParameters>
        {
            private readonly int _posNode, _negNode, _contPosNode, _contNegNode, _branchEq;

            /// <inheritdoc/>
            public VoltageDelayParameters Parameters { get; }

            /// <summary>
            /// Gets the branch equation row.
            /// </summary>
            public IVariable<double> Branch { get; }

            /// <summary>
            /// Gets the matrix elements.
            /// </summary>
            /// <value>
            /// The matrix elements.
            /// </value>
            protected ElementSet<double> BiasingElements { get; }

            /// <summary>
            /// Initializes a new instance of the <see cref="BiasingBehavior"/> class.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="context">The context.</param>
            public BiasingBehavior(string name, ComponentBindingContext context)
                : base(name)
            {
                context.ThrowIfNull(nameof(context));
                context.Nodes.CheckNodes(4);

                Parameters = context.GetParameterSet<VoltageDelayParameters>();
                var state = context.GetState<IBiasingSimulationState>();
                _posNode = state.Map[state.GetSharedVariable(context.Nodes[0])];
                _negNode = state.Map[state.GetSharedVariable(context.Nodes[1])];
                _contPosNode = state.Map[state.GetSharedVariable(context.Nodes[2])];
                _contNegNode = state.Map[state.GetSharedVariable(context.Nodes[3])];
                Branch = state.CreatePrivateVariable(Name.Combine("branch"), Units.Ampere);
                _branchEq = state.Map[Branch];

                BiasingElements = new ElementSet<double>(state.Solver, new[] {
                        new MatrixLocation(_posNode, _branchEq),
                        new MatrixLocation(_negNode, _branchEq),
                        new MatrixLocation(_branchEq, _posNode),
                        new MatrixLocation(_branchEq, _negNode),
                        new MatrixLocation(_branchEq, _contPosNode),
                        new MatrixLocation(_branchEq, _contNegNode)
                    });
            }

            void IBiasingBehavior.Load()
            {
                BiasingElements.Add(1, -1, 1, -1, -1, 1);
            }
        }
    }
}