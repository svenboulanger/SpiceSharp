﻿using NUnit.Framework;
using System.Numerics;
using SpiceSharp;
using SpiceSharp.Components;
using SpiceSharp.Simulations;

namespace SpiceSharpTest.Models
{
    [TestFixture]
    public class MOS1Tests : Framework
    {
        private Mosfet1 CreateMOS1(string name, string d, string g, string s, string b, string model)
        {
            var mos = new Mosfet1(name) {Model = model};
            mos.Connect(d, g, s, b);
            return mos;
        }

        private Mosfet1Model CreateMOS1Model(string name, string parameters)
        {
            var model = new Mosfet1Model(name);
            ApplyParameters(model, parameters);
            return model;
        }

        [Test]
        public void When_MOS1DC_Expect_Spice3f5Reference()
        {
            /*
             * Mosfet biased by voltage sources
             * Current is expected to behave like the reference. Reference is from Spice 3f5.
             * The model is part from the ntd20n06 (OnSemi) device.
             */
            // Create circuit
            var ckt = new Circuit(
                new VoltageSource("V1", "g", "0", 0.0),
                new VoltageSource("V2", "d", "0", 0),
                new VoltageSource("V3", "b", "0", -5.0),
                CreateMOS1("M1", "d", "g", "0", "b", "MM"),
                CreateMOS1Model("MM", "IS=1e-32 VTO=3.03646 LAMBDA=0 KP=5.28747 CGSO=6.5761e-06 CGDO=1e-11")
                );

            // Create simulation
            var dc = new DC("dc", new[] {
                new SweepConfiguration("V2", -5, 5, 0.5),
                new SweepConfiguration("V1", -5, 5, 0.5)
            });

            // Create exports
            Export<double>[] exports = { new RealPropertyExport(dc, "V2", "i") };

            // Create references
            var references = new double[1][];
            references[0] = new[]
            {
                0.000000000000000e+00, 0.000000000000000e+00, 0.000000000000000e+00, 0.000000000000000e+00,
                0.000000000000000e+00, 0.000000000000000e+00, 0.000000000000000e+00, 5.680575723775245e-01,
                2.454468244277527e+00, 5.662746416177523e+00, 1.019289208807752e+01, 1.604490525997753e+01,
                2.321878593187752e+01, 3.171453410377752e+01, 4.153214977567750e+01, 5.267163294757751e+01,
                6.513298361947750e+01, 7.834814421900002e+01, 9.156681921900000e+01, 1.047854942190000e+02,
                1.180041692190000e+02, -4.999999999999999e-13, -4.999999999999999e-13, -4.999999999999999e-13,
                -4.999999999999999e-13, -4.999999999999999e-13, -4.999999999999999e-13, -4.999999999999999e-13,
                -4.999999999999999e-13, 5.680575723770240e-01, 2.454468244277025e+00, 5.662746416177022e+00,
                1.019289208807702e+01, 1.604490525997703e+01, 2.321878593187702e+01, 3.171453410377702e+01,
                4.153214977567701e+01, 5.267163294757702e+01, 6.456492604709950e+01, 7.646173354709953e+01,
                8.835854104709949e+01, 1.002553485470995e+02, -9.999999999999998e-13, -9.999999999999998e-13,
                -9.999999999999998e-13, -9.999999999999998e-13, -9.999999999999998e-13, -9.999999999999998e-13,
                -9.999999999999998e-13, -9.999999999999998e-13, -9.999999999999998e-13, 5.680575723765238e-01,
                2.454468244276526e+00, 5.662746416176525e+00, 1.019289208807652e+01, 1.604490525997653e+01,
                2.321878593187651e+01, 3.171453410377652e+01, 4.153214977567652e+01, 5.210357537519901e+01,
                6.267851537519901e+01, 7.325345537519900e+01, 8.382839537519902e+01, -1.500000000000000e-12,
                -1.500000000000000e-12, -1.500000000000000e-12, -1.500000000000000e-12, -1.500000000000000e-12,
                -1.500000000000000e-12, -1.500000000000000e-12, -1.500000000000000e-12, -1.500000000000000e-12,
                -1.500000000000000e-12, 5.680575723760235e-01, 2.454468244276028e+00, 5.662746416176023e+00,
                1.019289208807602e+01, 1.604490525997603e+01, 2.321878593187601e+01, 3.171453410377602e+01,
                4.096409220329851e+01, 5.021716470329851e+01, 5.947023720329851e+01, 6.872330970329853e+01,
                -2.000000000000000e-12, -2.000000000000000e-12, -2.000000000000000e-12, -2.000000000000000e-12,
                -2.000000000000000e-12, -2.000000000000000e-12, -2.000000000000000e-12, -2.000000000000000e-12,
                -2.000000000000000e-12, -2.000000000000000e-12, -2.000000000000000e-12, 5.680575723755232e-01,
                2.454468244275526e+00, 5.662746416175525e+00, 1.019289208807552e+01, 1.604490525997553e+01,
                2.321878593187552e+01, 3.114647653139799e+01, 3.907768153139800e+01, 4.700888653139801e+01,
                5.494009153139801e+01, -2.500000000000000e-12, -2.500000000000000e-12, -2.500000000000000e-12,
                -2.500000000000000e-12, -2.500000000000000e-12, -2.500000000000000e-12, -2.500000000000000e-12,
                -2.500000000000000e-12, -2.500000000000000e-12, -2.500000000000000e-12, -2.500000000000000e-12,
                -2.500000000000000e-12, 5.680575723750239e-01, 2.454468244275026e+00, 5.662746416175024e+00,
                1.019289208807502e+01, 1.604490525997503e+01, 2.265072835949751e+01, 2.926006585949749e+01,
                3.586940335949749e+01, 4.247874085949751e+01, -3.000000000000000e-12, -3.000000000000000e-12,
                -3.000000000000000e-12, -3.000000000000000e-12, -3.000000000000000e-12, -3.000000000000000e-12,
                -3.000000000000000e-12, -3.000000000000000e-12, -3.000000000000000e-12, -3.000000000000000e-12,
                -3.000000000000000e-12, -3.000000000000000e-12, -3.000000000000000e-12, 5.680575723745234e-01,
                2.454468244274526e+00, 5.662746416174523e+00, 1.019289208807452e+01, 1.547684768759700e+01,
                2.076431768759700e+01, 2.605178768759699e+01, 3.133925768759698e+01, -3.500000000000000e-12,
                -3.500000000000000e-12, -3.500000000000000e-12, -3.500000000000000e-12, -3.500000000000000e-12,
                -3.500000000000000e-12, -3.500000000000000e-12, -3.500000000000000e-12, -3.500000000000000e-12,
                -3.500000000000000e-12, -3.500000000000000e-12, -3.500000000000000e-12, -3.500000000000000e-12,
                -3.500000000000000e-12, 5.680575723740233e-01, 2.454468244274027e+00, 5.662746416174024e+00,
                9.624834515696499e+00, 1.359043701569650e+01, 1.755603951569650e+01, 2.152164201569649e+01,
                -4.000000000000000e-12, -4.000000000000000e-12, -4.000000000000000e-12, -4.000000000000000e-12,
                -4.000000000000000e-12, -4.000000000000000e-12, -4.000000000000000e-12, -4.000000000000000e-12,
                -4.000000000000000e-12, -4.000000000000000e-12, -4.000000000000000e-12, -4.000000000000000e-12,
                -4.000000000000000e-12, -4.000000000000000e-12, -4.000000000000000e-12, 5.680575723735242e-01,
                2.454468244273526e+00, 5.094688843796002e+00, 7.738423843796001e+00, 1.038215884379600e+01,
                1.302589384379600e+01, -4.500000000000000e-12, -4.500000000000000e-12, -4.500000000000000e-12,
                -4.500000000000000e-12, -4.500000000000000e-12, -4.500000000000000e-12, -4.500000000000000e-12,
                -4.500000000000000e-12, -4.500000000000000e-12, -4.500000000000000e-12, -4.500000000000000e-12,
                -4.500000000000000e-12, -4.500000000000000e-12, -4.500000000000000e-12, -4.500000000000000e-12,
                -4.500000000000000e-12, 5.680575723730232e-01, 1.886410671895499e+00, 3.208278171895499e+00,
                4.530145671895499e+00, 5.852013171895498e+00, -5.000000000000000e-12, -5.000000000000000e-12,
                -5.000000000000000e-12, -5.000000000000000e-12, -5.000000000000000e-12, -5.000000000000000e-12,
                -5.000000000000000e-12, -5.000000000000000e-12, -5.000000000000000e-12, -5.000000000000000e-12,
                -5.000000000000000e-12, -5.000000000000000e-12, -5.000000000000000e-12, -5.000000000000000e-12,
                -5.000000000000000e-12, -5.000000000000000e-12, -5.000000000000000e-12, -5.000000000000000e-12,
                -5.000000000000000e-12, -5.000000000000000e-12, -5.000000000000000e-12, -5.500000000000000e-12,
                -5.500000000000000e-12, -5.500000000000000e-12, -5.500000000000000e-12, -5.500000000000000e-12,
                -5.500000000000000e-12, -5.500000000000000e-12, -5.500000000000000e-12, -5.500000000000000e-12,
                -5.500000000000000e-12, -5.500000000000000e-12, -5.500000000000000e-12, -5.500000000000000e-12,
                -5.500000000000000e-12, -5.500000000000000e-12, -5.500000000000000e-12, -5.500000000000000e-12,
                -5.680575723830241e-01, -1.886410671905500e+00, -3.208278171905500e+00, -4.530145671905499e+00,
                -5.999999999999999e-12, -5.999999999999999e-12, -5.999999999999999e-12, -5.999999999999999e-12,
                -5.999999999999999e-12, -5.999999999999999e-12, -5.999999999999999e-12, -5.999999999999999e-12,
                -5.999999999999999e-12, -5.999999999999999e-12, -5.999999999999999e-12, -5.999999999999999e-12,
                -5.999999999999999e-12, -5.999999999999999e-12, -5.999999999999999e-12, -5.999999999999999e-12,
                -5.999999999999999e-12, -5.680575723835251e-01, -2.454468244283522e+00, -5.094688843805997e+00,
                -7.738423843805997e+00, -6.500000000000000e-12, -6.500000000000000e-12, -6.500000000000000e-12,
                -6.500000000000000e-12, -6.500000000000000e-12, -6.500000000000000e-12, -6.500000000000000e-12,
                -6.500000000000000e-12, -6.500000000000000e-12, -6.500000000000000e-12, -6.500000000000000e-12,
                -6.500000000000000e-12, -6.500000000000000e-12, -6.500000000000000e-12, -6.500000000000000e-12,
                -6.500000000000000e-12, -6.500000000000000e-12, -5.680575723840242e-01, -2.454468244284023e+00,
                -5.662746416184021e+00, -9.624834515706503e+00, -7.000000000000000e-12, -7.000000000000000e-12,
                -7.000000000000000e-12, -7.000000000000000e-12, -7.000000000000000e-12, -7.000000000000000e-12,
                -7.000000000000000e-12, -7.000000000000000e-12, -7.000000000000000e-12, -7.000000000000000e-12,
                -7.000000000000000e-12, -7.000000000000000e-12, -7.000000000000000e-12, -7.000000000000000e-12,
                -7.000000000000000e-12, -7.000000000000000e-12, -7.000000000000000e-12, -5.680575723845251e-01,
                -2.454468244284524e+00, -5.662746416184522e+00, -1.019289208808452e+01, -7.500000000000000e-12,
                -7.500000000000000e-12, -7.500000000000000e-12, -7.500000000000000e-12, -7.500000000000000e-12,
                -7.500000000000000e-12, -7.500000000000000e-12, -7.500000000000000e-12, -7.500000000000000e-12,
                -7.500000000000000e-12, -7.500000000000000e-12, -7.500000000000000e-12, -7.500000000000000e-12,
                -7.500000000000000e-12, -7.500000000000000e-12, -7.500000000000000e-12, -7.500000000000000e-12,
                -5.680575723850243e-01, -2.454468244285025e+00, -5.662746416185023e+00, -1.019289208808502e+01,
                -8.000000000000000e-12, -8.000000000000000e-12, -8.000000000000000e-12, -8.000000000000000e-12,
                -8.000000000000000e-12, -8.000000000000000e-12, -8.000000000000000e-12, -8.000000000000000e-12,
                -8.000000000000000e-12, -8.000000000000000e-12, -8.000000000000000e-12, -8.000000000000000e-12,
                -8.000000000000000e-12, -8.000000000000000e-12, -8.000000000000000e-12, -8.000000000000000e-12,
                -8.000000000000000e-12, -5.680575723855252e-01, -2.454468244285522e+00, -5.662746416185520e+00,
                -1.019289208808552e+01, -8.500000000000000e-12, -8.500000000000000e-12, -8.500000000000000e-12,
                -8.500000000000000e-12, -8.500000000000000e-12, -8.500000000000000e-12, -8.500000000000000e-12,
                -8.500000000000000e-12, -8.500000000000000e-12, -8.500000000000000e-12, -8.500000000000000e-12,
                -8.500000000000000e-12, -8.500000000000000e-12, -8.500000000000000e-12, -8.500000000000000e-12,
                -8.500000000000000e-12, -8.500000000000000e-12, -5.680575723860244e-01, -2.454468244286023e+00,
                -5.662746416186021e+00, -1.019289208808603e+01, -9.000000000000000e-12, -9.000000000000000e-12,
                -9.000000000000000e-12, -9.000000000000000e-12, -9.000000000000000e-12, -9.000000000000000e-12,
                -9.000000000000000e-12, -9.000000000000000e-12, -9.000000000000000e-12, -9.000000000000000e-12,
                -9.000000000000000e-12, -9.000000000000000e-12, -9.000000000000000e-12, -9.000000000000000e-12,
                -9.000000000000000e-12, -9.000000000000000e-12, -9.000000000000000e-12, -5.680575723865253e-01,
                -2.454468244286524e+00, -5.662746416186522e+00, -1.019289208808652e+01, -9.500000000000000e-12,
                -9.500000000000000e-12, -9.500000000000000e-12, -9.500000000000000e-12, -9.500000000000000e-12,
                -9.500000000000000e-12, -9.500000000000000e-12, -9.500000000000000e-12, -9.500000000000000e-12,
                -9.500000000000000e-12, -9.500000000000000e-12, -9.500000000000000e-12, -9.500000000000000e-12,
                -9.500000000000000e-12, -9.500000000000000e-12, -9.500000000000000e-12, -9.500000000000000e-12,
                -5.680575723870245e-01, -2.454468244287025e+00, -5.662746416187023e+00, -1.019289208808702e+01,
                -9.999999999999999e-12, -9.999999999999999e-12, -9.999999999999999e-12, -9.999999999999999e-12,
                -9.999999999999999e-12, -9.999999999999999e-12, -9.999999999999999e-12, -9.999999999999999e-12,
                -9.999999999999999e-12, -9.999999999999999e-12, -9.999999999999999e-12, -9.999999999999999e-12,
                -9.999999999999999e-12, -9.999999999999999e-12, -9.999999999999999e-12, -9.999999999999999e-12,
                -9.999999999999999e-12, -5.680575723875254e-01, -2.454468244287522e+00, -5.662746416187520e+00,
                -1.019289208808753e+01
            };

            // Run simulation
            AnalyzeDC(dc, ckt, exports, references);
        }

