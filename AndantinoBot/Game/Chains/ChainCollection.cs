using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndantinoBot.Game
{
    public class ChainCollection
    {
        public IEnumerable<ChainRowCollection> AllChains => QRowChains.Concat<ChainRowCollection>(RRowChains).Concat(SRowChains);
        public QChainRowCollection[] QRowChains { get; }
        public RChainRowCollection[] RRowChains { get; }
        public SChainRowCollection[] SRowChains { get; }

        public int BoardRadius { get; }

        public ChainCollection(int boardRadius)
        {
            BoardRadius = boardRadius;

            var boardWidth = boardRadius * 2 + 1;
            QRowChains = new QChainRowCollection[boardWidth];
            RRowChains = new RChainRowCollection[boardWidth];
            SRowChains = new SChainRowCollection[boardWidth];

            for (var i = -boardRadius; i <= boardRadius; i++)
            {
                QRowChains[i + boardRadius] = new QChainRowCollection(i, boardRadius);
                RRowChains[i + boardRadius] = new RChainRowCollection(i, boardRadius);
                SRowChains[i + boardRadius] = new SChainRowCollection(i, boardRadius);
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

        public ChainRowCollection GetQChainsRow(HexCoordinate row)
        {
            return QRowChains[row.R + BoardRadius];
        }

        public ChainRowCollection GetRChainsRow(HexCoordinate row)
        {
            return RRowChains[row.Q + BoardRadius];
        }

        public ChainRowCollection GetSChainsRow(HexCoordinate row)
        {
            return SRowChains[row.S + BoardRadius];
        }
    }
}
