using AndantinoBot.Game;
using AndantinoBot.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndantinoBot.Agent
{
    public class TestMoveOrderer : IMoveOrderer
    {
        public HexCoordinate[] OrderMoves(Andantino state, HexCoordinate[] moves)
        {
            return moves;
        }
    }
}
