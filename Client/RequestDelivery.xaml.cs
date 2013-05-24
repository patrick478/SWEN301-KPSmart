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

        delegate void PricesDelegate(IDictionary<PathType, int> prices);
        PricesDelegate priceDelegate;

        delegate void ReturnHomeDelegate();
        ReturnHomeDelegate returnHomeDelegate;


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
            _clientController.DeliveryOK+= new ClientController.DeliveryConfirmedDelegate(DeliveryConfirmed);

        }

        private void DeliveryConfirmed()
        {
            MessageBox.Show("Delivery successfully added! Streets ahead!");

            try
            {
                this.Dispatcher.VerifyAccess();
                ReturnHome();
            }
            catch (InvalidOperationException e)
            {
                if (returnHomeDelegate == null)
                    returnHomeDelegate = new ReturnHomeDelegate(ReturnHome);
                this.Dispatcher.Invoke(returnHomeDelegate);
            }
        }

        public void ReturnHome()
        {
            NavigationService.Navigate(new Home(_clientState));
        }


        public void DeliveryOptions_Returned(IDictionary<PathType, int> prices)
        {
            try
            {
                air.Dispatcher.VerifyAccess();
                populateStats(prices);
            }
            catch (InvalidOperationException e)
            {
                if (priceDelegate == null)
                    priceDelegate = new PricesDelegate(populateStats);
                air.Dispatcher.Invoke(priceDelegate, System.Windows.Threading.DispatcherPriority.Normal, prices);
            }
        }

        public void populateStats(IDictionary<PathType, int> prices)
        {

            var standardPrice = -1;
            var standardExpressPrice = -1;
            var airPrice = -1;
            var airExpressPrice = -1;

            if(prices != null){
                standardPrice = prices.Keys.Contains(PathType.Standard) ? prices[PathType.Standard] : -1;
                standardExpressPrice = prices.Keys.Contains(PathType.Express) ? prices[PathType.Express] : -1;
                airPrice = prices.Keys.Contains(PathType.AirStandard) ? prices[PathType.AirStandard] : -1;
                airExpressPrice = prices.Keys.Contains(PathType.AirExpress) ? prices[PathType.AirExpress] : -1;
            }

            if (standardPrice < 0 && standardExpressPrice < 0 && airPrice < 0 && airExpressPrice < 0)
                submitDeliveryType.IsEnabled = false;
            else
                submitDeliveryType.IsEnabled = true;



            submitDeliveryType.Visibility = Visibility.Visible;
            submitDeliveryType.Content = "Sumbit Delivery";
            submitDeliveryType.Click += new RoutedEventHandler(submitDeliveryType_Click);
            air.Visibility = Visibility.Visible;
            airExpress.Visibility = Visibility.Visible;
            standard.Visibility = Visibility.Visible;
            standardExpress.Visibility = Visibility.Visible;
            if (standardPrice == -1)
            {
                air.Content = "Air: none available";
                air.IsEnabled = false;
            }
            else
            {
                air.Content = "Air: " + airPrice;
            }

            if (airExpressPrice == -1)
            {
                airExpress.Content = "Air Express: none available";
                airExpress.IsEnabled = false;
            }
            else
            {
                airExpress.Content = "Air Express: " + airExpressPrice;
            }

            if (standardPrice == -1)
            {
                standard.Content = "Standard: none available";
                standard.IsEnabled = false;
            }
            else
            {
                standard.Content = "Standard: " + standardPrice;
            }
            if (standardExpressPrice == -1)
            {
                standardExpress.Content = "Standard Express: none available";
                standardExpress.IsEnabled = false;
            }
            else
            {
                standardExpress.Content = "Standard Express: " + standardExpressPrice;
            }
            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void backToHomeButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Home(_clientState));
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

        private void submitDeliveryType_Click(object sender, RoutedEventArgs e)
        {
            

            PathType type = PathType.Standard;

            if ((bool)standard.IsChecked)
                type = PathType.Standard;
            else if ((bool)standardExpress.IsChecked)
                type = PathType.Express;
            else if ((bool)air.IsChecked)
                type = PathType.AirStandard;
            else if ((bool)airExpress.IsChecked)
                type = PathType.AirExpress;
            
            _clientController.ChooseDelivery(type);
            

        }

    }
}
