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
            CountryDataHelper cdh = new CountryDataHelper();
            Country country = new Country { Name = "Wellington", Code = "WLG" };
            cdh.Save(country);
            country.Code = "WLN";
            cdh.Save(country);
            country.Code = "BEN";
            cdh.Save(country);

        }

        private void IzziDBTests()
        {
            CompanyDataHelper comDH = new CompanyDataHelper();
            Company company = new Company{Name = "NZ Post"};
            comDH.Save(company);
        }

    }
}
