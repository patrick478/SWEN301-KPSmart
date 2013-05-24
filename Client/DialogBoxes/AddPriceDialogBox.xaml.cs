using System.Windows;
using System.Windows.Controls;
using Server.Business;
using Common;

namespace Client.DialogBoxes
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddPriceDialogBox : Window
    {
        public AddPriceDialogBox(ClientState clientState, bool domestic)
        {
            InitializeComponent();
            var locationService = new LocationService(clientState);
            foreach (var routeNode in clientState.GetAllRouteNodes())
            {
                var cbi = new ComboBoxItem();
                if (routeNode is DistributionCentre)
                    cbi.Content = ((DistributionCentre)routeNode).Name;
                else if (routeNode is InternationalPort && !domestic)
                    cbi.Content = routeNode.Country.Name;
                cbi.Tag = routeNode.ID;
                this.origin.Items.Add(cbi);

                var cbi2 = new ComboBoxItem();
                if (routeNode is DistributionCentre )
                    cbi2.Content = ((DistributionCentre)routeNode).Name;
                else if (routeNode is InternationalPort && !domestic)
                    cbi2.Content = routeNode.Country.Name;
                cbi2.Tag = routeNode.ID;
                this.dest.Items.Add(cbi2);
            }
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
           
        }
    }
}
