using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Windows.Media;

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
        }

        private void OnNewGameClick(object sender, EventArgs e)
        {
            gameViewModel.NewGame();
        }

        private void OnResetClick(object sender, EventArgs e)
        {
            gameViewModel.Reset();
        }

        private void OnPauseClick(object sender, EventArgs e)
        {
            gameViewModel.Pause();
        }

        private ChildControl FindVisualChild<ChildControl>(DependencyObject DependencyObj) where ChildControl : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(DependencyObj); i++)
            {
                DependencyObject Child = VisualTreeHelper.GetChild(DependencyObj, i);

                if (Child != null && Child is ChildControl)
                {
                    return (ChildControl)Child;
                }
                else
                {
                    ChildControl ChildOfChild = FindVisualChild<ChildControl>(Child);

                    if (ChildOfChild != null)
                    {
                        return ChildOfChild;
                    }
                }
            }
            return null;
        }
        /*
        private void GetSelectedCheckObjItem()
        {
            try
            {
                for (int i = 0; i < listBox.ItemCount; i++)
                {
                    // Get a all list items from listbox
                    ListBoxItem ListBoxItemObj = (ListBoxItem)listBox.ItemContainerGenerator.ContainerFromItem(listBox1.Items[i]);

                    // find a ContentPresenter of that list item.. [Call FindVisualChild Method]
                    ContentPresenter ContentPresenterObj = FindVisualChild<ContentPresenter>(ListBoxItemObj);

                    // call FindName on the DataTemplate of that ContentPresenter
                    DataTemplate DataTemplateObj = ContentPresenterObj.ContentTemplate;
                    CheckBox Chk = (CheckBox)DataTemplateObj.FindName("ChkList", ContentPresenterObj);

                    // get a selected checkbox items.
                    if (Chk.IsChecked == true)
                    {
                        MessageBox.Show(Chk.Content.ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }*/

    }
}