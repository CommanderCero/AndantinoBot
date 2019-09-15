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
        public IterativeDeepening AlphaBetaSearch { get; set; }

        public AndantinoAgent(IAndantinoHeuristic heuristic)
        {
            // 2^k * sizeof(TranspositionEntry) = MemorySize
            // <=> MemorySize / sizeof(TranspositionEntry)
            AlphaBetaSearch = new IterativeDeepening(heuristic, new TranspositionTable(20000000));
        }

        public HexCoordinate GetNextPlay(Andantino state)
        {
            var bestPlay = AlphaBetaSearch.GetBestPlay(state, 4000L);
            return bestPlay;
        }
    }
}