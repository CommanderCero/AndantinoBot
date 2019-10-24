using AndantinoBot.Game;
using AndantinoBot.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndantinoBot
{
    // TODO We do not need a serarch for the first 3 turns
    public class AndantinoAgent
    {
        public AlphaBetaSearch AlphaBetaSearch { get; set; }
        public IterativeDeepeningSearchResults PreviousSearchResults { get; set; }

        public AndantinoAgent(IAndantinoHeuristic heuristic)
        {
            AlphaBetaSearch = new AlphaBetaSearch(heuristic,  new TranspositionTable(20000000));
        }
        public HexCoordinate GetNextPlay(Andantino state)
        {
            // The first 2 turns do not matter, just return one action
            if (state.Turn <= 2)
                return state.GetValidPlacements()[0];

            PreviousSearchResults = AlphaBetaSearch.GetBestPlay(state, 4000L);
            return PreviousSearchResults.BestMove;
        }
    }
}