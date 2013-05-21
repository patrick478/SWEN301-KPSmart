using System;
using Server.Business;
using Server.Data;
using Common;
using Server.Network;
using System.Linq;
using System.Collections.Generic;

namespace Server.Gui
{
    public partial class MainForm 
    {
        public MainForm()
        {
            InitializeComponent();
        }


        private CountryService countryService;
        private CompanyService companyService;
        private RouteService routeService;
        private DeliveryService deliveryService;
        private PriceService priceService;
        private LocationService locationService;
        private EventService eventService;

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
            countryService = new CountryService(currentState);
            companyService = new CompanyService(currentState);
            routeService = new RouteService(currentState);
            var pathFinder = new PathFinder(routeService); // pathfinder needs the RouteService
            deliveryService = new DeliveryService(currentState, pathFinder); // DeliveryService needs the PathFinder
            priceService = new PriceService(currentState);      
            locationService = new LocationService(currentState);
            eventService = new EventService(currentState);

            // initialise network
            Network.Network network = Network.Network.Instance;
            network.Start();
            network.Open();

            // create controller
            var controller = new Controller(countryService, companyService, deliveryService, priceService, routeService,
                                            locationService);

            BenDBTests(countryService, routeService);
        }




        private void BenDBTests(CountryService countryService, RouteService routeService)
        {
            try
            {
                
                CountryDataHelper cdh = new CountryDataHelper();
                
                // create country if doesn't exist
                Country country = new Country { ID = 1, Name = "Wellington", Code = "WLG" };
                if (!countryService.Exists(country))
                {
                    country = countryService.Create("Wellington", "WLG");
                }

                country = countryService.Update(country.ID, "WLN");
                country = countryService.Update(country.ID, "BEN");

                // get latest version
                Country loadedCountry = countryService.Get(country.ID);

                cdh.LoadAll(DateTime.Now);

                // create new zealand
                country = new Country { Name = "New Zealand", Code = "NZ" };
                if (!countryService.Exists(country))
                {
                    country = countryService.Create(country.Name, country.Code);
                }

                // create australia
                country = new Country { Name = "Australia", Code = "AUS" };
                if (!countryService.Exists(country))
                {
                    country = countryService.Create(country.Name, country.Code);
                }

                // load all countries
                var allCountries = countryService.GetAll();


                // create christchurch depot
                RouteNode routeNode = new DistributionCentre("Christchurch");
                if (!locationService.Exists(routeNode))
                {
                    routeNode = locationService.CreateDistributionCentre("Christchurch");
                }
                
                // wellington depot
                routeNode = new DistributionCentre("Wellington");
                if (!locationService.Exists(routeNode))
                {
                    routeNode = locationService.CreateDistributionCentre("Wellington");
                }

                // australia port
                country = countryService.GetAll().AsQueryable().First(t => t.Name == "Australia");
                var destination = new InternationalPort(country);
                if (!locationService.Exists(destination))
                {
                    destination = locationService.CreateInternationalPort(country.ID);
                }

                // get a company
                var company = new Company() { Name = "NZ Post" };
                if (!companyService.Exists(company))
                {
                    company = companyService.Create(company.Name);
                }

                // create a new route
                Route route = new Route()
                    {
                        Origin = routeNode,
                        Destination = destination,
                        Company = company,
                        Duration = 300,
                        MaxVolume = 5000,
                        MaxWeight = 5000,
                        CostPerCm3 = 3,
                        CostPerGram = 5,
                        TransportType = TransportType.Air,
                        DepartureTimes = new List<WeeklyTime> { new WeeklyTime(DayOfWeek.Monday, 5, 30) }
                    };

                var routeDataHelper = new RouteDataHelper();

                int id = routeDataHelper.GetId(route);
                Logger.WriteLine("Route id is: " + id);
                if (id == 0)
                {
                    routeDataHelper.Create(route);
                }
                
                //route = routeDataHelper.Load(1);

                // edit departure times
                route.DepartureTimes.Add(new WeeklyTime(DayOfWeek.Wednesday, 14, 35));

                // update
                //routeDataHelper.Update(route);

                // delete
                routeDataHelper.Delete(route.ID);

                var routes = routeDataHelper.LoadAll();

                var delivery = new Delivery { Origin = routeNode, Destination = destination, Priority = Priority.Air, WeightInGrams = 200, VolumeInCm3 = 2000, TotalPrice = 2500, TotalCost = 1000, TimeOfRequest = DateTime.UtcNow, TimeOfDelivery = DateTime.UtcNow.AddHours(5.5) };

                var deliveryDataHelper = new DeliveryDataHelper();

                deliveryDataHelper.Create(delivery);

                deliveryDataHelper.Load(1);

                deliveryDataHelper.LoadAll();

                var price = new Price { Origin = routeNode, Destination = destination, Priority = Priority.Air, PricePerCm3 = 3, PricePerGram = 5 };
                var priceDataHelper = new PriceDataHelper();
                //priceDataHelper.Create(price);

                price.PricePerGram = 10;
                price.ID = 1;
                //priceDataHelper.Update(price);
                //priceDataHelper.Load(1);

                var prices = priceDataHelper.LoadAll();

                //priceDataHelper.Delete(1);

                prices = priceDataHelper.LoadAll();            
            }
            catch (Exception e) 
            {
                Logger.WriteLine(e.Message);
                Logger.Write(e.StackTrace);
            }

        }



    }
}
