using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndantinoBot.Game
{
    public abstract class ChainRowCollection
    {
        public abstract Direction Direction { get; }
        public int ChainCount => positions.Count / 2;

        protected int row;
        protected List<int> positions;

        public ChainRowCollection(int row)
        {
            this.row = row;
            positions = new List<int>();
        }

        public abstract int GetPosition(HexCoordinate c);

        public abstract bool GetChainStartExtension(int index, out int q, out int r);

        public abstract bool GetChainEndExtension(int index, out int q, out int r);

        public abstract void GetChainStart(int index, out int q, out int r);

        public abstract void GetChainEnd(int index, out int q, out int r);

        public abstract bool Contains(int index, HexCoordinate c);

        public int FindChainIndex(HexCoordinate c)
        {
            if (positions.Count == 0)
                return -1;

            var position = GetPosition(c);
            if (position == positions[0])
                return 0;
            if (position == positions[positions.Count - 1])
                return positions.Count / 2 - 1;

            for(var i = 2; i < positions.Count; i += 2)
            {
                if (positions[i - 1] == position)
                    return i / 2 - 1;
                if (positions[i] == position)
                    return i / 2;
            }

            return -1;
        }

        public void Add(HexCoordinate c)
        {
            var newPosition = GetPosition(c);
            var inserted = false;
            for (var i = 0; i < positions.Count; i += 2)
            {
                if (newPosition == positions[i] - 1) // Check if we can add the position to the beginning of the chain
                {
                    positions[i] = newPosition;
                }
                else if (newPosition < positions[i]) // Check if the position is a new chain
                {
                    positions.Insert(i, newPosition);
                    positions.Insert(i, newPosition);
                }
                else if (newPosition == positions[i + 1] + 1) // Check if we can add the position to the end of a chain
                {
                    // We found two chains that are now connected
                    if (i + 2 < positions.Count && positions[i + 1] + 1 == positions[i + 2] - 1)
                    {
                        positions.RemoveRange(i + 1, 2);

                    }
                    else
                    {
                        positions[i + 1] = newPosition;
                    }
                }
                else
                {
                    continue;
                }

                inserted = true;
                break;
            }

            if (!inserted)
            {
                positions.Add(newPosition);
                positions.Add(newPosition);
            }
        }

        public void Remove(HexCoordinate c)
        {
            var position = GetPosition(c);
            for (var i = 0; i < positions.Count; i += 2)
            {
                if (position == positions[i]) // Check if we the removed position is at the beginning of the chain
                {
                    if (positions[i + 1] == positions[i]) // We found a chain of length 1
                    {
                        positions.RemoveRange(i, 2);
                    }
                    else
                    {
                        positions[i] = position + 1;
                    }

                }
                else if (position == positions[i + 1]) // Check if the removed position is at the end of the chain
                {
                    positions[i + 1] = position - 1;
                }
                else if (positions[i] < position && position < positions[i + 1] ) // The removed position is in the middle of the chain
                {
                    positions.Insert(i + 1, position + 1);
                    positions.Insert(i + 1, position - 1);
                }
                else
                {
                    continue;
                }

                break;
            }
        }

        public int GetChainLength(int index)
        {
            return positions[index * 2 + 1] - positions[index * 2] + 1;
        }
    }
}
