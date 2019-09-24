using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AndantinoBot.Game;

namespace AndantinoBot.Search
{
    public class IterativeDeepening : IComparer<HexCoordinate>
    {
        public IAndantinoHeuristic Evaluator { get; set; }
        public IMoveOrderer MoveOrderer { get; set; }
        
        private readonly TranspositionTable transpositionTable;
        private Dictionary<Player, int[,]> historyTable;
        private Player activePlayer;

        // used for printing statistics
        private int evaluatedNodesCount = 0;
        private double averagePruningSteps = 0;
        private int averageCounter = 0;

        public IterativeDeepening(IAndantinoHeuristic evaluator, IMoveOrderer orderer, TranspositionTable transpositionTable)
        {
            Evaluator = evaluator;
            MoveOrderer = orderer;
            this.transpositionTable = transpositionTable;

            historyTable = new Dictionary<Player, int[,]>();
            historyTable.Add(Player.Black, new int[Andantino.BOARD_WIDTH, Andantino.BOARD_WIDTH]);
            historyTable.Add(Player.White, new int[Andantino.BOARD_WIDTH, Andantino.BOARD_WIDTH]);
        }

        public HexCoordinate GetBestPlay(Andantino state, long timeLimitMilliseconds)
        {
            var watch = new Stopwatch();
            watch.Start();

            HexCoordinate bestPlay = state.GetValidPlacements()[0];
            for (var i = 1; watch.ElapsedMilliseconds < timeLimitMilliseconds; i++)
            {
                bestPlay = GetBestPlay(state, i);
                Debug.WriteLine($"{i}. Time: {watch.ElapsedMilliseconds} Best Move: {bestPlay} Average Pruning: {averagePruningSteps:0.##} Evaluated Nodes: {evaluatedNodesCount}");
                evaluatedNodesCount = 0;
                averagePruningSteps = 0;
                averageCounter = 0;
            }

            return bestPlay;
        }

        public HexCoordinate GetBestPlay(Andantino state, int depth)
        {
            // int.MinValue = -2147483648 and int.MaxValue = 2147483647
            // When trying to multiply int.MinValue with a -1 we would get a overflow, so we have to reduce it by one
            var alpha = int.MinValue + 1;
            var beta = int.MaxValue;

            var maxValue = int.MinValue + 1;
            
            var actions = state.GetValidPlacements();
            activePlayer = state.ActivePlayer;
            Array.Sort(actions, this); // Sort according to our history table
            // actions = MoveOrderer.OrderMoves(state, actions);

            HexCoordinate bestPlay = actions[0];
            for (var i = 0; i < actions.Length; i++)
            {
                state.PlaceStone(actions[i]);
                var value = -GetValue(state, depth - 1, -beta, -alpha);
                state.UndoLastMove();

                if (value > maxValue)
                {
                    maxValue = value;
                    bestPlay = actions[i];
                }
                if (maxValue > alpha)
                {
                    alpha = maxValue;
                }
            }

            return bestPlay;
        }

        public int GetValue(Andantino state, int depth, int alpha, int beta)
        {
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
                bestValue = -GetValue(state, depth - 1, -beta, -alpha);
                state.UndoLastMove();
                if (bestValue > alpha)
                {
                    alpha = bestValue;
                }
                if (alpha >= beta)
                {
                    // We have a cutoff, update the history table
                    historyTable[state.ActivePlayer][bestMove.R + Andantino.BOARD_RADIUS, bestMove.Q + Andantino.BOARD_RADIUS] += depth * depth;
                    return bestValue;
                }
            }

            var actions = state.GetValidPlacements();
            activePlayer = state.ActivePlayer;
            Array.Sort(actions, this); // Sort according to our history table
            var i = 0;
            for (; i < actions.Length; i++)
            {
                var action = actions[i];
                if (action.Equals(entry.BestMove))
                    continue;

                state.PlaceStone(action);
                var value = -GetValue(state, depth - 1, -beta, -alpha);
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
                    // We have a cutoff, update the history table
                    historyTable[state.ActivePlayer][action.R + Andantino.BOARD_RADIUS, action.Q + Andantino.BOARD_RADIUS] += depth * depth;
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

        public int Compare(HexCoordinate x, HexCoordinate y)
        {
            return historyTable[activePlayer][y.R + Andantino.BOARD_RADIUS, y.Q + Andantino.BOARD_RADIUS] - historyTable[activePlayer][x.R + Andantino.BOARD_RADIUS, x.Q + Andantino.BOARD_RADIUS];
        }
    }
}
