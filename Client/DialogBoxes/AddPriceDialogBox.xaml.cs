using System.Windows;

namespace Client.DialogBoxes
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddPriceDialogBox : Window
    {
        public AddPriceDialogBox()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
           
        }
    }
}
