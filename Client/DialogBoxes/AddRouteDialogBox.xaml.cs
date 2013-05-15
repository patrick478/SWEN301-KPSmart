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
    }
}
