﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SpiceSharp.Properties {
    using System;
    using System.Reflection;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SpiceSharp.Properties.Resources", typeof(Resources).GetTypeInfo().Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The order reduction cannot be negative..
        /// </summary>
        internal static string Algebra_InvalidOrder {
            get {
                return ResourceManager.GetString("Algebra.InvalidOrder", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The pivot is invalid..
        /// </summary>
        internal static string Algebra_InvalidPivot {
            get {
                return ResourceManager.GetString("Algebra.InvalidPivot", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The pivot search region cannot be negative..
        /// </summary>
        internal static string Algebra_InvalidPivotSearchReduction {
            get {
                return ResourceManager.GetString("Algebra.InvalidPivotSearchReduction", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Singular matrix encountered..
        /// </summary>
        internal static string Algebra_SingularMatrix {
            get {
                return ResourceManager.GetString("Algebra.SingularMatrix", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Singular matrix encountered at elimination step {0}..
        /// </summary>
        internal static string Algebra_SingularMatrixIndexed {
            get {
                return ResourceManager.GetString("Algebra.SingularMatrixIndexed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The solver has not been factored yet..
        /// </summary>
        internal static string Algebra_SolverNotFactored {
            get {
                return ResourceManager.GetString("Algebra.SolverNotFactored", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An ambiguous type reference for type &apos;{0}&apos; has been encountered..
        /// </summary>
        internal static string AmbiguousType {
            get {
                return ResourceManager.GetString("AmbiguousType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid parameter value for &apos;{0}&apos;..
        /// </summary>
        internal static string BadParameterNamed {
            get {
                return ResourceManager.GetString("BadParameterNamed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid parameter value &apos;{0}&apos;: {1}.
        /// </summary>
        internal static string BadParameterReason {
            get {
                return ResourceManager.GetString("BadParameterReason", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid parameter value for &apos;{0}&apos; ({1})..
        /// </summary>
        internal static string BadParameterValue {
            get {
                return ResourceManager.GetString("BadParameterValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid parameter value &apos;{0}&apos; ({1}): {2}.
        /// </summary>
        internal static string BadParameterValueReason {
            get {
                return ResourceManager.GetString("BadParameterValueReason", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There are already behaviors for &apos;{0}&apos;..
        /// </summary>
        internal static string Behaviors_BehaviorsAlreadyExist {
            get {
                return ResourceManager.GetString("Behaviors.BehaviorsAlreadyExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not find behaviors for &apos;{0}&apos;..
        /// </summary>
        internal static string Behaviors_NoBehaviorFor {
            get {
                return ResourceManager.GetString("Behaviors.NoBehaviorFor", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}: depletion capacitance coefficient too large, limited to {1:g}..
        /// </summary>
        internal static string BJTs_DepletionCapCoefficientTooLarge {
            get {
                return ResourceManager.GetString("BJTs.DepletionCapCoefficientTooLarge", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Node mismatch: {0} nodes expected, but {1} were given..
        /// </summary>
        internal static string Components_NodeMismatch {
            get {
                return ResourceManager.GetString("Components.NodeMismatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Node mismatch for &apos;{0}&apos;: {1} nodes expected, but {1} were given..
        /// </summary>
        internal static string Components_NodeMismatchNamed {
            get {
                return ResourceManager.GetString("Components.NodeMismatchNamed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No model was specified for component &apos;{0}&apos;..
        /// </summary>
        internal static string Components_NoModel {
            get {
                return ResourceManager.GetString("Components.NoModel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The absolute tolerance should be greater than 0..
        /// </summary>
        internal static string Delays_AbsoluteToleranceTooSmall {
            get {
                return ResourceManager.GetString("Delays.AbsoluteToleranceTooSmall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Non-causal delay detected. Delays should be greater than 0..
        /// </summary>
        internal static string Delays_NonCausalDelay {
            get {
                return ResourceManager.GetString("Delays.NonCausalDelay", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Time points are not monotonically increasing. Time goes from {0:e3} to {1:e3}..
        /// </summary>
        internal static string Delays_NonIncreasingTime {
            get {
                return ResourceManager.GetString("Delays.NonIncreasingTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The relative tolerance should be greater than 0..
        /// </summary>
        internal static string Delays_RelativeToleranceTooSmall {
            get {
                return ResourceManager.GetString("Delays.RelativeToleranceTooSmall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}: activation energy too small, limited to {1:g}..
        /// </summary>
        internal static string Diodes_ActivationEnergyTooSmall {
            get {
                return ResourceManager.GetString("Diodes.ActivationEnergyTooSmall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}: breakdown current increased to {1:g5} to resolve incompatibility with specified saturation current..
        /// </summary>
        internal static string Diodes_BreakdownCurrentIncreased {
            get {
                return ResourceManager.GetString("Diodes.BreakdownCurrentIncreased", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}: depletion capacitance coefficient too large, limited to {1}..
        /// </summary>
        internal static string Diodes_DepletionCapCoefficientTooLarge {
            get {
                return ResourceManager.GetString("Diodes.DepletionCapCoefficientTooLarge", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}: grading coefficient too large, limited to {1:g}..
        /// </summary>
        internal static string Diodes_GradingCoefficientTooLarge {
            get {
                return ResourceManager.GetString("Diodes.GradingCoefficientTooLarge", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}: unable to match forward and reverse diode region. Bv = {1:g}, ibv = {2:g}..
        /// </summary>
        internal static string Diodes_ImpossibleFwdRevMatch {
            get {
                return ResourceManager.GetString("Diodes.ImpossibleFwdRevMatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An entity with the name &quot;{0}&quot; already exists..
        /// </summary>
        internal static string EntityCollection_KeyExists {
            get {
                return ResourceManager.GetString("EntityCollection.KeyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} has no DC value, 0 assumed..
        /// </summary>
        internal static string IndependentSources_NoDc {
            get {
                return ResourceManager.GetString("IndependentSources.NoDc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} has no DC value, waveform value at time 0 used..
        /// </summary>
        internal static string IndependentSources_NoDcUseWaveform {
            get {
                return ResourceManager.GetString("IndependentSources.NoDcUseWaveform", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}: depletion capacitance coefficient too large, limited to {1:g}..
        /// </summary>
        internal static string JFETs_DepletionCapCoefficientTooLarge {
            get {
                return ResourceManager.GetString("JFETs.DepletionCapCoefficientTooLarge", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}: effective channel length less than zero..
        /// </summary>
        internal static string Mosfets_EffectiveChannelTooSmall {
            get {
                return ResourceManager.GetString("Mosfets.EffectiveChannelTooSmall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Nsub &lt; Ni..
        /// </summary>
        internal static string Mosfets_NsubTooSmall {
            get {
                return ResourceManager.GetString("Mosfets.NsubTooSmall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The oxide thickness is invalid..
        /// </summary>
        internal static string Mosfets_OxideThicknessTooSmall {
            get {
                return ResourceManager.GetString("Mosfets.OxideThicknessTooSmall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No enough elements..
        /// </summary>
        internal static string NotEnoughElements {
            get {
                return ResourceManager.GetString("NotEnoughElements", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not find parameter &apos;{0}&apos; for {1}..
        /// </summary>
        internal static string ParameterNotFound {
            get {
                return ResourceManager.GetString("ParameterNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A parameter set of type &apos;{0}&apos; could not be found..
        /// </summary>
        internal static string Parameters_ParameterSetNotFound {
            get {
                return ResourceManager.GetString("Parameters.ParameterSetNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The resistance {0} is too small, increased to {1}..
        /// </summary>
        internal static string Resistors_ResistanceTooSmall {
            get {
                return ResourceManager.GetString("Resistors.ResistanceTooSmall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}: resistance is 0, set to 1000..
        /// </summary>
        internal static string Resistors_ZeroResistance {
            get {
                return ResourceManager.GetString("Resistors.ZeroResistance", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Gmin step failed..
        /// </summary>
        internal static string Simulation_Biasing_GminSteppingFailed {
            get {
                return ResourceManager.GetString("Simulation.Biasing.GminSteppingFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The solution of variable &apos;{0}&apos; is not a number for the current iteration..
        /// </summary>
        internal static string Simulation_VariableNotANumber {
            get {
                return ResourceManager.GetString("Simulation.VariableNotANumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not determine the operating point..
        /// </summary>
        internal static string Simulations_Biasing_NoOp {
            get {
                return ResourceManager.GetString("Simulations.Biasing.NoOp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Source stepping failed..
        /// </summary>
        internal static string Simulations_Biasing_SourceSteppingFailed {
            get {
                return ResourceManager.GetString("Simulations.Biasing.SourceSteppingFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Starting diagonal Gmin stepping..
        /// </summary>
        internal static string Simulations_Biasing_StartDiagonalGminStepping {
            get {
                return ResourceManager.GetString("Simulations.Biasing.StartDiagonalGminStepping", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Starting Gmin stepping..
        /// </summary>
        internal static string Simulations_Biasing_StartGminStepping {
            get {
                return ResourceManager.GetString("Simulations.Biasing.StartGminStepping", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Starting source stepping..
        /// </summary>
        internal static string Simulations_Biasing_StartSourceStepping {
            get {
                return ResourceManager.GetString("Simulations.Biasing.StartSourceStepping", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not set convergence aid: variable &apos;{0}&apos; could not be found..
        /// </summary>
        internal static string Simulations_ConvergenceAidVariableNotFound {
            get {
                return ResourceManager.GetString("Simulations.ConvergenceAidVariableNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot find the source &apos;{0}&apos;..
        /// </summary>
        internal static string Simulations_DC_CannotFindSource {
            get {
                return ResourceManager.GetString("Simulations.DC.CannotFindSource", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot find DC parameter entity &apos;{0}&apos;..
        /// </summary>
        internal static string Simulations_DC_InvalidEntity {
            get {
                return ResourceManager.GetString("Simulations.DC.InvalidEntity", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The sweep values cannot be used to reach the stop value..
        /// </summary>
        internal static string Simulations_DC_InvalidSweep {
            get {
                return ResourceManager.GetString("Simulations.DC.InvalidSweep", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not find a sweeping parameter &apos;{0}&apos;..
        /// </summary>
        internal static string Simulations_DC_NoSource {
            get {
                return ResourceManager.GetString("Simulations.DC.NoSource", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The sweep &apos;{0}&apos; does not contain any points to simulate..
        /// </summary>
        internal static string Simulations_DC_NoSweepPoints {
            get {
                return ResourceManager.GetString("Simulations.DC.NoSweepPoints", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Histories cannot track less than one point..
        /// </summary>
        internal static string Simulations_History_InvalidLength {
            get {
                return ResourceManager.GetString("Simulations.History.InvalidLength", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid initialization mode..
        /// </summary>
        internal static string Simulations_InvalidInitializationMode {
            get {
                return ResourceManager.GetString("Simulations.InvalidInitializationMode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The number of iterations cannot be lower than 1..
        /// </summary>
        internal static string Simulations_IterationsTooSmall {
            get {
                return ResourceManager.GetString("Simulations.IterationsTooSmall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}: there are no entities..
        /// </summary>
        internal static string Simulations_NoEntities {
            get {
                return ResourceManager.GetString("Simulations.NoEntities", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}: there are no unknown variables to solve..
        /// </summary>
        internal static string Simulations_NoVariables {
            get {
                return ResourceManager.GetString("Simulations.NoVariables", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A simulation state of type {0} was not defined..
        /// </summary>
        internal static string Simulations_StateNotDefined {
            get {
                return ResourceManager.GetString("Simulations.StateNotDefined", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The expansion factor should be greater or equal than 1..
        /// </summary>
        internal static string Simulations_Time_MaximumExpansionTooSmall {
            get {
                return ResourceManager.GetString("Simulations.Time.MaximumExpansionTooSmall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The integration order is too small..
        /// </summary>
        internal static string Simulations_Time_OrderTooSmall {
            get {
                return ResourceManager.GetString("Simulations.Time.OrderTooSmall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}: transient simulation was terminated..
        /// </summary>
        internal static string Simulations_Time_Terminated {
            get {
                return ResourceManager.GetString("Simulations.Time.Terminated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The timestep is invalid..
        /// </summary>
        internal static string Simulations_Time_TimestepInvalid {
            get {
                return ResourceManager.GetString("Simulations.Time.TimestepInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The timestep {0:e5}s is too small at t={1:e5}s..
        /// </summary>
        internal static string Simulations_Time_TimestepTooSmall {
            get {
                return ResourceManager.GetString("Simulations.Time.TimestepTooSmall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A timepoint cannot be negative..
        /// </summary>
        internal static string Simulations_Time_TimeTooSmall {
            get {
                return ResourceManager.GetString("Simulations.Time.TimeTooSmall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The simulation &apos;{0}&apos; has {1} rule violations..
        /// </summary>
        internal static string Simulations_ValidationFailed {
            get {
                return ResourceManager.GetString("Simulations.ValidationFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Size mismatch of {0} and {1}..
        /// </summary>
        internal static string SizeMismatch1 {
            get {
                return ResourceManager.GetString("SizeMismatch1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Size mismatch of {0}: expected {1}..
        /// </summary>
        internal static string SizeMismatch2 {
            get {
                return ResourceManager.GetString("SizeMismatch2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot compute an equivalent solution for subcircuit &apos;{0}&apos;..
        /// </summary>
        internal static string Subcircuits_NoEquivalent {
            get {
                return ResourceManager.GetString("Subcircuits.NoEquivalent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot reach the final sweep point..
        /// </summary>
        internal static string Sweeps_CannotReachFinalPoint {
            get {
                return ResourceManager.GetString("Sweeps.CannotReachFinalPoint", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The number of points should be greater than 0..
        /// </summary>
        internal static string Sweeps_PointsTooSmall {
            get {
                return ResourceManager.GetString("Sweeps.PointsTooSmall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid characteristic impedance..
        /// </summary>
        internal static string TransmissionLines_ImpedanceTooSmall {
            get {
                return ResourceManager.GetString("TransmissionLines.ImpedanceTooSmall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A value of type {0} already exists..
        /// </summary>
        internal static string TypeAlreadyExists {
            get {
                return ResourceManager.GetString("TypeAlreadyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unexpected type encountered: expected {0}, but got {1}..
        /// </summary>
        internal static string UnexpectedType {
            get {
                return ResourceManager.GetString("UnexpectedType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid exponent. Cannot represent the exponent {0}/{1}..
        /// </summary>
        internal static string Units_InvalidExponent {
            get {
                return ResourceManager.GetString("Units.InvalidExponent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The units are not matched..
        /// </summary>
        internal static string Units_UnitsNotMatched {
            get {
                return ResourceManager.GetString("Units.UnitsNotMatched", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Floating node detected: {0}..
        /// </summary>
        internal static string Validation_FloatingNodeFound {
            get {
                return ResourceManager.GetString("Validation.FloatingNodeFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not enough nodes have been specified..
        /// </summary>
        internal static string Validation_NoFixedVoltageNodes {
            get {
                return ResourceManager.GetString("Validation.NoFixedVoltageNodes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There is no ground node in the circuit..
        /// </summary>
        internal static string Validation_NoGround {
            get {
                return ResourceManager.GetString("Validation.NoGround", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There is no independent source driving the circuit..
        /// </summary>
        internal static string Validation_NoIndependentSource {
            get {
                return ResourceManager.GetString("Validation.NoIndependentSource", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No variables set has been specified for validation..
        /// </summary>
        internal static string Validation_NoVariableSet {
            get {
                return ResourceManager.GetString("Validation.NoVariableSet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}: all pins are short-circuited..
        /// </summary>
        internal static string Validation_ShortCircuitComponent {
            get {
                return ResourceManager.GetString("Validation.ShortCircuitComponent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A fixed voltage was applied to a short-circuit by &apos;{0}&apos;..
        /// </summary>
        internal static string Validation_ShortCircuitFixedVoltage {
            get {
                return ResourceManager.GetString("Validation.ShortCircuitFixedVoltage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The source {0} closes a loop of voltage sources..
        /// </summary>
        internal static string Validation_VoltageLoopFound {
            get {
                return ResourceManager.GetString("Validation.VoltageLoopFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not find a value of type {0}..
        /// </summary>
        internal static string ValueNotFound {
            get {
                return ResourceManager.GetString("ValueNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A variable with id &apos;{0}&apos; already exists..
        /// </summary>
        internal static string VariableDictionary_KeyExists {
            get {
                return ResourceManager.GetString("VariableDictionary.KeyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The variable &apos;{0}&apos; is already mapped..
        /// </summary>
        internal static string VariableMap_KeyExists {
            get {
                return ResourceManager.GetString("VariableMap.KeyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A variable by the name of &apos;{0}&apos; could not be found..
        /// </summary>
        internal static string VariableNotFound {
            get {
                return ResourceManager.GetString("VariableNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Fall time should be greater or equal than 0..
        /// </summary>
        internal static string Waveforms_Pulse_FallTimeTooSmall {
            get {
                return ResourceManager.GetString("Waveforms.Pulse.FallTimeTooSmall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The period should be greater than 0..
        /// </summary>
        internal static string Waveforms_Pulse_PeriodTooSmall {
            get {
                return ResourceManager.GetString("Waveforms.Pulse.PeriodTooSmall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Pulse width should be greater or equal than 0..
        /// </summary>
        internal static string Waveforms_Pulse_PulseWidthTooSmall {
            get {
                return ResourceManager.GetString("Waveforms.Pulse.PulseWidthTooSmall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Rise time should be greater or equal than 0..
        /// </summary>
        internal static string Waveforms_Pulse_RiseTimeTooSmall {
            get {
                return ResourceManager.GetString("Waveforms.Pulse.RiseTimeTooSmall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No points are specified..
        /// </summary>
        internal static string Waveforms_Pwl_Empty {
            get {
                return ResourceManager.GetString("Waveforms.Pwl.Empty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The time values are not monotonically increasing..
        /// </summary>
        internal static string Waveforms_Pwl_NoIncreasingTimeValues {
            get {
                return ResourceManager.GetString("Waveforms.Pwl.NoIncreasingTimeValues", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The frequency should be greater than or equal to 0..
        /// </summary>
        internal static string Waveforms_Sine_FrequencyTooSmall {
            get {
                return ResourceManager.GetString("Waveforms.Sine.FrequencyTooSmall", resourceCulture);
            }
        }
    }
}
