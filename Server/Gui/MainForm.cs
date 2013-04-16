using System;
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
            Logger.Instance.SetOutput(logBox);
            Logger.WriteLine("Server starting..");

            Network.Network network = Network.Network.Instance;
            network.Start();
            network.Open();

            Database.Instance.Connect();
            BenDBTests();
            IzziDBTests();
        }


        private void BenDBTests()
        {
            try
            {

                CountryDataHelper cdh = new CountryDataHelper();

                // create country if doesn't exist
                Country country = new Country {Name = "Wellington", Code = "WLG"};
                if (cdh.GetId(country) == 0)
                {
                    cdh.Create(country);
                }

                // perform updates
                country.Code = "WLN";
                cdh.Update(country);
                country.Code = "BEN";
                cdh.Update(country);

                // get latest version
                Country loadedCountry = cdh.Load(country.ID);
                Logger.WriteLine("Current country ( ID: {0}, Name: {1}, Code: {2} )", country.ID, country.Name,
                                     country.Code);
                
            }
            catch (DatabaseException e)
            {
                Logger.Write(e.Message);
            }
        }

        private void IzziDBTests()
        {
            try{
                CountryDataHelper cdh = new CountryDataHelper();
                Country country = new Country { Name = "Wellington", Code = "WLG" };

                int existingId = cdh.GetId(country);
                Logger.WriteLine("Existing country id: " + existingId);

            }
            catch (DatabaseException e)
            {
                Logger.Write(e.Message);
            }
        }

    }
}
