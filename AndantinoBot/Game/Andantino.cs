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
        public HexCoordinate Center { get; } = new HexCoordinate(0, 0);

        public HexagonBoard Board { get; }

        public Player ActivePlayer { get; private set; }
        public Player Winner { get; private set; }

        public bool IsGameOver { get; private set; }
        public bool IsFirstTurn { get; private set; } = true;
        public bool CanUndo => lastMoves.Count != 0;
        public int TurnCount => lastMoves.Count;

        public ChainCollection BlackChains { get; }
        public ChainCollection WhiteChains { get; }

        public Dictionary<Player, List<HexCoordinate>> EnclosedStones { get; }
        private Dictionary<HexCoordinate, List<HexCoordinate>> moveEnclosedStones;

        private readonly Stack<HexCoordinate> lastMoves;
        private readonly HashSet<HexCoordinate> validPlacements;

        public Andantino()
        {
            Board = new HexagonBoard();

            validPlacements = new HashSet<HexCoordinate>(Center.GetNeighborsClockwise());
            lastMoves = new Stack<HexCoordinate>();

            BlackChains = new ChainCollection(HexagonBoard.BOARD_RADIUS);
            WhiteChains = new ChainCollection(HexagonBoard.BOARD_RADIUS);

            moveEnclosedStones = new Dictionary<HexCoordinate, List<HexCoordinate>>();
            EnclosedStones = new Dictionary<Player, List<HexCoordinate>>();
            EnclosedStones.Add(Player.Black, new List<HexCoordinate>());
            EnclosedStones.Add(Player.White, new List<HexCoordinate>());

            Board.PlaceStone(Center, Player.Black);
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
            Board.PlaceStone(cord, ActivePlayer);
            lastMoves.Push(cord);

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
            var neighbors = Board.GetNeighbors(cord);
            for(var i = 0; i < neighbors.Length; i++)
            {
                var neighbor = neighbors[i];
                // Do not add coordinates where a stone is already placed
                if (Board[neighbor] != Player.None)
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
            Board.RemoveStone(lastMove);
            validPlacements.Add(lastMove);

            // Switch the active player
            ActivePlayer = ActivePlayer.GetOpponent();

            // Remove enclosed stones
            if (moveEnclosedStones.ContainsKey(lastMove))
            {
                foreach (var tile in moveEnclosedStones[lastMove])
                {
                    EnclosedStones[ActivePlayer].Remove(tile);
                }
                moveEnclosedStones.Remove(lastMove);
            }

            // Update the chain collection
            switch (ActivePlayer)
            {
                case Player.Black: BlackChains.Remove(lastMove); break;
                case Player.White: WhiteChains.Remove(lastMove); break;
            }
        
            // Remove now invalid placements
            if(CanUndo)
            {
                var neighbors = Board.GetNeighbors(lastMove);
                for(var i = 0; i < neighbors.Length; i++)
                {
                    var neighbor = neighbors[i];
                    if (Board[neighbor] != Player.None)
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
            var neighbors = Board.GetNeighbors(cord);
            for (var i = 0; i < neighbors.Length; i++)
            {
                var neighbor = neighbors[i];
                if(Board[neighbor] != Player.None)
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

            var player = Board[placementCord];
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
                        if (pos.TwiceLength() > HexagonBoard.BOARD_RADIUS * 2 || Board[pos] != player)
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
            var clockwiseNeighbors = Board.GetNeighbors(placementCord);
            var endIndex = clockwiseNeighbors.Length - 1;

            // Since the first and last index are actually connected, we do not want to double check the stones at the end of our array
            if (Board[clockwiseNeighbors[endIndex]] != player && Board[clockwiseNeighbors[0]] != player)
            {
                while (Board[clockwiseNeighbors[endIndex]] != player && 0 != endIndex)
                {
                    endIndex--;
                }
            }

            // We are sourrounded by empty or enemy tiles
            if(0 == endIndex)
            {
                // Check if the placed stone was in an enclosed area
                if (EnclosedStones[player.GetOpponent()].Contains(placementCord))
                {
                    return player.GetOpponent();
                }
            }
            else
            {
                var firstCheckIndex = -1;
                var needToCheck = false;
                var prevCouldReachBorder = false;
                for(var i = 0; i <= endIndex; i++)
                {
                    var currPosition = clockwiseNeighbors[i];
                    if (Board[currPosition] == player)
                    {
                        prevCouldReachBorder = false;
                    }
                    else if (!prevCouldReachBorder)
                    {
                        prevCouldReachBorder = true;
                        if(firstCheckIndex == -1)
                        {
                            // We should check this stone, but only after we know for sure that we've connected atleast 2 stones
                            firstCheckIndex = i;
                        }
                        else
                        {
                            // We found 2 stones that were connected
                            needToCheck = true;
                            var enclosedStones = GetEnclosedStones(currPosition, player.GetOpponent());
                            if(enclosedStones != null && enclosedStones.Any(x => Board[x] == player.GetOpponent()))
                            {
                                return player;
                            }
                            else if(enclosedStones != null && !EnclosedStones.Any(x => x.Value.Contains(enclosedStones[0])))
                            {
                                EnclosedStones[player].AddRange(enclosedStones);
                                moveEnclosedStones.Add(placementCord, enclosedStones);
                            }
                        }
                    }
                }

                if(needToCheck)
                {
                    var enclosedStones = GetEnclosedStones(clockwiseNeighbors[firstCheckIndex], player.GetOpponent());
                    if (enclosedStones != null && enclosedStones.Any(x => Board[x] == player.GetOpponent()))
                    {
                        return player;
                    }
                    else if(enclosedStones != null && !EnclosedStones.Any(x => x.Value.Contains(enclosedStones[0])) && !moveEnclosedStones.ContainsKey(placementCord))
                    {
                        EnclosedStones[player].AddRange(enclosedStones);
                        moveEnclosedStones.Add(placementCord, enclosedStones);
                    }
                }
            }

            return Player.None;
        }

        private List<HexCoordinate> GetEnclosedStones(HexCoordinate stoneCord, Player targetPlayer)
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

                if (next.TwiceLength() == HexagonBoard.BOARD_RADIUS * 2)
                {
                    return null;
                }

                var neighbors = Board.GetNeighbors(next);
                for(var i = 0; i < neighbors.Length; i++)
                {
                    var neighbor = neighbors[i];
                    if (Board[neighbor] == opponent || closedList.Contains(neighbor))
                        continue;

                    openList.Push(neighbor);
                    closedList.Add(neighbor);
                }
            }

            return closedList.ToList();
        }
    }
}
