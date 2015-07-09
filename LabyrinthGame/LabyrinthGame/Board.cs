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

        private Node _freePiece;

        private Graph _boardGraph;

        public Board()
        {
            _init();
        }

        /// <summary>
        /// Initializes the game board
        /// </summary>
        private void _init()
        {
            _board = new Node[7, 7];
            _initFixedPieces();
            _initMovingPieces();

            _boardGraph = new Graph(_board);
        }

        /// <summary>
        /// Initializes the fixed pieces on the board.
        /// </summary>
        private void _initFixedPieces()
        {
            //row 0 fixed
            _board[0, 0] = new Node(Pickup.None, Shape.L, Node.ROTATE_1);
            _board[0, 2] = new Node(Pickup.Bone, Shape.T, Node.ROTATE_0);
            _board[0, 4] = new Node(Pickup.Sword, Shape.T, Node.ROTATE_0);
            _board[0, 6] = new Node(Pickup.None, Shape.L, Node.ROTATE_2);

            //row 2 fixed
            _board[2, 0] = new Node(Pickup.Gold, Shape.T, Node.ROTATE_3);
            _board[2, 2] = new Node(Pickup.Key, Shape.T, Node.ROTATE_3);
            _board[2, 4] = new Node(Pickup.Gem, Shape.T, Node.ROTATE_0);
            _board[2, 6] = new Node(Pickup.Helm, Shape.T, Node.ROTATE_1);

            //row 4 fixed
            _board[4, 0] = new Node(Pickup.Book, Shape.T, Node.ROTATE_3);
            _board[4, 2] = new Node(Pickup.Crown, Shape.T, Node.ROTATE_2);
            _board[4, 4] = new Node(Pickup.Treasure, Shape.T, Node.ROTATE_1);
            _board[4, 6] = new Node(Pickup.Candle, Shape.T, Node.ROTATE_1);

            //row 6 fixed
            _board[6, 0] = new Node(Pickup.None, Shape.L, Node.ROTATE_0);
            _board[6, 2] = new Node(Pickup.Map, Shape.T, Node.ROTATE_2);
            _board[6, 4] = new Node(Pickup.Ring, Shape.T, Node.ROTATE_2);
            _board[6, 6] = new Node(Pickup.None, Shape.L, Node.ROTATE_3);
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

            int random = new Random().Next(0, remainingPieces.Count);

            //selects a free piece
            _freePiece = remainingPieces[random];
            remainingPieces.RemoveAt(random);

            //this loop inserts the pieces from the 'remaining pieces' collection randomly
            while (true)
            {
                //select a piece randomly from the board
                int index = _getRandom(remainingPieces.Count);

                //find an empty space in the board
                for (int i = 0; i < _board.GetLength(0); i++)
                {
                    bool breakFor = false;
                    for (int j = 0; j < _board.GetLength(1); j++)
                    {
                        //if we find an empty space in the board,
                        //insert our piece here and remove it from the list
                        if (_board[i, j] == null)
                        {
                            _board[i, j] = remainingPieces[index];
                            remainingPieces.RemoveAt(index);
                            breakFor = true;
                            break;
                        }
                    }//for j

                    //previous break will only break out of the inner loop
                    if (breakFor)
                        break;

                }//for i

                //if we've completed our tasks, 
                if (remainingPieces.Count == 0)
                    break;
                
            }//while
        }


        private List<Node> getRemainingPieces()
        {
            List<Node> remainingNodes = new List<Node>();
            remainingNodes.Add(new Node(Pickup.Genie, Shape.T, _getRandomRotation()));
            remainingNodes.Add(new Node(Pickup.Ogre, Shape.T, _getRandomRotation()));
            remainingNodes.Add(new Node(Pickup.Pixie, Shape.T, _getRandomRotation()));
            remainingNodes.Add(new Node(Pickup.Bat, Shape.T, _getRandomRotation()));
            remainingNodes.Add(new Node(Pickup.Ghost, Shape.T, _getRandomRotation()));
            remainingNodes.Add(new Node(Pickup.Dragon, Shape.T, _getRandomRotation()));

            remainingNodes.Add(new Node(Pickup.Butterfly, Shape.L, _getRandomRotation()));
            remainingNodes.Add(new Node(Pickup.Mouse, Shape.L, _getRandomRotation()));
            remainingNodes.Add(new Node(Pickup.Lizard, Shape.L, _getRandomRotation()));
            remainingNodes.Add(new Node(Pickup.Spider, Shape.L, _getRandomRotation()));
            remainingNodes.Add(new Node(Pickup.Bug, Shape.L, _getRandomRotation()));
            remainingNodes.Add(new Node(Pickup.Scroll, Shape.L, _getRandomRotation()));

            remainingNodes.Add(new Node(Pickup.None, Shape.S, _getRandomRotation()));
            remainingNodes.Add(new Node(Pickup.None, Shape.S, _getRandomRotation()));
            remainingNodes.Add(new Node(Pickup.None, Shape.S, _getRandomRotation()));
            remainingNodes.Add(new Node(Pickup.None, Shape.S, _getRandomRotation()));
            remainingNodes.Add(new Node(Pickup.None, Shape.S, _getRandomRotation()));
            remainingNodes.Add(new Node(Pickup.None, Shape.S, _getRandomRotation()));
            remainingNodes.Add(new Node(Pickup.None, Shape.S, _getRandomRotation()));
            remainingNodes.Add(new Node(Pickup.None, Shape.S, _getRandomRotation()));
            remainingNodes.Add(new Node(Pickup.None, Shape.S, _getRandomRotation()));
            remainingNodes.Add(new Node(Pickup.None, Shape.S, _getRandomRotation()));
            remainingNodes.Add(new Node(Pickup.None, Shape.S, _getRandomRotation()));
            remainingNodes.Add(new Node(Pickup.None, Shape.S, _getRandomRotation()));
            remainingNodes.Add(new Node(Pickup.None, Shape.S, _getRandomRotation()));

            remainingNodes.Add(new Node(Pickup.None, Shape.L, _getRandomRotation()));
            remainingNodes.Add(new Node(Pickup.None, Shape.L, _getRandomRotation()));
            remainingNodes.Add(new Node(Pickup.None, Shape.L, _getRandomRotation()));
            remainingNodes.Add(new Node(Pickup.None, Shape.L, _getRandomRotation()));
            remainingNodes.Add(new Node(Pickup.None, Shape.L, _getRandomRotation()));
            remainingNodes.Add(new Node(Pickup.None, Shape.L, _getRandomRotation()));
            remainingNodes.Add(new Node(Pickup.None, Shape.L, _getRandomRotation()));
            remainingNodes.Add(new Node(Pickup.None, Shape.L, _getRandomRotation()));
            remainingNodes.Add(new Node(Pickup.None, Shape.L, _getRandomRotation()));

            return remainingNodes;
        }

        private int _getRandomRotation()
        {
            return new Random().Next(0, 3);
        }

        private int _getRandom(int max)
        {
            return new Random().Next(0, max);
        }

        public void Print()
        {

        }
    }
}
