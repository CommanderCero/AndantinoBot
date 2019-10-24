using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndantinoBot.Search
{
    public class IterativeDeepeningSearchResults
    {
        public HexCoordinate BestMove => BestMoves[BestMoves.Count - 1];

        public List<int> BestMoveValues { get; set; }
        public List<HexCoordinate> BestMoves { get; set; }
        public List<double> AveragePrunings { get; set; }
        public List<int> EvaluatedNodes { get; set; }

        public IterativeDeepeningSearchResults()
        {
            BestMoveValues = new List<int>();
            BestMoves = new List<HexCoordinate>();
            AveragePrunings = new List<double>();
            EvaluatedNodes = new List<int>();
        }

        public void AddResults(HexCoordinate bestMove, int value, double averagePruning, int evaluatedNodes)
        {
            BestMoves.Add(bestMove);
            BestMoveValues.Add(value);
            AveragePrunings.Add(averagePruning);
            EvaluatedNodes.Add(evaluatedNodes);
        }
    }
}
