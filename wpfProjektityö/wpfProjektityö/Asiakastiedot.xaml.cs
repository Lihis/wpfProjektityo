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
using System.Xml;

namespace wpfProjektityö
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            Asiakastiedot();
        }
        public void Asiakastiedot()
        {
            XmlReader reader = XmlReader.Create(@"Resources\XMLasiakas.xml");

            while (reader.Read())
            {
                reader.MoveToContent();

                if (reader.NodeType == XmlNodeType.Element &&
                    reader.Name == "Asiakastiedot")
                {
                    // Lue kunnes ollaan halutun asiakkaan VarausID:n kohdalla
                    reader.Read();
                    if (reader.Value == "AID")
                    {
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                switch (reader.Name)
                                {
                                    //varaajan nimi
                                    case "Nimi":
                                        reader.Read();
                                        txtVaraajannimi.Text = reader.Value;
                                        break;
                                    // postinumero
                                    case "PostNum":
                                        reader.Read();
                                        txtPostinumero.Text = reader.Value;
                                        break;
                                    // postitoimipaikka
                                    case "Postitoimipaikka":
                                        reader.Read();
                                        txtPostitoimipaikka.Text = reader.Value;
                                        break;
                                    case "Osoite":
                                        reader.Read();
                                        txtPostiosoite.Text = reader.Value;
                                        break;
                                    case "Puh":
                                        reader.Read();
                                        txtPuh.Text = reader.Value;
                                        break;
                                    case "Email":
                                        reader.Read();
                                        txtEmail.Text = reader.Value;
                                        break;
                                    case "tyyppi":
                                        reader.Read();
                                        txtTyyppi.Text = reader.Value;
                                        break;
                                }
                            }
                        }  // Lopeta lukeminen kun saavutaan </asiakas> lopetus tagiin
                        if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "asiakas")
                        {
                            break;
                        }
                    }
                    // Asiakkaan tiedot haettu -> lopeta lukeminen
                    break;
                }

            }
            return;
        }
    }
}


