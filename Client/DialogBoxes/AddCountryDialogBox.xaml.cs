﻿using System;
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