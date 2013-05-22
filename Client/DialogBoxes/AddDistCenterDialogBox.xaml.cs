using System.Windows;

namespace Client.DialogBoxes
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddDistCenterDialogBox : Window
    {
        public AddDistCenterDialogBox()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
           
        }
    }
}
