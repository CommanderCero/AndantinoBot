using AndantinoBot.Game;
using AndantinoBot.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndantinoBot
{
    public class AndantinoAgent
    {
        public IterativeDeepening SearchAlgorithm { get; set; }

        public AndantinoAgent(IAndantinoHeuristic stateHeuristic, IMoveHeuristic moveHeuristic)
        {
            SearchAlgorithm = new IterativeDeepening(stateHeuristic, new MoveOrderer(moveHeuristic), new TranspositionTable(20000000));
        }

        public HexCoordinate GetNextPlay(Andantino state)
        {
            var bestPlay = SearchAlgorithm.GetBestPlay(state, 4000L);
            return bestPlay;
        }
    }
}