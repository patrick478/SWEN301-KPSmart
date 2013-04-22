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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Client.Countries;

namespace Client
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home 
    {
        public Home()
        {
            InitializeComponent();
        }


        private void addCountry_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate the dialog box
            var dlg = new AddCountryDialogBox();

            // Open the dialog box modally 
            dlg.ShowDialog();
            if (dlg.DialogResult != false)
            {
                countriesList.Items.Add(dlg.countryName.Text);
            }
        }

        private void editCountry_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate the dialog box
            var dlg = new AddCountryDialogBox
                {
                    countryName = {Text = ((ListBoxItem) countriesList.SelectedItem).Content.ToString()}
                };

            // Open the dialog box modally 
            dlg.ShowDialog();

            if (dlg.DialogResult != false)
            {
                countriesList.Items.Remove(countriesList.SelectedItem);
                countriesList.Items.Add(dlg.countryName.Text);
            }

            
        }

        private void deleteCountry_Click(object sender, RoutedEventArgs e)
        {
            countriesList.Items.Remove(countriesList.SelectedItem);
        }


       
    }
}
