using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;

namespace GuessTheColor
{
    public partial class MainPage : PhoneApplicationPage
    {
        private GameViewModel gameViewModel;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            
            this.Loaded += (sender, args) => window.WindowSize = new Size(Application.Current.RootVisual.RenderSize.Width, double.NaN);
            this.gameViewModel = new GameViewModel();
            this.DataContext = this.gameViewModel;
            this.listBox.ItemsSource = this.gameViewModel.Rows;
            this.listBoxHeader.ItemsSource = this.gameViewModel.HeaderRow;
        }

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            gameViewModel.OnColorClick(btn.Background);
            //this.listBox.ItemsSource = this.gameViewModel.Rows;
        }

        private void OnNewGameClick(object sender, EventArgs e)
        {
            gameViewModel.NewGame();
        }

        private void OnResetClick(object sender, EventArgs e)
        {
            gameViewModel.Reset();
        }

    }
}