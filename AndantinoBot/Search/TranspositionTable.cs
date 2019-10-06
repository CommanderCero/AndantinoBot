using AndantinoBot.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndantinoBot.Search
{
    public enum TranpositionValueType
    {
        Exact,
        UpperBound,
        LowerBound
    }

    public readonly struct TranspositionEntry
    {
        public long Key { get; }
        public TranpositionValueType ValueType { get; }
        public int Value { get; }
        public int Depth { get; }
        public HexCoordinate BestMove { get; }

        public TranspositionEntry(long key, int value, TranpositionValueType valueType, int depth, HexCoordinate bestMove)
        {
            Key = key;
            Value = value;
            ValueType = valueType;
            Depth = depth;
            BestMove = bestMove;
        }
    }

    public class TranspositionTable
    {
        public int Size { get; }

        private static TranspositionEntry EmptyEntry = new TranspositionEntry(0, 0, TranpositionValueType.Exact, -1, new HexCoordinate());
        private readonly TranspositionEntry[,] entries;

        public TranspositionTable(int size)
        {
            Size = size;
            entries = new TranspositionEntry[size, 2];

            // ToDo is this bad for performance?
            // ToDo Can we avoid this loop somehow?
            for(var i = 0; i < Size; i++)
            {
                for(var x = 0; x < 2; x++)
                {
                    entries[i, x] = EmptyEntry;
                }
                
            }
        }

        public void Store(Andantino key, int value, TranpositionValueType type, int depth, HexCoordinate bestMove)
        {
            var hashValue = key.Board.GetLongHashCode();
            var hashIndex = hashValue % Size;

            if(entries[hashIndex, 0].Depth < depth)
            {
                entries[hashIndex, 0] = new TranspositionEntry(hashValue, value, type, depth, bestMove);
            }
            else
            {
                entries[hashIndex, 1] = new TranspositionEntry(hashValue, value, type, depth, bestMove);
            }
        }

        public TranspositionEntry GetEntry(Andantino key)
        {
            var hashValue = key.Board.GetLongHashCode();
            var hashIndex = hashValue % Size;

            if(entries[hashIndex, 0].Key == hashValue)
            {
                return entries[hashIndex, 0];
            }
            else if(entries[hashIndex, 1].Key == hashValue)
            {
                return entries[hashIndex, 1];
            }

            return EmptyEntry;
        }
    }
}
