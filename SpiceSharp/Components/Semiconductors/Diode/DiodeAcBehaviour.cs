﻿using System.Numerics;
using SpiceSharp.Behaviours;

namespace SpiceSharp.Components.ComponentBehaviours
{
    /// <summary>
    /// AC behaviour for <see cref="Diode"/>
    /// </summary>
    public class DiodeAcBehaviour : CircuitObjectBehaviourAcLoad
    {
        /// <summary>
        /// Perform AC analysis
        /// </summary>
        /// <param name="ckt">Circuit</param>
        public override void Execute(Circuit ckt)
        {
            var diode = ComponentTyped<Diode>();

            var model = diode.Model as DiodeModel;
            var state = ckt.State;
            var cstate = state.Complex;
            double gspr, geq, xceq;

            gspr = model.DIOconductance * diode.DIOarea;
            geq = state.States[0][diode.DIOstate + Diode.DIOconduct];
            xceq = state.States[0][diode.DIOstate + Diode.DIOcapCurrent] * cstate.Laplace.Imaginary;
            cstate.Matrix[diode.DIOposNode, diode.DIOposNode] += gspr;
            cstate.Matrix[diode.DIOnegNode, diode.DIOnegNode] += new Complex(geq, xceq);

            cstate.Matrix[diode.DIOposPrimeNode, diode.DIOposPrimeNode] += new Complex(geq + gspr, xceq);

            cstate.Matrix[diode.DIOposNode, diode.DIOposPrimeNode] -= gspr;
            cstate.Matrix[diode.DIOnegNode, diode.DIOposPrimeNode] -= new Complex(geq, xceq);

            cstate.Matrix[diode.DIOposPrimeNode, diode.DIOposNode] -= gspr;
            cstate.Matrix[diode.DIOposPrimeNode, diode.DIOnegNode] -= new Complex(geq, xceq);
        }
    }
}