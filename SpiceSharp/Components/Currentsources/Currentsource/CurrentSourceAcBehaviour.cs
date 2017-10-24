﻿using System;
using System.Numerics;
using SpiceSharp.Behaviours;
using SpiceSharp.Circuits;

namespace SpiceSharp.Components.ComponentBehaviours
{
    /// <summary>
    /// Behaviour of a currentsource in AC analysis
    /// </summary>
    public class CurrentsourceAcBehaviour : CircuitObjectBehaviourAcLoad
    {
        /// <summary>
        /// Setup the behaviour
        /// </summary>
        /// <param name="component">Component</param>
        /// <param name="ckt">Circuit</param>
        public override void Setup(ICircuitObject component, Circuit ckt)
        {
            base.Setup(component, ckt);
            var isrc = ComponentTyped<Currentsource>();
            double radians = isrc.ISRCacPhase * Circuit.CONSTPI / 180.0;
            isrc.ISRCac = new Complex(isrc.ISRCacMag * Math.Cos(radians), isrc.ISRCacMag * Math.Sin(radians));
        }

        /// <summary>
        /// Execute AC behaviour
        /// </summary>
        /// <param name="ckt"></param>
        public override void Execute(Circuit ckt)
        {
            var source = ComponentTyped<Currentsource>();

            var cstate = ckt.State.Complex;
            cstate.Rhs[source.ISRCposNode] += source.ISRCac;
            cstate.Rhs[source.ISRCnegNode] -= source.ISRCac;
        }
    }
}