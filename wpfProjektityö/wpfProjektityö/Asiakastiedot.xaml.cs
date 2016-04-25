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
using System.Windows.Shapes;

namespace wpfProjektityö
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AsiakasTiedotWindow : Window
    {
        object valittuItem;

        public AsiakasTiedotWindow(object item)
        {
            InitializeComponent();
            valittuItem = item;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string tmp = valittuItem.ToString();
            string id = tmp.Substring((tmp.IndexOf("=") + 1), (tmp.IndexOf(",") - (tmp.IndexOf("=") + 1))).Trim();
            // Hae asikkaan tiedot tekstilaatikoihin
            XMLfunktiot.haeAsiakkaanTiedot(id, this);
        }

        // Sulje ikkuna kun painetaan peruuta
        private void btnPeruuta_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Asiakkaan tietojen tallentaminen XML:ään
        private void btnTallenna_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Muokkaamista ei ole implementoitu.");
        }

        // Asiakkaan poistaminen XML:stä
        private void btnPoistaAsiakas_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Poistamista ei ole implementoitu.");
        }
    }
}
