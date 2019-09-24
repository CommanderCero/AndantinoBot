using AndantinoBot.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndantinoBot.Search
{
    // ToDo this is a crap name, who the fuck came up with this
    public interface IMoveOrderer
    {
        HexCoordinate[] OrderMoves(Andantino state, HexCoordinate[] moves);
    }
}
