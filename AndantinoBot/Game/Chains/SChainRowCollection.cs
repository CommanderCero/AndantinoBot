using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndantinoBot.Game
{
    public sealed class SChainRowCollection : ChainRowCollection
    {
        public override Direction Direction => Direction.S;

        public int QStart { get; }
        public int QEnd { get; }
        public int RStart { get; }
        public int REnd { get; }

        public SChainRowCollection(int row, int boardRadius)
            : base(row)
        {
            QStart = Math.Max(-boardRadius, -row - boardRadius);
            RStart = Math.Min(boardRadius, -row + boardRadius);
            QEnd = RStart;
            REnd = QStart;
        }

        public override int GetPosition(HexCoordinate c)
        {
            return c.Q;
        }

        public override bool Contains(int index, HexCoordinate c)
        {
            return c.R == -c.Q - row && positions[index * 2] <= c.Q && c.Q <= positions[index * 2 + 1];
        }

        public override bool GetChainStartExtension(int index, out int q, out int r)
        {
            q = positions[index * 2] - 1;
            r = -q - row;

            return q >= QStart;
        }

        public override bool GetChainEndExtension(int index, out int q, out int r)
        {
            q = positions[index * 2 + 1] + 1;
            r = -q - row;

            return q <= QEnd;
        }

        public override void GetChainStart(int index, out int q, out int r)
        {
            q = positions[index * 2];
            r = -q - row;
        }

        public override void GetChainEnd(int index, out int q, out int r)
        {
            q = positions[index * 2 + 1];
            r = -q - row;
        }
    }
}
