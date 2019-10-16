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

        public AndantinoAgent(IAndantinoHeuristic heuristic)
        {
            AlphaBetaSearch = new AlphaBetaSearch(heuristic,  new TranspositionTable(30000000));
        }

        static HexCoordinate[] moves = new HexCoordinate[]
        {
            new HexCoordinate(0,2)  ,
            new HexCoordinate(2,-2) ,
            new HexCoordinate(0,3)  ,
            new HexCoordinate(-5,2) ,
            new HexCoordinate(-1,4) ,
            new HexCoordinate(-2,5) ,
            new HexCoordinate(-3,5) ,
            new HexCoordinate(-5,3) ,
            new HexCoordinate(-2,-4),
            new HexCoordinate(-3,-3),
            new HexCoordinate(-2,-3),
            new HexCoordinate(-3,1) ,
            new HexCoordinate(1,2)  ,
            new HexCoordinate(4,-4) ,
            new HexCoordinate(2,-3) ,
            new HexCoordinate(3,-3) ,
            new HexCoordinate(-4,0) ,
            new HexCoordinate(-4,2) ,
            new HexCoordinate(-2,4) ,
            new HexCoordinate(4,-3) ,
            new HexCoordinate(4,-2) ,
            new HexCoordinate(-4,-1),
            new HexCoordinate(1,-4) ,
            new HexCoordinate(3,1)  ,
            new HexCoordinate(0,-3) ,
            new HexCoordinate(-4,3) ,
            new HexCoordinate(-3,4) ,
            new HexCoordinate(-3,-2),
            new HexCoordinate(4,-1) ,
            new HexCoordinate(-3,3) ,
            new HexCoordinate(-2,3) ,
            new HexCoordinate(-1,-3),
            new HexCoordinate(-3,-1),
            new HexCoordinate(2,1)  ,
            new HexCoordinate(3,-2) ,
            new HexCoordinate(-3,2) ,
            new HexCoordinate(-1,2) ,
            new HexCoordinate(3,0)  ,
            new HexCoordinate(3,-1) ,
            new HexCoordinate(-2,-2),
            new HexCoordinate(-2,2) ,
            new HexCoordinate(-3,0) ,
            new HexCoordinate(1,-3) ,
            new HexCoordinate(-2,-1),
            new HexCoordinate(-1,-2),
            new HexCoordinate(-2,0) ,
            new HexCoordinate(1,1)  ,
            new HexCoordinate(-2,1) ,
            new HexCoordinate(-1,-1),
            new HexCoordinate(0,-2) ,
            new HexCoordinate(-1,1) ,
            new HexCoordinate(2,0)  ,
            new HexCoordinate(2,-1) ,
            new HexCoordinate(1,-2) ,
            new HexCoordinate(1,0)  ,
            new HexCoordinate(1,-1) ,
            new HexCoordinate(0,-1) ,
            new HexCoordinate(-1,0)

        };

        public HexCoordinate GetNextPlay(Andantino state)
        {
            return moves.Reverse().ToList()[state.TurnCount];
            //var bestPlay = AlphaBetaSearch.GetBestPlay(state, 4000L);
            //return bestPlay;
        }
    }
}