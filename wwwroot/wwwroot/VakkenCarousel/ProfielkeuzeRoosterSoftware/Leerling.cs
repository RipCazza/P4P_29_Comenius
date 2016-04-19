using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfielkeuzeRoosterSoftware
{
    [Serializable]
    public class Leerling
    {
        public int leerlingId = 0;
        public int aantal = 1;
        public List<byte> vakken;
        public List<byte> beschikbaar;
        public List<byte> gebruikteBlokken;
        public string email = "";
        public string oudersemail = "";

        public Leerling(int l, int a)
        {
            leerlingId = l;
            aantal = a;
            // blokken 0-7
            beschikbaar = new List<byte> { };
            gebruikteBlokken = new List<byte> { };
            // lessen 0-5
            vakken = new List<byte> { };
        }
    }
}
