using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AndantinoBot.Game;

namespace AndantinoBot.Search
{
    public class AlphaBetaSearch
    {
        public IAndantinoHeuristic Evaluator { get; set; }
        
        private readonly TranspositionTable transpositionTable;

        // used for printing statistics
        private int evaluatedNodesCount = 0;
        private double averagePruningSteps = 0;
        private int averageCounter = 0;

        public AlphaBetaSearch(IAndantinoHeuristic evaluator, TranspositionTable transpositionTable)
        {
            Evaluator = evaluator;
            this.transpositionTable = transpositionTable;
        }

        public IterativeDeepeningSearchResults GetBestPlay(Andantino state, long millisecondsTimelimit)
        {
            var searchResults = new IterativeDeepeningSearchResults();
            var actions = state.GetValidPlacements();
            var globalBestIndex = 0;

            var watch = new Stopwatch();
            watch.Start();
            for (var depth = 1; watch.ElapsedMilliseconds < millisecondsTimelimit; depth++)
            {
                // int.MinValue = -2147483648 and int.MaxValue = 2147483647
                // When trying to multiply int.MinValue with a -1 we would get a overflow, so we have to reduce it by one
                var alpha = int.MinValue + 1;
                var beta = int.MaxValue;

                // First execute the best move from the last iteration
                int localBestIndex = globalBestIndex;
                state.PlaceStone(actions[globalBestIndex]);
                var localMaxValue = -GetValue(state, depth - 1, -beta, -alpha, watch, millisecondsTimelimit);
                state.UndoLastMove();
                if (localMaxValue > alpha)
                {
                    alpha = localMaxValue;
                }

                // Calculate the values of the remaining moves, to check if there is a better move
                for (var i = 0; i < actions.Length && watch.ElapsedMilliseconds < millisecondsTimelimit; i++)
                {
                    if (i == globalBestIndex)
                        continue;

                    state.PlaceStone(actions[i]);
                    var value = -GetValue(state, depth - 1, -beta, -alpha, watch, millisecondsTimelimit);
                    state.UndoLastMove();

                    if (value > localMaxValue)
                    {
                        localMaxValue = value;
                        localBestIndex = i;
                    }
                    if (localMaxValue > alpha)
                    {
                        alpha = localMaxValue;
                    }
                }

                // Print debug informations
                globalBestIndex = localBestIndex;
                Debug.WriteLine($"{depth}. Time: {watch.ElapsedMilliseconds} Best Move: {actions[globalBestIndex]} Value: {localMaxValue} Average Pruning: {averagePruningSteps:0.##} Evaluated Nodes: {evaluatedNodesCount}");

                // Collect data for the search results
                searchResults.AddResults(actions[globalBestIndex], localMaxValue, averagePruningSteps, evaluatedNodesCount);

                // Reset counters
                evaluatedNodesCount = 0;
                averagePruningSteps = 0;
                averageCounter = 0;
            }

            Debug.WriteLine($"Aborted search after {watch.ElapsedMilliseconds}ms. returning best play {actions[globalBestIndex]}");
            return searchResults;
        }

        public int GetValue(Andantino state, int depth, int alpha, int beta, Stopwatch timer, long millisecondsTimelimit)
        {
            // Did we reach the time limit?
            if (timer.ElapsedMilliseconds > millisecondsTimelimit)
                return -1;

            // Check entry in transposition table
            var entry = transpositionTable.GetEntry(state);
            if(entry.Depth >= depth)
            {
                switch(entry.ValueType)
                {
                    case TranpositionValueType.Exact: return entry.Value;
                    case TranpositionValueType.LowerBound: alpha = Math.Max(entry.Value, alpha); break;
                    case TranpositionValueType.UpperBound: beta = Math.Min(entry.Value, beta); break;
                }

                if(alpha >= beta)
                {
                    return entry.Value;
                }
            }

            if (state.Winner != Player.None || depth == 0)
                return Evaluator.Evaluate(state);

            var oldAlpha = alpha;
            var bestValue = int.MinValue + 1;
            HexCoordinate bestMove = entry.BestMove;
            // First try the move from the transposition table
            if (entry.Depth != -1)
            {
                state.PlaceStone(bestMove);
                bestValue = -GetValue(state, depth - 1, -beta, -alpha, timer, millisecondsTimelimit);
                state.UndoLastMove();
                if (bestValue > alpha)
                {
                    alpha = bestValue;
                }
                if (alpha >= beta)
                {
                    return bestValue;
                }
            }

            var actions = state.GetValidPlacements();
            var i = 0;
            for (; i < actions.Length; i++)
            {
                var action = actions[i];
                if (action.Equals(entry.BestMove))
                    continue;

                state.PlaceStone(action);
                var value = -GetValue(state, depth - 1, -beta, -alpha, timer, millisecondsTimelimit);
                state.UndoLastMove();

                if (value > bestValue)
                {
                    bestValue = value;
                    bestMove = action;
                }
                if(bestValue > alpha)
                {
                    alpha = bestValue;
                }
                if(alpha >= beta)
                {
                    break;
                }
            }

            // store in transposition table
            var valueType = TranpositionValueType.Exact;
            if(bestValue <= oldAlpha)
            {
                valueType = TranpositionValueType.UpperBound;
            }
            else if(bestValue >= beta)
            {
                valueType = TranpositionValueType.LowerBound;
            }
            transpositionTable.Store(state, bestValue, valueType, depth, bestMove);

            // Calculate some statistics
            evaluatedNodesCount += i;
            averageCounter++;
            averagePruningSteps += (i + 1 - averagePruningSteps) / averageCounter;

            return bestValue;
        }
    }
}
