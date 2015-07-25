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
        /// Stores how many players there are in the current game
        /// </summary>
        private int _numPlayers;

        /// <summary>
        /// Direction of pending board shift
        /// </summary>
        private int _pendingDirection;

        /// <summary>
        /// Location of pending board shift
        /// </summary>
        private int _pendingLocation;

        /// <summary>
        /// X coordinate of pending move on board
        /// </summary>
        private int _pendingMoveX;

        /// <summary>
        /// X coordinate of pending move on board
        /// </summary>
        private int _pendingMoveY;

        /// <summary>
        /// Index of current player in the _players array
        /// </summary>
        private int _currentPlayer;

        /// <summary>
        /// Determines whether a location selected for a board shift
        /// is valid
        /// </summary>
        public bool ValidShift = false;

        /// <summary>
        /// Says whether the game is waiting for confirmation
        /// </summary>
        public bool WaitingForConfirmation;

        private CanvasButton _cantPressButton;

        /// <summary>
        /// Toggles whether the current player is movable
        /// </summary>
        public bool PlayerIsMovable;

        public MainPage()
        {
            this.InitializeComponent();
            PlayerIsMovable = false;
            _numPlayers = 2;

            _cantPressButton = new CanvasButton();
            _gameBoard = new GameBoard();
            ValidShift = true;

            _pendingDirection = -1;
            _pendingLocation = -1;
        }

        /// <summary>
        /// Initializes images and 
        /// </summary>
        private void _beginGame()
        {
            //initialize game objects
            _initDeck();
            _initPlayers(_numPlayers);
            _initGameBoardImages();
            _initButtons();
            _drawPlayerCards();

            //the extra call to switch player highlights the player on the ui;
            //lazy hack so i don't have to make ui changes outside of switchplayer()
            SwitchPlayer();

            //Add players to their starting locations

            PreGameGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            GameOverGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void _endGame()
        {
            WinnerText.Text = _players[_currentPlayer].Name + " is the winner!";
            GameOverGrid.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Switches the current player
        /// </summary>
        public void SwitchPlayer()
        {
            _currentPlayer++;
            if (_currentPlayer >= _numPlayers)
            {
                _currentPlayer = 0;
            }

            _playerSwitchUiUpdate();

            _drawPlayerCards();

            Player currentPlayer = _players[_currentPlayer];

            CurrentPlayerTextBlock.Text = currentPlayer.Name + "'s turn";
            CurrentPlayerTextBlock.Foreground = new SolidColorBrush(currentPlayer.Color);

            MessageText(currentPlayer.Name + ", please place the free tile.");
        }


        /// <summary>
        /// Updates the background for each player button based on whose turn it is
        /// </summary>
        private void _playerSwitchUiUpdate()
        {
            switch (_currentPlayer)
            {
                case 0:
                    //Make background light and reverse theme for contrast reasons
                    Player1ButtonGrid.Background = new SolidColorBrush(Colors.White);
                    Player1Button.RequestedTheme = ElementTheme.Light;

                    //this switch determines whose turn it was prior; this differs
                    //based on how many players are in the game
                    switch (_numPlayers)
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
                    if (_numPlayers < 3)
                        break;
                    Player3ButtonGrid.Background = new SolidColorBrush(Colors.White);
                    Player3Button.RequestedTheme = ElementTheme.Light;

                    Player2ButtonGrid.Background = new SolidColorBrush(Colors.Transparent);
                    Player2Button.RequestedTheme = ElementTheme.Default;

                    break;

                case 3:
                    if (_numPlayers < 4)
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
                            0,
                            0,
                            50,
                            50,
                            player.LostTreasures[0]);
            }

            //Draw the found treasures
            for(int i = 0; i < 6; i++)
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

                canvas.Children.Clear();

                Color color = Color.FromArgb(255, 29, 29, 29);
                if (i < player.FoundTreasures.Count)
                {
                    color = player.FoundTreasures[i];
                }

                _drawPickup(canvas,
                            0,
                            0,
                            25,
                            25,
                            color);
            }

        }

        /// <summary>
        /// Initializes player variables
        /// </summary>
        private void _initPlayers(int numPlayers)
        {
            Random random = new Random();
            _players = new Player[numPlayers];

            //determine player's name and initialize player object at index
            for (int i = 0; i < numPlayers; i++)
            {
                string name = "";
                switch (i)
                {
                    case 0:
                        name = Player1TextBox.Text;
                        break;
                    case 1:
                        name = Player2TextBox.Text;
                        break;
                    case 2:
                        name = Player3TextBox.Text;
                        break;
                    case 3:
                        name = Player4TextBox.Text;
                        break;
                }

                _players[i] = new Player(i, name);
            }
                
            //set current player to 0 for 'hand' initialization
            _currentPlayer = 0;

            _dealCards();

            //randomly set current player index
            _currentPlayer = random.Next(0, _numPlayers);

            //add players to their starting locations
            _gameBoard.Board[0, 0].Players.Add(_players[0]);
            Player1Button.Label = _players[0].Name;
            _gameBoard.Board[6, 0].Players.Add(_players[1]);
            Player2Button.Label = _players[1].Name;

            //conditionally enable other players
            if (_numPlayers >= 3)
            {
                _gameBoard.Board[6, 6].Players.Add(_players[2]);
                Player3Button.Label = _players[2].Name;
                Player3ButtonGrid.Visibility = Visibility.Visible;

                if (_numPlayers == 4)
                {

                    Player4ButtonGrid.Visibility = Visibility.Visible;
                    Player4Button.Label = _players[3].Name;
                    _gameBoard.Board[0, 6].Players.Add(_players[3]);
                }
            }
        }

        /// <summary>
        /// Deals cards to all participating players
        /// </summary>
        private void _dealCards()
        {
            Random random = new Random();

            //while deck
            while (Deck.Cards.Count != 0)
            {
                //pick a color, remove it from the list add it to current player's hand,
                //and remove it from the deck.
                int randomNum = random.Next(0, Deck.Cards.Count);

                Color color = Deck.Cards[randomNum];
                _players[_currentPlayer].LostTreasures.Add(color);
                Deck.Cards.RemoveAt(randomNum);

                //if current player's hand is full, move to next player.
                if (_players[_currentPlayer].LostTreasures.Count == 6)
                {
                    _currentPlayer++;
                    if (_currentPlayer > _numPlayers - 1)
                    {
                        break;
                    }
                }
            }
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
        }


        /// <summary>
        /// Draws the squares on the board
        /// </summary>
        private void _drawBoard()
        {
            //just for easier access
            BoardNode[,] spaces = _gameBoard.Board;

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

            _drawRect(FreePieceCanvas, 0, 0, (int)FreePieceCanvas.ActualWidth, (int)FreePieceCanvas.ActualHeight, Colors.Black, Colors.White);
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
        private void _drawNewSquare(BoardNode node, int col, int row)
        {
            //1 square is gameboard.width / 7
            CanvasButton canvasButton = new CanvasButton();

            Canvas newCanvas = canvasButton.Canvas;
            
            int squareWidth = (int)(BoardGrid.ActualWidth / 7);

            //Stretch properties to fill entire grid square
            //Set button type and click
            canvasButton.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            canvasButton.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;
            canvasButton.ButtonType = ButtonType.PlayerMove;
            canvasButton.MainPage = this;
            canvasButton.Click += MoveButtonClick;

            newCanvas.Background = new SolidColorBrush(Colors.Black);

            Grid.SetColumn(canvasButton, col);
            Grid.SetRow(canvasButton, row);

            BoardGrid.Children.Add(canvasButton);

            _drawBorder(newCanvas, col, row, squareWidth);
            _drawPath(newCanvas, node.Shape, node.Rotation, squareWidth);
            _drawPlayersOnSquare(newCanvas, row, col);
            _drawPickup(newCanvas,
                        squareWidth / 2 - 20,
                        squareWidth / 2 - 20,
                        30,
                        30,
                        node.Color);
            //_drawImage(row, col, node);
        }


        /// <summary>
        /// Draws a colored border around corner pieces
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <param name="squareWidth"></param>
        private void _drawBorder(Canvas canvas, int col, int row, int squareWidth)
        {
            //don't do anything if we're not in a corner
            if((col != 0 && col != 6) || (row != 0 && row != 6))
            {
                return;
            }

            Color outerColor = Colors.Transparent;

            if (col == 0 && row == 0)
            {
                //draw outer square for border
                outerColor = Colors.Blue;
            }
            else if (col == 6 && row == 0)
            {
                outerColor = Colors.Lime;
            }
            else if (col == 6 && row == 6)
            {
                outerColor = Colors.Yellow;
            }
            else if(col == 0 && row == 6)
            {
                outerColor = Colors.Red;
            }

            _drawRect(canvas, 0, 0, squareWidth - 10, squareWidth - 10, outerColor, outerColor);
            _drawRect(canvas,
                      2,
                      2,
                      squareWidth - 14,
                      squareWidth - 14,
                      Colors.Black,
                      outerColor);
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
                    _drawPathS(canvas, rotation, squareWidth);
                    break;


                case Shape.L:
                    _drawPathL(canvas, rotation, squareWidth);
                    break;


                case Shape.T:
                    _drawPathT(canvas, rotation, squareWidth);
                    break;
            }
        }

        /// <summary>
        /// Helper method for drawing a straight path
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="rotation"></param>
        /// <param name="squareWidth"></param>
        private void _drawPathS(Canvas canvas, int rotation, int squareWidth)
        {
            if (rotation == 0 || rotation == 2)
                _drawLine(canvas, squareWidth / 2 - 5, 0, squareWidth / 2 - 5, squareWidth - 10, 20);
            else
                _drawLine(canvas, 0, squareWidth / 2 - 5, squareWidth - 10, squareWidth / 2 - 5, 20);
        }

        /// <summary>
        /// Helper method for drawing an L-shaped path
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="rotation"></param>
        /// <param name="squareWidth"></param>
        private void _drawPathL(Canvas canvas, int rotation, int squareWidth)
        {
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
        }


        /// <summary>
        /// Helper method for drawing a T-shaped path
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="rotation"></param>
        /// <param name="squareWidth"></param>
        private void _drawPathT(Canvas canvas, int rotation, int squareWidth)
        {
            //long line
            if (rotation == 1 || rotation == 3)
                _drawLine(canvas, squareWidth / 2 - 5, 0, squareWidth / 2 - 5, squareWidth - 10, 20);
            else
                _drawLine(canvas, 0, squareWidth / 2 - 5, squareWidth - 10, squareWidth / 2 - 5, 20);

            //short line
            if (rotation == 0)
                _drawLine(canvas, squareWidth / 2 - 5, squareWidth / 2 - 5, squareWidth / 2 - 5, squareWidth - 10, 20);
            else if (rotation == 1)
                _drawLine(canvas, 0, squareWidth / 2 - 5, squareWidth / 2 - 5, squareWidth / 2 - 5, 20);
            else if (rotation == 2)
                _drawLine(canvas, squareWidth / 2 - 5, 0, squareWidth / 2 - 5, squareWidth / 2 - 5, 20);
            else
                _drawLine(canvas, squareWidth / 2 - 5, squareWidth / 2 - 5, squareWidth - 10, squareWidth / 2 - 5, 20);
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
        /// Draws each player on the specified square
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        private void _drawPlayersOnSquare(Canvas canvas, int row, int column)
        {
            int squareWidth = (int)(BoardGrid.ActualWidth / 7);

            //loop throught players.
            //if a player is found to be at the specified spot on the board,
            //draw their representation on the game board
            for (int i = 0; i < _numPlayers; i++)
            {
                if (_players[i].CurrentX == column && _players[i].CurrentY == row)
                {
                    switch (i)
                    {
                        case 0:
                            _drawRect(canvas,
                                      5,
                                      5,
                                      20,
                                      20,
                                      Colors.Blue,
                                      Colors.White);
                            break;

                        case 1:
                            _drawRect(canvas,
                                      squareWidth - 35,
                                      5,
                                      20,
                                      20,
                                      Colors.Lime,
                                      Colors.White);
                            break;

                        case 2:
                            _drawRect(canvas,
                                      squareWidth - 35,
                                      squareWidth - 35,
                                      20,
                                      20,
                                      Colors.Yellow,
                                      Colors.White);
                            break;

                        case 3:
                            _drawRect(canvas,
                                      5,
                                      squareWidth - 35,
                                      20,
                                      20,
                                      Colors.Red,
                                      Colors.White);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Draws a rectangle on a canvas
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        private void _drawRect(Canvas canvas, int x, int y, int w, int h, Color color, Color stroke)
        {
            Rectangle rect = new Rectangle();

            rect.Width = w;
            rect.Height = h;
            rect.Stroke = new SolidColorBrush(stroke);
            rect.StrokeThickness = 2;
            rect.Fill = new SolidColorBrush(color);

            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);

            canvas.Children.Add(rect);
        }


        /// <summary>
        /// Executes the currently pending board shift
        /// </summary>
        public void MakePendingBoardShift()
        {
            if (!ValidShift)
            {
                return;
            }
            if (_pendingLocation != -1 && _pendingDirection != -1)
            {
                _gameBoard.Shift(_pendingLocation, _pendingDirection);
                PlayerIsMovable = true;
            }
            //invalidate pending location and direction
            _pendingLocation = -1;
            _pendingDirection = -1;

            _initGameBoardImages();

            MessageText(_players[_currentPlayer].Name + ", please move your piece.");
        }

        /// <summary>
        /// Executes the currently pending player move
        /// </summary>
        public void MakePendingPlayerMove()
        {
            Player player = _players[_currentPlayer];

            BoardNode[,] board = _gameBoard.Board;

            if (_pendingMoveX != -1 && _pendingMoveY != -1)
            {
                //check if there's a valid path between the two active nodes
                bool valid = _gameBoard.IsValidPath(board[player.CurrentX, player.CurrentY],
                                                    board[_pendingMoveX, _pendingMoveY]);
                if (valid)
                {
                    board[player.CurrentX, player.CurrentY].Players.Remove(player);
                    player.Move(_pendingMoveX, _pendingMoveY);
                    board[player.CurrentX, player.CurrentY].Players.Add(player);
                    player.FindTreasure(board[player.CurrentX, player.CurrentY].Color);
                    PlayerIsMovable = false;

                    if (player.HasWon())
                    {
                        _endGame();
                        return;
                    }

                    SwitchPlayer();
                    _initGameBoardImages();
                }
                else
                {
                    AlertText("That space can't be reached from your current location. Try a different spot!");
                    return;
                }
            }

            //invalidate pending location and direction
            _pendingMoveX = -1;
            _pendingMoveX = -1;
        }


        /// <summary>
        /// Updates the text display with an alert to the user
        /// </summary>
        /// <param name="text"></param>
        public void AlertText(string text)
        {
            AlertTextBlock.Text = text;
            AlertTextBlock.Foreground = new SolidColorBrush(Colors.Red);
        }

        /// <summary>
        /// Updates the text display with a message to the user
        /// </summary>
        /// <param name="text"></param>
        public void MessageText(string text)
        {
            AlertTextBlock.Text = text;
            AlertTextBlock.Foreground = new SolidColorBrush(Colors.White);
        }

        /// <summary>
        /// Handles the click event for placing the free piece
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlacementButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender == _cantPressButton.Button)
            {
                ValidShift = false;
                return;
            }
            //determine location and direction of board move for camera
            if (sender == TopButton1.Button)
            {
                _cantPressButton = BottomButton1;
                _pendingLocation = 1;
                _pendingDirection = GameBoard.MOVE_DOWN;
            }
            else if (sender == TopButton2.Button)
            {
                _cantPressButton = BottomButton2;
                _pendingLocation = 3;
                _pendingDirection = GameBoard.MOVE_DOWN;
            }
            else if (sender == TopButton3.Button)
            {
                _cantPressButton = BottomButton3;
                _pendingLocation = 5;
                _pendingDirection = GameBoard.MOVE_DOWN;
            }

            else if (sender == RightButton1.Button)
            {
                _cantPressButton = LeftButton1;
                _pendingLocation = 1;
                _pendingDirection = GameBoard.MOVE_LEFT;
            }
            else if (sender == RightButton2.Button)
            {
                _cantPressButton = LeftButton2;
                _pendingLocation = 3;
                _pendingDirection = GameBoard.MOVE_LEFT;
            }
            else if (sender == RightButton3.Button)
            {
                _cantPressButton = LeftButton3;
                _pendingLocation = 5;
                _pendingDirection = GameBoard.MOVE_LEFT;
            }

            else if (sender == BottomButton1.Button)
            {
                _cantPressButton = TopButton1;
                _pendingLocation = 1;
                _pendingDirection = GameBoard.MOVE_UP;
            }
            else if (sender == BottomButton2.Button)
            {
                _cantPressButton = TopButton2;
                _pendingLocation = 3;
                _pendingDirection = GameBoard.MOVE_UP;
            }
            else if (sender == BottomButton3.Button)
            {
                _cantPressButton = TopButton3;
                _pendingLocation = 5;
                _pendingDirection = GameBoard.MOVE_UP;
            }

            else if (sender == LeftButton1.Button)
            {
                _cantPressButton = RightButton1;
                _pendingLocation = 1;
                _pendingDirection = GameBoard.MOVE_RIGHT;
            }
            else if (sender == LeftButton2.Button)
            {
                _cantPressButton = RightButton2;
                _pendingLocation = 3;
                _pendingDirection = GameBoard.MOVE_RIGHT;
            }
            else if (sender == LeftButton3.Button)
            {
                _cantPressButton = RightButton3;
                _pendingLocation = 5;
                _pendingDirection = GameBoard.MOVE_RIGHT;
            }

            ValidShift = true;

        }

        /// <summary>
        /// Handles the click action for move buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MoveButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Grid parent = VisualTreeHelper.GetParent(button) as Grid;

            CanvasButton canvasButton = VisualTreeHelper.GetParent(parent) as CanvasButton;

            _pendingMoveX = Grid.GetColumn(canvasButton);
            _pendingMoveY = Grid.GetRow(canvasButton);
        }

        /// <summary>
        /// Handles action for player button clicks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PlayerButtonClick(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Handles the click event for the add player button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddPlayerButton_Click(object sender, RoutedEventArgs e)
        {
            if (_numPlayers >= 4)
            {
                //do nothing if _numPlayers is more than 4
                return;
            }

            _numPlayers++;
            if (_numPlayers == 3)
                Player3TextBox.Visibility = Windows.UI.Xaml.Visibility.Visible;
            else
                Player4TextBox.Visibility = Windows.UI.Xaml.Visibility.Visible;

            //remove/add ui restrictions based on number of players
            RemovePlayerButton.IsEnabled = true;

            if (_numPlayers >= 4)
            {
                AddPlayerButton.IsEnabled = false;
            }
        }
        
        /// <summary>
        /// Handles the click action for the remove player button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemovePlayerButton_Click(object sender, RoutedEventArgs e)
        {
            if (_numPlayers <= 2)
            {
                return;
            }

            _numPlayers--;

            if (_numPlayers == 3)
                Player4TextBox.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            else
                Player3TextBox.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            AddPlayerButton.IsEnabled = true;

            if (_numPlayers <= 2)
            {
                RemovePlayerButton.IsEnabled = false;
            }
        }

        /// <summary>
        /// Handles the click action for PlayButton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            _beginGame();
        }

        /// <summary>
        /// Navigates back to the main menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainMenuPage));
        }


    }
}
