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
        private ClientState _clientState;
        private ClientController _clientCon;

        private CurrentState _currentState;


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
                case NetCodes.OBJECT_PRICE:
                    ReloadDomesticPrices();
                    ReloadIntlPrices();
                    return;
                case NetCodes.OBJECT_ROUTE:
                    ReloadRoutes();
                    return;

                case NetCodes.OBJECT_ROUTENODE:
                    ReloadRouteNodes();
                    return;
                case NetCodes.OBJECT_ALL:
                    ReloadAll();
                    return;
            }
        }


        public void SetUpHome()
        {
            // initialise network
            Network network = Network.Instance;
            network.BeginConnect("localhost", 8080);

            // initialise database
            Database.Instance.Connect();



            // initialise all the services (they set up the state themselves)

            _currentState = new CurrentState();









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


            domesticPriceList.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new Binding("ID") });
            domesticPriceList.Columns.Add(new DataGridTextColumn { Header = "Priority", Binding = new Binding("Priority") });
            domesticPriceList.Columns.Add(new DataGridTextColumn { Header = "Price per gram", Binding = new Binding("PricePerGram") });
            domesticPriceList.Columns.Add(new DataGridTextColumn { Header = "Price per cm^3", Binding = new Binding("PricePerCm3") });

            intlPriceList.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new Binding("ID") });
            intlPriceList.Columns.Add(new DataGridTextColumn { Header = "Origin", Binding = new Binding("Origin") });
            intlPriceList.Columns.Add(new DataGridTextColumn { Header = "Destination", Binding = new Binding("Destination") });
            intlPriceList.Columns.Add(new DataGridTextColumn { Header = "Priority", Binding = new Binding("Priority") });
            intlPriceList.Columns.Add(new DataGridTextColumn { Header = "Price per gram", Binding = new Binding("PricePerGram") });
            intlPriceList.Columns.Add(new DataGridTextColumn { Header = "Price per cm^3", Binding = new Binding("PricePerCm3") });
            intlPortList.Columns.Add(new DataGridTextColumn { Header = "Country", Binding = new Binding("Country") });
            intlPortList.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new Binding("ID") });
        }

        public Home()
        {
            InitializeComponent();

            SetUpHome();

            // initialise the state
            _clientState = new ClientState();

            _clientCon = new ClientController(_clientState);

            _clientCon.Updated += new ClientController.StateUpdatedDelegate(clientController_Updated);

        }

        public Home(ClientState state)
        {
            InitializeComponent();

            SetUpHome();

            _clientState = state;

            _clientCon = new ClientController(_clientState);

            _clientCon.Updated += new ClientController.StateUpdatedDelegate(clientController_Updated);

            ReloadAll();

        }

        //Helper methods to make it easier to referesh the information shown in a DataGrid

        delegate void NullArgumentDelegate();
        NullArgumentDelegate reloadCompaniesDelegate;
        NullArgumentDelegate reloadCountriesDelegate;
        NullArgumentDelegate reloadRoutesDelegate;
        NullArgumentDelegate reloadPricesDelegate;
        NullArgumentDelegate reloadRouteNodesDelegate;

        private void _doReloadCountries()
        {
            
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
            catch (InvalidOperationException e)
            {
                if (reloadCompaniesDelegate == null)
                    reloadCompaniesDelegate = new NullArgumentDelegate(_doReloadCompanies);
                companiesList.Dispatcher.Invoke(reloadCompaniesDelegate);
            }


        }

        private void ReloadRoutes()
        {
            try
            {
                routesList.Dispatcher.VerifyAccess();
                _doReloadRoutes();
            }
            catch (InvalidOperationException e)
            {
                if (reloadRoutesDelegate == null)
                    reloadRoutesDelegate = new NullArgumentDelegate(_doReloadRoutes);
                routesList.Dispatcher.Invoke(reloadRoutesDelegate);
            }


        }

        private void _doReloadRoutes()
        {

            routesList.Items.Clear();

            foreach (var c in _clientState.GetAllRoutes())
            {
                routesList.Items.Add(c);
            }
        }

        private void ReloadDomesticPrices()
        {
            try
            {
                domesticPriceList.Dispatcher.VerifyAccess();
                _doReloadDomesticPrices();
            }
            catch (InvalidOperationException e)
            {
                if (reloadDomesticPricesDelegate == null)
                    reloadDomesticPricesDelegate = new NullArgumentDelegate(_doReloadDomesticPrices);
                domesticPriceList.Dispatcher.Invoke(reloadDomesticPricesDelegate);
            }
        }
        private void ReloadIntlPrices()
        {
            try
            {
                intlPriceList.Dispatcher.VerifyAccess();
                _doReloadIntlPrices();
            }
            catch (InvalidOperationException e)
            {
                if (reloadInternationalPricesDelegate == null)
                    reloadInternationalPricesDelegate = new NullArgumentDelegate(_doReloadIntlPrices);
                intlPriceList.Dispatcher.Invoke(reloadInternationalPricesDelegate);
            }


        }

        private void _doReloadDomesticPrices()
        {

            domesticPriceList.Items.Clear();

            foreach (var c in _clientState.GetAllDomesticPrices())
            {
                domesticPriceList.Items.Add(c);
            }
        }

        private void _doReloadIntlPrices()
        {

            intlPriceList.Items.Clear();

            foreach (var c in _clientState.GetAllInternationalPrices())
            {
                intlPriceList.Items.Add(c);
            }
        }
        private void ReloadRouteNodes()
        {
            try
            {
                intlPortList.Dispatcher.VerifyAccess();
                distCenterList.Dispatcher.VerifyAccess();
                _doReloadRouteNodes();
            }
            catch (InvalidOperationException e)
            {
                if (reloadRouteNodesDelegate == null)
                    reloadRouteNodesDelegate = new NullArgumentDelegate(_doReloadRouteNodes);
                intlPortList.Dispatcher.Invoke(reloadRouteNodesDelegate);
            }


        }

        private void _doReloadRouteNodes()
        {

            distCenterList.Items.Clear();
            intlPortList.Items.Clear();

            foreach (var c in _clientState.GetAllRouteNodes())
            {
                if (c is DistributionCentre)
                {
                    distCenterList.Items.Add(c);
                }
                else if (c is InternationalPort)
                {
                    intlPortList.Items.Add(c);
                }
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

                if (name == String.Empty || code == String.Empty)
                {
                    MessageBox.Show("Cannot have empty fields");
                    return;
                }

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
                    countryName = { Text = ((Country)countriesList.SelectedItem).Name },
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
                if (dlg.countryCode.Text == String.Empty)
                {
                    MessageBox.Show("Cannot have empty fields");
                    return;
                }
                _clientCon.EditCountry(((Country)countriesList.SelectedItem).ID, dlg.countryCode.Text);
            }

        }

        private void deleteCountry_Click(object sender, RoutedEventArgs e)
        {

            _clientCon.DeleteCountry(((Country)countriesList.SelectedItem).ID);


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
                if (name == String.Empty)
                {
                    MessageBox.Show("Cannot have empty fields");
                    return;
                }

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
            NavigationService.Navigate(new RequestDelivery(_clientCon, _clientState));
        }

        private void deleteCompanyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _clientCon.DeleteCompany(((Company)companiesList.SelectedItem).ID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            ReloadCompanies();
        }


        private void addRoute_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate the dialog box
            var dlg = new AddRouteDialogBox(_clientState);

            // Open the dialog box modally 
            dlg.ShowDialog();


            if (dlg.DialogResult != false)
            {
                ComboBoxItem origin = dlg.originComboBox.SelectedItem as ComboBoxItem;
                ComboBoxItem dest = dlg.destComboBox.SelectedItem as ComboBoxItem;
                ComboBoxItem company = dlg.companyComboBox.SelectedItem as ComboBoxItem;

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

                var times = new List<WeeklyTime>();
                foreach (DayMinuteHourHolder time in dlg.timesGrid.Items)
                {
                    DayOfWeek day = DayOfWeek.Monday;
                    switch (time.Day)
                    {
                        case "Monday":
                            day = DayOfWeek.Monday;
                            break;
                        case "Tuesday":
                            day = DayOfWeek.Tuesday;
                            break;
                        case "Wednesday":
                            day = DayOfWeek.Wednesday;
                            break;
                        case "Thursday":
                            day = DayOfWeek.Thursday;
                            break;
                        case "Friday":
                            day = DayOfWeek.Friday;
                            break;
                        case "Saturday":
                            day = DayOfWeek.Saturday;
                            break;
                        case "Sunday":
                            day = DayOfWeek.Sunday;
                            break;
                    }
                    times.Add(new WeeklyTime(day, time.Hour, time.Minute));
                }

                if (origin == null || dest == null || company == null || dlg.weightCost.Text == String.Empty || dlg.volumeCost.Text == String.Empty || dlg.maxWeight.Text == String.Empty || dlg.maxVolume.Text == String.Empty || dlg.duration.Text == String.Empty)
                {
                    MessageBox.Show("Cannot have empty fields");
                    return;
                }

                try
                {
                    Int32.Parse(dlg.weightCost.Text);
                    Int32.Parse(dlg.volumeCost.Text);
                    Int32.Parse(dlg.maxVolume.Text);
                    Int32.Parse(dlg.maxWeight.Text);
                    Int32.Parse(dlg.duration.Text);


                }
                catch
                {
                    MessageBox.Show("You put NaN in a number field. Please enter integers where required");
                    return;
                }

                try
                {
                    _clientCon.AddRoute(Convert.ToInt32(origin.Tag), Convert.ToInt32(dest.Tag), Convert.ToInt32(company.Tag), transport, Convert.ToInt32(dlg.weightCost.Text), Convert.ToInt32(dlg.volumeCost.Text), Convert.ToInt32(dlg.maxWeight.Text), Convert.ToInt32(dlg.maxVolume.Text), Convert.ToInt32(dlg.duration.Text), times);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }


            }


        }

        private void addPrice_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new AddPriceDialogBox(_clientState, true);
            dlg.origin.Text = "NA";
            dlg.origin.IsEnabled = false;

            dlg.dest.Text = "NA";
            dlg.dest.IsEnabled = false;

            // Open the dialog box modally 
            dlg.ShowDialog();
            if (dlg.DialogResult != false)
            {
                ComboBoxItem origin = dlg.origin.SelectedItem as ComboBoxItem;
                ComboBoxItem dest = dlg.dest.SelectedItem as ComboBoxItem;
                Priority priority = Priority.Air;
                if (dlg.priority.SelectedIndex == 0)
                    priority = Priority.Standard;
                else if (dlg.priority.SelectedIndex == 1)
                    priority = Priority.Air;
                else
                {
                    //should never happen
                    MessageBox.Show("You must select Standard or Air priorty.");
                }

                if (dlg.gramPrice.Text == String.Empty || dlg.cubicCmPrice.Text == String.Empty)
                {
                    MessageBox.Show("Empty fields entered");
                    return;
                }

                try
                {
                    Int32.Parse(dlg.gramPrice.Text);
                    Int32.Parse(dlg.cubicCmPrice.Text);
                }
                catch
                {
                    MessageBox.Show("NaN enterered in a one of the price fields. Please enter a  number");
                    return;
                }

                var weightPrice = Convert.ToInt32(dlg.gramPrice.Text);
                var volumePrice = Convert.ToInt32(dlg.cubicCmPrice.Text);

                try
                {
                    _clientCon.AddDomesticPrice(priority, weightPrice, volumePrice);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }


        }

        private void addIntlPortButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new AddIntlPortDialogBox(_clientState);

            // Open the dialog box modally 
            dlg.ShowDialog();
            if (dlg.DialogResult != false)
            {
                ComboBoxItem country = dlg.countries.SelectedItem as ComboBoxItem;

                if (country == null)
                {
                    MessageBox.Show("Invalid Country");
                    return;
                }
                try
                {
                    _clientCon.AddInternationalPort(Convert.ToInt32(country.Tag));

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }


        private void editRoute_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate the dialog box
            var dlg = new AddRouteDialogBox(_clientState);
            var r = ((Route)routesList.SelectedItem);
            if (r == null)
                return;

            dlg.Title = "Edit Route";

            //uneditable fields
            dlg.originComboBox.IsEnabled = false;
            dlg.originComboBox.Text = r.Origin.Country.Name;
            dlg.destComboBox.IsEnabled = false;
            dlg.destComboBox.Text = r.Destination.Country.Name;
            dlg.companyComboBox.IsEnabled = false;
            dlg.companyComboBox.Text = r.Company.Name;
            dlg.transportComboBox.IsEnabled = false;
            dlg.transportComboBox.Text = r.TransportType.ToString();

            //editable fields
            dlg.duration.Text = Convert.ToString(r.Duration);
            dlg.weightCost.Text = Convert.ToString(r.CostPerGram);
            dlg.volumeCost.Text = Convert.ToString(r.CostPerCm3);
            dlg.maxWeight.Text = Convert.ToString(r.MaxWeight);
            dlg.maxVolume.Text = Convert.ToString(r.MaxVolume);

            var timesForList = new List<DayMinuteHourHolder>();

            foreach (var time in r.DepartureTimes)
            {
                String day = "";
                switch (time.DayComponent)
                {
                    case DayOfWeek.Monday:
                        day = "Monday";
                        break;
                    case DayOfWeek.Tuesday:
                        day = "Tuesday";
                        break;
                    case DayOfWeek.Wednesday:
                        day = "Wednesday";
                        break;
                    case DayOfWeek.Thursday:
                        day = "Thursday";
                        break;
                    case DayOfWeek.Friday:
                        day = "Friday";
                        break;
                    case DayOfWeek.Saturday:
                        day = "Saturday";
                        break;
                    case DayOfWeek.Sunday:
                        day = "Sunday";
                        break;

                }


                timesForList.Add(new DayMinuteHourHolder()
                    {
                        Day = day,
                        Hour = time.HourComponent,
                        Minute = time.MinuteComponent
                    });
            }

            foreach (var dayMinuteHourHolder in timesForList)
            {
                dlg.timesGrid.Items.Add(dayMinuteHourHolder);
            }


            // Open the dialog box modally 
            dlg.ShowDialog();

            if (dlg.DialogResult != false)
            {

                var times = new List<WeeklyTime>();
                foreach (DayMinuteHourHolder time in dlg.timesGrid.Items)
                {
                    DayOfWeek day = DayOfWeek.Monday;
                    switch (time.Day)
                    {
                        case "Monday":
                            day = DayOfWeek.Monday;
                            break;
                        case "Tuesday":
                            day = DayOfWeek.Tuesday;
                            break;
                        case "Wednesday":
                            day = DayOfWeek.Wednesday;
                            break;
                        case "Thursday":
                            day = DayOfWeek.Thursday;
                            break;
                        case "Friday":
                            day = DayOfWeek.Friday;
                            break;
                        case "Saturday":
                            day = DayOfWeek.Saturday;
                            break;
                        case "Sunday":
                            day = DayOfWeek.Sunday;
                            break;
                    }
                    times.Add(new WeeklyTime(day, time.Hour, time.Minute));



                    _clientCon.EditRoute(r.ID, Convert.ToInt32(dlg.weightCost.Text),
                                         Convert.ToInt32(dlg.volumeCost.Text), Convert.ToInt32(dlg.maxWeight.Text),
                                         Convert.ToInt32(dlg.maxVolume.Text), Convert.ToInt32(dlg.duration.Text), times);


                }
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ViewStats(_clientCon, _clientState));
        }

        private void addDistCenterButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new AddDistCenterDialogBox();

            // Open the dialog box modally 
            dlg.ShowDialog();
            if (dlg.DialogResult != false)
            {
                if (dlg.name.Text == String.Empty)
                {
                    MessageBox.Show("Can't have empty name");
                    return;
                }

                try
                {
                    _clientCon.AddDistributionCentre(dlg.name.Text);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void editPrice_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new AddPriceDialogBox(_clientState, true);

            // Open the dialog box modally 
            var price = ((Price)domesticPriceList.SelectedItem);
            dlg.Title = "Edit Domnestic Price";

            dlg.origin.IsEnabled = false;
            dlg.origin.Text = price.Origin.Country.Name;
            dlg.dest.IsEnabled = false;
            dlg.dest.Text = price.Destination.Country.Name;
            dlg.priority.Text = price.Priority.ToString();
            dlg.priority.IsEnabled = false;

            dlg.gramPrice.Text = Convert.ToString(((Price)domesticPriceList.SelectedItem).PricePerGram);
            dlg.cubicCmPrice.Text = Convert.ToString(((Price)domesticPriceList.SelectedItem).PricePerCm3);

            dlg.ShowDialog();
            if (dlg.DialogResult != false)
            {
                ComboBoxItem origin = dlg.origin.SelectedItem as ComboBoxItem;
                ComboBoxItem dest = dlg.dest.SelectedItem as ComboBoxItem;
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
                    _clientCon.EditPrice(price.ID, weightPrice, volumePrice);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void deletePrice_Click(object sender, RoutedEventArgs e)
        {
            if (domesticPriceList.SelectedItem == null)
                return;
            _clientCon.DeletePrice(((DomesticPrice)domesticPriceList.SelectedItem).ID);
        }

        private void deleteRoute_Click(object sender, RoutedEventArgs e)
        {
            if (routesList.SelectedItem == null)
                return;

            _clientCon.DeleteRoute(((Route)routesList.SelectedItem).ID);
        }

        private void deleteIntlPortButton_Click(object sender, RoutedEventArgs e)
        {
            if (intlPortList.SelectedItem == null)
                return;
            _clientCon.DeleteRouteNode(((RouteNode)intlPortList.SelectedItem).ID);
        }

        private void deleteDistCenter_Click(object sender, RoutedEventArgs e)
        {
            if (distCenterList.SelectedItem == null)
                return;
            _clientCon.DeleteRouteNode(((RouteNode)distCenterList.SelectedItem).ID);
        }

        private void addIntlPrice_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new AddPriceDialogBox(_clientState, false);

            // Open the dialog box modally 
            dlg.ShowDialog();
            if (dlg.DialogResult != false)
            {
                ComboBoxItem origin = dlg.origin.SelectedItem as ComboBoxItem;
                ComboBoxItem dest = dlg.dest.SelectedItem as ComboBoxItem;
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

                if (origin == null || dest == null)
                {
                    MessageBox.Show("Invalid origin/destination");
                    return;
                }
                var weightPrice = Convert.ToInt32(dlg.gramPrice.Text);
                var volumePrice = Convert.ToInt32(dlg.cubicCmPrice.Text);

                try
                {
                    _clientCon.AddPrice(Convert.ToInt32(origin.Tag), Convert.ToInt32(dest.Tag), priority, weightPrice, volumePrice);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void reload_Click_1(object sender, RoutedEventArgs e)
        {
            ReloadAll();
        }

        private void ReloadAll()
        {
            ReloadCompanies();
            ReloadIntlPrices();
            ReloadDomesticPrices();
            ReloadRouteNodes();
            ReloadRoutes();
            ReloadCountries();

            
        }

        private NullArgumentDelegate reloadInternationalPricesDelegate { get; set; }

        private NullArgumentDelegate reloadDomesticPricesDelegate { get; set; }

        private void deleteIntlPrice_Click(object sender, RoutedEventArgs e)
        {
            if (intlPriceList.SelectedItem == null)
                return;
            _clientCon.DeletePrice(((Price)intlPriceList.SelectedItem).ID);
        }
    }

    }

