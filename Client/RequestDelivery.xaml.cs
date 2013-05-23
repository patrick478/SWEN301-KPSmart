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
        private ClientController _clientController;

        public RequestDelivery(ClientController clientCon, ClientState clientState)
        {
            
            InitializeComponent();
            var state = new CurrentState();
            var routeService = new RouteService(state);
            _pathFinder = new PathFinder(routeService);
            _clientState = clientState;
            _pathfindService = new DeliveryService(state, _pathFinder);
            _clientController = clientCon;

            foreach (var routeNode in clientState.GetAllRouteNodes())
            {
                ComboBoxItem cbi = new ComboBoxItem();
                if (routeNode is DistributionCentre)
                    cbi.Content = ((DistributionCentre)routeNode).Name;
                else if (routeNode is InternationalPort)
                    cbi.Content = routeNode.Country.Name;
                cbi.Tag = routeNode.ID;
                this.origin.Items.Add(cbi);

                ComboBoxItem cbi2 = new ComboBoxItem();
                if (routeNode is DistributionCentre)
                    cbi2.Content = ((DistributionCentre)routeNode).Name;
                else if (routeNode is InternationalPort)
                    cbi2.Content = routeNode.Country.Name;
                cbi2.Tag = routeNode.ID;
                this.destination.Items.Add(cbi2);
            }




            _clientController.OptionsReceived += new ClientController.DeliveryOptionsDelegate(DeliveryOptions_Returned);

        }


        public void DeliveryOptions_Returned(IDictionary<PathType, int> prices)
        {
            MessageBox.Show("Recieved options!!!");


            var standardPrice = prices[PathType.Standard];
            var standardExpressPrice = prices[PathType.Express];
            var airPrice = prices[PathType.AirStandard];
            var airExpressPrice = prices[PathType.AirExpress];

           


            air.Visibility = Visibility.Visible;
            airExpress.Visibility = Visibility.Visible;
            standard.Visibility = Visibility.Visible;
            standardExpress.Visibility = Visibility.Visible;

            air.Content = "Air: " + airPrice;
            airExpress.Content = "Air Express: " + airExpressPrice;
            standard.Content = "Standard: " + standardPrice;
            standardExpress.Content = "Standard Express: " + standardExpressPrice;

        }

        public void populateStats(IDictionary<PathType, int> prices)
        {
            MessageBox.Show("Recieved options!!!");


            var standardPrice = prices[PathType.Standard];
            var standardExpressPrice = prices[PathType.Express];
            var airPrice = prices[PathType.AirStandard];
            var airExpressPrice = prices[PathType.AirExpress];




            air.Visibility = Visibility.Visible;
            airExpress.Visibility = Visibility.Visible;
            standard.Visibility = Visibility.Visible;
            standardExpress.Visibility = Visibility.Visible;

            air.Content = "Air: " + airPrice;
            airExpress.Content = "Air Express: " + airExpressPrice;
            standard.Content = "Standard: " + standardPrice;
            standardExpress.Content = "Standard Express: " + standardExpressPrice;
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

            ComboBoxItem orig = origin.SelectedItem as ComboBoxItem;
            ComboBoxItem dest = destination.SelectedItem as ComboBoxItem;

           
            int originNode = Convert.ToInt32(orig.Tag);
            int destNode = Convert.ToInt32(dest.Tag);
               



            
            _clientController.RequestDelivery(originNode, destNode, Convert.ToInt32(weight.Text),
                                                                                           Convert.ToInt32(volume.Text));
                
                
            
            
        }

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
           
        }

        private void standardCheap_Checked(object sender, RoutedEventArgs e)
        {

        }

    }
}
