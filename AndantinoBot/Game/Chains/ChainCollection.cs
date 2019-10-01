using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndantinoBot.Game
{
    public class ChainCollection
    {
        public IEnumerable<ChainRowCollection> AllChains => QRows.Concat<ChainRowCollection>(RRows).Concat(SRows);
        public QChainRowCollection[] QRows { get; }
        public RChainRowCollection[] RRows { get; }
        public SChainRowCollection[] SRows { get; }

        public int BoardRadius { get; }

        public ChainCollection(int boardRadius)
        {
            BoardRadius = boardRadius;

            var boardWidth = boardRadius * 2 + 1;
            QRows = new QChainRowCollection[boardWidth];
            RRows = new RChainRowCollection[boardWidth];
            SRows = new SChainRowCollection[boardWidth];

            for (var i = -boardRadius; i <= boardRadius; i++)
            {
                QRows[i + boardRadius] = new QChainRowCollection(i, boardRadius);
                RRows[i + boardRadius] = new RChainRowCollection(i, boardRadius);
                SRows[i + boardRadius] = new SChainRowCollection(i, boardRadius);
            }
        }

        public void Add(HexCoordinate cord)
        {
            GetQChainsRow(cord).Add(cord);
            GetRChainsRow(cord).Add(cord);
            GetSChainsRow(cord).Add(cord);
        }

        public void Remove(HexCoordinate cord)
        {
            GetQChainsRow(cord).Remove(cord);
            GetRChainsRow(cord).Remove(cord);
            GetSChainsRow(cord).Remove(cord);
        }

        public ChainRowCollection GetChainRow(Direction d, HexCoordinate c)
        {
            switch(d)
            {
                case Direction.Q: return GetQChainsRow(c);
                case Direction.R: return GetRChainsRow(c);
                case Direction.S: return GetSChainsRow(c);
            }

            return null;
        }

        public ChainRowCollection GetQChainsRow(HexCoordinate row)
        {
            return QRows[row.R + BoardRadius];
        }

        public ChainRowCollection GetRChainsRow(HexCoordinate row)
        {
            return RRows[row.Q + BoardRadius];
        }

        public ChainRowCollection GetSChainsRow(HexCoordinate row)
        {
            return SRows[row.S + BoardRadius];
        }
    }
}