        [Test]
        public void When_MOS1CommonSourceAmplifierSmallSignal_Expect_Spice3f5Reference()
        {
            /*
             * Simple common source amplifier
             * Output voltage gain is expected to match reference. Reference by Spice 3f5.
             */
            // Build circuit
            var ckt = new Circuit(
                new VoltageSource("V1", "in", "0", 0.0)
                    .SetParameter("acmag", 1.0),
                new VoltageSource("V2", "vdd", "0", 5.0),
                new Resistor("R1", "vdd", "out", 10.0e3),
                new Resistor("R2", "out", "g", 10.0e3),
                new Capacitor("Cin", "in", "g", 1e-6),
                CreateMOS1("M1", "out", "g", "0", "0", "MM"),
                CreateMOS1Model("MM", "IS=1e-32 VTO=3.03646 LAMBDA=0 KP=5.28747 CGSO=6.5761e-06 CGDO=1e-11")
                );

            // Create simulation
            var ac = new AC("ac", new DecadeSweep(10, 10e9, 5));

            // Create exports
            Export<Complex>[] exports = { new ComplexVoltageExport(ac, "out") };

            // Create references
            double[] riref =
            {
                -1.725813644006744e-03, -6.255567388468394e-01, -4.334997991949969e-03, -9.914292083082819e-01,
                -1.088870790416865e-02, -1.571263986482406e+00, -2.734921201531804e-02, -2.490104807455213e+00,
                -6.868558931531524e-02, -3.945830610208745e+00, -1.724514213823252e-01, -6.250857335307440e+00,
                -4.326808652667278e-01, -9.895562775467608e+00, -1.083718679773610e+00, -1.563829379084251e+01,
                -2.702649136180583e+00, -2.460721571760895e+01, -6.668576859467226e+00, -3.830945453134210e+01,
                -1.603760999419659e+01, -5.813162362216580e+01, -3.639300462484001e+01, -8.323207309729999e+01,
                -7.356417703660050e+01, -1.061546849651946e+02, -1.239747410542734e+02, -1.128771300396776e+02,
                -1.704839090551324e+02, -9.793908744049646e+01, -2.004160562764431e+02, -7.264486862286662e+01,
                -2.154771197776309e+02, -4.928026345664456e+01, -2.221224344754558e+02, -3.205256931555954e+01,
                -2.248834695491267e+02, -2.047502071380190e+01, -2.260018549839163e+02, -1.298284168009341e+01,
                -2.264501941354974e+02, -8.207439641802505e+00, -2.266291765967058e+02, -5.181955183396055e+00,
                -2.267005095543059e+02, -3.269540294698130e+00, -2.267289201967292e+02, -2.061484698952238e+00,
                -2.267402326137311e+02, -1.298056697906094e+00, -2.267447363683708e+02, -8.147282590010622e-01,
                -2.267465291092059e+02, -5.072375792911769e-01, -2.267472421017376e+02, -3.092289341889395e-01,
                -2.267475241460346e+02, -1.779661469529321e-01, -2.267476318976589e+02, -8.511710389733645e-02,
                -2.267476634094340e+02, -1.064055031339878e-02, -2.267476473568060e+02, 6.153924230708851e-02,
                -2.267475691320166e+02, 1.470022622053010e-01, -2.267473575512703e+02, 2.641955750327690e-01,
                -2.267468200791099e+02, 4.384147465157958e-01, -2.267454676298356e+02, 7.072623998910071e-01,
                -2.267420695496819e+02, 1.128758783561459e+00, -2.267335340265157e+02, 1.793841675558252e+00,
                -2.267120964325891e+02, 2.845900637604031e+00, -2.266582653689669e+02, 4.511350906221913e+00,
                -2.265231600192664e+02, 7.147007490994596e+00, -2.261844969618107e+02, 1.131116522055704e+01,
                -2.253382440974266e+02, 1.786070290176738e+01, -2.232401028216283e+02, 2.804520605312238e+01,
                -2.181374806913368e+02, 4.343740586524000e+01, -2.062891648647118e+02, 6.512151536164929e+01
            };
            var references = new Complex[1][];
            references[0] = new Complex[riref.Length / 2];
            for (var i = 0; i < riref.Length; i += 2)
                references[0][i / 2] = new Complex(riref[i], riref[i + 1]);

            // Run test
            AnalyzeAC(ac, ckt, exports, references);
            DestroyExports(exports);
        }

