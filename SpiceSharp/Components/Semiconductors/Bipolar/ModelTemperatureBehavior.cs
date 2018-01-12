﻿using System;
using SpiceSharp.Diagnostics;
using SpiceSharp.Circuits;
using SpiceSharp.Attributes;
using SpiceSharp.Components.Bipolar;

namespace SpiceSharp.Behaviors.Bipolar
{
    /// <summary>
    /// Temperature behavior for a <see cref="Components.BJTModel"/>
    /// </summary>
    public class ModelTemperatureBehavior : Behaviors.TemperatureBehavior
    {
        /// <summary>
        /// Necessary behaviors and parameters
        /// </summary>
        ModelBaseParameters mbp;

        /// <summary>
        /// Shared parameters
        /// </summary>
        [SpiceName("invearlyvoltf"), SpiceInfo("Inverse early voltage:forward")]
        public double BJTinvEarlyVoltF { get; internal set; }
        [SpiceName("invearlyvoltr"), SpiceInfo("Inverse early voltage:reverse")]
        public double BJTinvEarlyVoltR { get; internal set; }
        [SpiceName("invrollofff"), SpiceInfo("Inverse roll off - forward")]
        public double BJTinvRollOffF { get; internal set; }
        [SpiceName("invrolloffr"), SpiceInfo("Inverse roll off - reverse")]
        public double BJTinvRollOffR { get; internal set; }
        [SpiceName("collectorconduct"), SpiceInfo("Collector conductance")]
        public double BJTcollectorConduct { get; internal set; }
        [SpiceName("emitterconduct"), SpiceInfo("Emitter conductance")]
        public double BJTemitterConduct { get; internal set; }
        [SpiceName("transtimevbcfact"), SpiceInfo("Transit time VBC factor")]
        public double BJTtransitTimeVBCFactor { get; internal set; }
        [SpiceName("excessphasefactor"), SpiceInfo("Excess phase fact.")]
        public double BJTexcessPhaseFactor { get; internal set; }
        
        public double fact1 { get; protected set; }
        public double xfc { get; protected set; }

        public double BJTf2 { get; protected set; }
        public double BJTf3 { get; protected set; }
        public double BJTf6 { get; protected set; }
        public double BJTf7 { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name</param>
        public ModelTemperatureBehavior(Identifier name) : base(name) { }

        /// <summary>
        /// Setup behavior
        /// </summary>
        /// <param name="provider">Data provider</param>
        public override void Setup(SetupDataProvider provider)
        {
            // Get parameters
            mbp = provider.GetParameters<ModelBaseParameters>();
        }

        /// <summary>
        /// Execute behavior
        /// </summary>
        /// <param name="ckt">Circuit</param>
        public override void Temperature(Circuit ckt)
        {
            if (!mbp.BJTtnom.Given)
                mbp.BJTtnom.Value = ckt.State.NominalTemperature;
            fact1 = mbp.BJTtnom / Circuit.CONSTRefTemp;

            if (!mbp.BJTleakBEcurrent.Given)
            {
                if (mbp.BJTc2.Given)
                    mbp.BJTleakBEcurrent.Value = mbp.BJTc2 * mbp.BJTsatCur;
                else
                    mbp.BJTleakBEcurrent.Value = 0;
            }
            if (!mbp.BJTleakBCcurrent.Given)
            {
                if (mbp.BJTc4.Given)
                    mbp.BJTleakBCcurrent.Value = mbp.BJTc4 * mbp.BJTsatCur;
                else
                    mbp.BJTleakBCcurrent.Value = 0;
            }
            if (!mbp.BJTminBaseResist.Given)
                mbp.BJTminBaseResist.Value = mbp.BJTbaseResist;

            /* 
			 * COMPATABILITY WARNING!
			 * special note:  for backward compatability to much older models, spice 2G
			 * implemented a special case which checked if B - E leakage saturation
			 * current was >1, then it was instead a the B - E leakage saturation current
			 * divided by IS, and multiplied it by IS at this point.  This was not
			 * handled correctly in the 2G code, and there is some question on its 
			 * reasonability, since it is also undocumented, so it has been left out
			 * here.  It could easily be added with 1 line.  (The same applies to the B - C
			 * leakage saturation current).   TQ  6 / 29 / 84
			 */

            if (mbp.BJTearlyVoltF.Given && mbp.BJTearlyVoltF != 0)
                BJTinvEarlyVoltF = 1 / mbp.BJTearlyVoltF;
            else
                BJTinvEarlyVoltF = 0;
            if (mbp.BJTrollOffF.Given && mbp.BJTrollOffF != 0)
                BJTinvRollOffF = 1 / mbp.BJTrollOffF;
            else
                BJTinvRollOffF = 0;
            if (mbp.BJTearlyVoltR.Given && mbp.BJTearlyVoltR != 0)
                BJTinvEarlyVoltR = 1 / mbp.BJTearlyVoltR;
            else
                BJTinvEarlyVoltR = 0;
            if (mbp.BJTrollOffR.Given && mbp.BJTrollOffR != 0)
                BJTinvRollOffR = 1 / mbp.BJTrollOffR;
            else
                BJTinvRollOffR = 0;
            if (mbp.BJTcollectorResist.Given && mbp.BJTcollectorResist != 0)
                BJTcollectorConduct = 1 / mbp.BJTcollectorResist;
            else
                BJTcollectorConduct = 0;
            if (mbp.BJTemitterResist.Given && mbp.BJTemitterResist != 0)
                BJTemitterConduct = 1 / mbp.BJTemitterResist;
            else
                BJTemitterConduct = 0;
            if (mbp.BJTtransitTimeFVBC.Given && mbp.BJTtransitTimeFVBC != 0)
                BJTtransitTimeVBCFactor = 1 / (mbp.BJTtransitTimeFVBC * 1.44);
            else
                BJTtransitTimeVBCFactor = 0;
            BJTexcessPhaseFactor = (mbp.BJTexcessPhase / (180.0 / Circuit.CONSTPI)) * mbp.BJTtransitTimeF;
            if (mbp.BJTdepletionCapCoeff.Given)
            {
                if (mbp.BJTdepletionCapCoeff > 0.9999)
                {
                    mbp.BJTdepletionCapCoeff.Value = 0.9999;
                    throw new CircuitException($"BJT model {Name}, parameter fc limited to 0.9999");
                }
            }
            else
            {
                mbp.BJTdepletionCapCoeff.Value = .5;
            }
            xfc = Math.Log(1 - mbp.BJTdepletionCapCoeff);
            BJTf2 = Math.Exp((1 + mbp.BJTjunctionExpBE) * xfc);
            BJTf3 = 1 - mbp.BJTdepletionCapCoeff * (1 + mbp.BJTjunctionExpBE);
            BJTf6 = Math.Exp((1 + mbp.BJTjunctionExpBC) * xfc);
            BJTf7 = 1 - mbp.BJTdepletionCapCoeff * (1 + mbp.BJTjunctionExpBC);
        }
    }
}