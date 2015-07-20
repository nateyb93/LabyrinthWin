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
    public partial class CanvasButton : UserControl
    {
        public Button Button;
        public Canvas Canvas;

        public MainPage MainPage;

        public event RoutedEventHandler Click;

        public CanvasButton()
        {
            this.InitializeComponent();
            Button = button;
            Canvas = canvas;
        }

        private void HandleClick(object sender, RoutedEventArgs e)
        {
            if (this.Click != null)
            {
                this.Click(sender, e);
            }
        }

        protected void OnClick(EventArgs e)
        {

        }
    }
}
