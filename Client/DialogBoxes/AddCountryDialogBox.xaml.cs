﻿using System.Windows;

namespace Client.Countries
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddCountryDialogBox : Window
    {
        public AddCountryDialogBox()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
           
        }
    }
}
