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
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;

namespace wpfProjektityö
{
    /// <summary>
    /// Interaction logic for LisääVarausWindow.xaml
    /// </summary>
    public partial class LisääVarausWindow : Window
    {
        string itemTag = null, varausPvm = null, valittuSaliID = null, asiakasID = null;
        List<int> valitutItemit = new List<int>();

        public LisääVarausWindow(ListBox list, ListBoxItem valittuSali)
        {
            InitializeComponent();

            // Ota päivämäärä ListBox:n tägistä
            varausPvm = list.Tag.ToString();

            // SaliID
            valittuSaliID = valittuSali.Tag.ToString();

            // Looppaa kaikkien valittujen itemien läpi
            foreach (ListBoxItem valittu in list.SelectedItems)
            {
                // Hae Tag, tägissä on varausID jos on klikattu jo olemassa olevaa varausta
                if (valittu.Tag != null)
                    itemTag = valittu.Tag.ToString();

                // Lisää itemi väliaikaiseen listaan
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
            // Jos itemTag ei ole null, halutaan hakea jo olemassa olevan varauksen tiedot ...
            if (itemTag != null)
            {
                // Muuta ikkunan teksti ja ..
                this.Title = "Muokkaa varausta";
                // ..muuta napin teksti
                btnTeeVaraus.Content = "Tallenna";
                // Hae varauksen tiedot
                haeVarauksenTiedot(itemTag);
                // Hae asiakkaan tiedot (asiakasID, ikkuna)
                XMLfunktiot.haeAsiakkaanTiedot(asiakasID, this);
            }
            // ..muussa tapauksessa ollaan tekemässä uutta varausta
            else
            {
                cmbAlkuAika.SelectedIndex = valitutItemit.First();
                cmbLoppuAika.SelectedIndex = valitutItemit.Last();
                txtPvm.Text = varausPvm;
            }
        }

        // Hae varauksen ja asiakkaan tiedot "Lisää varaus"-ikkunaan
        void haeVarauksenTiedot(string varausID)
        {
            XmlReader reader = XmlReader.Create(@"Resources\varaukset.xml");

            // Hae varauksen tiedot
            while (reader.Read())
            {
                reader.MoveToContent();

                if (reader.NodeType == XmlNodeType.Element &&
                    reader.Name == "VarausID")
                {
                    // Lue kunnes ollaan halutun asiakkaan VarausID:n kohdalla
                    reader.Read();
                    if (reader.Value == varausID)
                    {
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                switch (reader.Name)
                                {
                                    // Päivämäärä
                                    case "Pvm":
                                        reader.Read();
                                        txtPvm.Text = reader.Value;
                                        break;
                                    // Päivämäärä
                                    case "AsiakasID":
                                        reader.Read();
                                        asiakasID = reader.Value;
                                        break;
                                    // Varauksen nimi ("otsikko")
                                    case "Nimi":
                                        reader.Read();
                                        txtVarauksenNimi.Text = reader.Value;
                                        break;
                                    // Varauksen alkuaika
                                    case "AlkuAika":
                                        reader.Read();
                                        cmbAlkuAika.SelectedIndex = positiot.haePositio(reader.Value);
                                        break;
                                    // Varauksen loppuaika
                                    case "LoppuAika":
                                        reader.Read();
                                        cmbLoppuAika.SelectedIndex = positiot.haePositio(reader.Value) - 1;
                                        break;
                                }
                            }
                            // Lopeta lukeminen kun saavutaan </varaus> lopetus tagiin
                            else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Varaus")
                            {
                                break;
                            }
                        }
                        // Varauksen tiedot haettu -> lopeta lukeminen
                        break;
                    }
                }
            }
            reader.Close();
        }

