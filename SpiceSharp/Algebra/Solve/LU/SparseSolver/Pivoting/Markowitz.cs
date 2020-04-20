﻿using System;
using System.Collections.ObjectModel;
using SpiceSharp.Attributes;

namespace SpiceSharp.Algebra.Solve
{
    /// <summary>
    /// A search strategy based on methods outlined by Markowitz.
    /// </summary>
    /// <typeparam name="T">The base value type.</typeparam>
    /// <seealso cref="SparsePivotStrategy{T}" />
    public class Markowitz<T> : SparsePivotStrategy<T> where T : IFormattable
    {
        /// <summary>
        /// Markowitz numbers
        /// </summary>
        private int[] _markowitzRow;
        private int[] _markowitzColumn;
        private int[] _markowitzProduct;

        /// <summary>
        /// The maximum Markowitz count that will not result in Int32 overflow when squared
        /// Markowitz counts are capped at this quantity.
        /// </summary>
        /// <remarks>
        /// To reach this quantity, a variable would have to be connected to this amount of
        /// other varibles. We could say that this is highly unlikely. In the event that this
        /// amount does get reached, we would probably need to do a sanity check.
        /// </remarks>
        private const int _maxMarkowitzCount = 46340;

        /// <summary>
        /// Gets the Markowitz row counts.
        /// </summary>
        public int RowCount(int row) => _markowitzRow[row];

        /// <summary>
        /// Gets the Markowitz column counts.
        /// </summary>
        public int ColumnCount(int column) => _markowitzColumn[column];

        /// <summary>
        /// Gets the Markowitz products.
        /// </summary>
        public int Product(int index) => _markowitzProduct[index];

        /// <summary>
        /// Gets the number of singletons.
        /// </summary>
        public int Singletons { get; private set; }

        /// <summary>
        /// Gets or sets the relative threshold for choosing a pivot.
        /// </summary>
        [ParameterName("pivrel"), ParameterInfo("The relative threshold for validating pivots")]
        public double RelativePivotThreshold { get; set; } = 1e-3;

        /// <summary>
        /// Gets or sets the absolute threshold for choosing a pivot.
        /// </summary>
        [ParameterName("pivtol"), ParameterInfo("The absolute threshold for validating pivots")]
        public double AbsolutePivotThreshold { get; set; } = 1e-13;

        /// <summary>
        /// Gets the strategies used for finding a pivot.
        /// </summary>
        public Collection<MarkowitzSearchStrategy<T>> Strategies { get; } = new Collection<MarkowitzSearchStrategy<T>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Markowitz{T}"/> class.
        /// </summary>
        public Markowitz(Func<T, double> magnitude)
            : base(magnitude)
        {
            // Register default strategies
            Strategies.Add(new MarkowitzSingleton<T>());
            Strategies.Add(new MarkowitzQuickDiagonal<T>());
            Strategies.Add(new MarkowitzDiagonal<T>());
            Strategies.Add(new MarkowitzEntireMatrix<T>());
        }

        /// <summary>
        /// This method will check whether or not a pivot element is valid or not.
        /// It checks for the submatrix right/below of the pivot.
        /// </summary>
        /// <param name="pivot">The pivot candidate.</param>
        /// <returns>
        /// True if the pivot can be used.
        /// </returns>
        public override bool IsValidPivot(ISparseMatrixElement<T> pivot)
        {
            pivot.ThrowIfNull(nameof(pivot));

            // Get the magnitude of the current pivot
            var magnitude = Magnitude(pivot.Value);

            // Search for the largest element below the pivot
            var element = pivot.Below;
            var largest = 0.0;
            while (element != null && element.Row <= _markowitzRow.Length - 1 - PivotSearchReduction)
            {
                largest = Math.Max(largest, Magnitude(element.Value));
                element = element.Below;
            }

            // Check the validity
            if (largest * RelativePivotThreshold < magnitude)
                return true;
            return false;
        }

        /// <summary>
        /// Initializes the pivot searching algorithm.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        public void Initialize(IMatrix<T> matrix)
        {
            matrix.ThrowIfNull(nameof(matrix));

            // Allocate arrays
            _markowitzRow = new int[matrix.Size + 1];
            _markowitzColumn = new int[matrix.Size + 1];
            _markowitzProduct = new int[matrix.Size + 2];
        }

        /// <summary>
        /// Clears the pivot strategy.
        /// </summary>
        public override void Clear()
        {
            base.Clear();
            _markowitzRow = null;
            _markowitzColumn = null;
            _markowitzProduct = null;
        }

