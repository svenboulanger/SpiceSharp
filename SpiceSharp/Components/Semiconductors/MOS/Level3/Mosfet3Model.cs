﻿using SpiceSharp.Behaviors;
using SpiceSharp.Components.MosfetBehaviors;
using SpiceSharp.Components.MosfetBehaviors.Level3;
using SpiceSharp.Simulations;

namespace SpiceSharp.Components
{
    /// <summary>
    /// A model for a <see cref="Mosfet3"/>
    /// </summary>
    public class Mosfet3Model : Model,
        IParameterized<ModelBaseParameters>,
        IParameterized<ModelNoiseParameters>
    {
        /// <summary>
        /// Gets the parameter set.
        /// </summary>
        /// <value>
        /// The parameter set.
        /// </value>
        public ModelBaseParameters Parameters { get; } = new ModelBaseParameters();

        /// <summary>
        /// Gets the noise parameters.
        /// </summary>
        /// <value>
        /// The noise parameters.
        /// </value>
        public ModelNoiseParameters NoiseParameters { get; } = new ModelNoiseParameters();

        /// <summary>
        /// Gets the parameter set.
        /// </summary>
        /// <value>
        /// The parameter set.
        /// </value>
        ModelNoiseParameters IParameterized<ModelNoiseParameters>.Parameters => NoiseParameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="Mosfet3Model"/> class.
        /// </summary>
        /// <param name="name">The name of the device</param>
        public Mosfet3Model(string name) 
            : base(name)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mosfet3Model"/> class.
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="nmos">True for NMOS transistors, false for PMOS transistors</param>
        public Mosfet3Model(string name, bool nmos) 
            : base(name)
        {
            if (nmos)
                Parameters.SetNmos(true);
            else
                Parameters.SetPmos(true);
        }

        /// <summary>
        /// Creates the behaviors for the specified simulation and registers them with the simulation.
        /// </summary>
        /// <param name="simulation">The simulation.</param>
        public override void CreateBehaviors(ISimulation simulation)
        {
            var behaviors = new BehaviorContainer(Name);
            CalculateDefaults();
            var context = new ModelBindingContext(this, simulation, LinkParameters);
            behaviors.AddIfNo<ITemperatureBehavior>(simulation, () => new ModelTemperatureBehavior(Name, context));
            simulation.EntityBehaviors.Add(behaviors);
        }
    }
}
