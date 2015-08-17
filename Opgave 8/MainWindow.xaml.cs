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
            try
            {
                listboxSoort.Items.Clear();
                int soortNr = Convert.ToInt32(comboboxSoort.SelectedValue);
                var manager = new TuinManager();
                var allePlanten = manager.GetPlanten(soortNr);
                foreach (var eenPlant in allePlanten)
                {
                    listboxSoort.Items.Add(eenPlant);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


    }
}
