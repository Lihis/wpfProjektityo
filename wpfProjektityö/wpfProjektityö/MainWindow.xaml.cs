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

            var gridView = new GridView();
            //this.lstAsiakas.View = gridView;
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Id",
                DisplayMemberBinding = new Binding("Id")
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Name",
                DisplayMemberBinding = new Binding("Name")
            });
            //this.lstAsiakas.Items.Add(new { Id = 1, Name = "David" });
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

        // Palauttaa itemin indexin ListBox:ssa kysytylle kellon-ajalle
        int haePositio(string kellonaika)
        {
            if (kellonaika == "9:00")
            {
                return 0;
            }
            else if (kellonaika == "9:30")
            {
                return 1;
            }
            else if (kellonaika == "10:00")
            {
                return 2;
            }
            else if (kellonaika == "10:30")
            {
                return 3;
            }
            else if (kellonaika == "11:00")
            {
                return 4;
            }
            else if (kellonaika == "11:30")
            {
                return 5;
            }
            else if (kellonaika == "12:00")
            {
                return 6;
            }
            else if (kellonaika == "12:30")
            {
                return 7;
            }
            else if (kellonaika == "13:00")
            {
                return 8;
            }
            else if (kellonaika == "13:30")
            {
                return 9;
            }
            else if (kellonaika == "14:00")
            {
                return 10;
            }
            else if (kellonaika == "14:30")
            {
                return 11;
            }
            else if (kellonaika == "15:00")
            {
                return 12;
            }
            else if (kellonaika == "15:30")
            {
                return 13;
            }
            else if (kellonaika == "16:00")
            {
                return 14;
            }
            else if (kellonaika == "16:30")
            {
                return 15;
            }
            else if (kellonaika == "17:00")
            {
                return 16;
            }
            else if (kellonaika == "17:30")
            {
                return 17;
            }
            else if (kellonaika == "18:00")
            {
                return 18;
            }
            else if (kellonaika == "18:30")
            {
                return 19;
            }
            else if (kellonaika == "19:00")
            {
                return 20;
            }
            else if (kellonaika == "19:30")
            {
                return 21;
            }
            else if (kellonaika == "20:00")
            {
                return 22;
            }
            else if (kellonaika == "20:30")
            {
                return 23;
            }
            else if (kellonaika == "21:00")
            {
                return 24;
            }
            else if (kellonaika == "21:30")
            {
                return 25;
            }
            else if (kellonaika == "22:00")
            {
                return 26;
            }

            return -1;
        }

        // Laskee itemin korkeuden halutulle varaus ajalle
        int laskeKorkeus(int alkuaika, int loppuaika)
        {
            int erotus = loppuaika - alkuaika;

            // 25 = puolituntia
            return (erotus * 25);
        }

        // Varaukset varaukset.xml:stä listalaatikkoihin
        void haeVaraukset(DateTime monday)
        {
            // Hae varaukset
            XmlReader reader = XmlReader.Create(@"Resources\varaukset.xml");
            DateTime date = DateTime.MinValue;
            ListBox päivä = null;
            ListViewItem item = null;
            int alkuaika = -1, loppuaika = -1;

            while (reader.Read())
            {
                reader.MoveToContent();

                if (reader.NodeType == XmlNodeType.Element &&
                    reader.Name == "Pvm")
                {
                    reader.Read();

                    date = DateTime.Parse(reader.Value, new CultureInfo("fi-FI"));
                    int dayOfWeek = (int)date.DayOfWeek;

                    // 0 = sunnuntai
                    if (dayOfWeek == 0)
                    {
                        päivä = lstSunnuntai;
                        item = new ListViewItem();
                        // TODO: What to tag?
                        item.Tag = "hidden";
                        // TODO: Text??
                        item.Content = "Sunnuntai";
                        item.IsEnabled = true;
                        item.MouseLeftButtonUp += ListBoxItem_MouseLeftButtonUp;
                    }
                    // 1 = maanantai
                    else if (dayOfWeek == 1)
                    {
                        päivä = lstMaanantai;
                        item = new ListViewItem();
                        // TODO: What to tag?
                        item.Tag = "hidden";
                        // TODO: Text??
                        item.Content = "Maanantai";
                        item.IsEnabled = true;
                        item.MouseLeftButtonUp += ListBoxItem_MouseLeftButtonUp;
                    }
                    // 2 = tiistai
                    else if (dayOfWeek == 2)
                    {
                        päivä = lstMaanantai;
                        item = new ListViewItem();
                        // TODO: What to tag?
                        item.Tag = "hidden";
                        // TODO: Text??
                        item.Content = "Tiistai";
                        item.MouseLeftButtonUp += ListBoxItem_MouseLeftButtonUp;
                    }
                    // 3 = keskiviikko
                    else if (dayOfWeek == 3)
                    {
                        päivä = lstMaanantai;
                        item = new ListViewItem();
                        // TODO: What to tag?
                        item.Tag = "hidden";
                        // TODO: Text??
                        item.Content = "Keskiviikko";
                        item.MouseLeftButtonUp += ListBoxItem_MouseLeftButtonUp;
                    }
                    // 4 = torstai
                    else if (dayOfWeek == 4)
                    {
                        päivä = lstMaanantai;
                        item = new ListViewItem();
                        // TODO: What to tag?
                        item.Tag = "hidden";
                        // TODO: Text??
                        item.Content = "Torstai";
                        item.MouseLeftButtonUp += ListBoxItem_MouseLeftButtonUp;
                    }
                    // 5 perjantai
                    else if (dayOfWeek == 5)
                    {
                        päivä = lstMaanantai;
                        item = new ListViewItem();
                        // TODO: What to tag?
                        item.Tag = "hidden";
                        // TODO: Text??
                        item.Content = "Perjatai";
                        item.MouseLeftButtonUp += ListBoxItem_MouseLeftButtonUp;
                    }
                    // 6 = lauantai
                    else if (dayOfWeek == 6)
                    {
                        päivä = lstMaanantai;
                        item = new ListViewItem();
                        // TODO: What to tag?
                        item.Tag = "hidden";
                        // TODO: Text??
                        item.Content = "Lauantai";
                        item.MouseLeftButtonUp += ListBoxItem_MouseLeftButtonUp;
                    }
                }

                // Varauksen alkuaika
                if (reader.NodeType == XmlNodeType.Element &&
                    reader.Name == "AlkuAika")
                {
                    reader.Read();

                    alkuaika = haePositio(reader.Value);
                }

                // Varauksen loppuaika
                if (reader.NodeType == XmlNodeType.Element &&
                    reader.Name == "LoppuAika")
                {
                    reader.Read();

                    loppuaika = haePositio(reader.Value);
                }

                // Lisää itemi listaan kun tarvittavat tiedot luettu XML:stä
                if (alkuaika != -1 && loppuaika != -1 && item != null)
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

                    // Itemin korkeus
                    item.Height = laskeKorkeus(alkuaika, loppuaika);

                    // Lisää itemi listaan
                    päivä.Items.Insert(alkuaika, item);

                    // "Tyhjennä" muuttujat
                    alkuaika = -1;
                    loppuaika = -1;
                    item = null;
                }
            }
        }

        private void ListBoxItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Santerin ikkuna
            MessageBox.Show("jee!");
        }
    }
}