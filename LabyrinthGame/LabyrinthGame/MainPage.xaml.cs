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

    struct Deck
    {
        public List<Color> Cards;
    }

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
        /// Deck of cards
        /// </summary>
        private Deck Deck;

        /// <summary>
        /// Stores the players playing the game
        /// </summary>
        private Player[] _players;

        /// <summary>
        /// Direction of pending board shift
        /// </summary>
        private int _pendingDirection;

        /// <summary>
        /// Location of pending board shift
        /// </summary>
        private int _pendingLocation;

        /// <summary>
        /// Index of current player in the _players array
        /// </summary>
        private int _currentPlayer;

        /// <summary>
        /// Stores whether the board has been initialized.
        /// </summary>
        private bool _initialized = false;

        public MainPage()
        {
            this.InitializeComponent();

            Loaded += delegate
            {
                _initGameBoardImages();
                _initDeck();
                _initPlayers(4);
                _initButtons();
                _drawPlayerCards();
            };

            _pendingDirection = -1;
            _pendingLocation = -1;

            _gameBoard = new GameBoard();
        }

        /// <summary>
        /// Switches the current player
        /// </summary>
        public void SwitchPlayer()
        {
            _currentPlayer++;
            if (_currentPlayer >= _players.Length)
            {
                _currentPlayer = 0;
            }

            _playerSwitchBackgroundChange();

            _drawPlayerCards();
        }


        /// <summary>
        /// Updates the background for each player button based on whose turn it is
        /// </summary>
        private void _playerSwitchBackgroundChange()
        {
            switch (_currentPlayer)
            {
                case 0:
                    //Make background light and reverse theme for contrast reasons
                    Player1ButtonGrid.Background = new SolidColorBrush(Colors.White);
                    Player1Button.RequestedTheme = ElementTheme.Light;

                    //this switch determines whose turn it was prior; this differs
                    //based on how many players are in the game
                    switch (_players.Length)
                    {
                        case 2:
                            Player2ButtonGrid.Background = new SolidColorBrush(Colors.Transparent);
                            Player2Button.RequestedTheme = ElementTheme.Default;
                            break;
                        case 3:
                            Player3ButtonGrid.Background = new SolidColorBrush(Colors.Transparent);
                            Player3Button.RequestedTheme = ElementTheme.Default;
                            break;
                        case 4:
                            Player4ButtonGrid.Background = new SolidColorBrush(Colors.Transparent);
                            Player4Button.RequestedTheme = ElementTheme.Default;
                            break;
                    }
                    break;

                case 1:
                    Player2ButtonGrid.Background = new SolidColorBrush(Colors.White);
                    Player2Button.RequestedTheme = ElementTheme.Light;
                    
                    Player1ButtonGrid.Background = new SolidColorBrush(Colors.Transparent);
                    Player1Button.RequestedTheme = ElementTheme.Default;

                    break;

                case 2:
                    if (_players.Length < 3)
                        break;
                    Player3ButtonGrid.Background = new SolidColorBrush(Colors.White);
                    Player3Button.RequestedTheme = ElementTheme.Light;

                    Player2ButtonGrid.Background = new SolidColorBrush(Colors.Transparent);
                    Player2Button.RequestedTheme = ElementTheme.Default;

                    break;

                case 3:
                    if (_players.Length < 4)
                        break;
                    Player4ButtonGrid.Background = new SolidColorBrush(Colors.White);
                    Player4Button.RequestedTheme = ElementTheme.Light;

                    Player3ButtonGrid.Background = new SolidColorBrush(Colors.Transparent);
                    Player3Button.RequestedTheme = ElementTheme.Default;
                    break;
            }

        }


        /// <summary>
        /// Draws the current player's cards on the current player interface
        /// </summary>
        private void _drawPlayerCards()
        {
            Player player = _players[_currentPlayer];

            //Draw current lost treasure
            if (_players[_currentPlayer].LostTreasures.Count != 0)
            {
                _drawPickup(CurrentCardCanvas,
                            (int)(CurrentCardCanvas.ActualWidth / 2 - 25),
                            (int)(CurrentCardCanvas.ActualHeight / 2 - 25),
                            50,
                            50,
                            player.LostTreasures[player.CurrentTreasure]);
            }

            //Draw the found treasures
            for(int i = 0; i < player.FoundTreasures.Count; i++)
            {
                Canvas canvas = null;

                switch (i)
                {
                    case 0:
                        canvas = FoundCard1;
                        break;
                    case 1:
                        canvas = FoundCard2;
                        break;
                    case 2:
                        canvas = FoundCard3;
                        break;
                    case 3:
                        canvas = FoundCard4;
                        break;
                    case 4:
                        canvas = FoundCard5;
                        break;
                    case 5:
                        canvas = FoundCard6;
                        break;
                }

                _drawPickup(canvas,
                            (int)(canvas.ActualWidth / 2 - 25),
                            (int)(canvas.ActualHeight / 2 - 25),
                            50,
                            50,
                            player.FoundTreasures[i]);
            }

        }

        /// <summary>
        /// Initializes player variables
        /// </summary>
        private void _initPlayers(int numPlayers)
        {
            Random random = new Random();
            _players = new Player[numPlayers];
            for (int i = 0; i < numPlayers; i++ )
                _players[i] = new Player(i);

            //set current player to 0 for 'hand' initialization
            _currentPlayer = 0;

            //while deck
            while(Deck.Cards.Count != 0)
            {
                //pick a color, remove it from the list add it to current player's hand,
                //and remove it from the deck.
                int randomNum = random.Next(0, Deck.Cards.Count);

                Color color = Deck.Cards[randomNum];
                _players[_currentPlayer].LostTreasures.Add(color);
                Deck.Cards.RemoveAt(randomNum);

                //if current player's hand is full, move to next player.
                if (_players[_currentPlayer].LostTreasures.Count >= 24 / _players.Length)
                {
                    _currentPlayer++;
                }
            }

            //randomly set current player index
            _currentPlayer = random.Next(0, _players.Length);
        }
        
        /// <summary>
        /// Initializes the deck of pickups
        /// </summary>
        private void _initDeck()
        {
            Deck.Cards = new List<Color>();

            Deck.Cards.Add(Colors.Teal);
            Deck.Cards.Add(Colors.Aquamarine);
            Deck.Cards.Add(Colors.Blue);
            Deck.Cards.Add(Colors.Brown);
            Deck.Cards.Add(Colors.Coral);
            Deck.Cards.Add(Colors.Crimson);
            Deck.Cards.Add(Colors.DarkMagenta);
            Deck.Cards.Add(Colors.RosyBrown);
            Deck.Cards.Add(Colors.Goldenrod);
            Deck.Cards.Add(Colors.Indigo);
            Deck.Cards.Add(Colors.ForestGreen);
            Deck.Cards.Add(Colors.Lime);
            Deck.Cards.Add(Colors.Maroon);
            Deck.Cards.Add(Colors.MidnightBlue);
            Deck.Cards.Add(Colors.Fuchsia);
            Deck.Cards.Add(Colors.Olive);
            Deck.Cards.Add(Colors.Orange);
            Deck.Cards.Add(Colors.Orchid);
            Deck.Cards.Add(Colors.Turquoise);
            Deck.Cards.Add(Colors.Thistle);
            Deck.Cards.Add(Colors.Salmon);
            Deck.Cards.Add(Colors.RoyalBlue);
            Deck.Cards.Add(Colors.SlateBlue);
            Deck.Cards.Add(Colors.SpringGreen);
        }

        /// <summary>
        /// Sets the buttons' MainPage reference so we can
        /// backwards reference the class
        /// </summary>
        private void _initButtons()
        {
            LeftButton1.MainPage = this;
            LeftButton2.MainPage = this;
            LeftButton3.MainPage = this;

            TopButton1.MainPage = this;
            TopButton2.MainPage = this;
            TopButton3.MainPage = this;

            RightButton1.MainPage = this;
            RightButton2.MainPage = this;
            RightButton3.MainPage = this;

            BottomButton1.MainPage = this;
            BottomButton2.MainPage = this;
            BottomButton3.MainPage = this;
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
        /// Initializes the images for the game board, including paths and pickups
        /// </summary>
        private void _initGameBoardImages()
        {
            BoardGrid.Children.Clear();
            _drawBoard();
            _drawFreePiece();
            _initialized = true;
        }


        /// <summary>
        /// Draws the squares on the board
        /// </summary>
        private void _drawBoard()
        {
            //just for easier access
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

            _drawPath(FreePieceCanvas, _gameBoard.FreePiece.Shape, _gameBoard.FreePiece.Rotation, (int)FreePieceCanvas.ActualWidth + 10);
            _drawPickup(FreePieceCanvas,
                        (int)(FreePieceCanvas.ActualWidth / 2 - 25),
                        (int)(FreePieceCanvas.ActualHeight / 2 - 25),
                        50,
                        50,
                        _gameBoard.FreePiece.Color);
        }

        /// <summary>
        /// Draws a square on the game board
        /// </summary>
        /// <param name="node">Node object square represents</param>
        /// <param name="row">Row of grid to build canvas on</param>
        /// <param name="col">Column of grid to build canvas on</param>
        private void _drawNewSquare(Node node, int col, int row)
        {
            //1 square is gameboard.width / 7
            Canvas newCanvas = new Canvas();
            int squareWidth;

            //Board grid's width isn't determined until after the page is drawn
            if (!_initialized)
                squareWidth = (int)(BoardGrid.ActualWidth);
            else
                squareWidth = (int)(BoardGrid.ActualWidth / 7);

            //Stretch properties to fill entire grid square
            newCanvas.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            newCanvas.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;
            newCanvas.Margin = new Thickness(5, 5, 5, 5);
            newCanvas.Background = new SolidColorBrush(Colors.Black);

            Grid.SetColumn(newCanvas, col);
            Grid.SetRow(newCanvas, row);

            BoardGrid.Children.Add(newCanvas);

            _drawPath(newCanvas, node.Shape, node.Rotation, squareWidth);
            _drawPickup(newCanvas,
                        squareWidth / 2 - 20,
                        squareWidth / 2 - 20,
                        30,
                        30,
                        node.Color);
            //_drawImage(row, col, node);
        }


        /// <summary>
        /// Draws the path for a certain piece
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="rotation"></param>
        private void _drawPath(Canvas canvas, Shape shape, int rotation, int squareWidth)
        {
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
        /// Draws an ellipse to represent a pickup on the game board
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        private void _drawPickup(Canvas canvas, int left, int top, int width, int height, Color color)
        {
            if (color == Colors.White)
                return;

            Ellipse ellipse = new Ellipse();

            Canvas.SetLeft(ellipse, left);
            Canvas.SetTop(ellipse, top);
            ellipse.Width = width;
            ellipse.Height = height;

            ellipse.Stroke = new SolidColorBrush(color);
            ellipse.Fill = new SolidColorBrush(color);

            canvas.Children.Add(ellipse);
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

            line.Stroke = new SolidColorBrush(Colors.Gray);
            canvas.Children.Add(line);
        }


        /// <summary>
        /// Draws the free piece on thespecified spot on the board.
        /// Usually used in conjunction with CanvasButton so as to
        /// draw on the canvas corresponding to its button
        /// </summary>
        /// <param name="canvas">Canvas of CanvasButton to draw on</param>
        private void _drawOnButton(Canvas canvas)
        {
            canvas.Children.Clear();

            int squareWidth = (int)canvas.ActualWidth;

            _drawPath(canvas, _gameBoard.FreePiece.Shape, _gameBoard.FreePiece.Rotation, squareWidth + 10);
            _drawPickup(canvas,
                        squareWidth / 2 - 20,
                        squareWidth / 2 - 20,
                        30,
                        30,
                        _gameBoard.FreePiece.Color);
        }

        /// <summary>
        /// Executes the currently pending board shift
        /// </summary>
        public void MakePendingBoardShift()
        {
            if(_pendingLocation != -1 && _pendingDirection != -1)
                _gameBoard.Shift(_pendingLocation, _pendingDirection);

            //invalidate pending location and direction
            _pendingLocation = -1;
            _pendingDirection = -1;

            _initGameBoardImages();
        }


        /// <summary>
        /// Handles the click event for placing the free piece
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlacementButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender == TopButton1.Button)
            {
                _drawOnButton(TopButton1.Canvas);
                _pendingLocation = 1;
                _pendingDirection = GameBoard.MOVE_DOWN;
            }
            else if (sender == TopButton2.Button)
            {
                _drawOnButton(TopButton2.Canvas);
                _pendingLocation = 3;
                _pendingDirection = GameBoard.MOVE_DOWN;
            }
            else if (sender == TopButton3.Button)
            {
                _drawOnButton(TopButton3.Canvas);
                _pendingLocation = 5;
                _pendingDirection = GameBoard.MOVE_DOWN;
            }

            else if (sender == RightButton1.Button)
            {
                _drawOnButton(RightButton1.Canvas);
                _pendingLocation = 1;
                _pendingDirection = GameBoard.MOVE_LEFT;
            }
            else if (sender == RightButton2.Button)
            {
                _drawOnButton(RightButton2.Canvas);
                _pendingLocation = 3;
                _pendingDirection = GameBoard.MOVE_LEFT;
            }
            else if (sender == RightButton3.Button)
            {
                _drawOnButton(RightButton3.Canvas);
                _pendingLocation = 5;
                _pendingDirection = GameBoard.MOVE_LEFT;
            }

            else if (sender == BottomButton1.Button)
            {
                _drawOnButton(BottomButton1.Canvas);
                _pendingLocation = 1;
                _pendingDirection = GameBoard.MOVE_UP;
            }
            else if (sender == BottomButton2.Button)
            {
                _drawOnButton(BottomButton2.Canvas);
                _pendingLocation = 3;
                _pendingDirection = GameBoard.MOVE_UP;
            }
            else if (sender == BottomButton3.Button)
            {
                _drawOnButton(BottomButton3.Canvas);
                _pendingLocation = 5;
                _pendingDirection = GameBoard.MOVE_UP;
            }

            else if (sender == LeftButton1.Button)
            {
                _drawOnButton(LeftButton1.Canvas);
                _pendingLocation = 1;
                _pendingDirection = GameBoard.MOVE_RIGHT;
            }
            else if (sender == LeftButton2.Button)
            {
                _drawOnButton(LeftButton2.Canvas);
                _pendingLocation = 3;
                _pendingDirection = GameBoard.MOVE_RIGHT;
            }
            else if (sender == LeftButton3.Button)
            {
                _drawOnButton(LeftButton3.Canvas);
                _pendingLocation = 5;
                _pendingDirection = GameBoard.MOVE_RIGHT;
            }

        }


        /// <summary>
        /// Handles action for player button clicks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PlayerButtonClick(object sender, RoutedEventArgs e)
        {
            SwitchPlayer();
        }
    }
}
