using AndantinoBot.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndantinoBot.Search
{
    public interface IMoveHeuristic
    {
        int Evaluate(HexCoordinate move, Andantino game);
    }
}
