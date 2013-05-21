using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Client.Countries;
using Client.DialogBoxes;
using Common;
using Server.Business;
using Server.Data;
using Server.Network;

namespace Client
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home
    {
        private readonly ClientState _clientState;
        private readonly ClientController _clientCon;

        private readonly CurrentState _currentState;


        public void clientController_Updated(string type)
        {
            switch (type)
            {
                case NetCodes.OBJECT_COUNTRY:
                    ReloadCountries();
                    return;
                case NetCodes.OBJECT_COMPANY:
                    ReloadCompanies();
                    return;
                case NetCodes.OBJECT_ALL:
                    ReloadCompanies();
                    ReloadCountries();
                    return;
            }
        }




        public Home()
        {
            InitializeComponent();

            // initialise network
            Network network = Network.Instance;
            network.BeginConnect("localhost", 8080);

            // initialise database
            Database.Instance.Connect();

            // initialise the state
            _clientState = new ClientState();

            _clientCon = new ClientController(_clientState);

            // initialise all the services (they set up the state themselves)

            _currentState = new CurrentState();






            _clientCon.Updated += new ClientController.StateUpdatedDelegate(clientController_Updated);


            //set up Columns in all of the DataGrids
            countriesList.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new Binding("ID") });
            countriesList.Columns.Add(new DataGridTextColumn { Header = "Name", Binding = new Binding("Name") });
            countriesList.Columns.Add(new DataGridTextColumn { Header = "Code", Binding = new Binding("Code") });

            companiesList.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new Binding("ID") });
            companiesList.Columns.Add(new DataGridTextColumn { Header = "Name", Binding = new Binding("Name") });

            distCenterList.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new Binding("ID") });
            distCenterList.Columns.Add(new DataGridTextColumn { Header = "Name", Binding = new Binding("Name") });

            routesList.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new Binding("ID") });
            routesList.Columns.Add(new DataGridTextColumn { Header = "Origin", Binding = new Binding("Origin") });
            routesList.Columns.Add(new DataGridTextColumn { Header = "Destination", Binding = new Binding("Destination") });
            routesList.Columns.Add(new DataGridTextColumn { Header = "Company", Binding = new Binding("Company") }); 
            routesList.Columns.Add(new DataGridTextColumn { Header = "TransportType", Binding = new Binding("TransportType") });

            
            priceList.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new Binding("ID") });
            priceList.Columns.Add(new DataGridTextColumn { Header = "Origin", Binding = new Binding("Origin") });
            priceList.Columns.Add(new DataGridTextColumn { Header = "Destination", Binding = new Binding("Destination") });
            priceList.Columns.Add(new DataGridTextColumn { Header = "Priority", Binding = new Binding("Priority") });

            intlPortList.Columns.Add(new DataGridTextColumn { Header = "Country", Binding = new Binding("Country") });
            
            //disable edit buttons until something is clicked in the corresponding datagrids
            editCountry.IsEnabled = false;


            //editCompanyButton.IsEnabled = false;

            editDistCenter.IsEnabled = false;
            editPrice.IsEnabled = false;
            editRoute.IsEnabled = false;
            editIntlPortButton.IsEnabled = false;

                        

        }


        //Helper methods to make it easier to referesh the information shown in a DataGrid

        delegate void NullArgumentDelegate();
        NullArgumentDelegate reloadCompaniesDelegate;
        NullArgumentDelegate reloadCountriesDelegate;

        private void _doReloadCountries()
        {
            MessageBox.Show("Reloading countries: " + _clientState.GetAllCountries().Count.ToString());
            countriesList.Items.Clear();

            foreach (var c in _clientState.GetAllCountries())
            {
                countriesList.Items.Add(c);
            }
        }





        private void ReloadCountries()
        {
            try
            {
                countriesList.Dispatcher.VerifyAccess();
                _doReloadCountries();
            }
            catch (InvalidOperationException e)
            {
                if (reloadCountriesDelegate == null)
                    reloadCountriesDelegate = new NullArgumentDelegate(_doReloadCountries);
                countriesList.Dispatcher.Invoke(reloadCountriesDelegate);
            }
        }

        private void _doReloadCompanies()
        {
            MessageBox.Show("Reloading companies: " + _clientState.GetAllCompanies().Count.ToString());
            companiesList.Items.Clear();

            foreach (var c in _clientState.GetAllCompanies())
            {
                companiesList.Items.Add(c);
            }
        }

        private void ReloadCompanies()
        {
            try
            {
                companiesList.Dispatcher.VerifyAccess();
                _doReloadCompanies();
            }
            catch(InvalidOperationException e)
            {
                if (reloadCompaniesDelegate == null)
                    reloadCompaniesDelegate = new NullArgumentDelegate(_doReloadCompanies);
                companiesList.Dispatcher.Invoke(reloadCompaniesDelegate);
            }

         
        }

       

        private void ReloadRoutes()
        {

            routesList.Items.Clear();

            foreach (var c in _clientState.GetAllRoutes())
            {
                routesList.Items.Add(c);
            }
        }

        private void ReloadPrices()
        {

            priceList.Items.Clear();

            foreach (var c in _clientState.GetAllPrices())
            {
                priceList.Items.Add(c);
            }
        }

        













        private void addCountry_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate the dialog box
            var dlg = new AddCountryDialogBox();

            // Open the dialog box modally 
            dlg.ShowDialog();
            if (dlg.DialogResult != false)
            {
                var name = dlg.countryName.Text;
                var code = dlg.countryCode.Text;
                
                try
                {
                    _clientCon.AddCountry(code, name);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        

        private void editCountry_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate the dialog box
            var dlg = new AddCountryDialogBox
                {
                    countryName = {Text = ((Country) countriesList.SelectedItem).Name},
                    countryCode = { Text = ((Country)countriesList.SelectedItem).Code },
                    Title = "Edit Country",
            
                };

            // Open the dialog box modally 
            dlg.Title = "Edit Country";
            dlg.countryName.IsReadOnly = true;
            dlg.ShowDialog();
            
            dlg.countryName.IsReadOnly = true;
            if (dlg.DialogResult != false)
            {

                _clientCon.EditCountry(((Country)countriesList.SelectedItem).ID, dlg.countryCode.Text);
                
            }
           
        }

        private void deleteCountry_Click(object sender, RoutedEventArgs e)
        {
            try

            {
                _clientCon.DeleteCountry(((Country) countriesList.SelectedItem).ID);
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.Message);
            }
            ReloadCountries();
        }
            

        

        private void countriesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            editCountry.IsEnabled = true;
        }

        private void addCompanyButton_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate the dialog box
            var dlg = new AddCompanyDialogBox();

            // Open the dialog box modally 
            dlg.ShowDialog();
            if (dlg.DialogResult != false)
            {
                var name = dlg.companyName.Text;
             

                try
                {
                    _clientCon.AddCompany(name);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void reload_Click(object sender, RoutedEventArgs e)
        {
            ReloadCountries();
            ReloadCompanies();
        }

       

        private void requestDelivery_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new System.Uri("RequestDelivery.xaml", UriKind.RelativeOrAbsolute));
        }

        private void deleteCompanyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               // _clientCon.DeleteCompany(((Company)companiesList.SelectedItem).ID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            ReloadCompanies();
        }

        private void editIntlPortButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addRoute_Click(object sender, RoutedEventArgs e)
        {
        // Instantiate the dialog box
            var dlg = new AddRouteDialogBox(_clientState);

            // Open the dialog box modally 
            dlg.ShowDialog();

            dlg.originComboBox.ItemsSource = _currentState.GetAllRouteNodes();
            dlg.destComboBox.ItemsSource = _currentState.GetAllRouteNodes();

            if (dlg.DialogResult != false)
            {
                ComboBoxItem origin = dlg.originComboBox.SelectedItem as ComboBoxItem;
                ComboBoxItem dest = dlg.destComboBox.SelectedItem as ComboBoxItem;

                TransportType transport = TransportType.Land;
                switch (dlg.transportComboBox.Text)
                {
                    case "Air":
                        transport = TransportType.Air;
                        break;
                    case "Land":
                        transport = TransportType.Land;
                        break;
                    case "Sea":
                        transport = TransportType.Sea;
                        break;
                }



                try
                {
                    _clientCon.AddRoute(Convert.ToInt32(origin.Tag), Convert.ToInt32(dest.Tag), transport, Convert.ToInt32(dlg.weightCost.Text), Convert.ToInt32(dlg.volumeCost.Text), Convert.ToInt32(dlg.maxWeight.Text), Convert.ToInt32(dlg.maxVolume.Text), Convert.ToInt32(dlg.duration.Text),  new List<WeeklyTime>() );
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            
        }

       
        }

        private void addPrice_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new AddPriceDialogBox();

            // Open the dialog box modally 
            dlg.ShowDialog();
            if (dlg.DialogResult != false)
            {
                var originID = Convert.ToInt32(dlg.origin.Text);
                var destId = Convert.ToInt32(dlg.destination.Text);
                Priority priority = Priority.Standard;
                if (dlg.priority.SelectedIndex == 0)
                    priority = Priority.Standard;
                else if (dlg.priority.SelectedIndex == 1)
                    priority = Priority.Air;
                else
                {
                    //should never happen
                    MessageBox.Show("You must select Standard or Air priorty.");
                }
                var weightPrice = Convert.ToInt32(dlg.gramPrice.Text);
                var volumePrice = Convert.ToInt32(dlg.cubicCmPrice.Text);

                try
                {
                    _clientCon.AddPrice(originID, destId, priority, weightPrice, volumePrice);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }


        }

        private void addIntlPortButton_Click(object sender, RoutedEventArgs e)
        {

            
        }


        private void editRoute_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate the dialog box
            var dlg = new AddRouteDialogBox(_clientState);
            var r = ((Route)routesList.SelectedItem);
            // Open the dialog box modally 
            dlg.ShowDialog();
            dlg.Title = "Edit Route";

            //uneditable fields
            dlg.originComboBox.IsReadOnly = true;
            dlg.originComboBox.Text = r.Origin.Country.Name;
            dlg.destComboBox.IsReadOnly = true;
            dlg.destComboBox.Text = r.Destination.Country.Name;
            dlg.companyComboBox.IsReadOnly = true;
            dlg.companyComboBox.Text = r.Company.Name;
            dlg.transportComboBox.IsReadOnly = true;
            dlg.transportComboBox.Text = r.TransportType.ToString();

            //editable fields
            dlg.duration.Text = Convert.ToString(r.Duration);
            dlg.weightCost.Text = Convert.ToString(r.CostPerGram);
            dlg.volumeCost.Text = Convert.ToString(r.PricePerCm3);
            dlg.maxWeight.Text = Convert.ToString(r.MaxWeight);
            dlg.maxVolume.Text = Convert.ToString(r.MaxVolume);


            if (dlg.DialogResult != false)
            {

                var times = new List<WeeklyTime>();

                try
                {
                    _clientCon.EditRoute(r.ID, Convert.ToInt32(dlg.weightCost.Text),
                                         Convert.ToInt32(dlg.volumeCost.Text), Convert.ToInt32(dlg.maxWeight),
                                         Convert.ToInt32(dlg.maxVolume), Convert.ToInt32(dlg.duration), times);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new System.Uri("ViewStats.xaml", UriKind.RelativeOrAbsolute));
        }


        }

    }

