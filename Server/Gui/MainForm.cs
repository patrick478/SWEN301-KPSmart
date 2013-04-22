using System;
using Server.Business;
using Server.Data;
using Common;

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

            // initialise network
            Network.Network network = Network.Network.Instance;
            network.Start();
            network.Open();

            // initialise database
            Database.Instance.Connect();

            // initialise the state
            var currentState = new CurrentState();

            // initialise all the services (they set up the state themselves)
            var countryService = new CountryService(currentState);



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
                    country = countryService.CreateCountry("Wellington", "WLG");
                }

                // perform updates
                country = countryService.EditCountry(country.ID, country.Name, "WLN");
                country = countryService.EditCountry(country.ID, country.Name, "BEN");

                // get latest version
                Country loadedCountry = countryService.LoadCountry(country.ID);

                // load all countries
                var allCountries = countryService.GetAllCountries();

                // create new zealand
                country = new Country { Name = "New Zealand", Code = "NZ" };
                if (!countryService.Exists(country))
                {
                    country = countryService.CreateCountry(country.Name, country.Code);
                }

                // create australia
                country = new Country {Name = "Australia", Code = "AUS"};
                if (!countryService.Exists(country))
                {
                    country = countryService.CreateCountry(country.Name, country.Code);
                }

                // delete australia
                foreach (Country c in allCountries)
                {
                    if (c.Name == "Australia")
                    {
                        countryService.DeleteCountry(c.ID);
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
