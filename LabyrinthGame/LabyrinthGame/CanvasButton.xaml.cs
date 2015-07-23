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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LabyrinthGame
{
    public enum ButtonType
    {
        BoardShift,
        PlayerMove
    }
    public partial class CanvasButton : UserControl
    {
        
        public Button Button;
        public Canvas Canvas;

        public ButtonType ButtonType { get; set; }

        public MainPage MainPage;

        public event RoutedEventHandler Click;

        public CanvasButton()
        {
            this.InitializeComponent();
            Button = button;
            Canvas = canvas;
        }

        /// <summary>
        /// Handles the routed click event for the CanvasButton parent object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleClick(object sender, RoutedEventArgs e)
        {
            if (this.Click != null)
            {
                this.Click(sender, e);

                //conditions for clickable button types
                if((MainPage.Movable && ButtonType == ButtonType.PlayerMove) || (ButtonType == ButtonType.BoardShift && !MainPage.Movable))
                    ConfirmationButtons.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }

        /// <summary>
        /// Handles confirmation buttons, which actually perform the actions on the board
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmationClick(object sender, RoutedEventArgs e)
        {
            if (sender == YesButton)
            {
                switch (ButtonType)
                {
                    case ButtonType.BoardShift:
                        MainPage.MakePendingBoardShift();
                        break;

                    case ButtonType.PlayerMove:
                        MainPage.MakePendingPlayerMove();
                        break;
                }
                
            }

            ConfirmationButtons.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

    }
}
