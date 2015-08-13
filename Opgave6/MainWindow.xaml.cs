using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Gemeenschap;

namespace Opgave6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonOpzoeken_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var manager = new TuinManager();
                var info = manager.PlantenGegevensOpvragen(Convert.ToInt32(tbSoortNummer.Text));
                labelNaam.Content = info.Naam;
                labelSoort.Content = info.Soort;
                labelLeverancier.Content = info.Leverancier;
                labelKleur.Content = info.Kleur;
                labelKostPrijs.Content = String.Format("{0:C}", info.Prijs);
                labelStatus.Content = string.Empty;
            }
            catch (Exception ex)
            {

                labelNaam.Content = string.Empty;
                labelSoort.Content = string.Empty;
                labelLeverancier.Content = string.Empty;
                labelKleur.Content = string.Empty;
                labelKostPrijs.Content = string.Empty;
                labelStatus.Content = ex.Message ;
            }
        }
    }
}
