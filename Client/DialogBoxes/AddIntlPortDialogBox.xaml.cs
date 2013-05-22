using System.Windows;
using System.Windows.Controls;

namespace Client.Countries
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddIntlPortDialogBox : Window
    {
        public AddIntlPortDialogBox(ClientState client)
        {
            InitializeComponent();

            foreach (var country in client.GetAllCountries())
            {
                //this.originComboBox.Items.Add(countries.Name);
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Content = country.Name;
                cbi.Tag = country.ID;
                this.countries.Items.Add(cbi);
            }
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
           
        }
    }
}
