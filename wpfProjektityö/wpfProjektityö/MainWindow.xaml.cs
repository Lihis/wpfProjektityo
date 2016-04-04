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
        ListBox[] laatikot;
        DateTime näytettäväVkPvm = viikonMaanataiPvm(DateTime.Today);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Hae kuluvan viikon maanantain päivämäärä

            // Populate laatikot
            laatikot = new ListBox[] { lstMaanantai, lstTiistai, lstKeskiviikko, lstTorstai, lstPerjantai, lstLauantai, lstSunnuntai };
            // Hae tilat listalaatikkoon
            haeTilat();
            // Välitä kuluvan viikon maanantain päivämäärä aliohjelmaan
            haeVaraukset();
            haeAsiakkaat();
        }

        static DateTime viikonMaanataiPvm(DateTime pvm)
        {
            DateTime maanataiPvm;
            int offset;

            offset = pvm.DayOfWeek - DayOfWeek.Monday;
            offset = (offset < 0) ? 6 : offset;
            
            maanataiPvm = pvm.AddDays(-offset);

            return maanataiPvm;
        }

        void haeAsiakkaat()
        {
            XmlReader reader = XmlReader.Create(@"Resources\XMLasiakas.xml");

            while (reader.Read())
            {
                reader.MoveToContent();

                if (reader.NodeType == XmlNodeType.Element &&
                    reader.Name == "Nimi")
                {
                    reader.Read();
                    //lstAsiakas.Items.Add(reader.Value);
                    
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
                    this.lstAsiakas.Items.Add(new { AID = reader.Value , Nimi = reader.Value });
                }                     
            }
            reader.Close();
        }

        // Haetaan tilat/salit listboxiin
        void haeTilat()
        {
            XmlReader reader = XmlReader.Create(@"Resources\salit.xml");
            ListBoxItem item = null;
            string tilaID = null, tilanNimi = null;

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "Nimi":
                            reader.Read();
                            tilanNimi = reader.Value;
                            break;
                        case "ID":
                            reader.Read();
                            tilaID = reader.Value;
                            break;
                        default:
                            break;
                    }
                }

                if (tilaID != null && tilanNimi != null)
                {
                    item = new ListBoxItem();
                    // Itemin teksti ("otsikko") joka näkyy ListBox:ssa
                    item.Content = tilanNimi;
                    // Itemin tagi
                    item.Tag = tilaID;

                    lstTilat.Items.Add(item);

                    // Nollaa arvot
                    item = null; tilanNimi = null; tilaID = null;
                }
            }
            reader.Close();

            // Kohdista valinta ListBox:n ensimmäiseen itemiin
            lstTilat.SelectedIndex = 0;
        }

        // Laskee itemin korkeuden halutulle varaus ajalle
        int laskeKorkeus(int alkuaika, int loppuaika)
        {
            int erotus = loppuaika - alkuaika;

            // 25 = puolituntia (erotus on 1)
            // 50 = tunti (erotus on 2) jne.
            return (erotus * 25);
        }

        void päivitäListBoxPvm(DateTime pvm)
        {
            ListBox[] laatikot = { lstMaanantai, lstTiistai, lstKeskiviikko, lstTorstai, lstPerjantai, lstLauantai, lstSunnuntai };

            for (int i = 0; i < laatikot.Length; i++)
            {
                laatikot[i].Tag = (pvm.AddDays(i).Date).ToString("d");
            }
        }

        // Varaukset varaukset.xml:stä listalaatikkoihin
        void haeVaraukset()
        {
            // Tyhjennä ListBoxItem:t listalaatikoista
            for (int i = 0; i < laatikot.Length; i++)
            {
                for (int j = laatikot[i].Items.Count - 1; j >= 0; j--)
                {
                    laatikot[i].Items.RemoveAt(j);
                }
            }

            // Lisää ListBoxItem:t listalaatikkoihin
            for (int i = 0; i < laatikot.Length; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    laatikot[i].Items.Add(new ListBoxItem());
                }
            }

            XmlReader reader = XmlReader.Create(@"Resources\varaukset.xml");
            DateTime date = DateTime.MinValue;
            ListBox päivä = null;
            ListViewItem item = null;
            string varauksenNimi = null;
            int alkuaika = -1, loppuaika = -1, varauksenID = -1, saliID = -1;

            // Päivitä listalaatikoille päivämäärä
            päivitäListBoxPvm(näytettäväVkPvm);

            // Hae varaukset
            while (reader.Read())
            {
                reader.MoveToContent();

                // Varauksen ID
                if (reader.NodeType == XmlNodeType.Element &&
                    reader.Name == "VarausID")
                {
                    reader.Read();
                    varauksenID = int.Parse(reader.Value);
                }

                if (reader.NodeType == XmlNodeType.Element &&
                    reader.Name == "SaliID")
                {
                    reader.Read();
                    saliID = int.Parse(reader.Value);
                }


                // Päivämäärä
                if (reader.NodeType == XmlNodeType.Element &&
                    reader.Name == "Pvm" && saliID == int.Parse(((ListBoxItem)lstTilat.SelectedItem).Tag.ToString()))
                {
                    reader.Read();

                    // Varauksen päivämäärä
                    date = DateTime.Parse(reader.Value, new CultureInfo("fi-FI"));

                    // Näytä vain halutun viikon varaukset
                    int eropäivissä = (int)(date - näytettäväVkPvm).TotalDays;
                    if (eropäivissä >= 0 && eropäivissä < 7)
                    {
                        // Parsi viikonpäivä (ma, ti, etc) päivämäärästä
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
                            päivä = lstTiistai;
                        }
                        // 3 = keskiviikko
                        else if (dayOfWeek == 3)
                        {
                            päivä = lstKeskiviikko;
                        }
                        // 4 = torstai
                        else if (dayOfWeek == 4)
                        {
                            päivä = lstTorstai;
                        }
                        // 5 perjantai
                        else if (dayOfWeek == 5)
                        {
                            päivä = lstPerjantai;
                        }
                        // 6 = lauantai
                        else if (dayOfWeek == 6)
                        {
                            päivä = lstLauantai;
                        }

                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                switch (reader.Name)
                                {
                                    case "Nimi":
                                        reader.Read();
                                        varauksenNimi = reader.Value;
                                        break;
                                    case "AlkuAika":
                                        reader.Read();
                                        alkuaika = positiot.haePositio(reader.Value);
                                        break;
                                    case "LoppuAika":
                                        reader.Read();
                                        loppuaika = positiot.haePositio(reader.Value);
                                        break;
                                    default:
                                        break;
                                }

                                // Lisää itemi listaan kun tarvittavat tiedot luettu XML:stä
                                if (päivä != null && alkuaika != -1 && loppuaika != -1 && varauksenID != -1 && varauksenNimi != null)
                                {
                                    // Poista tarvittavat itemit
                                    for (int i = alkuaika; i < loppuaika; i++)
                                    {
                                        päivä.Items.RemoveAt(i);
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
                                    // Teksti laatikon yläosaan jos varaus > puolituntia
                                    if (item.Height > 25)
                                        item.VerticalContentAlignment = System.Windows.VerticalAlignment.Top;

                                    // Lisää itemi listaan
                                    päivä.Items.Insert(alkuaika, item);

                                    // "Tyhjennä" muuttujat
                                    päivä = null;
                                    varauksenNimi = null;
                                    alkuaika = -1;
                                    loppuaika = -1;
                                    item = null;

                                    // Poistu sisemmästä while-loopista koska tarvittavat tiedot yhdelle varaukselle on luettu
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            reader.Close();
        }

        // Näytä "Muokkaa varausta"-ikkuna kun ListBox:n itemiä klikataan
        private void ListBoxItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Vanhan varauksen tiedot
            Window varausIkkuna = new LisääVarausWindow((ListBox)(sender as ListBoxItem).Parent, (ListBoxItem)lstTilat.SelectedItem);
            varausIkkuna.ShowDialog();
            // Päivitä viikkonäkymä
            haeVaraukset();
        }

        // Näytä "Lisää varaus"-ikkuna kun "Tee varaus"-nappia klikataan
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
                Window varausIkkuna = new LisääVarausWindow(listalaatikko, (ListBoxItem)lstTilat.SelectedItem);
                varausIkkuna.ShowDialog();
                // Päivitä viikkonäkymä
                haeVaraukset();
            }
        }

        void listBox_SelectionChange(bool poista)
        {
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

        private void lstTilat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            haeVaraukset();
        }

        private void kalenteri_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            näytettäväVkPvm = viikonMaanataiPvm(kalenteri.SelectedDate.Value);
            haeVaraukset();
        }
    }
}