using System;
using Server.Data;

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

            Database db = new Database();
            db.Connect();
        }
    }
}
