using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Common;
using Server.Business;

namespace Client
{
    /// <summary>
    /// Interaction logic for RequestDelivery.xaml
    /// </summary>
    public partial class ViewStats : Page
    {
        

        public ViewStats()
        {
            
            InitializeComponent();

            triples.Columns.Add(new DataGridTextColumn { Header = "Origin", Binding = new Binding("Origin") });
            triples.Columns.Add(new DataGridTextColumn { Header = "Destination", Binding = new Binding("Destination") });
            triples.Columns.Add(new DataGridTextColumn { Header = "Priority", Binding = new Binding("Priority") });
            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void backToHomeButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new System.Uri("Home.xaml", UriKind.RelativeOrAbsolute));
        }

        

    }
}
