﻿using System;

namespace SpiceSharp.Algebra.Solve
{
    /// <summary>
    /// A template for a search strategy for finding pivots. It is
    /// used for implementing the strategy outlined by Markowitz.
    /// </summary>
    /// <typeparam name="T">The base value type.</typeparam>
    public abstract class MarkowitzSearchStrategy<T> where T : IFormattable
    {
        /// <summary>
        /// Find a pivot in a matrix.
        /// </summary>
        /// <param name="markowitz">The Markowitz pivot strategy.</param>
        /// <param name="matrix">The matrix</param>
        /// <param name="eliminationStep">The current elimination step.</param>
        /// <param name="max">The maximum row/column index.</param>
        /// <returns>The pivot element, or null if no pivot was found.</returns>
        public abstract Pivot<ISparseMatrixElement<T>> FindPivot(Markowitz<T> markowitz, ISparseMatrix<T> matrix, int eliminationStep, int max);
    }
}
