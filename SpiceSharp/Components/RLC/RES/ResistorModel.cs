﻿using SpiceSharp.Behaviors;
using SpiceSharp.Components.ResistorBehaviors;

namespace SpiceSharp.Components
{
    /// <summary>
    /// A model for semiconductor <see cref="Resistor"/>
    /// </summary>
    public class ResistorModel : Model
    {
        static ResistorModel()
        {
            RegisterBehaviorFactory(typeof(ResistorModel), new BehaviorFactoryDictionary
            {
                { typeof(CommonBehaviors.ModelParameterContainer), e => new CommonBehaviors.ModelParameterContainer(e.Name) }
            });
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ResistorModel"/> class.
        /// </summary>
        /// <param name="name"></param>
        public ResistorModel(string name) : base(name)
        {
            Parameters.Add(new ModelBaseParameters());
        }
    }
}
