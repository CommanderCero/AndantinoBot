﻿using AndantinoBot.Game;
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

        public AndantinoAgent(IAndantinoHeuristic heuristic)
        {
            AlphaBetaSearch = new AlphaBetaSearch(heuristic,  new TranspositionTable(30000000));
        }
        public HexCoordinate GetNextPlay(Andantino state)
        {
            var bestPlay = AlphaBetaSearch.GetBestPlay(state, 4000L);
            return bestPlay;
        }
    }
}