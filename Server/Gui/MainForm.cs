using System;
using Server.Business;
using Server.Data;
using Common;
using Server.Network;

namespace Server.Gui
{
    public partial class MainForm 
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // initialise logger
            Logger.Instance.SetOutput(logBox);
            Logger.WriteLine("Server starting..");

            // initialise database
            Database.Instance.Connect();

            // initialise the state object
            var currentState = new CurrentState();

            // initialise all the services (they set up the state themselves) and pathfinder
            var countryService = new CountryService(currentState);
            var companyService = new CompanyService(currentState);
            var routeService = new RouteService(currentState);
            var pathFinder = new PathFinder(routeService); // pathfinder needs the RouteService
            var deliveryService = new DeliveryService(currentState, pathFinder); // DeliveryService needs the PathFinder
            var priceService = new PriceService(currentState);      
            var locationService = new LocationService(currentState);

            // create controller
            var controller = new Controller(countryService, companyService, deliveryService, priceService, routeService,
                                            locationService);

            // initialise network
            Network.Network network = Network.Network.Instance;
            network.Start();
            network.Open();
            // todo - pass controller to network

            BenDBTests(countryService);
        }




        private void BenDBTests(CountryService countryService)
        {
            try
            {
                CountryDataHelper cdh = new CountryDataHelper();

                // create country if doesn't exist
                Country country = new Country {Name = "Wellington", Code = "WLG"};
                if (!countryService.Exists(country))
                {
                    country = countryService.Create("Wellington", "WLG");
                }

                // perform updates
                country = countryService.Update(country.ID, country.Name, "WLN");
                country = countryService.Update(country.ID, country.Name, "BEN");

                // get latest version
                Country loadedCountry = countryService.Get(country.ID);

                // create new zealand
                country = new Country { Name = "New Zealand", Code = "NZ" };
                if (!countryService.Exists(country))
                {
                    country = countryService.Create(country.Name, country.Code);
                }

                // create australia
                country = new Country {Name = "Australia", Code = "AUS"};
                if (!countryService.Exists(country))
                {
                    country = countryService.Create(country.Name, country.Code);
                }

                // load all countries
                var allCountries = countryService.GetAll();

                // delete australia
                foreach (Country c in allCountries)
                {
                    if (c.Name == "Australia")
                    {
                        countryService.Delete(c.ID);
                    }
                }
            }
            catch (DatabaseException e)
            {
                Logger.Write(e.Message);
            }
        }



    }
}
