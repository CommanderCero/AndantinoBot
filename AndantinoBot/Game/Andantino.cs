using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AndantinoBot.Game
{
    // TODO Maybe replace GetNeighbor calls with an dictionary, that way we do not have to generate 6 new instances every time we want to get the neighbors
    public class Andantino
    {
        public const int BOARD_RADIUS = 9;
        public const int BOARD_WIDTH = BOARD_RADIUS * 2 + 1;

        public HexCoordinate Center { get; } = new HexCoordinate(0, 0);

        public Player ActivePlayer { get; private set; }
        public Player Winner { get; private set; }

        public bool IsGameOver { get; private set; }
        public bool IsFirstTurn { get; private set; } = true;
        public bool CanUndo => lastMoves.Count != 0;
        public int TurnCount => lastMoves.Count;

        public ChainCollection BlackChains { get; }
        public ChainCollection WhiteChains { get; }

        private readonly Stack<HexCoordinate> lastMoves;
        private readonly Player[,] map;
        private readonly HashSet<HexCoordinate> validPlacements;

        private static readonly HexCoordinate[,][] neighborLookup;

        // For Hashcode calculation
        private static Dictionary<Player, long[,]> randomNumbers;
        private long currentHashcode;

        static Andantino()
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

        public Andantino()
        {
            validPlacements = new HashSet<HexCoordinate>(Center.GetNeighborsClockwise());
            lastMoves = new Stack<HexCoordinate>();

            BlackChains = new ChainCollection(BOARD_RADIUS);
            WhiteChains = new ChainCollection(BOARD_RADIUS);

            map = new Player[BOARD_WIDTH, BOARD_WIDTH];
            this[Center] = Player.Black;
            BlackChains.Add(Center);
            ActivePlayer = Player.White;
        }

        public HexCoordinate[] GetValidPlacements()
        {
            return validPlacements.ToArray();
        }

        public void PlaceStone(HexCoordinate cord)
        {
            if(Winner != Player.None)
            {
                throw new Exception("The game is already over");
            }

            if(!validPlacements.Contains(cord))
            {
                throw new Exception("Tried to place a stone in an invalid position!");
            }

            // Update the chain collection
            switch(ActivePlayer)
            {
                case Player.Black: BlackChains.Add(cord); break;
                case Player.White: WhiteChains.Add(cord); break;
            }

            // Place stone
            this[cord] = ActivePlayer;
            lastMoves.Push(cord);

            // Update the hashcode
            currentHashcode ^= randomNumbers[ActivePlayer][cord.R + BOARD_RADIUS, cord.Q + BOARD_RADIUS];

            // Switch active player
            ActivePlayer = ActivePlayer.GetOpponent();

            // Remove the old placement coordinate
            if(IsFirstTurn)
            {
                validPlacements.Clear();
                IsFirstTurn = false;
            }
            else
            {
                validPlacements.Remove(cord);
            }

            // Add new placement coordinates
            var neighbors = GetNeighbors(cord);
            for(var i = 0; i < neighbors.Length; i++)
            {
                var neighbor = neighbors[i];
                // Do not add coordinates where a stone is already placed
                if (this[neighbor] != Player.None)
                    continue;

                var neighborCount = CountNeighborStones(neighbor);
                if (neighborCount >= 2)
                    validPlacements.Add(neighbor);
            }

            // Check if a player won or if it's a draw
            Winner = GetWinner(cord);
            if (Winner != Player.None || validPlacements.Count == 0)
            {
                IsGameOver = true;
                return;
            }
        }

        public void UndoLastMove()
        {
            if (lastMoves.Count == 0)
                throw new Exception("Tried to undo the last move, when no last moves are available");
        
            var lastMove = lastMoves.Pop();
        
            // Remove the stone
            this[lastMove] = Player.None;
            validPlacements.Add(lastMove);

            // Switch the active player
            ActivePlayer = ActivePlayer.GetOpponent();

            // Update the chain collection
            switch (ActivePlayer)
            {
                case Player.Black: BlackChains.Remove(lastMove); break;
                case Player.White: WhiteChains.Remove(lastMove); break;
            }

            // Update the hashcode
            currentHashcode ^= randomNumbers[ActivePlayer][lastMove.R + BOARD_RADIUS, lastMove.Q + BOARD_RADIUS];
        
            // Remove now invalid placements
            if(CanUndo)
            {
                var neighbors = GetNeighbors(lastMove);
                for(var i = 0; i < neighbors.Length; i++)
                {
                    var neighbor = neighbors[i];
                    if (this[neighbor] != Player.None)
                        continue;

                    var neighborCount = CountNeighborStones(neighbor);
                    if (neighborCount < 2)
                        validPlacements.Remove(neighbor);
                }
            }
            else
            {
                // We are at the beginning of the game
                IsFirstTurn = true;
                validPlacements.Clear();
                foreach (var neighbor in Center.GetNeighborsClockwise())
                    validPlacements.Add(neighbor);
            }

            // Remove the winning player
            Winner = Player.None;
            IsGameOver = false;
        }

        public int CountNeighborStones(HexCoordinate cord)
        {
            var count = 0;
            var neighbors = GetNeighbors(cord);
            for (var i = 0; i < neighbors.Length; i++)
            {
                var neighbor = neighbors[i];
                if(this[neighbor] != Player.None)
                {
                    count += 1;
                }
            }

            return count;
        }

        private Player GetWinner(HexCoordinate placementCord)
        {
            // We only need to check for diagonals after atleast 4 black and 4 white stones were placed
            if (lastMoves.Count < 8)
                return Player.None;

            var player = this[placementCord];
            // Check if the player has 5 stones in a row
            foreach(var axis in new HexCoordinate[] {HexCoordinate.East, HexCoordinate.NorthEast, HexCoordinate.NorthWest})
            {
                var colorCount = 1;
                for(var dir = -1; dir <= 1; dir += 2)
                {
                    // Count stones in the axis direction
                    for (var i = 1; i < 5; i++)
                    {
                        var pos = placementCord + axis * i * dir;
                        if (pos.TwiceLength() > BOARD_RADIUS * 2 || this[pos] != player)
                            break;

                        colorCount++;
                    }
                }

                if(colorCount >= 5)
                {
                    return player;
                }
            }

            // We only need to check for enclosed stones after atleast 5 black and 5 white stones were placed
            if (lastMoves.Count < 10)
                return Player.None;

            // Check if the active player enclosed the opponent
            var clockwiseNeighbors = GetNeighbors(placementCord);
            var startIndex = 0;
            var endIndex = 0;

            if (this[clockwiseNeighbors[endIndex]] != player)
            {
                endIndex = clockwiseNeighbors.Length - 1;
                while (this[clockwiseNeighbors[endIndex]] != player && startIndex != endIndex)
                {
                    endIndex = (endIndex - 1) % clockwiseNeighbors.Length;
                }
            }
            else
            {
                startIndex = 1;
            }

            // We are sourrounded by empty or enemy tiles
            if(startIndex == endIndex)
            {
                // Check if the placed stone was in an enclosed area
                if (!CanReachBorder(placementCord, player))
                {
                    return player.GetOpponent();
                }
            }
            else
            {
                var prevCouldReachBorder = false;
                while (startIndex != endIndex)
                {
                    var currPosition = clockwiseNeighbors[startIndex];
                    if (this[currPosition] == player)
                    {
                        prevCouldReachBorder = false;
                    }
                    else if (!prevCouldReachBorder)
                    {
                        if (CanReachBorder(currPosition, player.GetOpponent()))
                        {
                            prevCouldReachBorder = true;
                        }
                        else
                        {
                            return player;
                        }
                    }
                    startIndex = (startIndex + 1) % clockwiseNeighbors.Length;
                }
            }

            return Player.None;
        }

        private bool CanReachBorder(HexCoordinate stoneCord, Player targetPlayer)
        {
            var opponent = targetPlayer.GetOpponent();

            // Use Stack instead of Queue because depth first is MUCH faster, as we first try to move directly to the border
            var openList = new Stack<HexCoordinate>(); 
            var closedList = new HashSet<HexCoordinate>();
            openList.Push(stoneCord);
            closedList.Add(stoneCord);
            while(openList.Count > 0)
            {
                var next = openList.Pop();

                if (next.TwiceLength() == BOARD_RADIUS * 2)
                {
                    return true;
                }

                var neighbors = GetNeighbors(next);
                for(var i = 0; i < neighbors.Length; i++)
                {
                    var neighbor = neighbors[i];
                    if (this[neighbor] == opponent || closedList.Contains(neighbor))
                        continue;

                    openList.Push(neighbor);
                    closedList.Add(neighbor);
                }
            }

            return !closedList.Any(x => this[x] == targetPlayer);
        }

        public HexCoordinate[] GetNeighbors(HexCoordinate pos)
        {
            return neighborLookup[pos.R + BOARD_RADIUS, pos.Q + BOARD_RADIUS];
        }

        public bool IsValidCoordinate(HexCoordinate c)
        {
            // Distance to the center, because the center is (0,0) we do not need to consider it
            return Math.Abs(c.Q) + Math.Abs(c.R) + Math.Abs(c.S) <= BOARD_RADIUS * 2;
        }

        public long GetLongHashCode()
        {
            return currentHashcode;
        }

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
    }
}
