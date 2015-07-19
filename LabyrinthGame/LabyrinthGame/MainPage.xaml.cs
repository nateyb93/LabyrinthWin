using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LabyrinthGame
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        /// <summary>
        /// The game board object
        /// </summary>
        private GameBoard _gameBoard;

        /// <summary>
        /// Stores the players playing the game
        /// </summary>
        private Player[] _players;

        /// <summary>
        /// Toggles whether the user can place a new piece
        /// </summary>
        private bool _newTurn;

        /// <summary>
        /// Index of current player in the _players array
        /// </summary>
        private int _currentPlayer;

        public MainPage()
        {
            this.InitializeComponent();

            Loaded += delegate
            {
                _initGameBoardImages();
            };

            _gameBoard = new GameBoard();
            _players = new Player[4];
            _initPlayers(4);
            _newTurn = true;
        }

        /// <summary>
        /// Initializes player variables
        /// </summary>
        private void _initPlayers(int numPlayers)
        {
            for (int i = 0; i < numPlayers; i++ )
                _players[i] = new Player(i);

            _currentPlayer = new Random().Next(0, 4);
        }

        /// <summary>
        /// Advances the game to the next player's turn
        /// </summary>
        private void _nextPlayer()
        {
            if (_currentPlayer == 3)
            {
                _currentPlayer = 0;
            }
            else
            {
                _currentPlayer++;
            }
        }


        /// <summary>
        /// Handles actions associated with rotating the free piece
        /// </summary>
        private void _rotateFreePiece()
        {
            _gameBoard.FreePiece.RotateCounter();
            _drawFreePiece();
        }

        /// <summary>
        /// Handles the click event for rotating the free piece
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RotateButtonClick(object sender, RoutedEventArgs e)
        {
            _rotateFreePiece();
        }

        /// <summary>
        /// Handles the click event for placing the free piece
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlacementButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender == TopButton1)
                _gameBoard.Shift(1, GameBoard.MOVE_DOWN);
            else if(sender == TopButton2)
                _gameBoard.Shift(3, GameBoard.MOVE_DOWN);
            else if(sender == TopButton3)
                _gameBoard.Shift(5, GameBoard.MOVE_DOWN);

            else if(sender == RightButton1)
                _gameBoard.Shift(1, GameBoard.MOVE_LEFT);
            else if (sender == RightButton2)
                _gameBoard.Shift(3, GameBoard.MOVE_LEFT);
            else if (sender == RightButton3)
                _gameBoard.Shift(5, GameBoard.MOVE_LEFT);

            else if (sender == BottomButton1)
                _gameBoard.Shift(1, GameBoard.MOVE_UP);
            else if (sender == BottomButton2)
                _gameBoard.Shift(3, GameBoard.MOVE_UP);
            else if (sender == BottomButton3)
                _gameBoard.Shift(5, GameBoard.MOVE_UP);

            else if (sender == LeftButton1)
                _gameBoard.Shift(1, GameBoard.MOVE_RIGHT);
            else if (sender == LeftButton2)
                _gameBoard.Shift(3, GameBoard.MOVE_RIGHT);
            else if (sender == LeftButton3)
                _gameBoard.Shift(5, GameBoard.MOVE_RIGHT);

            //redraws the game board
            _initGameBoardImages();
            
        }

        /// <summary>
        /// Initializes the images for the game board, including paths and pickups
        /// </summary>
        private void _initGameBoardImages()
        {
            BoardGrid.Children.Clear();
            _drawBoard();
            _drawFreePiece();
        }


        private void _drawBoard()
        {
            Node[,] spaces = _gameBoard.Board;

            for (int i = 0; i < spaces.GetLength(0); i++)
            {
                for (int j = 0; j < spaces.GetLength(1); j++)
                {
                    _drawNewSquare(spaces[i, j], i, j);
                }
            }

        }


        /// <summary>
        /// Draws the free piece on the board
        /// </summary>
        private void _drawFreePiece()
        {
            //clear images from the canvas
            FreePieceCanvas.Children.Clear();

            _drawPath(FreePieceCanvas, _gameBoard.FreePiece.Shape, _gameBoard.FreePiece.Rotation);

            BitmapImage bmp = new BitmapImage();
            bmp.UriSource = new Uri("ms-appx:/Assets/" + _gameBoard.FreePiece.Pickup.ToString() + ".png");
            FreePieceImage.Source = bmp;
        }

        /// <summary>
        /// Draws a square on the game board
        /// </summary>
        /// <param name="node"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        private void _drawNewSquare(Node node, int col, int row)
        {
            //1 square is gameboard.width / 7
            Canvas newCanvas = new Canvas();
            int squareWidth = (int)(BoardGrid.ActualWidth);

            newCanvas.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            newCanvas.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;
            newCanvas.Margin = new Thickness(5, 5, 5, 5);

            Grid.SetColumn(newCanvas, col);
            Grid.SetRow(newCanvas, row);

            BoardGrid.Children.Add(newCanvas);

            _drawPath(newCanvas, node.Shape, node.Rotation);
            _drawImage(row, col, node);
        }


        /// <summary>
        /// Draws the path for a certain piece
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="rotation"></param>
        private void _drawPath(Canvas canvas, Shape shape, int rotation)
        {
            int squareWidth;
            if (canvas != FreePieceCanvas)
                squareWidth = (int)(BoardGrid.ActualWidth);
            else
                squareWidth = (int)canvas.ActualWidth + 10;

            //S 0: |   1: --  2: |   3: --
            //            _     _
            //L 0: |_  1:|    2: |   3: _|
            //    _ _
            //T 0: |   1: -|  2: _|_ 3: |- 
            switch (shape)
            {
                case Shape.S:
                    if (rotation == 0 || rotation == 2)
                        _drawLine(canvas, squareWidth / 2 - 5, 0, squareWidth / 2 - 5, squareWidth - 10, 20);
                    else
                        _drawLine(canvas, 0, squareWidth / 2 - 5, squareWidth - 10, squareWidth / 2 - 5, 20);
                    break;

                case Shape.L:
                    //vertical line
                    if (rotation == 0 || rotation == 3)
                        _drawLine(canvas, squareWidth / 2 - 5, 0, squareWidth / 2 - 5, squareWidth / 2 + 5, 20);
                    else
                        _drawLine(canvas, squareWidth / 2 - 5, squareWidth / 2 - 10, squareWidth / 2 - 5, squareWidth - 10, 20);

                    //horizontal line
                    if (rotation == 0 || rotation == 1)
                        _drawLine(canvas, squareWidth / 2 - 15, squareWidth / 2 - 5, squareWidth - 10, squareWidth / 2 - 5, 20);
                    else
                        _drawLine(canvas, 0, squareWidth / 2 - 5, squareWidth / 2 + 5, squareWidth / 2 - 5, 20);

                    break;


                case Shape.T:
                    //long line
                    if (rotation == 1 || rotation == 3)
                        _drawLine(canvas, squareWidth / 2 - 5, 0, squareWidth / 2 - 5, squareWidth - 10, 20);
                    else
                        _drawLine(canvas, 0, squareWidth / 2 - 5, squareWidth - 10, squareWidth / 2 - 5, 20);

                    //short line
                    if(rotation == 0)
                        _drawLine(canvas, squareWidth / 2 - 5, squareWidth / 2 - 5, squareWidth / 2 - 5, squareWidth - 10, 20);
                    else if(rotation == 1)
                        _drawLine(canvas, 0, squareWidth / 2 - 5, squareWidth / 2 - 5, squareWidth / 2 - 5, 20);
                    else if(rotation == 2)
                        _drawLine(canvas, squareWidth / 2 - 5, 0, squareWidth / 2 - 5, squareWidth / 2 - 5, 20);
                    else
                        _drawLine(canvas, squareWidth / 2 - 5, squareWidth / 2 - 5, squareWidth - 10, squareWidth / 2 - 5, 20);


                    break;
            }
        }


        /// <summary>
        /// Draws a pickup image on a tile
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="n"></param>
        private void _drawImage(int row, int col, Node n)
        {
            //<Image Source="Assets/GreenStar.png" Width="50" Height="50"/>

            Image image = new Image();

            BitmapImage bmp = new BitmapImage();
            bmp.UriSource = new Uri("ms-appx:/Assets/" + n.Pickup.ToString() + ".png");

            image.Width = 50;
            image.Height = 50;
            image.Source = bmp;

            Grid.SetColumn(image, col);
            Grid.SetRow(image, row);

            BoardGrid.Children.Add(image);
        }

        /// <summary>
        /// Draws a line on a canvas
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="endX"></param>
        /// <param name="endY"></param>
        /// <param name="strokeWidth"></param>
        private void _drawLine(Canvas canvas, int startX, int startY, int endX, int endY, int strokeWidth)
        {
            Line line = new Line();

            line.X1 = startX;
            line.Y1 = startY;
            line.X2 = endX;
            line.Y2 = endY;

            line.StrokeThickness = strokeWidth;

            line.Stroke = new SolidColorBrush(Colors.White);
            canvas.Children.Add(line);
        }

    }
}
