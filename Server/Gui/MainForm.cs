﻿using System;
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

            // create controller
            var controller = new Controller(countryService, companyService, deliveryService, priceService, routeService,
                                            locationService);

            // initialise network
            Network.Network network = Network.Network.Instance;
            network.Start();
            network.Open();
            // todo - pass controller to network

            BenDBTests(countryService, routeService);
        }




        private void BenDBTests(CountryService countryService, RouteService routeService)
        {
            try
            {
                CountryDataHelper cdh = new CountryDataHelper();

                // create country if doesn't exist
                Country country = new Country {ID=1, Name = "Wellington", Code = "WLG"};
                if (!countryService.Exists(country))
                {
                    country = countryService.Create("Wellington", "WLG");
                }

                // perform updates
                country = countryService.Update(country.ID, "WLN");
                country = countryService.Update(country.ID, "BEN");

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


                // create australia again
                country = new Country {Name = "Australia", Code = "AUS"};
                if (!countryService.Exists(country))
                {
                    country = countryService.Create(country.Name, country.Code);
                }


                // create christchurch depot
                RouteNode routeNode = new DistributionCentre("Christchurch");
                if (!locationService.Exists(routeNode)){
                    routeNode = locationService.CreateDistributionCentre("Christchurch");
                }

                // wellington depot
                routeNode = new DistributionCentre("Wellington");
                if (!locationService.Exists(routeNode)){
                    routeNode = locationService.CreateDistributionCentre("Wellington");
                }

                // australia port
                country = countryService.GetAll().AsQueryable().First(t => t.Name == "Australia");
                var destination = new InternationalPort(country);
                if (!locationService.Exists(destination)){
                    destination = locationService.CreateInternationalPort(country.ID);
                }

                // get a company
                var company = new Company(){Name = "NZ Post"};
                if(!companyService.Exists(company)){
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
                        DepartureTimes = new List<WeeklyTime>{new WeeklyTime(DayOfWeek.Monday, 5, 30)}
                    };
                if (!routeService.Exists(route)) {
                    route = routeService.Create(route.TransportType, route.Company.ID, route.Origin.ID, route.Destination.ID, route.DepartureTimes, route.Duration, route.MaxWeight, route.MaxVolume, route.CostPerGram, route.CostPerCm3); 
                }

            }
            catch (Exception e)
            {
                Logger.Write(e.Message);
            }
        }



    }
}
