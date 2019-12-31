﻿using SpiceSharp.Behaviors;

namespace SpiceSharp.Components.CapacitorBehaviors
{
    /// <summary>
    /// A generic behavior for a <see cref="CapacitorModel"/>.
    /// </summary>
    /// <seealso cref="Behavior" />
    /// <seealso cref="IParameterized{T}" />
    public class ModelBehavior : Behavior, IParameterized<ModelBaseParameters>
    {
        /// <summary>
        /// Gets the parameter set.
        /// </summary>
        /// <value>
        /// The parameter set.
        /// </value>
        public ModelBaseParameters Parameters { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBehavior"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="context">The context.</param>
        public ModelBehavior(string name, ModelBindingContext context)
            : base(name)
        {
            Parameters = context.GetParameterSet<ModelBaseParameters>();
        }
    }
}