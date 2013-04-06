using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Server
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Logger.Instance.SetOutput(logBox);
            Logger.WriteLine("Server starting..");

            Network network = Network.Instance;
            network.Start();
            network.Open();

            Database db = new Database();
            db.Connect();
        }
    }
}
