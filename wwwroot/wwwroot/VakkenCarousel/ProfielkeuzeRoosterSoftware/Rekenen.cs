using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfielkeuzeRoosterSoftware
{
    public class Rekenen
    {
        Klas[][] rooster;
        int aantalBlokken;
        int aantalLessen;

        public Rekenen(Klas[][] r, int ab, int al)
        {
            rooster = r;
            aantalBlokken = ab;
            aantalLessen = al;
        }

        public double BerekenRuimteStrafpunten()
        {
            double strafpunten = 0;
            for (int i = 0; i < aantalBlokken; i++)
            {
                for (int j = 0; j < aantalLessen; j++)
                {
                    int p = rooster[i][j].bezet / rooster[i][j].ruimte * 100;
                    if (p == 0)
                    {
                        continue;
                    }
                    else
                    {
                        strafpunten += ((0.0191 * Math.Pow(p, 2)) - (4.0046 * p) + 209.71);
                    }
                }
            }
            return strafpunten;
        }

        public double BerekenTussenuurStrafpunten(int aantalDagen, int aantalBlokken, List<Leerling> leerlingen)
        {
            double strafpunten = 0;
            int blokkenDag = aantalBlokken / aantalDagen;

            foreach (Leerling leerling in leerlingen)
            {
                for (byte i = 1; i <= (leerling.beschikbaar.Count() / blokkenDag); i++)
                {
                    byte min = (byte)((blokkenDag * i) - blokkenDag);
                    byte max = (byte)(blokkenDag * i);
                    int t = 0;
                    int ch = -1;
                    foreach (byte blok in leerling.gebruikteBlokken)
                    {
                        if (blok < min)
                        {
                            continue;
                        }
                        if (blok < max)
                        {
                            int c = blok % blokkenDag;
                            t += (c - ch - 1);
                            ch = c;
                        }
                        else
                        {
                            strafpunten += (t * 15 * leerling.aantal);
                            break;
                        }
                    }
                }
            }

            return strafpunten;
        }
    }
}
