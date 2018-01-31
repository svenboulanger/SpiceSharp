﻿using System;
using System.Numerics;
using SpiceSharp.Sparse;
using SpiceSharp.Attributes;
using SpiceSharp.Behaviors;
using SpiceSharp.Simulations;

namespace SpiceSharp.Components.VoltagesourceBehaviors
{
    /// <summary>
    /// AC behavior for <see cref="VoltageSource"/>
    /// </summary>
    public class FrequencyBehavior : Behaviors.FrequencyBehavior, IConnectedBehavior
    {
        /// <summary>
        /// AC excitation vector
        /// </summary>
        public Complex AC { get; protected set; }

        /// <summary>
        /// Nodes
        /// </summary>
        int posourceNode, negateNode, branchEq;

        /// <summary>
        /// Matrix elements
        /// </summary>
        protected MatrixElement PosBranchPtr { get; private set; }
        protected MatrixElement NegBranchPtr { get; private set; }
        protected MatrixElement BranchPosPtr { get; private set; }
        protected MatrixElement BranchNegPtr { get; private set; }
        protected MatrixElement BranchBranchPtr { get; private set; }

        /// <summary>
        /// Properties
        /// </summary>
        [PropertyName("v"), PropertyInfo("Complex voltage")]
        public Complex Voltage => AC;
        [PropertyName("i"), PropertyName("c"), PropertyInfo("Complex current")]
        public Complex GetCurrent(State state)
        {
			if (state == null)
				throw new ArgumentNullException(nameof(state));
            return state.ComplexSolution[branchEq];
        }
        [PropertyName("p"), PropertyInfo("Complex power")]
        public Complex GetPower(State state)
        {
			if (state == null)
				throw new ArgumentNullException(nameof(state));

            Complex v = state.ComplexSolution[posourceNode] - state.ComplexSolution[negateNode];
            Complex i = state.ComplexSolution[branchEq];
            return -v * Complex.Conjugate(i);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name</param>
        public FrequencyBehavior(Identifier name) : base(name) { }

        /// <summary>
        /// Setup the behavior
        /// </summary>
        /// <param name="provider">Data provider</param>
        public override void Setup(SetupDataProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            // Get parameters
            var ap = provider.GetParameterSet<FrequencyParameters>(0);

            // Calculate AC vector
            double radians = ap.ACPhase * Math.PI / 180.0;
            AC = new Complex(ap.ACMagnitude * Math.Cos(radians), ap.ACMagnitude * Math.Sin(radians));

            // Get behaviors
            var load = provider.GetBehavior<LoadBehavior>(0);
            branchEq = load.BranchEq;
        }
        
        /// <summary>
        /// Connect the behavior
        /// </summary>
        /// <param name="pins">Pins</param>
        public void Connect(params int[] pins)
        {
            if (pins == null)
                throw new ArgumentNullException(nameof(pins));
            if (pins.Length != 2)
                throw new Diagnostics.CircuitException("Pin count mismatch: 2 pins expected, {0} given".FormatString(pins.Length));
            posourceNode = pins[0];
            negateNode = pins[1];
        }

        /// <summary>
        /// Get matrix pointers
        /// </summary>
        /// <param name="matrix">Matrix</param>
        public override void GetMatrixPointers(Matrix matrix)
        {
			if (matrix == null)
				throw new ArgumentNullException(nameof(matrix));

            PosBranchPtr = matrix.GetElement(posourceNode, branchEq);
            BranchPosPtr = matrix.GetElement(branchEq, posourceNode);
            NegBranchPtr = matrix.GetElement(negateNode, branchEq);
            BranchNegPtr = matrix.GetElement(branchEq, negateNode);
        }

        /// <summary>
        /// Unsetup the behavior
        /// </summary>
        public override void Unsetup()
        {
            PosBranchPtr = null;
            BranchPosPtr = null;
            NegBranchPtr = null;
            BranchNegPtr = null;
        }

        /// <summary>
        /// Execute behavior for AC analysis
        /// </summary>
        /// <param name="simulation">Frequency-based simulation</param>
        public override void Load(FrequencySimulation simulation)
        {
			if (simulation == null)
				throw new ArgumentNullException(nameof(simulation));

            var state = simulation.State;
            PosBranchPtr.Add(1.0);
            BranchPosPtr.Add(1.0);
            NegBranchPtr.Sub(1.0);
            BranchNegPtr.Sub(1.0);
            state.ComplexRhs[branchEq] += AC;
        }
    }
}