using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace LabyrinthGame
{
    public class GameBoard
    {
        public const int MOVE_RIGHT = 0;
        public const int MOVE_DOWN = 1;
        public const int MOVE_LEFT = 2;
        public const int MOVE_UP = 3;

        private BoardNode[,] _board;
        /// <summary>
        /// Returns the array representation of the board
        /// </summary>
        public BoardNode[,] Board
        {
            get
            {
                return _board;
            }
        }

        private BoardNode _freePiece;
        /// <summary>
        /// Stores the current free piece of the board
        /// </summary>
        public BoardNode FreePiece
        {
            get
            {
                return _freePiece;
            }
            set
            {
                _freePiece = value;
            }
        }

        /// <summary>
        /// Stores the graph representation of the board
        /// </summary>
        private BoardGraph _boardGraph;

        public GameBoard()
        {
            _init();
        }

        /// <summary>
        /// Initializes the game board
        /// </summary>
        private void _init()
        {
            _board = new BoardNode[7, 7];
            _initFixedPieces();
            _initMovingPieces();

            _boardGraph = new BoardGraph(_board);
        }
        

        /// <summary>
        /// Initializes the fixed pieces on the board.
        /// </summary>
        private void _initFixedPieces()
        {
            //row 0 fixed
            _board[0, 0] = new BoardNode(Colors.White, Shape.L, BoardNode.ROTATE_1);
            _board[2, 0] = new BoardNode(Colors.Teal, Shape.T, BoardNode.ROTATE_0);
            _board[4, 0] = new BoardNode(Colors.Aquamarine, Shape.T, BoardNode.ROTATE_0);
            _board[6, 0] = new BoardNode(Colors.White, Shape.L, BoardNode.ROTATE_2);

            //row 2 fixed
            _board[0, 2] = new BoardNode(Colors.Blue, Shape.T, BoardNode.ROTATE_3);
            _board[2, 2] = new BoardNode(Colors.Brown, Shape.T, BoardNode.ROTATE_3);
            _board[4, 2] = new BoardNode(Colors.Coral, Shape.T, BoardNode.ROTATE_3);
            _board[6, 2] = new BoardNode(Colors.Crimson, Shape.T, BoardNode.ROTATE_1);

            //row 4 fixed
            _board[0, 4] = new BoardNode(Colors.DarkMagenta, Shape.T, BoardNode.ROTATE_3);
            _board[2, 4] = new BoardNode(Colors.RosyBrown, Shape.T, BoardNode.ROTATE_2);
            _board[4, 4] = new BoardNode(Colors.Goldenrod, Shape.T, BoardNode.ROTATE_1);
            _board[6, 4] = new BoardNode(Colors.Indigo, Shape.T, BoardNode.ROTATE_1);
            
            //row 6 fixed
            _board[0, 6] = new BoardNode(Colors.White, Shape.L, BoardNode.ROTATE_0);
            _board[2, 6] = new BoardNode(Colors.ForestGreen, Shape.T, BoardNode.ROTATE_2);
            _board[4, 6] = new BoardNode(Colors.Lime, Shape.T, BoardNode.ROTATE_2);
            _board[6, 6] = new BoardNode(Colors.White, Shape.L, BoardNode.ROTATE_3);
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
            List<BoardNode> remainingPieces = _getRemainingPieces();

            Random random = new Random();

            int randomNum = random.Next(0, remainingPieces.Count);

            //selects a free piece
            _freePiece = remainingPieces[randomNum];
            remainingPieces.RemoveAt(randomNum);

            //this loop inserts the pieces from the 'remaining pieces' collection randomly
            while (true)
            {
                //select a piece randomly from the board
                int index = random.Next(0, remainingPieces.Count);

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

        /// <summary>
        /// Initializes the non-fixed pieces on the board
        /// </summary>
        /// <returns></returns>
        private List<BoardNode> _getRemainingPieces()
        {
            List<BoardNode> remainingNodes = new List<BoardNode>();

            Random random = new Random();

            //organized by shape and pickup vs. no-pickup
            remainingNodes.Add(new BoardNode(Colors.Maroon, Shape.T, random.Next(0, 4)));
            remainingNodes.Add(new BoardNode(Colors.MidnightBlue, Shape.T, random.Next(0, 4)));
            remainingNodes.Add(new BoardNode(Colors.Fuchsia, Shape.T, random.Next(0, 4)));
            remainingNodes.Add(new BoardNode(Colors.Olive, Shape.T, random.Next(0, 4)));
            remainingNodes.Add(new BoardNode(Colors.Orange, Shape.T, random.Next(0, 4)));
            remainingNodes.Add(new BoardNode(Colors.Orchid, Shape.T, random.Next(0, 4)));

            remainingNodes.Add(new BoardNode(Colors.Turquoise, Shape.L, random.Next(0, 4)));
            remainingNodes.Add(new BoardNode(Colors.Thistle, Shape.L, random.Next(0, 4)));
            remainingNodes.Add(new BoardNode(Colors.Salmon, Shape.L, random.Next(0, 4)));
            remainingNodes.Add(new BoardNode(Colors.RoyalBlue, Shape.L, random.Next(0, 4)));
            remainingNodes.Add(new BoardNode(Colors.SlateBlue, Shape.L, random.Next(0, 4)));
            remainingNodes.Add(new BoardNode(Colors.SpringGreen, Shape.L, random.Next(0, 4)));

            remainingNodes.Add(new BoardNode(Colors.White, Shape.S, random.Next(0, 4)));
            remainingNodes.Add(new BoardNode(Colors.White, Shape.S, random.Next(0, 4)));
            remainingNodes.Add(new BoardNode(Colors.White, Shape.S, random.Next(0, 4)));
            remainingNodes.Add(new BoardNode(Colors.White, Shape.S, random.Next(0, 4)));
            remainingNodes.Add(new BoardNode(Colors.White, Shape.S, random.Next(0, 4)));
            remainingNodes.Add(new BoardNode(Colors.White, Shape.S, random.Next(0, 4)));
            remainingNodes.Add(new BoardNode(Colors.White, Shape.S, random.Next(0, 4)));
            remainingNodes.Add(new BoardNode(Colors.White, Shape.S, random.Next(0, 4)));
            remainingNodes.Add(new BoardNode(Colors.White, Shape.S, random.Next(0, 4)));
            remainingNodes.Add(new BoardNode(Colors.White, Shape.S, random.Next(0, 4)));
            remainingNodes.Add(new BoardNode(Colors.White, Shape.S, random.Next(0, 4)));
            remainingNodes.Add(new BoardNode(Colors.White, Shape.S, random.Next(0, 4)));
            remainingNodes.Add(new BoardNode(Colors.White, Shape.S, random.Next(0, 4)));

            remainingNodes.Add(new BoardNode(Colors.White, Shape.L, random.Next(0, 4)));
            remainingNodes.Add(new BoardNode(Colors.White, Shape.L, random.Next(0, 4)));
            remainingNodes.Add(new BoardNode(Colors.White, Shape.L, random.Next(0, 4)));
            remainingNodes.Add(new BoardNode(Colors.White, Shape.L, random.Next(0, 4)));
            remainingNodes.Add(new BoardNode(Colors.White, Shape.L, random.Next(0, 4)));
            remainingNodes.Add(new BoardNode(Colors.White, Shape.L, random.Next(0, 4)));
            remainingNodes.Add(new BoardNode(Colors.White, Shape.L, random.Next(0, 4)));
            remainingNodes.Add(new BoardNode(Colors.White, Shape.L, random.Next(0, 4)));
            remainingNodes.Add(new BoardNode(Colors.White, Shape.L, random.Next(0, 4)));

            return remainingNodes;
        }


        /// <summary>
        /// Checks if there is a valid path between the start and end nodes in the graph
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public bool IsValidPath(BoardNode start, BoardNode end)
        {
            if (_boardGraph.HasPath(start, end))
            {
                return true;
            }
            return false;
        }

        
        /// <summary>
        /// Shifts the board in the specified direction at the specified position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        public void Shift(int position, int direction)
        {
            switch (direction)
            {
                case MOVE_RIGHT:
                    _shiftRight(position);
                    break;

                case MOVE_DOWN:
                    _shiftDown(position);
                    break;

                case MOVE_LEFT:
                    _shiftLeft(position);
                    break;

                case MOVE_UP:
                    _shiftUp(position);
                    break;
            }

            _boardGraph = new BoardGraph(_board);
        }

        /// <summary>
        /// Shifts the board right at the specified position
        /// </summary>
        /// <param name="position">Position to shift the board at</param>
        private void _shiftRight(int position)
        {
            int size = _board.GetLength(0);
          
            //save the end of the row we're shifting
            //this will get erased if we don't save it
            BoardNode temp = _board[size - 1, position];

            while (temp.Players.Count != 0)
            {
                Player p = temp.Players[0];
                _freePiece.Players.Add(p);
                p.Move(0, position);
                temp.Players.Remove(p);
            }

            //move each node from left to right
            for (int i = size - 1; i > 0; i--)
            {
                _board[i, position] = _board[i - 1, position];
            }

            //the free piece
            _board[0, position] = _freePiece;

            //the piece we pushed off becomes our new free piece
            _freePiece = temp;
        }

        /// <summary>
        /// Shifts the board right at the specified position
        /// </summary>
        /// <param name="position">Position to shift the board at</param>
        private void _shiftDown(int position)
        {
            int size = _board.GetLength(1);

            BoardNode temp = _board[position, size - 1];

            while (temp.Players.Count != 0)
            {
                Player p = temp.Players[0];
                _freePiece.Players.Add(p);
                p.Move(position, 0);
                temp.Players.Remove(p);
            }

            for (int i = size - 1; i > 0; i--)
            {
                _board[position, i] = _board[position, i - 1];
            }

            _board[position, 0] = _freePiece;

            _freePiece = temp;
        }

        /// <summary>
        /// Shifts the board Left at the specified position
        /// </summary>
        /// <param name="position">Position to shift the board at</param>
        private void _shiftLeft(int position)
        {
            int size = _board.GetLength(0);

            BoardNode temp = _board[0, position];

            while (temp.Players.Count != 0)
            {
                Player p = temp.Players[0];
                _freePiece.Players.Add(p);
                p.Move(size - 1, position);
                temp.Players.Remove(p);
            }

            for (int i = 0; i < size - 1; i++)
            {
                _board[i, position] = _board[i + 1, position];
            }

            _board[size - 1, position] = _freePiece;

            _freePiece = temp;
        }

        /// <summary>
        /// Shifts the board right at the specified position
        /// </summary>
        /// <param name="position">Position to shift the board at</param>
        private void _shiftUp(int position)
        {
            int size = _board.GetLength(1);

            BoardNode temp = _board[position, 0];

            while (temp.Players.Count != 0)
            {
                Player p = temp.Players[0];
                _freePiece.Players.Add(p);
                p.Move(position, size - 1);
                temp.Players.Remove(p);
            }

            for (int i = 0; i < size - 1; i++)
            {
                _board[position, i] = _board[position, i + 1];
            }

            _board[position, size - 1] = _freePiece;

            _freePiece = temp;
        }

    }
}
