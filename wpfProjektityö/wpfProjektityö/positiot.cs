using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfProjektityö
{
    public static class positiot
    {
        // Palauttaa itemin indexin ListBox:ssa kysytylle kellon-ajalle
        public static int haePositio(string kellonaika)
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
    }
}
