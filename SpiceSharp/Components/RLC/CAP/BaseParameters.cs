using SpiceSharp.Attributes;
using System;

namespace SpiceSharp.Components.CapacitorBehaviors
{
    /// <summary>
    /// Base parameters for a capacitor
    /// </summary>
    [GeneratedParameters]
    public class BaseParameters : ParameterSet
    {
        private GivenParameter<double> _temperature = new GivenParameter<double>(Constants.ReferenceTemperature, false);
        private double _parallelMultiplier = 1.0;
        private GivenParameter<double> _length = new GivenParameter<double>();
        private GivenParameter<double> _width = new GivenParameter<double>();
        private GivenParameter<double> _capacitance = new GivenParameter<double>();

        /// <summary>
        /// Gets the capacitance parameter.
        /// </summary>
        [ParameterName("capacitance"), ParameterInfo("Device capacitance", Units = "F", IsPrincipal = true)]
        [GreaterThanOrEquals(0)]
        public GivenParameter<double> Capacitance
        {
            get => _capacitance;
            set
            {
                if (value < 0)
                    throw new ArgumentException(Properties.Resources.Parameters_TooSmall.FormatString(nameof(Capacitance), value, 0));
                _capacitance = value;
            }
        }

        /// <summary>
        /// Gets the initial voltage parameter.
        /// </summary>
        [ParameterName("ic"), ParameterInfo("Initial capacitor voltage", Units = "V", Interesting = false)]
        public double InitialCondition { get; set; }

        /// <summary>
        /// Gets the width parameter.
        /// </summary>
        [ParameterName("w"), ParameterInfo("Device width", Units = "m", Interesting = false)]
        [GreaterThanOrEquals(0)]
        public GivenParameter<double> Width
        {
            get => _width;
            set
            {
                if (value < 0)
                    throw new ArgumentException(Properties.Resources.Parameters_TooSmall.FormatString(nameof(Width), value, 0));
                _width = value;
            }
        }

        /// <summary>
        /// Gets the length parameter.
        /// </summary>
        [ParameterName("l"), ParameterInfo("Device length", Units = "m", Interesting = false)]
        [GreaterThanOrEquals(0)]
        public GivenParameter<double> Length
        {
            get => _length;
            set
            {
                if (value < 0)
                    throw new ArgumentException(Properties.Resources.Parameters_TooSmall.FormatString(nameof(Length), value, 0));
                _length = value;
            }
        }

        /// <summary>
        /// Gets or sets the parallel multiplier.
        /// </summary>
        [ParameterName("m"), ParameterInfo("Parallel multiplier")]
        [GreaterThanOrEquals(0)]
        public double ParallelMultiplier
        {
            get => _parallelMultiplier;
            set
            {
                if (value < 0)
                    throw new ArgumentException(Properties.Resources.Parameters_TooSmall.FormatString(nameof(ParallelMultiplier), value, 0));
                _parallelMultiplier = value;
            }
        }

        /// <summary>
        /// Gets or sets the temperature in degrees Celsius.
        /// </summary>
        [ParameterName("temp"), ParameterInfo("Instance operating temperature", Units = "\u00b0C", Interesting = false)]
        [DerivedProperty(), GreaterThan(Constants.CelsiusKelvin)]
        public double TemperatureCelsius
        {
            get => Temperature - Constants.CelsiusKelvin;
            set => Temperature = value + Constants.CelsiusKelvin;
        }

        /// <summary>
        /// Gets the temperature parameter (in degrees Kelvin).
        /// </summary>
        [GreaterThan(0)]
        public GivenParameter<double> Temperature
        {
            get => _temperature;
            set
            {
                if (value <= 0)
                    throw new ArgumentException(Properties.Resources.Parameters_TooSmall.FormatString(nameof(Temperature), value, 0));
                _temperature = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseParameters"/> class.
        /// </summary>
        public BaseParameters()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseParameters"/> class.
        /// </summary>
        /// <param name="cap">Capacitance</param>
        public BaseParameters(double cap)
        {
            Capacitance = cap;
        }
    }
}
