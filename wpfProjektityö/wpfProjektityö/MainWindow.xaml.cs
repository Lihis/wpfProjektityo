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
using System.Globalization;
using System.Xml;

namespace wpfProjektityö
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
            haeTilat();
            // Kuluvan viikon maanantai päivä aliohjelmaan
            haeVaraukset(DateTime.Today.AddDays(-(DateTime.Today.DayOfWeek - DayOfWeek.Monday)));
            //MessageBox.Show(DateTime.Today.AddDays(-(DateTime.Today.DayOfWeek - DayOfWeek.Monday)).ToString("dd.MM.yyyy").ToString());
            haeAsiakkaat();
        }

        void haeAsiakkaat()
        {
            XmlReader reader = XmlReader.Create(@"Resources\XMLasiakas.xml");

            while (reader.Read())
            {
                reader.MoveToContent();

                if (reader.NodeType == XmlNodeType.Element &&
                    reader.Name == "asiakas")
                {
                    reader.Read();
                    lstAsiakas.Items.Add(reader.Value);
           
     
                    
                    var gridView = new GridView();                  
                    this.lstAsiakas.View = gridView;
                    gridView.Columns.Add(new GridViewColumn
                    {
                        Header = lstNimi.Header,
                        DisplayMemberBinding = new Binding("AID")

                    });
                    gridView.Columns.Add(new GridViewColumn
                    {
                        Header = lstNimi.Header,
                        DisplayMemberBinding = new Binding("Nimi")
                    });
                    this.lstAsiakas.Items.Add(new { AID = "AID" , Nimi = "David" });
                }                     
            }
            reader.Close();
        }

        // Haetaan tilat/salit listboxiin
        void haeTilat()
        {
            XmlReader reader = XmlReader.Create(@"Resources\salit.xml");

            while (reader.Read())
            {
                reader.MoveToContent();

                if (reader.NodeType == XmlNodeType.Element &&
                    reader.Name == "Nimi")
                {
                    reader.Read();
                    lstTilat.Items.Add(reader.Value);
                }
            }
            reader.Close();
        }

        // Laskee itemin korkeuden halutulle varaus ajalle
        int laskeKorkeus(int alkuaika, int loppuaika)
        {
            int erotus = loppuaika - alkuaika;

            // 25 = puolituntia (erotus on 1)
            // 50 = tunti (erotus on 2) jne.
            return (erotus * 25);
        }

        // Varaukset varaukset.xml:stä listalaatikkoihin
        void haeVaraukset(DateTime monday)
        {
            XmlReader reader = XmlReader.Create(@"Resources\varaukset.xml");
            DateTime date = DateTime.MinValue;
            ListBox päivä = null;
            ListViewItem item = null;
            string varauksenNimi = null;
            int alkuaika = -1, loppuaika = -1, varauksenID = -1;

            // Hae varaukset
            while (reader.Read())
            {
                reader.MoveToContent();

                if (reader.NodeType == XmlNodeType.Element &&
                    reader.Name == "Pvm")
                {
                    reader.Read();

                    date = DateTime.Parse(reader.Value, new CultureInfo("fi-FI"));
                    // TODO: Näytä vain kuluvan viikon varaukset
                    int dayOfWeek = (int)date.DayOfWeek;

                    // 0 = sunnuntai
                    if (dayOfWeek == 0)
                    {
                        päivä = lstSunnuntai;
                    }
                    // 1 = maanantai
                    else if (dayOfWeek == 1)
                    {
                        päivä = lstMaanantai;
                    }
                    // 2 = tiistai
                    else if (dayOfWeek == 2)
                    {
                        päivä = lstMaanantai;
                    }
                    // 3 = keskiviikko
                    else if (dayOfWeek == 3)
                    {
                        päivä = lstMaanantai;
                    }
                    // 4 = torstai
                    else if (dayOfWeek == 4)
                    {
                        päivä = lstMaanantai;
                    }
                    // 5 perjantai
                    else if (dayOfWeek == 5)
                    {
                        päivä = lstMaanantai;
                    }
                    // 6 = lauantai
                    else if (dayOfWeek == 6)
                    {
                        päivä = lstMaanantai;
                    }
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

                // Lisää itemi listaan kun tarvittavat tiedot luettu XML:stä
                if (alkuaika != -1 && loppuaika != -1 && varauksenID != -1 && varauksenNimi != null)
                {
                    // Poista tarvittavat itemit
                    if (alkuaika == loppuaika)
                    {
                        päivä.Items.RemoveAt(alkuaika);
                    }
                    else
                    {
                        for (int i = alkuaika; i < loppuaika; i++)
                        {
                            päivä.Items.RemoveAt(i);
                        }
                    }

                    /* Itemin luominen */
                    item = new ListViewItem();
                    // Itemin teksti ("otsikko") joka näkyy ListBox:ssa
                    item.Content = varauksenNimi;
                    // Itemin tagi
                    item.Tag = varauksenID;
                    // Itemin Klikkaus-eventti
                    item.MouseLeftButtonUp += ListBoxItem_MouseLeftButtonUp;
                    // Itemin korkeus
                    item.Height = laskeKorkeus(alkuaika, loppuaika);

                    // Lisää itemi listaan
                    päivä.Items.Insert(alkuaika, item);

                    // "Tyhjennä" muuttujat
                    varauksenNimi = null;
                    alkuaika = -1;
                    loppuaika = -1;
                    item = null;
                }
            }
        }

        // Näytä "Lisää varaus"-ikkuna kun ListBox:n itemiä klikataan
        // TODO: Mitä välitetään ikkunalla kun se avataan
        private void ListBoxItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Vanhan varauksen tiedot
            //Window varausIkkuna = new LisääVarausWindow("Varauksen tiedot");
            //varausIkkuna.ShowDialog();
            // TODO: Päivitä viikkonäkymä
            //MessageBox.Show("Yay, we're back!");
            //haeVaraukset(DateTime.Today.AddDays(-(DateTime.Today.DayOfWeek - DayOfWeek.Monday)));
        }

        // Näytä "Lisää varaus"-ikkuna kun ListBox:n itemiä klikataan
        // "Lisää varaus" ikkunalle välitetään ListBox jossa on aika/aikaväli valittuna
        private void teeVaraus_Click(object sender, RoutedEventArgs e)
        {
            ListBox listalaatikko = null;

            if (lstMaanantai.SelectedIndex != -1)
            {
                listalaatikko = lstMaanantai;
            }
            else if (lstTiistai.SelectedIndex != -1)
            {
                listalaatikko = lstTiistai;
            }
            else if (lstKeskiviikko.SelectedIndex != -1)
            {
                listalaatikko = lstKeskiviikko;
            }
            else if (lstTorstai.SelectedIndex != -1)
            {
                listalaatikko = lstTorstai;
            }
            else if (lstPerjantai.SelectedIndex != -1)
            {
                listalaatikko = lstPerjantai;
            }
            else if (lstLauantai.SelectedIndex != -1)
            {
                listalaatikko = lstLauantai;
            }
            else if (lstSunnuntai.SelectedIndex != -1)
            {
                listalaatikko = lstSunnuntai;
            }
            else
            {
                MessageBox.Show("Valitse ensin varattava aika!");
            }

            // Näytä Varaus-ikkuna vain jos varattava aika valittu
            if (listalaatikko != null)
            {
                // Uusi varaus
                Window varausIkkuna = new LisääVarausWindow(listalaatikko);
                varausIkkuna.ShowDialog();
                // TODO: Päivitä viikkonäkymä
            }
        }

        void listBox_SelectionChange(bool poista)
        {
            ListBox[] laatikot = { lstMaanantai, lstTiistai, lstKeskiviikko, lstTorstai, lstPerjantai, lstLauantai, lstSunnuntai };

            if (poista == true)
            {
                foreach (ListBox element in laatikot)
                {
                    element.SelectionChanged -= listalaatikko_SelectionChanged;
                }
            }
            else
            {
                foreach (ListBox element in laatikot)
                {
                    element.SelectionChanged += listalaatikko_SelectionChanged;
                }
            }
        }

        // Nollaa valinnat muilta päiviltä kun valitaan jokin toinen päivä
        void nollaaListalaatikkojenValinnat(ListBox listalaatikko)
        {
            ListBox[] laatikot = { lstMaanantai, lstTiistai, lstKeskiviikko, lstTorstai, lstPerjantai, lstLauantai, lstSunnuntai };

            // Poista SelectionChanged event jotta uutta valintaa ei poisteta seuraavassa loopissa
            listBox_SelectionChange(true);

            // Poista valinnat muista kuin juuri valitusta ListBox:sta
            foreach(ListBox element in laatikot)
            {
                if (element != listalaatikko)
                {
                    element.SelectedIndex = -1;
                }
            }

            // Ota SelectionChanged käyttöön uudelleen
            listBox_SelectionChange(false);
        }

        private void listalaatikko_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            nollaaListalaatikkojenValinnat((sender as ListBox));
        }
    }
}