        [Test]
        public void When_MOS1SwitchTransient_Expect_Spice3f5Reference()
        {
            // Create circuit
            var ckt = new Circuit(
                new VoltageSource("V1", "in", "0", new Pulse(1, 5, 1e-6, 1e-9, 0.5e-6, 2e-6, 6e-6)),
                new VoltageSource("Vsupply", "vdd", "0", 5),
                new Resistor("R1", "out", "vdd", 1.0e3),
                CreateMOS1("M1", "out", "in", "0", "0", "MM"),
                CreateMOS1Model("MM", "IS=1e-32 VTO=3.03646 LAMBDA=0 KP=5.28747 CGSO=6.5761e-06 CGDO=1e-11")
                );

            // Create simulation
            var tran = new Transient("tran", 1e-9, 10e-6);

            // Create exports
            Export<double>[] exports = { new RealVoltageExport(tran, "out") };

            // Create references
            var references = new double[1][];
            references[0] = new[]
            {
                4.999999995, 4.999999995, 4.999999995, 4.999999995, 4.999999995, 4.999999995,
                4.999999995, 4.999999995, 4.999999995, 4.999999995, 4.999999995, 4.999999995,
                4.999999995, 4.999999995, 4.999999995, 4.999999995, 4.999999995, 4.999999995,
                4.999999995, 4.999999995, 4.999999995, 5.00396039103568, 5.00403881471948,
                5.00396192390432, 0.000582292616641902, 0.000481056037412557, 0.000481608079367986,
                0.000481608082475126, 0.000481608079367997, 0.000481608082475121, 0.000481608079368,
                0.000481608082475119, 0.000481608079368001, 0.000481608082475119, 0.000481608079368001,
                0.000481608082475119, 0.000481608079368001, 0.000481608082475119, 0.000481608079368001,
                0.000481608082475119, 0.000481608079368001, 0.000481608082475119, 0.000481608079368001,
                0.000481608082475119, 0.000481608079368001, 0.000481608082475119, 0.000481608079368001,
                0.000481608082475119, 0.000524340258157584, 0.000637470476683906, 0.00112152016845835,
                -17.201701900935, 4.99997891421405, 5.00000507534358, 4.99997891596964, 4.99999999219434,
                4.99999999780491, 4.99999999219546, 4.99999999780435, 4.99999999219574, 4.9999999978042,
                4.99999999219586, 4.99999999780409, 4.99999999219597, 4.99999999780398, 4.99999999219608,
                4.99999999780386, 4.99999999219619, 4.99999999780375, 4.99999999219631, 4.99999999780364,
                4.99999999219642, 4.99999999780353, 4.99999999219653, 4.99999999780342, 4.99999999219664,
                4.99999999780319, 5.00396039106345, 5.00403881469225, 5.00396192393101, 0.000582292618101685,
                0.000481056037413949, 0.00048160807936799, 0.000481608082475123, 0.000481608079368001,
                0.000481608082475117, 0.000481608079368003, 0.000481608082475116, 0.000481608079368004,
                0.000481608082475116, 0.000481608079368004, 0.000481608082475115, 0.000481608079368004,
                0.000481608082475115, 0.000481608079368004, 0.000481608082475115, 0.000481608079368004,
                0.000481608082475115, 0.000481608079368004, 0.000481608082475115, 0.000481608079368004,
                0.000481608082475115, 0.000481608079368004, 0.000481608082475116, 0.00052434025815758,
                0.000637470476683901, 0.00112152016845832, -17.2017019009448, 4.99997891421405, 5.00000507534358,
                4.99997891596964, 4.99999999219434, 4.99999999780491, 4.99999999219546, 4.99999999780435,
                4.99999999219574, 4.9999999978042, 4.99999999219597
            };

            // Run test
            AnalyzeTransient(tran, ckt, exports, references);
            DestroyExports(exports);

        }

