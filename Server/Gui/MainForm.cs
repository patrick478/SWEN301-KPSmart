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

        private CurrentState currentState;
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
            currentState = new CurrentState();

            // initialise all the services (they set up the state themselves) and pathfinder
            countryService = new CountryService(currentState);
            companyService = new CompanyService(currentState);
            routeService = new RouteService(currentState);
            var pathFinder = new PathFinder(routeService); // pathfinder needs the RouteService and state
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

            //BenDBTests(countryService, routeService);
            SetUpDatabaseWithData();

        }


        private void SetUpDatabaseWithData () 
        {
            var countryDataHelper = new CountryDataHelper();
            
            // countries
            var newZealand = new Country { Name = "New Zealand", Code = "NZ"};
            countryDataHelper.Create(newZealand);

            var australia = new Country { Name = "Australia", Code = "AUS" };
            countryDataHelper.Create(australia);

            var japan = new Country { Name = "Japan", Code = "JAP" };
            countryDataHelper.Create(japan);


            var routeNodeDataHelper = new RouteNodeDataHelper();
            // international ports
            var australiaP = new InternationalPort(australia);
            routeNodeDataHelper.Create(australiaP);

            var japanP = new InternationalPort(japan);
            routeNodeDataHelper.Create(japanP);

            // distribution centres
            var auckland = new DistributionCentre("Auckland");
            routeNodeDataHelper.Create(auckland);
            var wellington = new DistributionCentre("Wellington");
            routeNodeDataHelper.Create(wellington);
            var christchurch = new DistributionCentre("Christchurch");
            routeNodeDataHelper.Create(christchurch);
            var hamilton = new DistributionCentre("Hamilton");
            routeNodeDataHelper.Create(hamilton);
            var rotorua = new DistributionCentre("Rotorua");
            routeNodeDataHelper.Create(rotorua);
            var palmerstonNorth = new DistributionCentre("Palmerston North");
            routeNodeDataHelper.Create(palmerstonNorth);
            var dunedin = new DistributionCentre("Dunedin");
            routeNodeDataHelper.Create(dunedin);

            // company
            var companyDataHelper = new CompanyDataHelper();
            var nzPost = new Company{ Name = "NZ Post" };
            companyDataHelper.Create(nzPost);
            var quantas = new Company{ Name = "Quantas" };
            companyDataHelper.Create(quantas);
            var airNZ = new Company { Name = "Air New Zealand" };
            companyDataHelper.Create(airNZ);

            // routes
            var routeDataHelper = new RouteDataHelper();
            var wellToAuckLand = new Route { Origin = wellington, Destination = auckland, Company = nzPost, TransportType = TransportType.Land, CostPerCm3 = 2, CostPerGram = 2, MaxVolume = 10000, MaxWeight = 5000, Duration = 480, DepartureTimes = new List<WeeklyTime> { new WeeklyTime(DayOfWeek.Monday, 8, 0), new WeeklyTime(DayOfWeek.Tuesday, 8, 0), new WeeklyTime(DayOfWeek.Wednesday, 8, 0), new WeeklyTime(DayOfWeek.Thursday, 8, 0), new WeeklyTime(DayOfWeek.Friday, 8, 0) } };
            routeDataHelper.Create(wellToAuckLand);

            var wellToAuckAir = new Route { Origin = wellington, Destination = auckland, Company = airNZ, TransportType = TransportType.Air, CostPerCm3 = 8, CostPerGram = 10, MaxVolume = 10000, MaxWeight = 5000, Duration = 100, DepartureTimes = new List<WeeklyTime> { new WeeklyTime(DayOfWeek.Monday, 8, 0), new WeeklyTime(DayOfWeek.Tuesday, 8, 0), new WeeklyTime(DayOfWeek.Wednesday, 8, 0), new WeeklyTime(DayOfWeek.Thursday, 8, 0), new WeeklyTime(DayOfWeek.Friday, 8, 0) } };
            routeDataHelper.Create(wellToAuckAir);

            var auckToAusAir = new Route { Origin = auckland, Destination = australiaP, Company = airNZ, TransportType = TransportType.Air, CostPerCm3 = 10, CostPerGram = 12, MaxVolume = 8000, MaxWeight = 3000, Duration = 150, DepartureTimes = new List<WeeklyTime> { new WeeklyTime(DayOfWeek.Monday, 11, 0), new WeeklyTime(DayOfWeek.Tuesday, 11, 0), new WeeklyTime(DayOfWeek.Wednesday, 11, 0), new WeeklyTime(DayOfWeek.Thursday, 11, 0), new WeeklyTime(DayOfWeek.Friday, 11, 0) } };
            routeDataHelper.Create(auckToAusAir);


            // prices
            var priceDataHelper = new PriceDataHelper();
            var wellToAuckStandardPrice = new Price { Origin = wellington, Destination = auckland, Priority = Priority.Standard, PricePerCm3 = 4, PricePerGram = 4 };
            priceDataHelper.Create(wellToAuckStandardPrice);

            var wellToAuckAirPrice = new Price { Origin = wellington, Destination = auckland, Priority = Priority.Air, PricePerCm3 = 12, PricePerGram = 12 };
            priceDataHelper.Create(wellToAuckAirPrice);

            var auckToAusAirPrice = new Price { Origin = auckland, Destination = australiaP, Priority = Priority.Air, PricePerCm3 = 15, PricePerGram = 15 };
            priceDataHelper.Create(auckToAusAirPrice);





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

                var delivery = new Delivery { Origin = routeNode, Destination = destination, Priority = Priority.Air, WeightInGrams = 200, VolumeInCm3 = 2000, TotalPrice = 2500, TotalCost = 1000, TimeOfRequest = DateTime.UtcNow, TimeOfDelivery = DateTime.UtcNow.AddHours(5.5), Routes = new List<RouteInstance> { new RouteInstance(route, DateTime.UtcNow)} };

                var deliveryDataHelper = new DeliveryDataHelper();

                deliveryDataHelper.Create(delivery);

                deliveryDataHelper.Load(delivery.ID);

                deliveryDataHelper.LoadAll();

                var price = new Price { Origin = routeNode, Destination = destination, Priority = Priority.Air, PricePerCm3 = 3, PricePerGram = 5 };
                var priceDataHelper = new PriceDataHelper();
                //priceDataHelper.Create(price);

                price.PricePerGram = 10;
                price.ID = 1;

                Logger.WriteLine(price.ToString());
                
            }
            catch (Exception e) 
            {
                Logger.WriteLine(e.Message);
                Logger.Write(e.StackTrace);
            }

        }

        private void logBox_TextChanged(object sender, EventArgs e)
        {
            logBox.SelectionStart = logBox.Text.Length;
            logBox.ScrollToCaret();
        }



    }
}
