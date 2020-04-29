﻿using SpiceSharp.Simulations;
using SpiceSharp.Simulations.IntegrationMethods;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiceSharp.Components
{
    public partial class Pwl
    {
        /// <summary>
        /// An instance of the <see cref="IWaveform"/> interface for a <see cref="Pwl"/>.
        /// </summary>
        /// <seealso cref="IWaveform"/>
        protected class Instance : IWaveform
        {
            private readonly double[] _times, _values;
            private readonly IIntegrationMethod _method;
            private Line _line;
            private int _index;

            /// <summary>
            /// A description of a line segment.
            /// </summary>
            protected class Line
            {
                private readonly double _m, _q;

                /// <summary>
                /// Initializes a new instance of the <see cref="Line"/> class.
                /// </summary>
                /// <param name="x1">The x-coordinate of the first point.</param>
                /// <param name="y1">The y-coordinate of the first point.</param>
                /// <param name="x2">The x-coordinate of the second point.</param>
                /// <param name="y2">The y-coordinate of the second point.</param>
                public Line(double x1, double y1, double x2, double y2)
                {
                    _m = (y2 - y1) / (x2 - x1);
                    if (_m.Equals(0.0))
                        _q = y1;
                    else
                        _q = y1 - (_m * x1);
                }

                /// <summary>
                /// Interpolates the line at the specified x-coordinate.
                /// </summary>
                /// <param name="x">The x-coordinate.</param>
                /// <returns>The value at the specified coordinate.</returns>
                public double At(double x)
                    => _m * x + _q;
            }

            /// <inheritdoc/>
            public double Value { get; private set; }

            /// TODO: Work with a 2D point.
            /// <summary>
            /// Initializes a new instance of the <see cref="Instance"/> class.
            /// </summary>
            /// <param name="times">The times.</param>
            /// <param name="values">The values.</param>
            /// <param name="method">The integration method.</param>
            /// <exception cref="ArgumentException">Thrown if no points are specified, or if the time values are not monotonically increasing.</exception>
            public Instance(IEnumerable<double> times, IEnumerable<double> values, IIntegrationMethod method)
            {
                _method = method;
                _times = times.ThrowIfNull(nameof(times)).ToArray();
                _values = values.ThrowIfNull(nameof(values)).ToArray();
                if (_times.Length == 0 || _values.Length == 0)
                    throw new ArgumentException(Properties.Resources.Waveforms_Pwl_Empty);
                if (_times.Length != _values.Length)
                    throw new ArgumentException(Properties.Resources.Waveforms_Pwl_TimeValueLength);

                // Check monotonically increasing timepoints
                for (var i = 1; i < _times.Length; i++)
                {
                    if (_times[i - 1] >= _times[i])
                        throw new ArgumentException(Properties.Resources.Waveforms_Pwl_NoIncreasingTimeValues);
                }
                _index = 0;

                Probe();
            }

            /// <inheritdoc/>
            public void Probe()
            {
                var time = _method?.Time ?? 0.0;

                // Find the line segment
                // The line segment is likely to be very close to the current segment.
                while (_index > 1 && _times[_index - 1] > time)
                {
                    _line = null;
                    _index--;
                }
                while (_index < _times.Length && time >= _times[_index])
                {
                    _line = null;
                    _index++;
                }
                if (_line == null)
                {
                    if (_index == 0)
                        _line = new Line(
                            double.NegativeInfinity, _values[0],
                            _times[0], _values[0]);
                    else if (_index >= _times.Length)
                    {
                        _line = new Line(
                            double.NegativeInfinity, _values[_times.Length - 1],
                            double.PositiveInfinity, _values[_times.Length - 1]
                            );
                    }
                    else if (time > _times[_index])
                        _line = new Line(
                            _times[_index], _values[_index],
                            double.PositiveInfinity, _values[_index]);
                    else
                        _line = new Line(
                            _times[_index - 1], _values[_index - 1],
                            _times[_index], _values[_index]);
                }

                Value = _line.At(time);
            }

            /// <inheritdoc/>
            public void Accept()
            {
                if (_method is IBreakpointMethod breakpoints)
                {
                    if (breakpoints.Break)
                    {
                        // Add the next point as a breakpoint
                        if (_index < _times.Length && _times[_index] > _method.Time)
                            breakpoints.Breakpoints.SetBreakpoint(_times[_index]);
                    }
                }
            }
        }
    }
}
