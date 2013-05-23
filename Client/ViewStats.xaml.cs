using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Common;
using Server.Business;

namespace Client
{
    /// <summary>
    /// Interaction logic for RequestDelivery.xaml
    /// </summary>
    public partial class ViewStats : Page
    {
        private readonly ClientController _clientCon;
        private DateTime lastDate;
        private DateTime firstDate;

        delegate void DateArgumentDelegate(DateTime date);
        DateArgumentDelegate initialDateDelegate;


        public ViewStats(ClientController clientCon)
        {
            _clientCon = clientCon;

            InitializeComponent();

            triples.Columns.Add(new DataGridTextColumn { Header = "Origin", Binding = new Binding("Origin") });
            triples.Columns.Add(new DataGridTextColumn { Header = "Destination", Binding = new Binding("Destination") });
            triples.Columns.Add(new DataGridTextColumn { Header = "Priority", Binding = new Binding("Priority") });

            lastDate = DateTime.UtcNow;

            dateSlider.IsEnabled = false;

            _clientCon.StatsReceived += new ClientController.StatisticsReceivedDelegate(Stats_Recieved);

            _clientCon.InitialDateReceived += new ClientController.InitialDateDeleage(InitialDate_Recieved);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void backToHomeButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new System.Uri("Home.xaml", UriKind.RelativeOrAbsolute));
        }

        private void dateSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            selectedDate.Content = "Selected Date: " + firstDate.AddDays(dateSlider.Value).Date;
        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
            _clientCon.StatsRequest(firstDate.AddDays(dateSlider.Value));
        }

        public void Stats_Recieved(Statistics stats)
        {
            MessageBox.Show("Stats recieved");
        }


        private void InitialDate_Recieved(DateTime date)
        {
            try
            {
                dateSlider.Dispatcher.VerifyAccess();
                do_InitialDate(date);
            }
            catch (InvalidOperationException e)
            {
                if (initialDateDelegate == null)
                    initialDateDelegate = new DateArgumentDelegate(do_InitialDate);
                //dateSlider.Dispatcher.Invoke(do_InitialDate(date));
            }


        }

        private void do_InitialDate(DateTime date)
        {
            firstDate = date;

            var numDays = (lastDate - firstDate).Days;

            dateSlider.Maximum = numDays;
            dateSlider.IsEnabled = true;
           
        }

    }
}
