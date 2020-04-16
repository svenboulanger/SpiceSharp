﻿using SpiceSharp.Simulations.Variables;
using System;

namespace SpiceSharp.Simulations
{
    /// <summary>
    /// Describes an <see cref="IVariable"/> that also returns its value.
    /// </summary>
    /// <typeparam name="T">The base value type.</typeparam>
    /// <seealso cref="IVariable" />
    public interface IVariable<T> : IVariable where T : IFormattable
    {
        /// <summary>
        /// Gets the value of the variable.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        T Value { get; }
    }

    /// <summary>
    /// Describes an unknown variable that will be solved by a simulation.
    /// </summary>
    public interface IVariable
    {
        /// <summary>
        /// Gets the name of the variable.
        /// </summary>
        /// <value>
        /// The name of the variable.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets the units of the quantity.
        /// </summary>
        /// <value>
        /// The units.
        /// </value>
        IUnit Unit { get; }
    }
}