using AndantinoBot;
using AndantinoBot.Game;
using CsvHelper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace HeuristicEvaluationDataGenerator
{
    public class StateHeuristicContainer
    {
        public static List<string> validHeuristicNames = new List<string>()
        {
            "PlacementCount",
            "BlackCapturedTiles",
            "WhiteCapturedTiles",
            "BlackControlTiles",
            "WhiteControlTiles",
            "BlackControlTilesCntDouble",
            "WhiteControlTilesCntDouble",
            "White_chain_1_E0_P0", "White_chain_1_E1_P0", "White_chain_1_E1_P1", "White_chain_1_E2_P0", "White_chain_1_E2_P1", "White_chain_1_E2_P2",
            "White_chain_2_E0_P0", "White_chain_2_E1_P0", "White_chain_2_E1_P1", "White_chain_2_E2_P0", "White_chain_2_E2_P1", "White_chain_2_E2_P2",
            "White_chain_3_E0_P0", "White_chain_3_E1_P0", "White_chain_3_E1_P1", "White_chain_3_E2_P0", "White_chain_3_E2_P1", "White_chain_3_E2_P2",
            "White_chain_4_E0_P0", "White_chain_4_E1_P0", "White_chain_4_E1_P1", "White_chain_4_E2_P0", "White_chain_4_E2_P1", "White_chain_4_E2_P2",
            "Black_chain_1_E0_P0", "Black_chain_1_E1_P0", "Black_chain_1_E1_P1", "Black_chain_1_E2_P0", "Black_chain_1_E2_P1", "Black_chain_1_E2_P2",
            "Black_chain_2_E0_P0", "Black_chain_2_E1_P0", "Black_chain_2_E1_P1", "Black_chain_2_E2_P0", "Black_chain_2_E2_P1", "Black_chain_2_E2_P2",
            "Black_chain_3_E0_P0", "Black_chain_3_E1_P0", "Black_chain_3_E1_P1", "Black_chain_3_E2_P0", "Black_chain_3_E2_P1", "Black_chain_3_E2_P2",
            "Black_chain_4_E0_P0", "Black_chain_4_E1_P0", "Black_chain_4_E1_P1", "Black_chain_4_E2_P0", "Black_chain_4_E2_P1", "Black_chain_4_E2_P2"
        };

        public long StateHashCode { get; set; }
        public int TurnCount { get; set; }
        public Dictionary<string, int> HeuristicValues { get; } = new Dictionary<string, int>();

        public StateHeuristicContainer()
        {
            foreach(var name in validHeuristicNames)
            {
                HeuristicValues.Add(name, 0);
            }
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var threadCount = 8;
            var games = new Andantino[threadCount];
            var threads = new Thread[threadCount];
            var winCounters = new Dictionary<long, int[]>[threadCount];
            var heuristicContainers = new List<StateHeuristicContainer>[threadCount];
            CancellationTokenSource source = new CancellationTokenSource();
            var cancelToken = source.Token;

            for (var x = 0; x < threadCount; x++)
            {
                games[x] = new Andantino();
                winCounters[x] = new Dictionary<long, int[]>();
                heuristicContainers[x] = new List<StateHeuristicContainer>();
                // The first 2 moves lead to the same game state, just rotated
                // We do not need to analyze the same game states multiple times to figure out a good heuristic
                for (var i = 0; i < 2; i++)
                    games[x].PlaceStone(games[x].GetValidPlacements()[0]);

                // Start the thread
                var xCopy = x;
                threads[x] = new Thread(() => CollectData(cancelToken, xCopy, games[xCopy], winCounters[xCopy], heuristicContainers[xCopy]));
                threads[x].Start();
            }

            var timer = new Stopwatch();
            timer.Start();
            while(true)
            {
                // Check if we should stop
                if(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
                {
                    source.Cancel();
                    for (var i = 0; i < threads.Length; i++)
                    {
                        threads[i].Join();
                    }
                    break;
                }

                // Check if we should protocol our current winCount
                if(timer.Elapsed.TotalMinutes >= 5)
                {
                    Console.WriteLine("\n\n**** Saving intermediate win counts ****");

                    // Stop the threads
                    source.Cancel();
                    for (var i = 0; i < threads.Length; i++)
                    {
                        threads[i].Join();
                    }

                    // Protocoll the wins
                    ProtocolWinCount(winCounters);
                    Console.WriteLine();

                    // Restart the threads
                    source.Dispose();
                    source = new CancellationTokenSource();
                    cancelToken = source.Token;
                    for (var x = 0; x < threadCount; x++)
                    {
                        // Start the thread
                        var xCopy = x;
                        winCounters[xCopy].Clear();
                        threads[x] = new Thread(() => CollectData(cancelToken, xCopy, games[xCopy], winCounters[xCopy], heuristicContainers[xCopy]));
                        threads[x].Start();
                    }

                    // Restart 5min timer
                    timer.Restart();
                }
            }

            // Protocoll the wins
            ProtocolWinCount(winCounters);
        }

        private static void ProtocolWinCount(Dictionary<long, int[]>[] winCounters)
        {
            Console.WriteLine("Calculating final win counts");
            var finalWinCounter = new Dictionary<long, int[]>();
            foreach (var winCounter in winCounters)
            {
                foreach (var entry in winCounter)
                {
                    if (!finalWinCounter.ContainsKey(entry.Key))
                        finalWinCounter.Add(entry.Key, entry.Value);
                    else
                    {
                        for (var i = 0; i < 3; i++)
                        {
                            finalWinCounter[entry.Key][i] += entry.Value[i];
                        }
                    }
                }
            }

            Console.WriteLine("Saving final win counts");
            var time = DateTime.Now;
            using (var winCounterWriter = new CsvWriter(new StreamWriter($"./winCounter_{time.ToString("yyyyMMdd_HHmmss")}.csv")))
            {
                winCounterWriter.WriteField("HashCode");
                winCounterWriter.WriteField("None");
                winCounterWriter.WriteField("Black");
                winCounterWriter.WriteField("White");
                winCounterWriter.NextRecord();

                foreach (var entry in finalWinCounter)
                {
                    winCounterWriter.WriteField(entry.Key);
                    for (var i = 0; i < 3; i++)
                    {
                        winCounterWriter.WriteField(entry.Value[i]);
                    }
                    winCounterWriter.NextRecord();
                }
            }

            Console.WriteLine("Finished");
        }

        private static void CollectData(
            CancellationToken token,
            int threadID, 
            Andantino game,
            Dictionary<long, int[]> winCounter, 
            List<StateHeuristicContainer> heuristicContainers)
        {
            // Initialize Writer
            var heuristicWriter = new CsvWriter(new StreamWriter($"./heuristics_{threadID}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv"));
            heuristicWriter.WriteField("HashCode");
            heuristicWriter.WriteField("Turn");
            foreach (var name in StateHeuristicContainer.validHeuristicNames)
            {
                heuristicWriter.WriteField(name);
            }
            heuristicWriter.NextRecord();

            var random = new Random(threadID*2000);
            Console.WriteLine($"{threadID}: Started collecting from turn {game.TurnCount}");

            var newStateCounter = 0;
            var totalStateCounter = 0;
            var watch = new Stopwatch();
            watch.Start();
            while (true)
            {
                var trajLength = MoveRandomly(game, random);

                // Collect trajectory data
                var winner = game.Winner;
                // We do not want to protocol the winning state
                game.UndoLastMove();
                for (var i = 0; i <= trajLength-1; i++)
                {
                    var hashCode = game.Board.GetLongHashCode();
                    if (!winCounter.ContainsKey(hashCode))
                    {
                        var container = new StateHeuristicContainer() { TurnCount = game.TurnCount, StateHashCode = hashCode};
                        AddGameInformation(game, container);
                        AddChainLengthHeuristic(game, container);
                        AddCapturedTileHeuristic(game, container);
                        AddTileControlHeuristic(game, container);

                        winCounter.Add(hashCode, new int[3]);
                        heuristicContainers.Add(container);
                        newStateCounter++;
                    }

                    totalStateCounter++;
                    winCounter[hashCode][(int)winner]++;

                    if (i != 0)
                        game.UndoLastMove();
                }

                // Log progress
                if(watch.ElapsedMilliseconds > 5000)
                {
                    // Save data from heuristic container
                    foreach(var container in heuristicContainers)
                    {
                        heuristicWriter.WriteField(container.StateHashCode);
                        heuristicWriter.WriteField(container.TurnCount);
                        foreach(var heuristic in StateHeuristicContainer.validHeuristicNames)
                        {
                            heuristicWriter.WriteField(container.HeuristicValues[heuristic]);
                        }

                        heuristicWriter.NextRecord();
                    }

                    // Log collected data
                    Console.WriteLine($"{threadID}: {newStateCounter}/{totalStateCounter}\tcancellation = {token.IsCancellationRequested}");
                    newStateCounter = 0;
                    totalStateCounter = 0;

                    // Clear our memory
                    heuristicWriter.Flush();
                    heuristicContainers.Clear();
                    watch.Restart();

                    if(token.IsCancellationRequested)
                    {
                        Console.WriteLine($"{threadID}: Cancellation requested");
                        break;
                    }
                }
            }

            heuristicWriter.Dispose();
            Console.WriteLine($"{threadID}: Finished collecting");
        }

        private static int MoveRandomly(Andantino game, Random random)
        {
            var i = 0;
            for (; !game.IsGameOver; i++)
            {
                var validMoves = game.GetValidPlacements();
                var randomMove = validMoves[random.Next(0, validMoves.Length)];
                game.PlaceStone(randomMove);
            }

            return i;
        }

        private static void AddGameInformation(Andantino game, StateHeuristicContainer container)
        {
            container.HeuristicValues["PlacementCount"] = game.GetValidPlacements().Length;
        }

        private static void AddChainLengthHeuristic(Andantino game, StateHeuristicContainer container)
        {
            AddChainLengthHeuristic(Player.White, game, container, game.WhiteChains.QRowChains);
            AddChainLengthHeuristic(Player.White, game, container, game.WhiteChains.RRowChains);
            AddChainLengthHeuristic(Player.White, game, container, game.WhiteChains.SRowChains);

            AddChainLengthHeuristic(Player.Black, game, container, game.BlackChains.QRowChains);
            AddChainLengthHeuristic(Player.Black, game, container, game.BlackChains.RRowChains);
            AddChainLengthHeuristic(Player.Black, game, container, game.BlackChains.SRowChains);
        }

        private static void AddChainLengthHeuristic(Player p, Andantino game, StateHeuristicContainer container, ChainRowCollection[] rows)
        {
            var validPlacements = game.GetValidPlacements();
            for (var row = 0; row < rows.Length; row++)
            {
                var chainRow = rows[row];
                for (var i = 0; i < chainRow.ChainCount; i++)
                {
                    var isStartExtendable = chainRow.GetChainStartExtension(i, out var qStart, out var rStart) && game.Board[qStart, rStart] == Player.None;
                    var isEndExtendable = chainRow.GetChainEndExtension(i, out var qEnd, out var rEnd) && game.Board[qEnd, rEnd] == Player.None;
                    var isStartPlaceable = isStartExtendable && validPlacements.Any(x => x.Q == qStart && x.R == rStart);
                    var isEndPlaceable = isEndExtendable && validPlacements.Any(x => x.Q == qEnd && x.R == rEnd);

                    var extensionCount = (isStartExtendable ? 1 : 0) + (isEndExtendable ? 1 : 0);
                    var placeableCount = (isStartPlaceable ? 1 : 0) + (isEndPlaceable ? 1 : 0);
                    var length = chainRow.GetChainLength(i);

                    var chainName = $"{p}_chain_{length}_E{extensionCount}_P{placeableCount}";
                    container.HeuristicValues[chainName]++;
                }
            }
        }

        private static void AddTileControlHeuristic(Andantino game, StateHeuristicContainer container)
        {
            var blackCount = 0;
            var whiteCount = 0;
            var blackCountDbl = 0;
            var whiteCountDbl = 0;
            foreach (var move in game.GetValidPlacements())
            {
                var countBlack = true;
                var countWhite = true;
                foreach (var neighbor in game.Board.GetNeighbors(move))
                {
                    if (game.Board[neighbor] == Player.Black)
                    {
                        if(countBlack)
                            blackCount++;

                        countBlack = false;
                        blackCountDbl++;
                    }
                    
                    if (countWhite && game.Board[neighbor] == Player.Black)
                    {
                        if(countWhite)
                            whiteCount++;

                        countWhite = false;
                        whiteCountDbl++;
                    }

                }
            }

            container.HeuristicValues["BlackControlTiles"] = blackCount;
            container.HeuristicValues["WhiteControlTiles"] = whiteCount;
            container.HeuristicValues["BlackControlTilesCntDouble"] = blackCountDbl;
            container.HeuristicValues["WhiteControlTilesCntDouble"] = whiteCountDbl;
        }

        private static void AddCapturedTileHeuristic(Andantino game, StateHeuristicContainer container)
        {
            container.HeuristicValues["BlackCapturedTiles"] = game.EnclosedStones[Player.Black].Count();
            container.HeuristicValues["WhiteCapturedTiles"] = game.EnclosedStones[Player.White].Count();
        }
    }
}