        [Test]
        public void When_MOS1CommonSourceAmplifierNoise_Expect_Spice3f5Reference()
        {
            // Create circuit
            var ckt = new Circuit(
                new VoltageSource("V1", "in", "0", 0.0)
                    .SetParameter("acmag", 1.0),
                new VoltageSource("V2", "vdd", "0", 5.0),
                new Resistor("R1", "vdd", "out", 10e3),
                new Resistor("R2", "out", "g", 10e3),
                new Capacitor("Cin", "in", "g", 1e-6),
                CreateMOS1("M1", "out", "g", "0", "0", "MM")
                    .SetParameter("w", 100e-6)
                    .SetParameter("l", 100e-6),
                CreateMOS1Model("MM", "IS = 1e-32 VTO = 3.03646 LAMBDA = 0 KP = 5.28747 CGSO = 6.5761e-06 CGDO = 1e-11 KF = 1e-25")
                );

            // Create simulation, exports and references
            var noise = new Noise("noise", "out", "V1", new DecadeSweep(10, 10e9, 10));
            Export<double>[] exports = { new InputNoiseDensityExport(noise), new OutputNoiseDensityExport(noise) };
            var references = new double[2][];
            references[0] = new[]
            {
                2.815651317421889e-12, 1.645027613662841e-12, 1.010218811439069e-12, 6.538511835765611e-13,
                4.448954302782106e-13, 3.160638431808556e-13, 2.323481430226209e-13, 1.751824608495907e-13,
                1.344515862942468e-13, 1.044423847431228e-13, 8.178038471381158e-14, 6.436837445436251e-14,
                5.083280290976165e-14, 4.022913978256862e-14, 3.188055911876120e-14, 2.528625121005242e-14,
                2.006686143767511e-14, 1.593030775258497e-14, 1.264921826655891e-14, 1.004530985270185e-14,
                7.978132791490454e-15, 6.336708373912842e-15, 5.033176185391673e-15, 3.997893232665373e-15,
                3.175613873077495e-15, 2.522491942006981e-15, 2.003717769837784e-15, 1.591650381587094e-15,
                1.264338456987390e-15, 1.004347798555624e-15, 7.978311162761375e-16, 6.337897136092530e-16,
                5.034873162956860e-16, 3.999846069863884e-16, 3.177695668556620e-16, 2.524638826984041e-16,
                2.005897565348039e-16, 1.593846853472544e-16, 1.266543401716210e-16, 1.006557062235251e-16,
                8.000425902841744e-17, 6.360023242564495e-17, 5.057005148155293e-17, 4.021981116261989e-17,
                3.199832321658799e-17, 2.546776331071195e-17, 2.028035524788774e-17, 1.615985059334290e-17,
                1.288681742566722e-17, 1.028695477987395e-17, 8.221810481485991e-18, 6.581408061120865e-18,
                5.278390105156135e-18, 4.243366154135094e-18, 3.421217407310980e-18, 2.768161445242110e-18,
                2.249420656137898e-18, 1.837370201113151e-18, 1.510066890721238e-18, 1.250080630061784e-18,
                1.043566202644526e-18, 8.795259621097400e-19, 7.492241674473541e-19, 6.457217729274123e-19,
                5.635068986080232e-19, 4.982013026270218e-19, 4.463272238560322e-19, 4.051221784377176e-19,
                3.723918474462766e-19, 3.463932214025209e-19, 3.257417786627270e-19, 3.093377545919019e-19,
                2.963075750859589e-19, 2.859573355641487e-19, 2.777358480182294e-19, 2.712052882386572e-19,
                2.660178800750258e-19, 2.618973750817693e-19, 2.586243412714054e-19, 2.560244775456920e-19,
                2.539593315022365e-19, 2.523189263006598e-19, 2.510159039337331e-19, 2.499808729981372e-19,
                2.491587131957750e-19, 2.485056397337421e-19, 2.479868712390628e-19, 2.475747769129639e-19,
                2.472474041220591e-19, 2.469873078063680e-19, 2.467806190347303e-19
            };
            references[1] = new[]
            {
                1.101832532914486e-12, 1.020253230193121e-12, 9.929952548173648e-13, 1.018604943979157e-12,
                1.098441958732390e-12, 1.236749625666466e-12, 1.440876787782532e-12, 1.721660933077368e-12,
                2.093988925590930e-12, 2.577557075810235e-12, 3.197854860291897e-12, 3.987392558889523e-12,
                4.987174542006099e-12, 6.248371072384571e-12, 7.834031434867882e-12, 9.820452019717962e-12,
                1.229736153915473e-11, 1.536524524926090e-11, 1.912668338451432e-11, 2.366640647887245e-11,
                2.901240489602347e-11, 3.507047218951613e-11, 4.153391472507595e-11, 4.779958110319135e-11,
                5.297084398705647e-11, 5.605061300765190e-11, 5.632647597529333e-11, 5.373019247628322e-11,
                4.887531438615962e-11, 4.273855574886992e-11, 3.625653556498441e-11, 3.009145926495366e-11,
                2.459985331204318e-11, 1.990800234848295e-11, 1.600470036830737e-11, 1.281197788956573e-11,
                1.022843301800413e-11, 8.152046737396919e-12, 6.490451390928037e-12, 5.164410386114776e-12,
                4.107982887713966e-12, 3.267266582522150e-12, 2.598675415452822e-12, 2.067199850497022e-12,
                1.644835756571984e-12, 1.309240679081301e-12, 1.042618193670920e-12, 8.308074217267168e-13,
                6.625475993808368e-13, 5.288877753997852e-13, 4.227148424629126e-13, 3.383770965728897e-13,
                2.713844460468793e-13, 2.181698910811493e-13, 1.758998656630966e-13, 1.423234892106491e-13,
                1.156527739427722e-13, 9.446744565262983e-14, 7.763932783504480e-14, 6.427227167499369e-14,
                5.365443763032955e-14, 4.522038977757301e-14, 3.852098548440520e-14, 3.319945751360530e-14,
                2.897241513585387e-14, 2.561475263262926e-14, 2.294766157361505e-14, 2.082910849476789e-14,
                1.914627086303746e-14, 1.780952846972553e-14, 1.674769018959033e-14, 1.590420177688972e-14,
                1.523413253099298e-14, 1.470178001275520e-14, 1.427876470269168e-14, 1.394251226156354e-14,
                1.367504118754215e-14, 1.346198912774333e-14, 1.329182255520127e-14, 1.315518232604495e-14,
                1.304432183279279e-14, 1.295259477360910e-14, 1.287394532117294e-14, 1.280234382749476e-14,
                1.273109557296487e-14, 1.265192982980225e-14, 1.255375930538705e-14, 1.242100993671518e-14,
                1.223151914363468e-14, 1.195431702088460e-14, 1.154835357346553e-14
            };

            // Run test
            AnalyzeNoise(noise, ckt, exports, references);
            DestroyExports(exports);
        }

