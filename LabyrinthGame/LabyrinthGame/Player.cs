using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace LabyrinthGame
{
    public class Player
    {
        public List<Color> LostTreasures;
        public int CurrentTreasure;
        public List<Color> FoundTreasures;
        public int StartX;
        public int StartY;

        public int CurrentX;
        public int CurrentY;

        /// <summary>
        /// Default constructor for the player class
        /// </summary>
        /// <param name="index">Index the players appear in the players array;</param>
        public Player(int index)
        {
            CurrentTreasure = 0;

            switch (index)
            {
                case 0:
                default:
                    StartX = 0;
                    StartY = 0;
                    break;
                case 1:
                    StartX = 6;
                    StartY = 0;
                    break;
                case 2:
                    StartX = 6;
                    StartY = 6;
                    break;
                case 3:
                    StartX = 0;
                    StartY = 6;
                    break;
            }

            CurrentX = StartX;
            CurrentY = StartY;
        }

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
        public void FindTreasure(Color color)
        {
            if (LostTreasures.Contains(color))
            {
                FoundTreasures.Add(color);
                LostTreasures.Remove(color);
                CurrentTreasure++;
            }
        }
    }
}
