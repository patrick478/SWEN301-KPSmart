using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Common;

namespace Client.DialogBoxes
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddRouteDialogBox : Window
    {
        public AddRouteDialogBox(ClientState clientState)
        {
            InitializeComponent();
            //this.comboBox1.ItemsSource = clientState.GetAllRouteNodes();

            timesGrid.Columns.Add(new DataGridTextColumn { Header = "Day", Binding = new Binding("Day") });
            timesGrid.Columns.Add(new DataGridTextColumn { Header = "Hour", Binding = new Binding("Hour") });
            timesGrid.Columns.Add(new DataGridTextColumn { Header = "Minute", Binding = new Binding("Minute") });

            foreach (var routeNode in clientState.GetAllRouteNodes())
            {
                ComboBoxItem cbi = new ComboBoxItem();
                if (routeNode is DistributionCentre)
                    cbi.Content = ((DistributionCentre)routeNode).Name;
                else if (routeNode is InternationalPort)
                    cbi.Content = routeNode.Country.Name;
                cbi.Tag = routeNode.ID;
                this.originComboBox.Items.Add(cbi);

                ComboBoxItem cbi2 = new ComboBoxItem();
                if (routeNode is DistributionCentre)
                    cbi2.Content = ((DistributionCentre)routeNode).Name;
                else if (routeNode is InternationalPort)
                    cbi2.Content = routeNode.Country.Name;
                cbi2.Tag = routeNode.ID;
                this.destComboBox.Items.Add(cbi2);
            }

            foreach (var company in clientState.GetAllCompanies())
            {
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Content = company.Name;
                cbi.Tag = company.ID;
                this.companyComboBox.Items.Add(cbi);
            }
            
        } 

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {

            if (Convert.ToInt32(hours.Text) > 23 || Convert.ToInt32(hours.Text) < 0 || Convert.ToInt32(minutes.Text) > 59 || Convert.ToInt32(minutes.Text) < 0)
            {
                MessageBox.Show("Invalid hours and/or minute entered");
                return;
            }

            try
            {
                timesGrid.Items.Add(new DayMinuteHourHolder
                    {
                        Day = day.Text,
                        Hour = Convert.ToInt32(hours.Text),
                        Minute = Convert.ToInt32(minutes.Text)
                    });
            }
            catch(Exception ex)
            {
                MessageBox.Show("You have entered invalid data. " + ex.Message);
            }
        }

        private void remove_Click(object sender, RoutedEventArgs e)
        {
            timesGrid.Items.Remove(timesGrid.SelectedItem);
        }
    }
}
