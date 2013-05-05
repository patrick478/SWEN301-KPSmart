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

namespace Client
{
    /// <summary>
    /// Interaction logic for RequestDelivery.xaml
    /// </summary>
    public partial class RequestDelivery : Page
    {
        public RequestDelivery()
        {
            InitializeComponent();
            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new System.Uri("Home.xaml", UriKind.RelativeOrAbsolute));
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            
            label2.Visibility = Visibility.Hidden;
            label3.Visibility = Visibility.Hidden;
            label4.Visibility = Visibility.Hidden;
            label5.Visibility = Visibility.Hidden;

            origin.Visibility = Visibility.Hidden;
            destination.Visibility = Visibility.Hidden;
            weight.Visibility = Visibility.Hidden;
            volume.Visibility = Visibility.Hidden;

            submitButton.Visibility = Visibility.Hidden;
            

            //placeholder prices until Josh pushes code
            var standaredCheapPrice = 100;
            var standaredFastPrice = 150;
            var airFastPrice = 75;
            var airCheapPrice = 50;

            airCheap.Visibility = Visibility.Visible;
            airFast.Visibility = Visibility.Visible;
            standardCheap.Visibility = Visibility.Visible;
            standardFast.Visibility = Visibility.Visible;

            airCheap.Content = "Air cheap: " + airCheapPrice;
            airFast.Content = "Air cheap: " + airFastPrice;
            standardCheap.Content = "Air cheap: " + standaredCheapPrice;
            standardFast.Content = "Air cheap: " + standaredFastPrice;
        }

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
           
        }

    }
}
