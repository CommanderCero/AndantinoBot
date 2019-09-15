using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndantinoBot.Game
{
    public enum Player
    {
        None,
        Black,
        White
    }

    public static class PlayerExtension
    {
        public static Player GetOpponent(this Player p)
        {
            switch(p)
            {
                case Player.Black: return Player.White;
                case Player.White: return Player.Black;
                default: throw new Exception("Tried to get the opponent for a not existing Player");
            }
        }
    }

}