        [Test]
        public void When_MOS1OPExample_Expect_Convergence()
        {
            // Found by Marcin Golebiowski
            var ckt = new Circuit(
                new VoltageSource("V1", "g", "0", 10),
                new VoltageSource("V2", "d", "0", 10),
                CreateMOS1("Md", "d", "g", "0", "0", "my-nmos"),
                CreateMOS1Model("my-nmos", "is=1e-32")
            );

            // Create the simulation
            var op = new OP("op");
            var export = new RealPropertyExport(op, "my-nmos", "is");
            op.ExportSimulationData += (sender, args) =>
            {
                var current = export.Value;
            };

            op.Run(ckt);
            export.Destroy();
        }

        [Test]
        public void When_MOS1PMOSDifferentialPair_Expect_Reference()
        {
            var model = new Mosfet1Model("Model")
                .SetParameter("pmos", true)
                .SetParameter("vto", -0.7)
                .SetParameter("kp", 12.57e-4);

            var ckt = new Circuit(
                new NodeMapper("VDD", "B12", "B13", "CTRL", "B14", "TH"),
                model,
                new VoltageSource("Vsupply", "VDD", "0", 5.0),
                new CurrentSource("IBBIAS", "VDD", "B12", 10e-6),
                new Mosfet1("MB1", "B13", "CTRL", "B12", "VDD", "Model"),
                new Mosfet1("MB2", "B14", "TH", "B12", "VDD", "Model"),
                new Resistor("RBD1", "B13", "0", 10e3),
                new Resistor("RBD2", "B14", "0", 10e3),

                // These resistors seem to be necessary for convergence... Any higher and the operating point can't be calculated
                // new Resistor("Racc1", "B12", "B13", 1e6),
                // new Resistor("Racc2", "B12", "B14", 1e6),

                new VoltageSource("INA", "CTRL", "0", 3.333333333),
                new VoltageSource("INB", "TH", "0", 1.6631)
                );

            // Calculate the operating point
            var op = new OP("op");
            op.ExportSimulationData += (sender, args) =>
            {
                var v = op.States.Get<BiasingSimulationState>().Solution;
            };

            // Disable source stepping and see if it converges
            var config = op.Configurations.Get<BiasingConfiguration>();
            // config.SourceSteps = 0;

            op.Run(ckt);
        }
    }
}
