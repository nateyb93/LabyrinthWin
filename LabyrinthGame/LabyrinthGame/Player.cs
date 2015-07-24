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
        /// <summary>
        /// Collection of objectives to find
        /// </summary>
        public List<Color> LostTreasures;

        /// <summary>
        /// Player's collection of found objects
        /// </summary>
        public List<Color> FoundTreasures;

        /// <summary>
        /// Start X coordinate of player object
        /// </summary>
        public int StartX;

        /// <summary>
        /// Start Y coordinate of player object
        /// </summary>
        public int StartY;

        /// <summary>
        /// Current X coordinate of player
        /// </summary>
        public int CurrentX;

        /// <summary>
        /// Current Y coordinate of player
        /// </summary>
        public int CurrentY;

        /// <summary>
        /// Index of the player object in the MainPage array
        /// </summary>
        public int Index;

        /// <summary>
        /// Default constructor for the player class
        /// </summary>
        /// <param name="index">Index the players appear in the players array;</param>
        public Player(int index)
        {
            Index = index;

            LostTreasures = new List<Color>();
            FoundTreasures = new List<Color>();

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

        public void Move(int x, int y)
        {
            CurrentX = x;
            CurrentY = y;
        }

        /// <summary>
        /// Moves the player one space to the left
        /// </summary>
        public void MoveLeft()
        {
            CurrentX--;
            if (CurrentX < 0)
                CurrentX = 6;
        }

        /// <summary>
        /// Moves the player one space to the right
        /// </summary>
        public void MoveRight()
        {
            CurrentX++;
            if (CurrentX > 6)
                CurrentX = 0;
        }

        /// <summary>
        /// Moves the player one space up
        /// </summary>
        public void MoveUp()
        {
            CurrentY--;
            if (CurrentY < 0)
                CurrentY = 6;
        }

        /// <summary>
        /// Moves the player one space down
        /// </summary>
        public void MoveDown()
        {
            CurrentY++;
            if (CurrentY > 6)
                CurrentY = 0;
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
            if (LostTreasures[0] == color)
            {
                FoundTreasures.Add(color);
                LostTreasures.Remove(color);
            }
        }
    }
}
