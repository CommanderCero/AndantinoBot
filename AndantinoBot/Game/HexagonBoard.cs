using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndantinoBot.Game
{
    public class HexagonBoard
    {
        public const int BOARD_RADIUS = 9;
        public const int BOARD_WIDTH = BOARD_RADIUS * 2 + 1;

        private readonly Player[,] map;
        private long currentHashcode;

        private static readonly HexCoordinate[,][] neighborLookup;
        private static Dictionary<Player, long[,]> randomNumbers;

        public Player this[HexCoordinate cord]
        {
            get { return map[cord.R + BOARD_RADIUS, cord.Q + BOARD_RADIUS]; }
            private set { map[cord.R + BOARD_RADIUS, cord.Q + BOARD_RADIUS] = value; }
        }

        public Player this[int q, int r]
        {
            get { return map[r + BOARD_RADIUS, q + BOARD_RADIUS]; }
            private set { map[r + BOARD_RADIUS, q + BOARD_RADIUS] = value; }
        }

        static HexagonBoard()
        {
            randomNumbers = new Dictionary<Player, long[,]>();
            neighborLookup = new HexCoordinate[BOARD_WIDTH, BOARD_WIDTH][];

            var blackRandomNumbers = new long[BOARD_WIDTH, BOARD_WIDTH];
            var whiteRandomNumbers = new long[BOARD_WIDTH, BOARD_WIDTH];
            var random = new Random(1); // always use the same seed for deterministic results
            var byteBuffer = new byte[64];
            for (var r = -BOARD_RADIUS; r <= BOARD_RADIUS; r++)
            {
                var q_start = Math.Max(-BOARD_RADIUS - r, -BOARD_RADIUS);
                var q_end = Math.Min(BOARD_RADIUS - r, BOARD_RADIUS);
                for (var q = q_start; q <= q_end; q++)
                {
                    // Generate random numbers for zobris hashing
                    var arrayPos = new HexCoordinate(r + BOARD_RADIUS, q + BOARD_RADIUS);
                    random.NextBytes(byteBuffer);
                    blackRandomNumbers[arrayPos.R, arrayPos.Q] = Math.Abs(BitConverter.ToInt64(byteBuffer, 0));
                    random.NextBytes(byteBuffer);
                    whiteRandomNumbers[arrayPos.R, arrayPos.Q] = Math.Abs(BitConverter.ToInt64(byteBuffer, 0));

                    // Generate valid neighbors for faster access
                    var pos = new HexCoordinate(r, q);
                    neighborLookup[arrayPos.R, arrayPos.Q] = pos.GetNeighborsClockwise().Where(x => x.Distance(new HexCoordinate(0, 0)) <= BOARD_RADIUS).ToArray();

                }
            }

            randomNumbers.Add(Player.Black, blackRandomNumbers);
            randomNumbers.Add(Player.White, whiteRandomNumbers);
        }

        public HexagonBoard()
        {
            map = new Player[BOARD_WIDTH, BOARD_WIDTH];
        }

        public void PlaceStone(HexCoordinate c, Player p)
        {
            // Place the stone
            this[c] = p;

            // Update the hashcode
            currentHashcode ^= randomNumbers[p][c.R + BOARD_RADIUS, c.Q + BOARD_RADIUS];
        }

        public void RemoveStone(HexCoordinate c)
        {
            // Update the hashcode
            currentHashcode ^= randomNumbers[this[c]][c.R + BOARD_RADIUS, c.Q + BOARD_RADIUS];

            // Remove the stone
            this[c] = Player.None;
        }

        public HexCoordinate[] GetNeighbors(HexCoordinate pos)
        {
            return neighborLookup[pos.R + BOARD_RADIUS, pos.Q + BOARD_RADIUS];
        }

        public long GetLongHashCode()
        {
            return currentHashcode;
        }
    }
}