        /// <summary>
        /// Count the Markowitz numbers.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="rhs">The right-hand side vector.</param>
        /// <param name="step">The elimination step.</param>
        private void Count(ISparseMatrix<T> matrix, ISparseVector<T> rhs, int step)
        {
            ISparseMatrixElement<T> element;
            var limit = matrix.Size - PivotSearchReduction;

            // Get the first element in the vector
            var rhsElement = rhs.GetFirstInVector();

            // Generate Markowitz row count
            for (var i = limit; i >= step; i--)
            {
                // Set count to -1 initially to remove count due to pivot element
                var count = -1;
                element = matrix.GetFirstInRow(i);
                while (element != null && element.Column < step)
                    element = element.Right;
                while (element != null) // We want to count the elements outside the limit as well
                {
                    count++;
                    element = element.Right;
                }

                // Include elements on the Rhs vector
                while (rhsElement != null && rhsElement.Index < step)
                    rhsElement = rhsElement.Below;
                if (rhsElement != null && rhsElement.Index == i)
                    count++;
                
                _markowitzRow[i] = Math.Min(count, _maxMarkowitzCount);
            }
            
            // Generate Markowitz column count
            for (var i = step; i <= limit; i++)
            {
                // Set count to -1 initially to remove count due to pivot element
                var count = -1;
                element = matrix.GetFirstInColumn(i);
                while (element != null && element.Row < step)
                    element = element.Below;
                while (element != null)
                {
                    count++;
                    element = element.Below;
                }
                _markowitzColumn[i] = Math.Min(count, _maxMarkowitzCount);
            }
        }

        /// <summary>
        /// Calculate the Markowitz products.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="step">The elimination step.</param>
        private void Products(ISparseMatrix<T> matrix, int step)
        {
            var limit = matrix.Size - PivotSearchReduction;

            Singletons = 0;
            for (var i = step; i <= limit; i++)
            {
                // UpdateMarkowitzProduct(i);
                _markowitzProduct[i] = _markowitzRow[i] * _markowitzColumn[i];
                if (_markowitzProduct[i] == 0)
                    Singletons++;
            }
        }

        /// <summary>
        /// Setup the pivot strategy.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="rhs">The right-hand side vector.</param>
        /// <param name="eliminationStep">The current elimination step.</param>
        public override void Setup(ISparseMatrix<T> matrix, ISparseVector<T> rhs, int eliminationStep)
        {
            matrix.ThrowIfNull(nameof(matrix));
            rhs.ThrowIfNull(nameof(rhs));

            // Initialize Markowitz row, column and product vectors if necessary
            if (_markowitzRow == null || _markowitzRow.Length != matrix.Size + 1)
                Initialize(matrix);
            Count(matrix, rhs, eliminationStep);
            Products(matrix, eliminationStep);
        }

        /// <summary>
        /// Move the pivot to the diagonal for this elimination step.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="rhs">The right-hand side vector.</param>
        /// <param name="pivot">The pivot element.</param>
        /// <param name="eliminationStep">The elimination step.</param>
        /// <remarks>
        /// This is done by swapping the rows and columns of the diagonal and that of the pivot.
        /// </remarks>
        public override void MovePivot(ISparseMatrix<T> matrix, ISparseVector<T> rhs, ISparseMatrixElement<T> pivot, int eliminationStep)
        {
            matrix.ThrowIfNull(nameof(matrix));
            rhs.ThrowIfNull(nameof(rhs));
            pivot.ThrowIfNull(nameof(pivot));

            // If we haven't setup, just skip
            if (_markowitzProduct == null)
                return;
            int oldProduct;

            var row = pivot.Row;
            var column = pivot.Column;

            // If the pivot is a singleton, then we just consumed it
            if (_markowitzProduct[pivot.Row] == 0 || _markowitzProduct[pivot.Column] == 0)
                Singletons--;

            // Exchange rows
            if (pivot.Row != eliminationStep)
            {
                // Swap row Markowitz numbers
                var tmp = _markowitzRow[row];
                _markowitzRow[row] = _markowitzRow[eliminationStep];
                _markowitzRow[eliminationStep] = tmp;

                // Update the Markowitz product
                oldProduct = _markowitzProduct[row];
                _markowitzProduct[row] = _markowitzRow[row] * _markowitzColumn[row];
                if (oldProduct == 0)
                {
                    if (_markowitzProduct[row] != 0)
                        Singletons--;
                }
                else
                {
                    if (_markowitzProduct[row] == 0)
                        Singletons++;
                }
            }

            // Exchange columns
            if (column != eliminationStep)
            {
                // Swap column Markowitz numbers
                var tmp = _markowitzColumn[column];
                _markowitzColumn[column] = _markowitzColumn[eliminationStep];
                _markowitzColumn[eliminationStep] = tmp;

                // Update the Markowitz product
                oldProduct = _markowitzProduct[column];
                _markowitzProduct[column] = _markowitzRow[column] * _markowitzColumn[column];
                if (oldProduct == 0)
                {
                    if (_markowitzProduct[column] != 0)
                        Singletons--;
                }
                else
                {
                    if (_markowitzProduct[column] == 0)
                        Singletons++;
                }
            }

            // Also update the moved pivot
            oldProduct = _markowitzProduct[eliminationStep];
            _markowitzProduct[eliminationStep] = _markowitzRow[eliminationStep] * _markowitzColumn[eliminationStep];
            if (oldProduct == 0)
            {
                if (_markowitzProduct[eliminationStep] != 0)
                    Singletons--;
            }
            else
            {
                if (_markowitzProduct[eliminationStep] == 0)
                    Singletons++;
            }
        }

