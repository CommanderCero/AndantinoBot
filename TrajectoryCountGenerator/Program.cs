using AndantinoBot.Game;
using System;
using System.Diagnostics;

namespace TrajectoryCountGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Andantino();
            var nodeCount = 0;
            var trajectorieCount = 0;
            var watch = new Stopwatch();
            var random = new Random();

            watch.Start();
            while(watch.ElapsedMilliseconds < 4000)
            {
                trajectorieCount++;
                while (!game.IsGameOver)
                {
                    var actions = game.GetValidPlacements();
                    game.PlaceStone(actions[random.Next(0, actions.Length - 1)]);
                    nodeCount++;
                }

                while(game.CanUndo)
                {
                    game.UndoLastMove();
                }
            }

            watch.Stop();
            Console.WriteLine($"Time {watch.ElapsedMilliseconds}ms");
            Console.WriteLine($"Nodes: {nodeCount}");
            Console.WriteLine($"Trajectories: {trajectorieCount}");
        }
    }
}
