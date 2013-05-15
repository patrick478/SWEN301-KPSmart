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
    public partial class RequestDelivery : Page
    {
        // initialise routeService
            
            // initialise pathfinder
        private readonly PathFinder _pathFinder;
        private readonly ClientState _clientState;
        private DeliveryService _pathfindService;

        public RequestDelivery()
        {
            
            InitializeComponent();
            var state = new CurrentState();
            var routeService = new RouteService(state);
            _pathFinder = new PathFinder(routeService);
            _clientState = new ClientState();
            _pathfindService = new DeliveryService(state, _pathFinder);

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void backToHomeButton_Click(object sender, RoutedEventArgs e)
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

            try
            {
                int originNode = Convert.ToInt32(origin.Text);
                int destNode = Convert.ToInt32(destination.Text);
               



                var results = _pathfindService.GetBestRoutes(0,originNode, destNode, Convert.ToInt32(weight),
                                                                                           Convert.ToInt32(volume));

                
                var standardDelivery = results[PathType.Standard];
                var expressDelivery = results[PathType.Express];
                var airStandardDelivery  = results[PathType.AirStandard];
                var airExpressDelivery = results[PathType.AirExpress];

                var standardPrice = 0;
                var standardExpressPrice = 0;
                var airPrice = 0;
                var airExpressPrice = 0;


                

                air.Visibility = Visibility.Visible;
                airExpress.Visibility = Visibility.Visible;
                standard.Visibility = Visibility.Visible;
                standardExpress.Visibility = Visibility.Visible;

                air.Content = "Air: " + airPrice;
                airExpress.Content = "Air Express: " + airExpressPrice;
                standard.Content = "Standard: " + standardPrice;
                standardExpress.Content = "Standard Express: " + standardExpressPrice;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
           
        }

        private void standardCheap_Checked(object sender, RoutedEventArgs e)
        {

        }

    }
}
