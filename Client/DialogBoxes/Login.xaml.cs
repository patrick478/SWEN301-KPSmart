using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client.Countries
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Login : Window
    {
        Network network = new Network();

        public Login()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            network.OnConnectComplete += new Network.OnConnectDelegate(network_OnConnect);
            network.DataReady += new Network.DataReadyDelegate(network_DataReady);
            network.BeginConnect("127.0.0.1", 23333);
        }

        void network_DataReady(string msg)
        {
            MessageBox.Show("Message from server: " + msg);
        }

        void network_OnConnect()
        {
            MessageBox.Show("Connected! Sending message..");
            network.WriteLine("Hello from client!");       
        }
    }
}
