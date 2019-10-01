using AndantinoBot.Game;
using AndantinoBot.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AndantinoBot.Agent
{
    // TODO Featuregewichtung mit TD-Learning approximieren (TD-Leaf(lambda))
    // Good things checklist
    // 1. Many Long chains
    // 2. Add random factor?
    public class ChainLengthHeuristic : IAndantinoHeuristic
    {
        public int Evaluate(Andantino state)
        {
            // Calculate winning score, should get lower the more turns the agent needs to win
            if (state.Winner == state.ActivePlayer)
                return 1000000 - state.TurnCount;
            else if (state.Winner == state.ActivePlayer.GetOpponent())
                return -1000000 + state.TurnCount;

            var whiteScore = CalculateChainsScore(state, state.WhiteChains.QRows);
            whiteScore += CalculateChainsScore(state, state.WhiteChains.RRows);
            whiteScore += CalculateChainsScore(state, state.WhiteChains.SRows);

            var blackScore = CalculateChainsScore(state, state.BlackChains.QRows);
            blackScore += CalculateChainsScore(state, state.BlackChains.RRows);
            blackScore += CalculateChainsScore(state, state.BlackChains.SRows);

            var chainScore = state.ActivePlayer == Player.Black ? blackScore - whiteScore : whiteScore - blackScore;
            return chainScore;
        }

        public static int counter = 0;

        private int CalculateChainsScore(Andantino state, ChainRowCollection[] rows)
        {
            var score = 0;
            for(var row = 0; row < rows.Length; row++)
            {
                var chainRow = rows[row];
                for(var i = 0; i < chainRow.ChainCount; i++)
                {
                    counter += 2;
                    var isStartExtendable = chainRow.GetChainStartExtension(i, out var qStart, out var rStart) && state[qStart, rStart] == Player.None;
                    var isEndExtendable = chainRow.GetChainEndExtension(i, out var qEnd, out var rEnd) && state[qEnd, rEnd] == Player.None;

                    // We found a useless chain, because we cannot add any stones to it
                    if (!isStartExtendable && !isEndExtendable)
                    {
                        continue;
                    }

                    var length = chainRow.GetChainLength(i);
                    score += length * length;
                }
            }

            return score;
        }
    }
}
