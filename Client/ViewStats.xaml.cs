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
        private readonly ClientState _clientState;

        private DateTime lastDate;
        private DateTime firstDate;

        delegate void DateArgumentDelegate(DateTime date);
        DateArgumentDelegate initialDateDelegate;

        delegate void StatsDelegate(Statistics stats);
        StatsDelegate statsDelegate;


        public ViewStats(ClientController clientCon, ClientState clientState)
        {
            _clientCon = clientCon;
            _clientState = clientState;

            InitializeComponent();

            triples.Columns.Add(new DataGridTextColumn { Header = "Origin", Binding = new Binding("Origin") });
            triples.Columns.Add(new DataGridTextColumn { Header = "Destination", Binding = new Binding("Destination") });
            triples.Columns.Add(new DataGridTextColumn { Header = "Priority", Binding = new Binding("Priority") });

            lastDate = DateTime.UtcNow;

            firstDate = _clientState.FirstEvent;

            var numDays = lastDate.Day - firstDate.Day;
            dateSlider.Maximum = numDays;
            dateSlider.IsEnabled = true;

            firstDayLabel.Content = firstDate.ToShortDateString();
            lastDayLabel.Content = lastDate.ToShortDateString();

            

            _clientCon.StatsReceived += new ClientController.StatisticsReceivedDelegate(Stats_Recieved);

            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void backToHomeButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Home(_clientState));
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
            try
            {
                revenue.Dispatcher.VerifyAccess();
                PopulateStats(stats);
            }
            catch (InvalidOperationException e)
            {
                if (statsDelegate == null)
                    statsDelegate = new StatsDelegate(PopulateStats);
                revenue.Dispatcher.Invoke(statsDelegate, System.Windows.Threading.DispatcherPriority.Normal, stats);
            }
        }


        public void PopulateStats(Statistics stats)
        {
            MessageBox.Show("Stats recieved");
            revenue.Text = Convert.ToString(stats.TotalRevenue);
            expenditure.Text = Convert.ToString(stats.TotalExpenditure);
            events.Text = Convert.ToString(stats.TotalEvents);
        }

                

    }
}
