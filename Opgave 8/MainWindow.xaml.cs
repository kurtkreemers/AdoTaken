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

namespace Opgave_8
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Plant> ListBoxPlantenLijst = new List<Plant>();
       
        public MainWindow()
        {
            InitializeComponent();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var manager = new TuinManager();
                comboboxSoort.DisplayMemberPath = "SoortNaam";
                comboboxSoort.SelectedValuePath = "SoortNr";
                comboboxSoort.ItemsSource = manager.GetSoorten();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void comboboxSoort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WijzigingenOpslaan();

            try
            {
                var manager = new TuinManager();
                ListBoxPlantenLijst = manager.GetPlanten(Convert.ToInt32(comboboxSoort.SelectedValue));
                listboxSoort.ItemsSource = ListBoxPlantenLijst;
                listboxSoort.DisplayMemberPath = "PlantNaam";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btOpslaan_Click(object sender, RoutedEventArgs e)
        {
            WijzigingenOpslaan();

        }

        public void WijzigingenOpslaan()
        {
            List<Plant> GewijzigdePlantenLijst = new List<Plant>();
            foreach (Plant pl in ListBoxPlantenLijst)
            {
                if (pl.Changed == true)
                {
                    GewijzigdePlantenLijst.Add(pl);
                    pl.Changed = false;
                }
            }
            if ((GewijzigdePlantenLijst.Count() != 0) && (MessageBox.Show("Gewijzigde planten van soort \"" + ((Soort)comboboxSoort.SelectedItem).SoortNaam + "\" opslaan?", "Opslaan", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes))
            {
                var manager = new TuinManager();
                try
                {
                    manager.SchrijfWijzigingen(GewijzigdePlantenLijst);
                    MessageBox.Show("Planten opgeslagen", "Opslaan", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Er is een fout opgetreden: " + ex.Message, "Opslaan", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }

            GewijzigdePlantenLijst.Clear();
        }
    }
}
