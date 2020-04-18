﻿namespace SpiceSharp.Attributes
{
    /// <summary>
    /// An attribute that indicates a minimum value for a parameter.
    /// </summary>
    /// <seealso cref="RuleAttribute" />
    public sealed class GreaterThanAttribute : RuleAttribute
    {
        /// <summary>
        /// Gets the minimum value.
        /// </summary>
        /// <value>
        /// The minimum.
        /// </value>
        public double Minimum { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GreaterThanAttribute"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public GreaterThanAttribute(double value)
        {
            Minimum = value;
        }
    }
}
