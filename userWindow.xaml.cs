using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Navigation;

namespace DateTodayProject
{
    /// <summary>
    /// Interaction logic for userWindow.xaml
    /// </summary>
    public partial class userWindow : Window
    {
        private Dictionary<string, Uri> allViews = new Dictionary<string, Uri>();
        public userWindow()
        {
            InitializeComponent();

            allViews.Add("mainPage", new Uri("pages/mainPage.xaml", UriKind.Relative));
            allViews.Add("search", new Uri("pages/search.xaml", UriKind.Relative));
            allViews.Add("mePage", new Uri("pages/mePage.xaml", UriKind.Relative));
            allViews.Add("userProfit", new Uri("pages/userProfit.xaml", UriKind.Relative));
            allViews.Add("aboutUs", new Uri("pages/aboutUs.xaml", UriKind.Relative));
            mainFrame.Navigate(allViews["mainPage"]);

        }
        //导航
        public void page1Button(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(allViews["mainPage"]);
        }
        public void searchButton(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(allViews["search"]); 
        }
        public void page3Button(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(allViews["mePage"]);
            
           

        }


        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Label_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            mainFrame.Navigate(allViews["aboutUs"]);
        }
    }
}
