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
    public class TestHeuristic : IAndantinoHeuristic
    {
        public int Evaluate(Andantino state)
        {
            // Calculate winning score, should get lower them ore turns the agent needs to win
            if (state.Winner == state.ActivePlayer)
                return 1000000 - state.TurnCount;
            else if (state.Winner == state.ActivePlayer.GetOpponent())
                return -1000000 + state.TurnCount;

            var whiteScore = CalculateChainsScore(state, state.WhiteChains.QRowChains);
            whiteScore += CalculateChainsScore(state, state.WhiteChains.RRowChains);
            whiteScore += CalculateChainsScore(state, state.WhiteChains.SRowChains);

            var blackScore = CalculateChainsScore(state, state.BlackChains.QRowChains);
            blackScore += CalculateChainsScore(state, state.BlackChains.RRowChains);
            blackScore += CalculateChainsScore(state, state.BlackChains.SRowChains);

            var chainScore = state.ActivePlayer == Player.Black ? blackScore - whiteScore : whiteScore - blackScore;
            return chainScore;
        }

        private int CalculateChainsScore(Andantino state, ChainRowCollection[] rows)
        {
            var score = 0;
            for(var row = 0; row < rows.Length; row++)
            {
                var chainRow = rows[row];
                for(var i = 0; i < chainRow.ChainCount; i++)
                {
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
