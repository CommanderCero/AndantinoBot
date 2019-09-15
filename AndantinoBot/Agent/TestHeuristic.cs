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
            var winningScore = 0;
            if (state.Winner == state.ActivePlayer)
                winningScore = 1000000;
            else if (state.Winner == state.ActivePlayer.GetOpponent())
                winningScore = -1000000;

            var whiteScore = 0;
            var blackScore = 0;
            QFirst(state, ref whiteScore, ref blackScore);
            RFirst(state, ref whiteScore, ref blackScore);
            SFirst(state, ref whiteScore, ref blackScore);

            var chainScore = state.ActivePlayer == Player.Black ? blackScore - whiteScore : whiteScore - blackScore;
            return winningScore + chainScore;
        }

        private void QFirst(Andantino state, ref int whiteScore, ref int blackScore)
        {
            for (var r = -9; r <= 9; r++)
            {
                var q_start = Math.Max(-9 - r, -9);
                var q_end = Math.Min(9 - r, 9);

                var startOwner = state.ActivePlayer.GetOpponent();
                var q = q_start;
                while(q <= q_end)
                {
                    var currentColor = state[q, r];
                    if (currentColor == Player.None)
                    {
                        startOwner = currentColor;
                        q++;
                        continue;
                    }

                    var count = 0;
                    while(q <= q_end && state[q, r] == currentColor)
                    {
                        count++;
                        q++;
                    }

                    // We found a useless chain, because we cannot add any stones to it
                    if(startOwner == currentColor.GetOpponent() && (q > q_end || state[q,r] != Player.None))
                    {
                        continue;
                    }

                    var score = count * count;
                    switch(currentColor)
                    {
                        case Player.White: whiteScore += score;break;
                        case Player.Black: blackScore += score;break;
                    }

                    startOwner = currentColor;
                }
            }
        }

        private void RFirst(Andantino state, ref int whiteScore, ref int blackScore)
        {
            for (var q = -9; q <= 9; q++)
            {
                int r_start = Math.Max(-9, -q - 9);
                int r_end = Math.Min(9, -q + 9);

                var startOwner = state.ActivePlayer.GetOpponent();
                var r = r_start;
                while (r <= r_end)
                {
                    var currentColor = state[q, r];
                    if (currentColor == Player.None)
                    {
                        startOwner = currentColor;
                        r++;
                        continue;
                    }

                    var count = 0;
                    while (r <= r_end && state[q, r] == currentColor)
                    {
                        count++;
                        r++;
                    }

                    // We found a useless chain, because we cannot add any stones to it
                    if (startOwner == currentColor.GetOpponent() && (q > r_end || state[q, r] != Player.None))
                    {
                        continue;
                    }

                    var score = count * count;
                    switch (currentColor)
                    {
                        case Player.White: whiteScore += score; break;
                        case Player.Black: blackScore += score; break;
                    }

                    startOwner = currentColor;
                }
            }
        }

        private void SFirst(Andantino state, ref int whiteScore, ref int blackScore)
        {
            for (int i = -9; i <= 9; i++)
            {
                var q_start = Math.Max(-9, i - 9);
                var r_start = Math.Min(9, i + 9);

                var x = 0;
                var startOwner = state.ActivePlayer.GetOpponent();
                while (x <= r_start - q_start)
                {
                    var currentColor = state[q_start + x, r_start - x];
                    if (currentColor == Player.None)
                    {
                        startOwner = currentColor;
                        x++;
                        continue;
                    }

                    var count = 0;
                    while (x <= r_start - q_start && state[q_start + x, r_start - x] == currentColor)
                    {
                        count++;
                        x++;
                    }

                    // We found a useless chain, because we cannot add any stones to it
                    if (startOwner == currentColor.GetOpponent() && (x > r_start - q_start || state[q_start + x, r_start - x] != Player.None))
                    {
                        continue;
                    }

                    var score = count * count;
                    switch (currentColor)
                    {
                        case Player.White: whiteScore += score; break;
                        case Player.Black: blackScore += score; break;
                    }

                    startOwner = currentColor;
                }
            }
        }
    }
}
