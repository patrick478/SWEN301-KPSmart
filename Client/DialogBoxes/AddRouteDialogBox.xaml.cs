using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Client.DialogBoxes
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddRouteDialogBox : Window
    {
        public AddRouteDialogBox(ClientState client)
        {
            InitializeComponent();
            //this.comboBox1.ItemsSource = client.GetAllRouteNodes();

            timesGrid.Columns.Add(new DataGridTextColumn { Header = "Day", Binding = new Binding("Day") });
            timesGrid.Columns.Add(new DataGridTextColumn { Header = "Hour", Binding = new Binding("Hour") });
            timesGrid.Columns.Add(new DataGridTextColumn { Header = "Minute", Binding = new Binding("Minute") });

            foreach (var country in client.GetAllCountries())
            {
                //this.originComboBox.Items.Add(countries.Name);
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Content = country.Name;
                cbi.Tag = country.ID;
                this.originComboBox.Items.Add(cbi);

                ComboBoxItem cbi2 = new ComboBoxItem();
                cbi2.Content = country.Name;
                cbi2.Tag = country.ID;
                this.destComboBox.Items.Add(cbi2);
            }

            foreach (var company in client.GetAllCompanies())
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
