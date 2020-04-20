﻿using SpiceSharp.Algebra;
using SpiceSharp.Simulations;
using SpiceSharp.Simulations.Variables;
using System;
using System.Collections.Generic;

namespace SpiceSharp.Components.SubcircuitBehaviors
{
    /// <summary>
    /// A simulation state that has a local solver.
    /// </summary>
    /// <typeparam name="T">The solver value type.</typeparam>
    /// <typeparam name="S">The parent simulation state type.</typeparam>
    /// <seealso cref="ISolverSimulationState{T}" />
    public abstract partial class LocalSolverState<T, S> : SubcircuitSolverState<T, S>
        where S : ISolverSimulationState<T>
        where T : IFormattable
    {
        private bool _shouldPreorder, _shouldReorder;
        private readonly List<Bridge<Element<T>>> _elements = new List<Bridge<Element<T>>>();
        private readonly List<Bridge<int>> _indices = new List<Bridge<int>>();
        private readonly VariableMap _map;

        /// <summary>
        /// The local solution vector.
        /// </summary>
        protected IVector<T> LocalSolution;

        /// <summary>
        /// Gets the solver used to solve the system of equations.
        /// </summary>
        /// <value>
        /// The solver.
        /// </value>
        public override ISparsePivotingSolver<T> Solver { get; }

        /// <summary>
        /// Gets the solution.
        /// </summary>
        /// <value>
        /// The solution.
        /// </value>
        public override IVector<T> Solution => LocalSolution;

        /// <summary>
        /// Gets the map that maps variables to indices for the solver.
        /// </summary>
        /// <value>
        /// The map.
        /// </value>
        public override IVariableMap Map => _map;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="LocalSolverState{T,S}"/> has updated its solution.
        /// </summary>
        /// <value>
        ///   <c>true</c> if updated; otherwise, <c>false</c>.
        /// </value>
        protected bool Updated { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalSolverState{T,S}"/> class.
        /// </summary>
        /// <param name="name">The name of the subcircuit instance.</param>
        /// <param name="nodes">The nodes.</param>
        /// <param name="parent">The parent simulation state.</param>
        /// <param name="solver">The local solver.</param>
        protected LocalSolverState(string name, S parent, ISparsePivotingSolver<T> solver)
            : base(name, parent)
        {
            Solver = solver.ThrowIfNull(nameof(solver));

            // We will keep our own map! We will use the parent's variable set though, to make sure our parent can find back
            // our solved variables.
            _map = new VariableMap(parent.Map[0]);
        }

        /// <summary>
        /// Initializes the specified shared.
        /// </summary>
        /// <param name="nodes">The node map.</param>
        public virtual void Initialize(IReadOnlyList<Bridge<string>> nodes)
        {
            LocalSolution = new DenseVector<T>(Solver.Size);
            _shouldPreorder = true;
            _shouldReorder = true;
            Updated = true;
            ReorderLocalSolver(nodes);
        }

        /// <summary>
        /// Reorders the local solver.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        private void ReorderLocalSolver(IReadOnlyList<Bridge<string>> nodes)
        {
            // Map each local node on the global node
            _indices.Clear();
            foreach (var bridge in nodes)
            {
                var loc = GetSharedVariable(bridge.Local);
                var glob = Parent.GetSharedVariable(bridge.Global);
                _indices.Add(new Bridge<int>(_map[loc], Parent.Map[glob]));
            }

            Solver.Precondition((matrix, vector) =>
            {
                for (var i = 0; i < _indices.Count; i++)
                {
                    // Let's move these variables to the back of the matrix
                    int index = _indices[i].Local;
                    var location = Solver.ExternalToInternal(new MatrixLocation(index, index));
                    int target = matrix.Size - i;
                    matrix.SwapColumns(location.Column, target);
                    matrix.SwapRows(location.Row, target);
                }
            });
            Solver.Degeneracy = _indices.Count;
            Solver.PivotSearchReduction = _indices.Count;

            // Get the elements that need to be shared
            Solver.Precondition((m, v) =>
            {
                var matrix = (ISparseMatrix<T>)m;
                var vector = (ISparseVector<T>)v;
                foreach (var row in _indices)
                {
                    LinkElement(vector, row);
                    foreach (var col in _indices)
                        LinkElement(matrix, row, col);
                }
            });
        }

        /// <summary>
        /// Links vector elements.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <param name="row">The row variable.</param>
        private void LinkElement(ISparseVector<T> vector, Bridge<int> row)
        {
            var loc = Solver.ExternalToInternal(new MatrixLocation(row.Local, row.Local));

            // Do we need to create an element?
            var local_elt = vector.FindElement(loc.Row);
            if (local_elt == null)
            {
                // Check if solving will result in an element
                var first = vector.GetFirstInVector();
                if (first == null || first.Index > Solver.Size - Solver.Degeneracy)
                    return;
                local_elt = vector.GetElement(loc.Row);
            }
            if (local_elt == null)
                return;
            var parent_elt = Parent.Solver.GetElement(row.Global);
            _elements.Add(new Bridge<Element<T>>(local_elt, parent_elt));
        }

        /// <summary>
        /// Links matrix elements.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="row">The row variable.</param>
        /// <param name="column">The column variable.</param>
        private void LinkElement(ISparseMatrix<T> matrix, Bridge<int> row, Bridge<int> column)
        {
            var loc = Solver.ExternalToInternal(new MatrixLocation(row.Local, column.Local));

            // Do we need to create an element?
            var local_elt = matrix.FindElement(loc.Row, loc.Column);
            if (local_elt == null)
            {
                // Check if solving will result in an element
                var left = matrix.GetFirstInRow(loc.Row);
                if (left == null || left.Column > Solver.Size - Solver.Degeneracy)
                    return;

                var top = matrix.GetFirstInColumn(loc.Column);
                if (top == null || top.Row > Solver.Size - Solver.Degeneracy)
                    return;

                // Create the element because decomposition will cause these elements to be created
                local_elt = matrix.GetElement(loc.Row, loc.Column);
            }
            if (local_elt == null)
                return;
            var parent_elt = Parent.Solver.GetElement(row.Global, column.Global);
            _elements.Add(new Bridge<Element<T>>(local_elt, parent_elt));
        }

        /// <summary>
        /// Applies the local solver to the parent solver.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the application was successful; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="NoEquivalentSubcircuitException">Thrown if no equivalent contributions could be calculated.</exception>
        public virtual bool Apply()
        {
            if (_shouldPreorder)
            {
                Solver.Precondition((matrix, vector) =>
                    ModifiedNodalAnalysisHelper<T>.PreorderModifiedNodalAnalysis(matrix, Solver.Size - Solver.PivotSearchReduction));
                _shouldPreorder = false;
            }
            if (_shouldReorder)
            {
                if (Solver.OrderAndFactor() < Solver.Size - Solver.Degeneracy)
                    throw new NoEquivalentSubcircuitException();
                _shouldReorder = false;
            }
            else
            {
                if (!Solver.Factor())
                {
                    _shouldReorder = true;
                    return false;
                }
            }

            // Copy the necessary elements
            foreach (var bridge in _elements)
                bridge.Global.Add(bridge.Local.Value);
            Updated = false;
            return true;
        }

        /// <summary>
        /// Updates the state with the new solution.
        /// </summary>
        public virtual void Update()
        {
            // No need to update again
            if (Updated)
                return;

            // Fill in the shared variables
            foreach (var pair in _indices)
                Solution[pair.Local] = Parent.Solution[pair.Global];
            Solver.Solve(Solution);
            Solution[0] = default;
            Updated = true;
        }

        /// <summary>
        /// Maps a shared node in the simulation.
        /// </summary>
        /// <param name="name">The name of the shared node.</param>
        /// <returns>
        /// The shared node variable.
        /// </returns>
        public override IVariable<T> GetSharedVariable(string name)
        {
            // Get the local node!
            if (!TryGetValue(name, out var result))
            {
                var index = _map.Count;
                result = new SolverVariable<T>(this, name, index, Units.Volt);
                Add(name, result);
                _map.Add(result, index);
            }
            return result;
        }

        /// <summary>
        /// Creates a local variable that should not be shared by the state with anyone else.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="unit">The unit of the variable.</param>
        /// <returns>
        /// The local variable.
        /// </returns>
        public override IVariable<T> CreatePrivateVariable(string name, IUnit unit)
        {
            var index = _map.Count;
            var result = new SolverVariable<T>(this, name, index, unit);
            _map.Add(result, index);
            return result;
        }
    }
}