        /// <summary>
        /// Update the strategy after the pivot was moved.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="pivot">The pivot element.</param>
        /// <param name="eliminationStep">The elimination step.</param>
        public override void Update(ISparseMatrix<T> matrix, ISparseMatrixElement<T> pivot, int eliminationStep)
        {
            matrix.ThrowIfNull(nameof(matrix));
            pivot.ThrowIfNull(nameof(pivot));
            var limit = matrix.Size - PivotSearchReduction;

            // If we haven't setup, just skip
            if (_markowitzProduct == null)
                return;

            // Go through all elements below the pivot. If they exist, then we can subtract 1 from the Markowitz row vector!
            for (var column = pivot.Below; column != null && column.Row <= limit; column = column.Below)
            {
                var row = column.Row;
                
                // Update the Markowitz product
                _markowitzProduct[row] -= _markowitzColumn[row];
                --_markowitzRow[row];

                // If we reached 0, then the row just turned to a singleton row
                if (_markowitzRow[row] == 0)
                    Singletons++;
            }

            // go through all elements right of the pivot. For every element, we can subtract 1 from the Markowitz column vector!
            for (var row = pivot.Right; row != null && row.Column <= limit; row = row.Right)
            {
                var column = row.Column;
                
                // Update the Markowitz product
                _markowitzProduct[column] -= _markowitzRow[column];
                --_markowitzColumn[column];

                // If we reached 0, then the column just turned to a singleton column
                // This only adds a singleton if the row wasn't detected as a singleton row first
                if (_markowitzColumn[column] == 0 && _markowitzRow[column] != 0)
                    Singletons++;
            }
        }

        /// <summary>
        /// Notifies the strategy that a fill-in has been created
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="fillin">The fill-in.</param>
        public override void CreateFillin(ISparseMatrix<T> matrix, ISparseMatrixElement<T> fillin)
        {
            matrix.ThrowIfNull(nameof(matrix));
            fillin.ThrowIfNull(nameof(fillin));

            if (_markowitzProduct == null)
                return;

            // Update the markowitz row count
            int index = fillin.Row;
            _markowitzRow[index]++;
            _markowitzProduct[index] =
                Math.Min(_markowitzRow[index] * _markowitzColumn[index], _maxMarkowitzCount);
            if (_markowitzRow[index] == 1 && _markowitzColumn[index] != 0)
                Singletons--;

            // Update the markowitz column count
            index = fillin.Column;
            _markowitzColumn[index]++;
            _markowitzProduct[index] =
                Math.Min(_markowitzRow[index] * _markowitzColumn[index], _maxMarkowitzCount);
            if (_markowitzRow[index] != 0 && _markowitzColumn[index] == 1)
                Singletons--;
        }

        /// <summary>
        /// Find a pivot in the matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="eliminationStep">The current elimination step.</param>
        /// <returns></returns>
        /// <remarks>
        /// The pivot should be searched for in the submatrix towards the right and down of the
        /// current diagonal at row/column <paramref name="eliminationStep" />. This pivot element
        /// will be moved to the diagonal for this elimination step.
        /// </remarks>
        public override Pivot<T> FindPivot(ISparseMatrix<T> matrix, int eliminationStep)
        {
            matrix.ThrowIfNull(nameof(matrix));

            // Fix the search limit to allow our strategies to work
            foreach (var strategy in Strategies)
            {
                var chosen = strategy.FindPivot(this, matrix, eliminationStep);
                if (chosen.Info != PivotInfo.None)
                    return chosen;
            }
            return Pivot<T>.Empty;
        }

        #if DEBUG
        /// <summary>
        /// Checks whether or not the Markowitz products are still correct. Can be used when debugging matrix decomposition.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="step">The current step.</param>
        public void CheckMarkowitzCounts(ISparseMatrix<T> matrix, int step)
        {
            if (_markowitzProduct == null)
                return;
            var limit = matrix.Size - PivotSearchReduction;

            // Recalculate the rows and columns completely and check with the current ones
            int singletons = 0;
            for (var i = step; i <= limit; i++)
            {
                // Count the elements in the row
                int count = -1;
                var e = matrix.GetLastInRow(i);
                while (e != null && e.Column >= step)
                {
                    count++;
                    e = e.Left;
                }
                if (_markowitzRow[i] != count && _markowitzRow[i] != count + 1)
                    throw new SpiceSharpException("Invalid row count");

                // Count the elements in the column
                count = -1;
                e = matrix.GetLastInColumn(i);
                while (e != null && e.Row >= step)
                {
                    count++;
                    e = e.Above;
                }
                if (_markowitzColumn[i] != count)
                    throw new SpiceSharpException("Invalid column count");

                if (_markowitzProduct[i] != Math.Min(_markowitzRow[i] * _markowitzColumn[i], _maxMarkowitzCount))
                    throw new SpiceSharpException("Invalid product");
                if (_markowitzProduct[i] == 0)
                    singletons++;
            }

            if (singletons != Singletons)
                throw new SpiceSharpException("Invalid singleton count");
        }
        #endif
    }
}
