using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlServerCe;

namespace ProfielkeuzeRoosterSoftware
{
    public static class Global
    {
        static double hoogstePunten = -99999999;
        static Klas[][] besteKlassenRooster;
        static int aantalBlokken = 8;
        static int aantalLessen = 8;
        static int aantalDagen = 2;
        //zo lang doet de rooster software er over in minuten
        public static byte tijdBezig = 180;
        public static string connectionString = "Data source = " + System.IO.Directory.GetCurrentDirectory() + @"\Draaimolen v9.2\App_Data\Draaimolen.sdf;Persist Security Info=False";

        public static Klas[][] GlobalKlas
        {
            get
            {
                return besteKlassenRooster;
            }
            set
            {
                besteKlassenRooster = value;
            }
        }

        public static double GlobalPunten
        {
            get
            {
                return hoogstePunten;
            }
            set
            {
                hoogstePunten = value;
            }
        }

        public static int GlobalAB
        {
            get
            {
                return aantalBlokken;
            }
            set
            {
                aantalBlokken = value;
            }
        }

        public static int GlobalAL
        {
            get
            {
                return aantalLessen;
            }
            set
            {
                aantalLessen = value;
            }
        }

        public static int GlobalAD
        {
            get
            {
                return aantalDagen;
            }
            set
            {
                aantalDagen = value;
            }
        }
    }
}
