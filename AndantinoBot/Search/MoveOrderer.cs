using AndantinoBot.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndantinoBot.Search
{
    public class MoveOrderer
    {
        public IMoveHeuristic Heuristic { get; set; }

        public MoveOrderer(IMoveHeuristic heuristic)
        {
            Heuristic = heuristic;
        }

        public void OrderMoves(HexCoordinate[] moves, Andantino game)
        {
            var scores = new int[moves.Length];
            for(var i = 0; i < moves.Length; i++)
            {
                scores[i] = Heuristic.Evaluate(moves[i], game);
            }

            // Sort the moves with insertion sort
            for(var i = 1; i < moves.Length; i++)
            {
                var insertionValue = scores[i];
                var insertionMove = moves[i];
                var j = i;
                for(; j > 0 && scores[j - 1] < insertionValue; j--)
                {
                    scores[j] = scores[j - 1];
                    moves[j] = moves[j - 1];
                }

                scores[j] = insertionValue;
                moves[j] = insertionMove;
            }
        }
    }
}
