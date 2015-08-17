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

namespace Opgave3
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

        private void Toevoegen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var manager = new TuinManager();
                var deLeverancier = new Leverancier();
                deLeverancier.Naam = tbNaam.Text;
                deLeverancier.Adres = tbAdres.Text;
                deLeverancier.PostNr = tbPostcode.Text;
                deLeverancier.Woonplaats = tbPlaats.Text;
                manager.LeverancierToevoegen(deLeverancier);

                labelStatus.Content = "Leverancier met nummer " + deLeverancier.LevNr +  " is toegevoegd";

            }
            catch (Exception ex)
            {
                
                labelStatus.Content = ex.Message;
            }
        }

        private void Korting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var manager = new TuinManager();
                labelStatus.Content = manager.Eindejaarskorting().ToString()
                + " plantenprijzen aangepast";
            }
            catch (Exception ex)
            {
                labelStatus.Content = ex.Message;
            }
        }

        private void Vervangen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var manager = new TuinManager();
                manager.VervangLeverancier(2, 3);
                labelStatus.Content = "Leverancier 2 is verwijderd en vervangen door 3";
            }
            catch (Exception ex)
            {
                
                labelStatus.Content = ex.Message;
            }
        }
    }
}
