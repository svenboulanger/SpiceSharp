﻿using SpiceSharp.ParameterSets;
using SpiceSharp.Behaviors;
using SpiceSharp.Components.VoltageControlledVoltageSources;
using SpiceSharp.Simulations;
using SpiceSharp.Validation;
using System.Linq;
using System;
using SpiceSharp.Attributes;

namespace SpiceSharp.Components
{
    /// <summary>
    /// A voltage-controlled current-source.
    /// </summary>
    /// <seealso cref="Component"/>
    /// <seealso cref="IParameterized{P}"/>
    /// <seealso cref="VoltageControlledCurrentSources.Parameters"/>
    /// <seealso cref="IRuleSubject"/>
    [Pin(0, "V+"), Pin(1, "V-"), Pin(2, "VC+"), Pin(3, "VC-")]
    [VoltageDriver(0, 1), Connected(0, 1)]
    public class VoltageControlledVoltageSource : Component,
        IParameterized<Parameters>,
        IRuleSubject
    {
        /// <inheritdoc/>
        public Parameters Parameters { get; } = new Parameters();

        /// <summary>
        /// The pin count for voltage-controlled voltage sources.
        /// </summary>
        [ParameterName("pincount"), ParameterInfo("Number of pins")]
		public const int PinCount = 4;

        /// <summary>
        /// Initializes a new instance of the <see cref="VoltageControlledVoltageSource"/> class.
        /// </summary>
        /// <param name="name">The name of the entity.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is <c>null</c>.</exception>
        public VoltageControlledVoltageSource(string name) 
            : base(name, PinCount)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VoltageControlledVoltageSource"/> class.
        /// </summary>
        /// <param name="name">The name of the voltage-controlled voltage source</param>
        /// <param name="pos">The positive node</param>
        /// <param name="neg">The negative node</param>
        /// <param name="controlPos">The positive controlling node</param>
        /// <param name="controlNeg">The negative controlling node</param>
        /// <param name="gain">The voltage gain</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is <c>null</c>.</exception>
        public VoltageControlledVoltageSource(string name, string pos, string neg, string controlPos, string controlNeg, double gain) 
            : this(name)
        {
            Parameters.Coefficient = gain;
            Connect(pos, neg, controlPos, controlNeg);
        }

        /// <inheritdoc/>
        public override void CreateBehaviors(ISimulation simulation)
        {
            var behaviors = new BehaviorContainer(Name);
            var context = new ComponentBindingContext(this, simulation, LinkParameters);
            behaviors
                .AddIfNo<IFrequencyBehavior>(simulation, () => new Frequency(Name, context))
                .AddIfNo<IBiasingBehavior>(simulation, () => new Biasing(Name, context));
            simulation.EntityBehaviors.Add(behaviors);
        }

        /// <inheritdoc/>
        void IRuleSubject.Apply(IRules rules)
        {
            var p = rules.GetParameterSet<ComponentRuleParameters>();
            var nodes = Nodes.Select(name => p.Factory.GetSharedVariable(name)).ToArray();
            foreach (var rule in rules.GetRules<IConductiveRule>())
            {
                rule.AddPath(this, nodes[0], nodes[1]);
                rule.AddPath(this, ConductionTypes.None, nodes[2], nodes[3]);
            }
            foreach (var rule in rules.GetRules<IAppliedVoltageRule>())
                rule.Fix(this, nodes[0], nodes[1]);
        }
    }
}
