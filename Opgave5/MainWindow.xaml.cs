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


namespace Opgave5
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

        private void buttonGemiddelde_Click(object sender, RoutedEventArgs e)
        {
            var manager = new TuinManager();
            try
            {
                labelStatus.Content = manager.Gemiddelde1SoortBerekenen(tbSoort.Text).ToString("N");
            }
            catch (Exception ex)
            {
                
                labelStatus.Content = ex.Message;
            }
        }
    }
}
