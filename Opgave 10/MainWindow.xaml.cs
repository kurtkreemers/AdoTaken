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
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Opgave_10
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CollectionViewSource leverancierViewSource = new CollectionViewSource();
        public ObservableCollection<Leverancier> levOb = new ObservableCollection<Leverancier>();
        public List<Leverancier> OudeLeveranciers = new List<Leverancier>();
        public List<Leverancier> NieuweLeveranciers = new List<Leverancier>();
        public List<Leverancier> GewijzigdeLeveranciers = new List<Leverancier>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            leverancierViewSource = ((CollectionViewSource)(this.FindResource("leverancierViewSource")));
            var manager = new TuinManager();
            levOb = manager.GetLeveranciers();
            leverancierViewSource.Source = levOb;
            levOb.CollectionChanged += this.OnCollectionChanged;
            cbPostcode.Items.Add("alles");
            List<string> pc = manager.GetPostCodes();
            foreach (var p in pc)
            {
                cbPostcode.Items.Add(p);
            }
            cbPostcode.SelectedItem = "alles";
        }

        private void cbPostcode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbPostcode.SelectedIndex == 0)
                leverancierDataGrid.Items.Filter = null;
            else
                leverancierDataGrid.Items.Filter = new Predicate<object>(PostCodeFilter);
        }
        public bool PostCodeFilter(object leverancier)
        {
            Leverancier lev = leverancier as Leverancier;
            bool result = (lev.PostNr == Convert.ToString(cbPostcode.SelectedValue));
            return result;
        }
        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs
e)
        {
            if (e.OldItems != null)
            {
                foreach (Leverancier oudeLeverancier in e.OldItems)
                {                  
                    OudeLeveranciers.Add(oudeLeverancier);
                }
            }
            if(e.NewItems != null)
            {
                foreach (Leverancier nieweLeverancier in e.NewItems)
                {
                    NieuweLeveranciers.Add(nieweLeverancier);
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var manager = new TuinManager();
            foreach (Leverancier lev in levOb)
            {
                if (lev.Changed == true)
                    GewijzigdeLeveranciers.Add(lev);
                lev.Changed = false;
            }
            if (OudeLeveranciers.Count() != 0 || NieuweLeveranciers.Count() != 0 || GewijzigdeLeveranciers.Count() != 0)
            {
                if (MessageBox.Show("Wilt u alles wegschrijven naar de database ?", "Opslaan", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (OudeLeveranciers.Count() != 0)
                        manager.SchrijfVerwijderingen(OudeLeveranciers);
                    else if (NieuweLeveranciers.Count() != 0)
                        manager.SchrijfToevoegingen(NieuweLeveranciers);
                    else if (GewijzigdeLeveranciers.Count() != 0)
                        manager.SchrijfWijzigingen(GewijzigdeLeveranciers);
                }
            }
            OudeLeveranciers.Clear();
            NieuweLeveranciers.Clear();
            GewijzigdeLeveranciers.Clear();
           
        }

    }     
}
