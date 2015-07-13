using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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

        private void PlacementButtonClick(object sender, RoutedEventArgs e)
        {
            if()
        }

    }
}
