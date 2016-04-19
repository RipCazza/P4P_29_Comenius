using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfielkeuzeRoosterSoftware
{
    [Serializable]
    public class Klas
    {
        public int ruimte;
        public int minimum = 5;
        public int bezet = 0;
        public List<Leerling> leerlingen = new List<Leerling>();
        public int blok;
        public int les;
        public int lesId;

        public Klas(int r, int b, int l, int li)
        {
            ruimte = r;
            minimum = r / 3;
            blok = b;
            les = l;
            lesId = li;
        }

        public bool VoegLeerlingToe(Leerling leerling)
        {
            if (bezet + leerling.aantal > ruimte)
            {
                return false;
            }
            else
            {
                bezet += leerling.aantal;
                leerlingen.Add(leerling);
                return true;
            }
        }

        public void VerwijderLeerling(Leerling leerling)
        {
            leerlingen.Remove(leerling);
            bezet -= leerling.aantal;
        }
    }
}
