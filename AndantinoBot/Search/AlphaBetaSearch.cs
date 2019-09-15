using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AndantinoBot.Game;

namespace AndantinoBot.Search
{
    public class AlphaBetaSearch
    {
        public IAndantinoHeuristic Evaluator { get; set; }
        public int EvaluatedNodesCount = 0;
        public double AveragePruningSteps = 0;

        private int averageCounter = 0;

        public AlphaBetaSearch(IAndantinoHeuristic evaluator)
        {
            Evaluator = evaluator;
        }

        public HexCoordinate? GetBestPlay(Andantino state, int depth)
        {
            // int.MinValue = -2147483648 and int.MaxValue = 2147483647
            // When trying to multiply int.MinValue with a -1 we would get a overflow, so we have to reduce it by one
            var alpha = int.MinValue + 1;
            var beta = int.MaxValue;

            var maxValue = int.MinValue;
            HexCoordinate? bestPlay = null;
            var actions = state.GetValidPlacements();
            for(var i = 0; i < actions.Length; i++)
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
                if (maxValue >= beta)
                {
                    break;
                }
            }

            return bestPlay;
        }

        public int GetValue(Andantino state, int depth, int alpha, int beta)
        {
            if (state.Winner != Player.None || depth == 0)
                return Evaluator.Evaluate(state);

            var maxValue = int.MinValue;
            var actions = state.GetValidPlacements();
            var i = 0;
            for (; i < actions.Length; i++)
            {
                state.PlaceStone(actions[i]);
                var value = -GetValue(state, depth - 1, -beta, -alpha);
                state.UndoLastMove();

                if (value > maxValue)
                {
                    maxValue = value;
                }
                if(maxValue > alpha)
                {
                    alpha = maxValue;
                }
                if(maxValue >= beta)
                {
                    break;
                }
            }

            EvaluatedNodesCount += i;
            averageCounter++;
            AveragePruningSteps += (i + 1 - AveragePruningSteps) / averageCounter;
            return maxValue;
        }
    }
}