        // Hae asiakkaan tiedot asiakkaan nimellä
        void haeAsiakkaanTiedotNimellä(string asiakkaanNimi)
        {
            // Hae asiakkaan tiedot
            XmlReader reader = XmlReader.Create(@"Resources\XMLasiakas.xml");

            while (reader.Read())
            {
                reader.MoveToContent();

                if (reader.NodeType == XmlNodeType.Element &&
                    reader.Name == "AID")
                {
                    reader.Read();
                    asiakasID = reader.Value;
                }

                if (reader.NodeType == XmlNodeType.Element &&
                    reader.Name == "Nimi")
                {
                    // Lue kunnes ollaan halutun asiakkaan nimen kohdalla
                    reader.Read();
                    if (reader.Value == asiakkaanNimi)
                    {
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                switch (reader.Name)
                                {
                                    case "Osoite":
                                        reader.Read();
                                        txtPostiosoite.Text = reader.Value;
                                        break;
                                    case "PostNum":
                                        reader.Read();
                                        txtPostinumero.Text = reader.Value;
                                        break;
                                    case "Postitoimipaik":
                                        reader.Read();
                                        txtPostitoimipaikka.Text = reader.Value;
                                        break;
                                    case "Email":
                                        reader.Read();
                                        txtEmail.Text = reader.Value;
                                        break;
                                    case "Puh":
                                        reader.Read();
                                        txtPuhNro.Text = reader.Value;
                                        break;
                                    case "tyyppi":
                                        reader.Read();
                                        if (reader.Value == "Yksityinen")
                                        {
                                            cmbTyyppi.SelectedIndex = 0;
                                        }
                                        else
                                        {
                                            cmbTyyppi.SelectedIndex = 1;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }

                            // Lopeta lukeminen kun saavutaan </asiakas> lopetus tagiin
                            if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "asiakas")
                            {
                                break;
                            }
                        }
                        // Asiakkaan tiedot haettu -> lopeta lukeminen
                        break;
                    }
                }
            }
            reader.Close();
        }

        // Lisää varaus XML:ään
        void lisääVaraus()
        {
            string varausID = null, saliID = valittuSaliID, pvm = txtPvm.Text, nimi = null, alkuaika = cmbAlkuAika.Text, loppuaika = cmbLoppuAika.Text;
            int tmpVarausID = -1;

            // Tarkista että varauksella on nimi
            if (string.IsNullOrWhiteSpace(txtVarauksenNimi.Text) || txtVarauksenNimi.Text.Length < 2)
            {
                MessageBox.Show("Varauksen nimen pitää olla vähintään kolme merkkiä pitkä.");
                return;
            }
            nimi = txtVarauksenNimi.Text;

            // Tarkista että varauksella on asiakas
            if (asiakasID == null || string.IsNullOrWhiteSpace(asiakasID))
            {
                MessageBox.Show("Lisää varaukselle asiakas!");
                return;
            }

            // Uuden varauksen varausID:n hakeminen/laskeminen
            XmlReader reader = XmlReader.Create(@"Resources\varaukset.xml");
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "VarausID":
                            reader.Read();
                            int.TryParse(reader.Value, out tmpVarausID);
                            break;
                        default:
                            break;
                    }
                }
            }
            reader.Close();

            if (tmpVarausID < 0)
            {
                MessageBox.Show("VarausID hakeminen epäonnistui.");
                return;
            }
            // Lisää yksi varausID:n koska ollaan tekemässä uutta varausta
            varausID = (tmpVarausID + 1).ToString();

            //// XML kirjoitus
            // XML lataus
            XmlDocument doc = new XmlDocument();
            doc.Load(@"Resources\varaukset.xml");
            XmlNode rootElement = doc.DocumentElement;

            // Uuden varaus elementin ja ali-elementtien luominen
            XmlElement varausElement = doc.CreateElement("Varaus");
            XmlElement elmVarausID = doc.CreateElement("VarausID");
            XmlElement elmSaliID = doc.CreateElement("SaliID");
            XmlElement elmAsiakasID = doc.CreateElement("AsiakasID");
            XmlElement elmPvm = doc.CreateElement("Pvm");
            XmlElement elmNimi = doc.CreateElement("Nimi");
            XmlElement elmAlkuAika = doc.CreateElement("AlkuAika");
            XmlElement elmLoppuAika = doc.CreateElement("LoppuAika");

            // Elementtien tekstit
            elmVarausID.InnerText = varausID;
            elmSaliID.InnerText = saliID;
            elmAsiakasID.InnerText = asiakasID;
            elmPvm.InnerText = pvm;
            elmNimi.InnerText = nimi;
            elmAlkuAika.InnerText = alkuaika;
            elmLoppuAika.InnerText = loppuaika;

            // Ali-elementit Varaus elementtiin
            varausElement.AppendChild(elmVarausID);
            varausElement.AppendChild(elmSaliID);
            varausElement.AppendChild(elmAsiakasID);
            varausElement.AppendChild(elmPvm);
            varausElement.AppendChild(elmNimi);
            varausElement.AppendChild(elmAlkuAika);
            varausElement.AppendChild(elmLoppuAika);

            // Lisää varaus element root elementtiin
            rootElement.AppendChild(varausElement);

            // XML Writerin avaaminen
            FileStream fileStream = new FileStream(@"Resources\varaukset.xml", FileMode.Create);
            StreamWriter streamWriter = new StreamWriter(fileStream);
            XmlTextWriter textWriter = new XmlTextWriter(streamWriter);
            textWriter.Formatting = Formatting.Indented;

            // XML tallennus
            doc.Save(textWriter);

            // XML writer sulkeminen
            textWriter.Close();
            streamWriter.Close();
            fileStream.Close();

            MessageBox.Show("Varaus lisätty.");
            this.Close();
        }

        void lisääAsiakas()
        {
            string asiakasID = null, nimi = null, osoite = null, postNum = null, postitoimipaik = null,
                email = null, puhNro = null, tyyppi = null;
            int tmpAsiakasID = -1;
            // Uuden asiakkaan AID:n hakeminen/laskeminen
            XmlReader reader = XmlReader.Create(@"Resources\XMLasiakas.xml");
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "AID":
                            reader.Read();
                            int.TryParse(reader.Value, out tmpAsiakasID);
                            break;
                        default:
                            break;
                    }
                }
            }
            reader.Close();
           
            if (tmpAsiakasID < 0)
            {
                MessageBox.Show("AsikasID hakeminen epäonnistui.");
                return;
            }
            // Lisää yksi asikasID:n koska ollaan lisäämässä uutta asiakasta
            asiakasID = (tmpAsiakasID + 1).ToString();

            nimi = txtVaraajanNimi.Text;
            osoite = txtPostiosoite.Text;
            postNum = txtPostinumero.Text;
            postitoimipaik = txtPostitoimipaikka.Text;
            email = txtEmail.Text;
            puhNro = txtPuhNro.Text;
            if (cmbTyyppi.SelectedIndex != -1)
            {
                tyyppi = ((ComboBoxItem)cmbTyyppi.SelectedItem).Content.ToString();
            }
            else
            {
                MessageBox.Show("Valitse asiakkaan tyyppi.");
                return;
            }

            //// XML kirjoitus
            // XML lataus
            XmlDocument doc = new XmlDocument();
            doc.Load(@"Resources\XMLasiakas.xml");
            XmlNode rootElement = doc.DocumentElement;

            // Uuden varaus elementin ja ali-elementtien luominen
            XmlElement asiakasElement = doc.CreateElement("asiakas");
            XmlElement elmAsiakasID = doc.CreateElement("AID");
            XmlElement elmNimi = doc.CreateElement("Nimi");
            XmlElement elmOsoite = doc.CreateElement("Osoite");
            XmlElement elmPostNro = doc.CreateElement("PostNum");
            XmlElement elmPostitoimipaik = doc.CreateElement("Postitoimipaik");
            XmlElement elmEmail = doc.CreateElement("Email");
            XmlElement elmPuh = doc.CreateElement("Puh");
            XmlElement elmTyyppi = doc.CreateElement("tyyppi");

            // Elementtien tekstit
            elmAsiakasID.InnerText = asiakasID;
            elmNimi.InnerText = nimi;
            elmOsoite.InnerText = osoite;
            elmPostNro.InnerText = postNum;
            elmPostitoimipaik.InnerText = postitoimipaik;
            elmEmail.InnerText = email;
            elmPuh.InnerText = puhNro;
            elmTyyppi.InnerText = tyyppi;

            // Ali-elementit Varaus elementtiin
            asiakasElement.AppendChild(elmAsiakasID);
            asiakasElement.AppendChild(elmNimi);
            asiakasElement.AppendChild(elmOsoite);
            asiakasElement.AppendChild(elmPostNro);
            asiakasElement.AppendChild(elmPostitoimipaik);
            asiakasElement.AppendChild(elmEmail);
            asiakasElement.AppendChild(elmPuh);
            asiakasElement.AppendChild(elmTyyppi);

            // Lisää varaus element root elementtiin
            rootElement.AppendChild(asiakasElement);

            // XML Writerin avaaminen
            FileStream fileStream = new FileStream(@"Resources\XMLasiakas.xml", FileMode.Create);
            StreamWriter streamWriter = new StreamWriter(fileStream);
            XmlTextWriter textWriter = new XmlTextWriter(streamWriter);
            textWriter.Formatting = Formatting.Indented;

            // XML tallennus
            doc.Save(textWriter);

            // XML writer sulkeminen
            textWriter.Close();
            streamWriter.Close();
            fileStream.Close();

            MessageBox.Show("Asiakas lisätty.");
        }

        void haeAsiakasTietokannasta()
        {
            // Tarkista että on annettu nimi jota hakea (min. kolme merkkiä)
            if (string.IsNullOrWhiteSpace(txtHaeVaraajanNimellä.Text) || txtHaeVaraajanNimellä.Text.Length < 2)
            {
                MessageBox.Show("Anna vähintään kolme merkkiä!");
                return;
            }

            XmlReader reader = XmlReader.Create(@"Resources\XMLAsiakas.xml");
            List<string> asiakkaanNimi = new List<string>();

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "Nimi":
                            reader.Read();
                            if (Regex.IsMatch(reader.Value, txtHaeVaraajanNimellä.Text, RegexOptions.IgnoreCase))
                            {
                                asiakkaanNimi.Add(reader.Value);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            reader.Close();

            // Asiakkaita ei löytynyt
            if (asiakkaanNimi.Count == 0)
            {
                MessageBox.Show("Asiakkaita ei löytynyt.");
            }
            // Asiakkaita löytyi enemmän kuin yksi, näytä MessageBox jossa löydetyt asiakkaat
            else if (asiakkaanNimi.Count > 1)
            {
                string message = "Rajaa hakua, asiakkaita löytyi: " + asiakkaanNimi.Count.ToString() + " kpl.";

                for (int i = 0; i < asiakkaanNimi.Count; i++)
                {
                    message += "\n" + asiakkaanNimi[i];
                }
                MessageBox.Show(message);
            }
            // Asiakkaita löytyi vain yksi joten hae tiedot
            else
            {
                // Asiakkaan tiedot laatikoihin
                haeAsiakkaanTiedotNimellä(asiakkaanNimi[0]);
                txtVaraajanNimi.Text = asiakkaanNimi[0];
            }
        }

        // Varauksen lisääminen varaus.xml
        private void btnTeeVaraus_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Varauksen muokkaamisen implementointi?
            if (itemTag != null)
            {
                MessageBox.Show("Varauksen muokkausta ei ole implementoitu.");
                return;
            }

            // Lisää varaus XML:ään
            lisääVaraus();
        }

        // Asiakkaan lisääminen XMLasiakas.xml:lään
        private void btnLisääAsiakas_Click(object sender, RoutedEventArgs e)
        {
            // Asiakkaan lisääminen
            lisääAsiakas();
        }

        // "Hae tietokannasta" nappin Click-event
        private void btnHaeAsiaksTietokannasta_Click(object sender, RoutedEventArgs e)
        {
            haeAsiakasTietokannasta();
        }

        // "Hae tietokannasta" tekstikentän KeyDown-event
        private void txtHaeVaraajanNimellä_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                haeAsiakasTietokannasta();
        }
    }
}
