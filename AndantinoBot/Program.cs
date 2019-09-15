using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AndantinoBot.Game;

namespace AndantinoBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var game = new Andantino();
            while(true)
            {
                var validPlacements = game.GetValidPlacements().ToList();
                for(var i = 0; i < validPlacements.Count; i++)
                {
                    Console.WriteLine($"{i}: {validPlacements[i]}");
                }
            
                Console.WriteLine("Choose a placement");
                game.PlaceStone(validPlacements[int.Parse(Console.ReadLine())]);
                Console.WriteLine("Placed Stone");
                Console.WriteLine();
            
                if(game.Winner != Player.None)
                {
                    Console.WriteLine($"Player {game.Winner} won!");
                    break;
                }
            }
        }
    }
}
