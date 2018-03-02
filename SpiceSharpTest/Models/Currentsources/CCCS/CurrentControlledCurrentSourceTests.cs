﻿using System;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpiceSharp;
using SpiceSharp.Components;
using SpiceSharp.Simulations;

namespace SpiceSharpTest.Models
{
    [TestClass]
    public class CurrentControlledCurrentSourceTests : Framework
    {
        [TestMethod]
        public void When_CCCSDC_Expect_Reference()
        {
            double gain = 0.85;
            double resistance = 1e4;

            // Build the circuit
            Circuit ckt = new Circuit();
            ckt.Objects.Add(
                new CurrentSource("I1", "in", "0", 0.0),
                new VoltageSource("V1", "in", "0", 0.0),
                new CurrentControlledCurrentSource("F1", "0", "out", "V1", gain),
                new Resistor("R1", "out", "0", resistance)
                );

            // Make the simulation, exports and references
            DC dc = new DC("DC", "I1", -10.0, 10.0, 1e-3);
            Export<double>[] exports = { new RealVoltageExport(dc, "out"), new RealPropertyExport(dc, "R1", "i") };
            Func<double, double>[] references = { (double sweep) => sweep * gain * resistance, (double sweep) => sweep * gain };
            AnalyzeDC(dc, ckt, exports, references);
        }

        [TestMethod]
        public void When_CCCSSmallSignal_Expect_Reference()
        {
            double magnitude = 0.6;
            double gain = 0.85;
            double resistance = 1e4;

            // Build the circuit
            Circuit ckt = new Circuit();
            ckt.Objects.Add(
                new CurrentSource("I1", "in", "0", 0.0),
                new VoltageSource("V1", "in", "0", 0.0),
                new CurrentControlledCurrentSource("F1", "0", "out", "V1", gain),
                new Resistor("R1", "out", "0", resistance)
                );
            ckt.Objects["I1"].ParameterSets.SetProperty("acmag", magnitude);

            // Make the simulation, exports and references
            AC ac = new AC("AC", new SpiceSharp.Simulations.Sweeps.DecadeSweep(1, 1e4, 3));
            Export<Complex>[] exports = { new ComplexVoltageExport(ac, "out"), new ComplexPropertyExport(ac, "R1", "i") };
            Func<double, Complex>[] references = { (double freq) => magnitude * gain * resistance, (double freq) => magnitude * gain };
            AnalyzeAC(ac, ckt, exports, references);
        }
    }
}