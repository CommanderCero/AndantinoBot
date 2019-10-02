using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AndantinoBot.Game;

namespace AndantinoBot.Search
{
    public class AlphaBetaSearch
    {
        public IAndantinoHeuristic Evaluator { get; set; }
        public IMoveOrderer MoveOrderer { get; set; }
        
        private readonly TranspositionTable transpositionTable;

        // used for printing statistics
        private int evaluatedNodesCount = 0;
        private double averagePruningSteps = 0;
        private int averageCounter = 0;

        public AlphaBetaSearch(IAndantinoHeuristic evaluator, IMoveOrderer orderer, TranspositionTable transpositionTable)
        {
            Evaluator = evaluator;
            MoveOrderer = orderer;
            this.transpositionTable = transpositionTable;
        }

        public HexCoordinate GetBestPlay(Andantino state, long millisecondsTimelimit)
        {
            var actions = state.GetValidPlacements();
            var globalBestPlay = actions[0];
            var depth = 1;

            var watch = new Stopwatch();
            watch.Start();
            while (true)
            {
                // int.MinValue = -2147483648 and int.MaxValue = 2147483647
                // When trying to multiply int.MinValue with a -1 we would get a overflow, so we have to reduce it by one
                var alpha = int.MinValue + 1;
                var beta = int.MaxValue;
                var localMaxValue = int.MinValue + 1;
                HexCoordinate localBestPlay = actions[0];
                for (var i = 0; i < actions.Length && watch.ElapsedMilliseconds < millisecondsTimelimit; i++)
                {
                    state.PlaceStone(actions[i]);
                    var value = -GetValue(state, depth - 1, -beta, -alpha, watch, millisecondsTimelimit);
                    state.UndoLastMove();

                    if (value > localMaxValue)
                    {
                        localMaxValue = value;
                        localBestPlay = actions[i];
                    }
                    if (localMaxValue > alpha)
                    {
                        alpha = localMaxValue;
                    }
                }

                if (watch.ElapsedMilliseconds > millisecondsTimelimit)
                {
                    Debug.WriteLine($"{depth}: Aborted search after {watch.ElapsedMilliseconds}ms. returning best play from depth {depth - 1}");
                    return globalBestPlay;
                }
                    

                globalBestPlay = localBestPlay;
                Debug.WriteLine($"{depth}. Time: {watch.ElapsedMilliseconds} Best Move: {globalBestPlay} Average Pruning: {averagePruningSteps:0.##} Evaluated Nodes: {evaluatedNodesCount}");
                evaluatedNodesCount = 0;
                averagePruningSteps = 0;
                averageCounter = 0;
                depth++;
            }
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
