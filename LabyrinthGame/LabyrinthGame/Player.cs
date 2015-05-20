using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame
{
    public class Player
    {
        public List<Pickup> LostTreasures;
        public List<Pickup> FoundTreasures;
        public int StartX;
        public int StartY;

        public int CurrentX;
        public int CurrentY;

        /// <summary>
        /// Checks to see if the player has won or not
        /// </summary>
        /// <returns></returns>
        public bool HasWon()
        {
            //Winning conditions:
            //No more treasures to find (LostTreasures.Count is 0)
            //CurrentX and CurrentY are equivalent to the starting position
            if (LostTreasures.Count == 0 && CurrentX == StartX && CurrentY == StartY)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Performs the actions associated with finding a treasure on the game board
        /// </summary>
        public void FindTreasure(Pickup pickup)
        {
            if (LostTreasures.Contains(pickup))
            {
                FoundTreasures.Add(pickup);
                LostTreasures.Remove(pickup);
            }
        }
    }
}
