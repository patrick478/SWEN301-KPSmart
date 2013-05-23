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
                case NetCodes.OBJECT_PRICE:
                    ReloadPrices();
                    return;
                case NetCodes.OBJECT_ROUTE:
                    ReloadRoutes();
                    return;
                
                case NetCodes.OBJECT_ROUTENODE:
                    ReloadRouteNodes();
                    return;
                case NetCodes.OBJECT_ALL:
                    ReloadCompanies();
                    ReloadCountries();
                    ReloadPrices();
                    ReloadRoutes();
                    ReloadRouteNodes();
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


            domesticPriceList.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new Binding("ID") });
            domesticPriceList.Columns.Add(new DataGridTextColumn { Header = "Origin", Binding = new Binding("Origin") });
            domesticPriceList.Columns.Add(new DataGridTextColumn { Header = "Destination", Binding = new Binding("Destination") });
            domesticPriceList.Columns.Add(new DataGridTextColumn { Header = "Priority", Binding = new Binding("Priority") });

            intlPriceList.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new Binding("ID") });
            intlPriceList.Columns.Add(new DataGridTextColumn { Header = "Origin", Binding = new Binding("Origin") });
            intlPriceList.Columns.Add(new DataGridTextColumn { Header = "Destination", Binding = new Binding("Destination") });
            intlPriceList.Columns.Add(new DataGridTextColumn { Header = "Priority", Binding = new Binding("Priority") });

            intlPortList.Columns.Add(new DataGridTextColumn { Header = "Country", Binding = new Binding("Country") });
            
            //disable edit buttons until something is clicked in the corresponding datagrids
            editCountry.IsEnabled = false;


            //editCompanyButton.IsEnabled = false;

            /*editDistCenter.IsEnabled = false;
            editPrice.IsEnabled = false;
            editRoute.IsEnabled = false;
            editIntlPortButton.IsEnabled = false;*/

                        

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

        private void ReloadPrices()
        {
            try
            {
                domesticPriceList.Dispatcher.VerifyAccess();
                _doReloadDomesticPrices();
            }
            catch (InvalidOperationException e)
            {
                if (reloadPricesDelegate == null)
                    reloadPricesDelegate = new NullArgumentDelegate(_doReloadDomesticPrices);
                domesticPriceList.Dispatcher.Invoke(reloadPricesDelegate);
            }
        }
        private void ReloadIntlPrices(){
            try
            {
                intlPriceList.Dispatcher.VerifyAccess();
                _doReloadIntlPrices();
            }
            catch (InvalidOperationException e)
            {
                if (reloadPricesDelegate == null)
                    reloadPricesDelegate = new NullArgumentDelegate(_doReloadIntlPrices);
                intlPriceList.Dispatcher.Invoke(reloadPricesDelegate);
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

            foreach (var c in _clientState.GetAllPrices())
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
                _doReloadDomesticPrices();
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
           
            _clientCon.DeleteCountry(((Country) countriesList.SelectedItem).ID);
            
            
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

                

                try
                {
                    _clientCon.AddRoute(Convert.ToInt32(origin.Tag), Convert.ToInt32(dest.Tag),Convert.ToInt32(company.Tag) , transport, Convert.ToInt32(dlg.weightCost.Text), Convert.ToInt32(dlg.volumeCost.Text), Convert.ToInt32(dlg.maxWeight.Text), Convert.ToInt32(dlg.maxVolume.Text), Convert.ToInt32(dlg.duration.Text),  times );
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            
        }

       
        }

        private void addPrice_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new AddPriceDialogBox(_clientState);

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
            var r = ((Route) routesList.SelectedItem);

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
            NavigationService.Navigate(new ViewStats(_clientCon));
        }

        private void addDistCenterButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new AddDistCenterDialogBox();

            // Open the dialog box modally 
            dlg.ShowDialog();
            if (dlg.DialogResult != false)
            {
                

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
            var dlg = new AddPriceDialogBox(_clientState);

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
            _clientCon.DeletePrice(((Price)domesticPriceList.SelectedItem).ID);
        }

        private void deleteRoute_Click(object sender, RoutedEventArgs e)
        {
            _clientCon.DeleteRoute(((Route)routesList.SelectedItem).ID);
        }

        private void deleteIntlPortButton_Click(object sender, RoutedEventArgs e)
        {
            _clientCon.DeleteRouteNode(((RouteNode)intlPortList.SelectedItem).ID);
        }

        private void editDistCenter_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deleteDistCenter_Click(object sender, RoutedEventArgs e)
        {
            _clientCon.DeleteRouteNode(((RouteNode)distCenterList.SelectedItem).ID);
        }

        private void routesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void addIntlPrice_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new AddPriceDialogBox(_clientState);

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
            ReloadCompanies();
            ReloadIntlPrices();
            ReloadPrices();
            ReloadRouteNodes();
            ReloadRoutes();
            ReloadCountries();
        }


        }

    }

