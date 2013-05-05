using System;
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
        public void clientController_Updated(Type type)
        {
            if(type == typeof(Country)){
                    ReloadCountries();
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

            routeNodeList.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new Binding("ID") });
            routeNodeList.Columns.Add(new DataGridTextColumn { Header = "Country", Binding = new Binding("Country") });

            priceList.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new Binding("ID") });
            priceList.Columns.Add(new DataGridTextColumn { Header = "Origin", Binding = new Binding("Origin") });
            priceList.Columns.Add(new DataGridTextColumn { Header = "Destination", Binding = new Binding("Destination") });
            priceList.Columns.Add(new DataGridTextColumn { Header = "Priority", Binding = new Binding("Priority") });

            //disable edit buttons until something is clicked in the corresponding datagrids
            editCountry.IsEnabled = false;
            editCompanyButton.IsEnabled = false;
            editDistCenter.IsEnabled = false;
            editPrice.IsEnabled = false;
            editRoute.IsEnabled = false;
            editRouteNode.IsEnabled = false;



            ReloadCountries();

            

        }


        //Helper methods to make it easier to referesh the information shown in a DataGrid

        private void ReloadCountries()
        {

            countriesList.Items.Clear();

            foreach (var c in _clientState.GetAllCountries())
            {
                countriesList.Items.Add(c);
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

            ReloadCountries();
        }

        

        private void editCountry_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate the dialog box
            var dlg = new AddCountryDialogBox
                {
                    countryName = {Text = ((Country) countriesList.SelectedItem).Name},
                    countryCode = { Text = ((Country)countriesList.SelectedItem).Code }
                };

            // Open the dialog box modally 
            dlg.ShowDialog();

            if (dlg.DialogResult != false)
            {
                var name = dlg.countryName.Text;
                var code = dlg.countryCode.Text;
                try
                {
                  //  _countryService.Update(((Country) countriesList.SelectedItem).ID, code);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            ReloadCountries();
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
             

                var country = new Company { Name = name};

                companiesList.Items.Add(country);

            }
        }

        private void reload_Click(object sender, RoutedEventArgs e)
        {
            ReloadCountries();
        }

       

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new System.Uri("RequestDelivery.xaml", UriKind.RelativeOrAbsolute));
        }


       
    }
}
