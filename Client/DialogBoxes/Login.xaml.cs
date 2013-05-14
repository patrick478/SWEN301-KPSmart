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
        
        public Login()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {

            Network.Instance.BeginLogin(userName.Text, passwordBox1.Password);

            DialogResult = true;
           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Network.Instance.OnConnectComplete += new Network.OnConnectDelegate(network_OnConnect);
            Network.Instance.DataReady += new Network.DataReadyDelegate(network_DataReady);
            Network.Instance.BeginConnect("127.0.0.1", 23333);
        }

        void network_DataReady(string msg)
        {
//            MessageBox.Show("Message from server: " + msg);
        }

        void network_OnConnect()
        {
            //MessageBox.Show("Connected! Sending message..");
            //Network.Instance.WriteLine("Hello from client!");       
        }
    }
}
