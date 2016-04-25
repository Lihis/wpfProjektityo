using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace wpfProjektityö
{
    public class XMLfunktiot
    {
        // Hae asiakkaan tiedot
        public static void haeAsiakkaanTiedot(string asiakasID, Window ikkuna)
        {
            XmlReader reader = XmlReader.Create(@"Resources\XMLasiakas.xml");

            while (reader.Read())
            {
                reader.MoveToContent();

                if (reader.NodeType == XmlNodeType.Element &&
                    reader.Name == "AID")
                {
                    // Lue kunnes ollaan halutun asiakkaan AID kohdalla
                    reader.Read();
                    if (reader.Value == asiakasID)
                    {
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                switch (reader.Name)
                                {
                                    case "Nimi":
                                        reader.Read();
                                        (ikkuna.FindName("txtVaraajanNimi") as TextBox).Text = reader.Value;
                                        break;
                                    case "Osoite":
                                        reader.Read();
                                        (ikkuna.FindName("txtPostiosoite") as TextBox).Text = reader.Value;
                                        break;
                                    case "PostNum":
                                        reader.Read();
                                        (ikkuna.FindName("txtPostinumero") as TextBox).Text = reader.Value;
                                        break;
                                    case "Postitoimipaik":
                                        reader.Read();
                                        (ikkuna.FindName("txtPostitoimipaikka") as TextBox).Text = reader.Value;
                                        break;
                                    case "Email":
                                        reader.Read();
                                        (ikkuna.FindName("txtEmail") as TextBox).Text = reader.Value;
                                        break;
                                    case "Puh":
                                        reader.Read();
                                        (ikkuna.FindName("txtPuhNro") as TextBox).Text = reader.Value;
                                        break;
                                    case "tyyppi":
                                        reader.Read();
                                        if (reader.Value == "Yksityinen")
                                        {
                                            (ikkuna.FindName("cmbTyyppi") as ComboBox).SelectedIndex = 0;
                                        }
                                        else
                                        {
                                            (ikkuna.FindName("cmbTyyppi") as ComboBox).SelectedIndex = 1;
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
    }
}
