using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndantinoBot.Game
{
    public class RChainRowCollection : ChainRowCollection
    {
        public override Direction Direction => Direction.R;

        public int MinR { get; }
        public int MaxR { get; }

        public RChainRowCollection(int row, int boardRadius)
            : base(row)
        {
            MinR = Math.Max(-boardRadius, -row - boardRadius);
            MaxR = Math.Min(boardRadius, -row + boardRadius);
        }

        public override int GetPosition(HexCoordinate c)
        {
            return c.R;
        }

        public override bool Contains(int index, HexCoordinate c)
        {
            return c.Q == row && positions[index * 2] <= c.R && c.R <= positions[index * 2 + 1];
        }

        public override bool GetChainStartExtension(int index, out int q, out int r)
        {
            q = row;
            r = positions[index * 2] - 1;

            return r >= MinR;
        }

        public override bool GetChainEndExtension(int index, out int q, out int r)
        {
            q = row;
            r = positions[index * 2 + 1] + 1;

            return r <= MaxR;
        }

        public override void GetChainStart(int index, out int q, out int r)
        {
            q = row;
            r = positions[index * 2];
        }

        public override void GetChainEnd(int index, out int q, out int r)
        {
            q = row;
            r = positions[index * 2 + 1];
        }
    }
}
