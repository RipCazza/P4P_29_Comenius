using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlServerCe;

namespace ProfielkeuzeRoosterSoftware
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Instellingen laden...");
            InstellingenInladen();
            Console.WriteLine("Instellingen geladen!");
            Console.WriteLine("Leerlingen laden...");
            List<Leerling> alleLeerlingen = KrijgLeerlingen();
            Console.WriteLine("Leerlingen geladen!");
            Console.WriteLine("Rooster software prepareren...");
            Klas[][] klassenRooster = MaakRooster(Global.GlobalAB, Global.GlobalAL);
            Global.GlobalKlas = MaakRooster(Global.GlobalAB, Global.GlobalAL);
            Task task = Task.Factory.StartNew(() => RoosterMaken(0, 0, alleLeerlingen, klassenRooster));
            Console.WriteLine("Rooster software gestart... (" + Global.tijdBezig.ToString() + "m)");
            task.Wait(Global.tijdBezig * 1000 * 60);
            Console.WriteLine("Rooster gemaakt!");
            Console.WriteLine("Hoogste gevonden score: " + Global.GlobalPunten.ToString());
            VoegRoosterToe(Global.GlobalKlas);
            Console.WriteLine("Rooster toegevoegd in database!");
            Console.ReadLine();
        }

        private static void InstellingenInladen()
        {
            //database connectie
            using (SqlCeConnection conn = new SqlCeConnection(Global.connectionString))
            {
                //verbind met de database
                conn.Open();
                //krijg het totaal aantal lessen
                using (SqlCeCommand cmd = new SqlCeCommand("SELECT COUNT(*) FROM(SELECT DISTINCT Vak, niveau FROM Voorlichtingen) x;", conn))
                {
                    //voer de sql command uit
                    SqlCeDataReader reader = cmd.ExecuteReader();
                    //loop door elke row
                    while (reader.Read())
                    {
                        //als de waarde niet gelijk is aan null
                        if (!(reader.IsDBNull(0)))
                        {
                            Global.GlobalAL = reader.GetInt32(0);
                        }
                    }
                }
                //krijg het totaal aantal blokken
                using (SqlCeCommand cmd = new SqlCeCommand("SELECT COUNT(*) FROM(SELECT DISTINCT rondeId FROM Voorlichtingen) x;", conn))
                {
                    SqlCeDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (!(reader.IsDBNull(0)))
                        {
                            if (!(reader.IsDBNull(0)))
                            {
                                Global.GlobalAB = reader.GetInt32(0);
                            }
                        }
                    }
                }
                //krijg het totaal aantal dagen
                using (SqlCeCommand cmd = new SqlCeCommand("SELECT COUNT(*) FROM(SELECT DISTINCT datum FROM Voorlichtingen) x;", conn))
                {
                    SqlCeDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (!(reader.IsDBNull(0)))
                        {
                            if (!(reader.IsDBNull(0)))
                            {
                                Global.GlobalAD = reader.GetInt32(0);
                            }
                        }
                    }
                }
            }
        }

        private static void VoegRoosterToe(Klas[][] rooster)
        {
            List<LeerlingDatabase> leerlingen = new List<LeerlingDatabase>();
            //loop door alle blokken
            for (int i = 0; i < Global.GlobalAB; i++)
            {
                //loop door alle lessen
                for (int j = 0; j < Global.GlobalAL; j++)
                {
                    //loop door alle leerlingen van dat blok/les
                    foreach (Leerling l in rooster[i][j].leerlingen)
                    {
                        bool gelukt = false;
                        //loop door alle leerlingen heen
                        foreach (LeerlingDatabase l1 in leerlingen)
                        {
                            //als ze matchen geef die leerling een ronde id erbij van bijbehorende blok/les 
                            if (l.leerlingId == l1.leerlingId)
                            {
                                l1.lesids += "," + rooster[i][j].lesId.ToString();
                                gelukt = true;
                                break;
                            }
                        }
                        if (!gelukt)
                        {
                            LeerlingDatabase leerling = new LeerlingDatabase();
                            leerling.leerlingId = l.leerlingId;
                            leerling.lesids += rooster[i][j].lesId.ToString();
                            leerlingen.Add(leerling);
                        }
                    }
                }
            }

            using (SqlCeConnection conn = new SqlCeConnection(Global.connectionString))
            {
                conn.Open();
                //maak de rooster database leeg
                using (SqlCeCommand cmd = new SqlCeCommand("DELETE FROM Carousel", conn))
                {
                    cmd.ExecuteNonQuery();
                }
                //voor elke leerling
                foreach (LeerlingDatabase leerling in leerlingen)
                {
                    //voeg de leerling toe aan de rooster database
                    using (SqlCeCommand cmd = new SqlCeCommand("INSERT INTO Carousel (ronde1, gebruikerID) Values (@lesids, @leerlingid)", conn))
                    {
                        cmd.Parameters.AddWithValue("@lesids", leerling.lesids);
                        cmd.Parameters.AddWithValue("@leerlingid", leerling.leerlingId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private static List<Leerling> KrijgLeerlingen()
        {
            List<Leerling> leerlingen = new List<Leerling>();
            using (SqlCeConnection conn = new SqlCeConnection(Global.connectionString))
            {
                conn.Open();
                //krijg de keuzes van de leerlingen
                using (SqlCeCommand cmd = new SqlCeCommand("SELECT Keuze.GebruikerID, Keuze.keuze1, Keuze.aantalpers, Leerling.blokken, Leerling.oudersemail, Gebruiker.email FROM Keuze, Leerling, Gebruiker WHERE Leerling.GebruikerId = Gebruiker.GebruikerId", conn))
                {
                    SqlCeDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        //checkt of er geen lege data is
                        if (!(reader.IsDBNull(0) || reader.IsDBNull(1) || reader.IsDBNull(2) || reader.IsDBNull(3) || reader.IsDBNull(4) || reader.IsDBNull(5)))
                        {
                            int leerlingId = reader.GetInt32(0);
                            int aantal = reader.GetInt32(2);
                            Leerling leerling = new Leerling(leerlingId, aantal);
                            string lessen = reader.GetString(1);
                            foreach (string les in lessen.Split(','))
                            {
                                leerling.vakken.Add(Convert.ToByte(les));
                            }
                            string blokken = reader.GetString(3);
                            foreach (string blok in blokken.Split(','))
                            {
                                leerling.beschikbaar.Add(Convert.ToByte(blok));
                            }
                            leerling.email = reader.GetString(4);
                            leerling.oudersemail = reader.GetString(5);
                            leerlingen.Add(leerling);
                        }
                    }
                }
            }

            //zet de leerlingen in klassen waar de lessen en blokken matchen
            List<KlasSorteren> klassen = new List<KlasSorteren>();

            for (int i = 0; i < leerlingen.Count(); i++)
            {
                bool addClass = true;
                foreach (KlasSorteren klas in klassen)
                {
                    if (klas.leerlingen.Count() == 0 | klas.blokken.Count() == 0 | klas.lessen.Count() == 0)
                    {
                        klas.leerlingen.Add(leerlingen[i]);
                        addClass = false;
                    }
                    //als de vakken en blokken matchen
                    else if (leerlingen[i].beschikbaar.SequenceEqual(klas.blokken) && leerlingen[i].vakken.SequenceEqual(klas.lessen))
                    {
                        klas.leerlingen.Add(leerlingen[i]);
                        addClass = false;
                    }
                    if (!addClass)
                    {
                        break;
                    }
                }
                //als er geen klas voor een leerling gevonden is
                if (addClass)
                {
                    //voeg een lege klas toe
                    klassen.Add(new KlasSorteren());
                    i--;
                }
            }

            //zet de klassen weer om in een leerlinglijst
            leerlingen = new List<Leerling>();

            foreach (KlasSorteren klas in klassen)
            {
                foreach (Leerling leerling in klas.leerlingen)
                {
                    leerlingen.Add(leerling);
                }
            }

            return leerlingen;
        }

        //maak een leeg rooster
        private static Klas[][] MaakRooster(int aantalBlokken, int aantalLessen)
        {
            Klas[][] klassen = new Klas[aantalBlokken][];
            for (byte i = 0; i < aantalBlokken; i++)
            {
                klassen[i] = new Klas[aantalLessen];
            }
            using (SqlCeConnection conn = new SqlCeConnection(Global.connectionString))
            {
                conn.Open();
                //krijg alle informatie van elk blok/les
                using (SqlCeCommand cmd = new SqlCeCommand("SELECT Voorlichtingen.Vakid, Bloktijden.blokId, Vakken.Vakid, Lokalen.maxPersonen FROM Bloktijden, Lokalen, Vakken, Voorlichtingen WHERE Vakken.Vaknaam = Voorlichtingen.Vak AND Vakken.Niveau = Voorlichtingen.Niveau AND Bloktijden.tijd = Voorlichtingen.rondeId AND Voorlichtingen.Lokaal = Lokalen.lokaal;", conn))
                {
                    SqlCeDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (!(reader.IsDBNull(0) || reader.IsDBNull(1) || reader.IsDBNull(2) || reader.IsDBNull(3)))
                        {
                            int ruimte = reader.GetInt32(3);
                            int blok = reader.GetInt32(1);
                            int les = reader.GetInt32(2);
                            int lesid = reader.GetInt32(0);
                            klassen[blok][les] = new Klas(ruimte, blok, les, lesid);
                        }
                    }
                }
            }
            return klassen;
        }

        //backtracking rooster algoritme
        private static void RoosterMaken(int leerlingNummer, int lesNummer, List<Leerling> alleLeerlingen, Klas[][] klassenRooster)
        {
            //als alle leerlingen ingevoerd zijn
            if (leerlingNummer == alleLeerlingen.Count())
            {
                Rekenen reken = new Rekenen(klassenRooster, Global.GlobalAB, Global.GlobalAL);
                double punten = 0;
                punten -= reken.BerekenTussenuurStrafpunten(Global.GlobalAD, Global.GlobalAB, alleLeerlingen);
                punten -= reken.BerekenRuimteStrafpunten();
                if (punten > Global.GlobalPunten)
                {
                    Global.GlobalPunten = punten;
                    Global.GlobalKlas = klassenRooster.DeepClone();
                }
            }
            //als alles lessen ingevoerd zijn
            else if (alleLeerlingen[leerlingNummer].vakken.Count() == lesNummer)
            {
                RoosterMaken((leerlingNummer + 1), 0, alleLeerlingen, klassenRooster);
            }
            else
            {
                byte les = alleLeerlingen[leerlingNummer].vakken[lesNummer];
                //loop door alle blokken van leerling
                for (int i = 0; i < alleLeerlingen[leerlingNummer].beschikbaar.Count(); i++)
                {
                    byte blok = alleLeerlingen[leerlingNummer].beschikbaar[i];
                    //voeg leerling toe aan blok/les
                    if (klassenRooster[blok][les].VoegLeerlingToe(alleLeerlingen[leerlingNummer]))
                    {
                        //verwijder beschikbare blok
                        alleLeerlingen[leerlingNummer].beschikbaar.Remove(blok);
                        alleLeerlingen[leerlingNummer].gebruikteBlokken.Add(blok);

                        //doe alles opniuew met volgende les of volgende leerling
                        RoosterMaken(leerlingNummer, (byte)(lesNummer + 1), alleLeerlingen, klassenRooster);

                        //maak de veranderingen ongedaan
                        alleLeerlingen[leerlingNummer].beschikbaar.Insert(i, blok);
                        alleLeerlingen[leerlingNummer].gebruikteBlokken.Remove(blok);
                        klassenRooster[blok][les].VerwijderLeerling(alleLeerlingen[leerlingNummer]);
                    }
                }
            }
        }
    }
}