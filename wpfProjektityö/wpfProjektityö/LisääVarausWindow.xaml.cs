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
    /// Interaction logic for LisääVarausWindow.xaml
    /// </summary>
    public partial class LisääVarausWindow : Window
    {
        int itemTag = -1;
        List<int> valitutItemit = new List<int>();

        // TODO: Mitä passataan??
        public LisääVarausWindow(ListBox list)
        {
            InitializeComponent();

            foreach (ListBoxItem valittu in list.SelectedItems)
            {
                // Hae Tag
                if (valittu.Tag != null)
                    itemTag = int.Parse(valittu.Tag.ToString());

                // Lisää itemi listaan
                valitutItemit.Add(list.Items.IndexOf(valittu));
            }
        }

        // Sulje ikkuna kun painetaan "Peruuta"
        private void btnPeruuta_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Jos itemTag ei ole null, halutaan hakea jo olemassa olevan varauksen tiedot
            // muussa tapauksessa ollaan tekemässä uutta varausta
            if (itemTag != -1)
            {
                MessageBox.Show("Tagi: " + itemTag);
            }
            cmbAlkuAika.SelectedIndex = valitutItemit.First();
            cmbLoppuAika.SelectedIndex = valitutItemit.Last();
        }

        void haeVarauksenTiedot()
        {
            XmlReader reader = XmlReader.Create(@"Resources\varaukset.xml");

            // Hae varaukset
            while (reader.Read())
            {
                reader.MoveToContent();

                if (reader.NodeType == XmlNodeType.Element &&
                    reader.Name == "Pvm")
                {
                    reader.Read();
                    
                }

                // Varauksen nimi ("otsikko")
                if (reader.NodeType == XmlNodeType.Element &&
                    reader.Name == "Nimi")
                {
                    reader.Read();

                    varauksenNimi = reader.Value;
                }

                // Varauksen ID
                if (reader.NodeType == XmlNodeType.Element &&
                    reader.Name == "VarausID")
                {
                    reader.Read();

                    varauksenID = int.Parse(reader.Value);
                }

                // Varauksen alkuaika
                if (reader.NodeType == XmlNodeType.Element &&
                    reader.Name == "AlkuAika")
                {
                    reader.Read();

                    alkuaika = positiot.haePositio(reader.Value);
                }

                // Varauksen loppuaika
                if (reader.NodeType == XmlNodeType.Element &&
                    reader.Name == "LoppuAika")
                {
                    reader.Read();

                    loppuaika = positiot.haePositio(reader.Value);
                }
        }
    }
}
