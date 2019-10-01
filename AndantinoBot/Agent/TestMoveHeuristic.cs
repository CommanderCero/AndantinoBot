using AndantinoBot.Game;
using AndantinoBot.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndantinoBot.Agent
{
    public class TestMoveHeuristic : IMoveHeuristic
    {
        public int Evaluate(HexCoordinate move, Andantino game)
        {
            var score = CalculateScore(move, game, Direction.Q);
            score += CalculateScore(move, game, Direction.R);
            score += CalculateScore(move, game, Direction.S);
            return score;
        }

        private int CalculateScore(HexCoordinate move, Andantino game, Direction d)
        {
            var hexDir = d.ToHex();
            for(var dir = -1; dir <= 1; dir+=2)
            {
                var neighbor = move + hexDir * dir;
                if (!game.IsValidCoordinate(neighbor))
                    continue;

                if(game[neighbor] == Player.None && game.CountNeighborStones(neighbor) == 1)
                {
                    // With this move we made a chain extendable
                    var directions = (Direction[])Enum.GetValues(typeof(Direction));
                    for(var i = 0; i < directions.Length; i++)
                    {
                        var d2 = directions[i];
                        var hexDir2 = d2.ToHex();
                        for (var dir2 = -1; dir2 <= 1; dir2 += 2)
                        {
                            var neighbor2 = neighbor + hexDir2 * dir2;
                            if (!game.IsValidCoordinate(neighbor2))
                                continue;

                            var chainColor = game[neighbor2];
                            if(chainColor != Player.None)
                            {
                                var chainCollection = game.GetPlayerChains(chainColor);
                                var chainRow = chainCollection.GetChainRow(d2, neighbor2);

                                var chainIndex = chainRow.FindChainIndex(neighbor2);
                                var chainLength = chainRow.GetChainLength(chainIndex);
                                if (chainLength == 4 && game.ActivePlayer.GetOpponent() == chainColor)
                                    return -100000;
                            }
                        }
                    }
                }
                else if(game[neighbor] != Player.None)
                {
                    // We blocked the chain of an opposing player or extended our own chain
                    var chainCollection = game.GetPlayerChains(game[neighbor]);
                    var chainRow = chainCollection.GetChainRow(d, neighbor);

                    var chainIndex = chainRow.FindChainIndex(neighbor);
                    if(chainIndex != -1)
                    {
                        var chainLength = chainRow.GetChainLength(chainIndex);
                        if (chainLength == 4)
                            return 100000;
                    }
                }
            }

            return 0;
        }
    }
}
