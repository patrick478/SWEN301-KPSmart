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

            foreach (var country in _clientState.GetAllCountries())
            {
                //this.originComboBox.Items.Add(countries.Name);
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Content = country.Name;
                cbi.Tag = country.ID;
                this.origin.Items.Add(cbi);

                ComboBoxItem cbi2 = new ComboBoxItem();
                cbi2.Content = country.Name;
                cbi2.Tag = country.ID;
                this.destination.Items.Add(cbi2);
            }





            _clientController.OptionsReceived += new ClientController.DeliveryOptionsDelegate(DeliveryOptions_Returned);

        }


        public void DeliveryOptions_Returned(IDictionary<PathType, int> prices)
        {
            MessageBox.Show("Recieved options!!!");


            var standardDelivery = prices[PathType.Standard];
            var expressDelivery = prices[PathType.Express];
            var airStandardDelivery = prices[PathType.AirStandard];
            var airExpressDelivery = prices[PathType.AirExpress];

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

           
            int originNode = Convert.ToInt32(origin.Tag);
            int destNode = Convert.ToInt32(destination.Tag);
               



            
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
