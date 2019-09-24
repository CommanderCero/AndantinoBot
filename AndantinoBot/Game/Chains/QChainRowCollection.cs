using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndantinoBot.Game
{
    public sealed class QChainRowCollection : ChainRowCollection
    {
        public override Direction Direction => Direction.Q;

        public int MinQ { get; }
        public int MaxQ { get; }

        public QChainRowCollection(int row, int boardRadius)
            : base(row)
        {
            MinQ = Math.Max(-boardRadius - row, -boardRadius);
            MaxQ = Math.Min(boardRadius - row, boardRadius);
        }

        public override int GetPosition(HexCoordinate c)
        {
            return c.Q;
        }

        public override bool Contains(int index, HexCoordinate c)
        {
            return c.R == row && positions[index * 2] <= c.Q && c.Q <= positions[index * 2 + 1];
        }

        public override bool GetChainStartExtension(int index, out int q, out int r)
        {
            q = positions[index * 2] - 1;
            r = row;

            return q >= MinQ;
        }

        public override bool GetChainEndExtension(int index, out int q, out int r)
        {
            q = positions[index * 2 + 1] + 1;
            r = row;

            return q <= MaxQ;
        }

        public override void GetChainStart(int index, out int q, out int r)
        {
            q = positions[index * 2];
            r = row;
        }

        public override void GetChainEnd(int index, out int q, out int r)
        {
            q = positions[index * 2 + 1];
            r = row;
        }
    }
}
