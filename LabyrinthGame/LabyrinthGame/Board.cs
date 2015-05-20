using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame
{
    public class Board
    {
        private Node[,] _board;

        private Graph _boardGraph;

        public Board()
        {
            _init();
        }

        private void _init()
        {
            _board = new Node[7, 7];
            _initFixedPieces();
            _initMovingPieces();

        }

        /// <summary>
        /// Initializes the fixed pieces on the board.
        /// </summary>
        private void _initFixedPieces()
        {
            //row 0 fixed
            _board[0, 0] = new Node(Pickup.None, new bool[] { false, false, true, true });
            _board[0, 2] = new Node(Pickup.Skeleton, new bool[] { true, false, true, true });
            _board[0, 4] = new Node(Pickup.Sword, new bool[] { true, false, true, true });
            _board[0, 6] = new Node(Pickup.None, new bool[] { true, false, false, true });

            //row 2 fixed
            _board[2, 0] = new Node(Pickup.Gold, new bool[] { false, true, true, true });
            _board[2, 2] = new Node(Pickup.Key, new bool[] { false, true, true, true });
            _board[2, 4] = new Node(Pickup.Gem, new bool[] { true, false, true, true });
            _board[2, 6] = new Node(Pickup.Helm, new bool[] { true, true, false, true });

            //row 4 fixed
            _board[4, 0] = new Node(Pickup.Book, new bool[] { false, true, true, true });
            _board[4, 2] = new Node(Pickup.Crown, new bool[] { true, true, true, false });
            _board[4, 4] = new Node(Pickup.Treasure, new bool[] { true, true, false, true });
            _board[4, 6] = new Node(Pickup.Candle, new bool[] { true, true, false, true });

            //row 6 fixed
            _board[6, 0] = new Node(Pickup.None, new bool[] { false, true, true, false });
            _board[6, 2] = new Node(Pickup.Map, new bool[] { true, true, true, false });
            _board[6, 4] = new Node(Pickup.Ring, new bool[] { true, true, true, false });
            _board[6, 6] = new Node(Pickup.None, new bool[] { true, true, false, false });
        }


        /// <summary>
        /// Initializes the moving pieces on the board
        /// </summary>
        private void _initMovingPieces()
        {
            //after fixed pieces are elimiated, we are left with 6 tri pieces, 15 corners, and 13 straight.
            //Each of the six tri pieces has a pickup on it, and 6 of the thirteen corners has a pickup
            //the 13 straight pieces are all blank, along with 9 corners.

            //strategy:
            //1. Insert each pickup-containing piece randomly, rotating each piece 0-3 times before inserting
            //2. Insert each blank piece randomly, rotating each piece 0-3 times before inserting.
            List<Node> remainingPieces = getRemainingPieces();

            foreach (Node node in remainingPieces)
            {
                int x = 0;
                int y = 0;

                while (true)
                {
                    x = _getRandom();
                    y = _getRandom();

                    //check if the spot is empty, break if so.
                    if (_board[x, y] != null)
                    {
                        break;
                    }
                }

                _board[x, y] = node;
                
            }
        }

        private List<Node> getRemainingPieces()
        {
            List<Node> remainingNodes = new List<Node>();
            remainingNodes.Add(new Node(Pickup.Genie, new bool[] { true, true, true, false }));
            remainingNodes.Add(new Node(Pickup.Ogre, new bool[] { true, true, true, false }));
            remainingNodes.Add(new Node(Pickup.Pixie, new bool[] { true, true, true, false }));
            remainingNodes.Add(new Node(Pickup.Bat, new bool[] { true, true, true, false }));
            remainingNodes.Add(new Node(Pickup.Ghost, new bool[] { true, true, true, false }));
            remainingNodes.Add(new Node(Pickup.Dragon, new bool[] { true, true, true, false }));

            remainingNodes.Add(new Node(Pickup.Butterfly, new bool[] { true, true, false, false }));
            remainingNodes.Add(new Node(Pickup.Mouse, new bool[] { true, true, false, false }));
            remainingNodes.Add(new Node(Pickup.Lizard, new bool[] { true, true, false, false }));
            remainingNodes.Add(new Node(Pickup.Spider, new bool[] { true, true, false, false }));
            remainingNodes.Add(new Node(Pickup.Bug, new bool[] { true, true, false, false }));
            remainingNodes.Add(new Node(Pickup.Scroll, new bool[] { true, true, false, false }));

            remainingNodes.Add(new Node(Pickup.None, new bool[] { true, false, true, false }));
            remainingNodes.Add(new Node(Pickup.None, new bool[] { true, false, true, false }));
            remainingNodes.Add(new Node(Pickup.None, new bool[] { true, false, true, false }));
            remainingNodes.Add(new Node(Pickup.None, new bool[] { true, false, true, false }));
            remainingNodes.Add(new Node(Pickup.None, new bool[] { true, false, true, false }));
            remainingNodes.Add(new Node(Pickup.None, new bool[] { true, false, true, false }));
            remainingNodes.Add(new Node(Pickup.None, new bool[] { true, false, true, false }));
            remainingNodes.Add(new Node(Pickup.None, new bool[] { true, false, true, false }));
            remainingNodes.Add(new Node(Pickup.None, new bool[] { true, false, true, false }));
            remainingNodes.Add(new Node(Pickup.None, new bool[] { true, false, true, false }));
            remainingNodes.Add(new Node(Pickup.None, new bool[] { true, false, true, false }));
            remainingNodes.Add(new Node(Pickup.None, new bool[] { true, false, true, false }));
            remainingNodes.Add(new Node(Pickup.None, new bool[] { true, false, true, false }));

            return remainingNodes;
        }

        private int _getRandom()
        {
            return new Random().Next(0, 6);
        }
    }
}
