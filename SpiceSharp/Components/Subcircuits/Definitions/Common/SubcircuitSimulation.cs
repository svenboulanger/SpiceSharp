﻿using System;
using System.Collections.Generic;
using SpiceSharp.Behaviors;
using SpiceSharp.Entities;
using SpiceSharp.General;
using SpiceSharp.Simulations;

namespace SpiceSharp.Components.SubcircuitBehaviors
{
    /// <summary>
    /// A subcircuit simulation that captures created behaviors in a local container.
    /// </summary>
    /// <seealso cref="ISimulation" />
    public class SubcircuitSimulation : ISimulation
    {
        /// <summary>
        /// Occurs when the behaviors have been created.
        /// </summary>
        public event EventHandler<EventArgs> AfterBehaviorCreation;

        /// <summary>
        /// Gets the local states.
        /// </summary>
        /// <value>
        /// The local states.
        /// </value>
        public ITypeDictionary<ISimulationState> LocalStates { get; } = new InterfaceTypeDictionary<ISimulationState>();

        /// <summary>
        /// Gets the local parameters.
        /// </summary>
        /// <value>
        /// The local parameters.
        /// </value>
        public IParameterSetDictionary LocalConfiguration { get; }

        /// <summary>
        /// Gets the variables that are shared between the subcircuit simulation and the parent simulation.
        /// </summary>
        /// <value>
        /// The shared variables.
        /// </value>
        public IEnumerable<Variable> SharedVariables { get; }

        /// <summary>
        /// Gets the parent simulation.
        /// </summary>
        /// <value>
        /// The parent simulation.
        /// </value>
        protected ISimulation Parent { get; }

        /// <summary>
        /// Gets all the states that the class uses.
        /// </summary>
        /// <value>
        /// The states.
        /// </value>
        public IEnumerable<Type> States => Parent.States;

        /// <summary>
        /// Gets all behavior types that are used by the class.
        /// </summary>
        /// <value>
        /// The behaviors.
        /// </value>
        public IEnumerable<Type> Behaviors => Parent.Behaviors;

        /// <summary>
        /// Gets the name of the <see cref="ISimulation" />.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name => Parent.Name;

        /// <summary>
        /// Gets the current status of the <see cref="ISimulation" />.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public SimulationStatus Status => Parent.Status;

        /// <summary>
        /// Gets a set of configurations for the <see cref="ISimulation" />.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public ParameterSetDictionary Configurations => Parent.Configurations;

        /// <summary>
        /// Gets the variables.
        /// </summary>
        /// <value>
        /// The variables.
        /// </value>
        public IVariableSet Variables { get; }

        /// <summary>
        /// Gets the entity behaviors.
        /// </summary>
        /// <value>
        /// The entity behaviors.
        /// </value>
        public IBehaviorContainerCollection EntityBehaviors { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubcircuitSimulation" /> class.
        /// </summary>
        /// <param name="name">The name of the subcircuit.</param>
        /// <param name="parent">The parent simulation.</param>
        /// <param name="configuration">The configuration for the subcircuit.</param>
        /// <param name="shared">The shared variables.</param>
        public SubcircuitSimulation(string name, ISimulation parent, IParameterSetDictionary configuration, IEnumerable<Variable> shared)
        {
            Parent = parent.ThrowIfNull(nameof(parent));
            Variables = new SubcircuitVariableSet(name, parent.Variables);
            LocalConfiguration = configuration.ThrowIfNull(nameof(configuration));
            SharedVariables = shared.ThrowIfNull(nameof(shared));
        }

        /// <summary>
        /// Runs the <see cref="ISimulation" /> on the specified <see cref="IEntityCollection" />.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public void Run(IEntityCollection entities)
        {
            EntityBehaviors = new BehaviorContainerCollection(Parent.EntityBehaviors.Comparer);

            void BehaviorsNotFound(object sender, BehaviorsNotFoundEventArgs args)
            {
                if (entities.TryGetEntity(args.Name, out var entity))
                {
                    entity.CreateBehaviors(this);
                    if (EntityBehaviors.TryGetBehaviors(entity.Name, out var container))
                        args.Behaviors = container;
                }
                else
                {
                    // Try finding it in the parent simulation
                    args.Behaviors = Parent.EntityBehaviors[args.Name];
                }
            }
            EntityBehaviors.BehaviorsNotFound += BehaviorsNotFound;

            foreach (var entity in entities)
            {
                if (!EntityBehaviors.Contains(entity.Name))
                    entity.CreateBehaviors(this);
            }

            EntityBehaviors.BehaviorsNotFound -= BehaviorsNotFound;

            // Invoke the event
            AfterBehaviorCreation?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Gets the state of the specified type.
        /// </summary>
        /// <typeparam name="S">The simulation state type.</typeparam>
        /// <returns>
        /// The type, or <c>null</c> if the state isn't used.
        /// </returns>
        public S GetState<S>() where S : ISimulationState
        {
            if (LocalStates.TryGetValue(out S result))
                return result;
            return Parent.GetState<S>();
        }

        /// <summary>
        /// Checks if the class uses the specified state.
        /// </summary>
        /// <typeparam name="S">The simulation state type.</typeparam>
        /// <returns>
        ///   <c>true</c> if the class uses the state; otherwise <c>false</c>.
        /// </returns>
        public bool UsesState<S>() where S : ISimulationState => Parent.UsesState<S>();

        /// <summary>
        /// Checks if the class uses the specified behaviors.
        /// </summary>
        /// <typeparam name="B">The behavior type.</typeparam>
        /// <returns>
        ///   <c>true</c> if the class uses the behavior; otherwise <c>false</c>.
        /// </returns>
        public bool UsesBehaviors<B>() where B : IBehavior => Parent.UsesBehaviors<B>();
    }
}
