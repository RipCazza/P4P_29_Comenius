using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DateChecker
{
    class Program
    {
        /*
         * TODO:
         * uncomment startrooster()
         * 
         * */
        //static string connectionString = @"data source= " + System.IO.Directory.GetCurrentDirectory() + @"\Draaimolen v9.2\App_Data\Draaimolen.sdf;Persist Security Info=False";
        static string connectionString = @"server=10.2.0.65;database=Draaimolen;user id=Draaimolen;password=Blaatschaap01;MultipleActiveResultSets=true";
        static void Main(string[] args)
        {
            CheckSeptemberMail();
            CheckDatabase();
            CheckGeplandeEmails();
        }

        
        /// <summary>
        /// Kijkt welke datum de eerste voorlichting is en roept daarna de methode checkvoortaken aan
        /// </summary>
        private static void CheckDatabase()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT datum FROM Voorlichtingen", conn))
                {
                    List<DateTime> voorlichtingsData = new List<DateTime>();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (!(reader.IsDBNull(0)))
                        {
                            string voorlichtingDatum = reader.GetString(0);
                            DateTime geldigeVoorlichtingsDatum;
                            bool validDateTime = DateTime.TryParse(voorlichtingDatum, out geldigeVoorlichtingsDatum);
                            if (validDateTime)
                            {
                                voorlichtingsData.Add(geldigeVoorlichtingsDatum);
                            }
                        }
                    }
                    if (voorlichtingsData.Count > 0)
                    {
                        //voorlichtingsdata gevonden
                        DateTime eersteVoorlichting = voorlichtingsData[0];
                        foreach (DateTime dat in voorlichtingsData)
                        {
                            if (dat < eersteVoorlichting)
                            {
                                eersteVoorlichting = dat;
                            }
                        }
                        //eerste voorlichting gevonden 
                        //check of er iets uitgevoerd moet worden
                        CheckVoorTaken(eersteVoorlichting);
                    }
                    //else
                    //{
                    //    //geen voorlichtingsdata gevonden
                    //}
                }
                conn.Close();
            }
        }

        /// <summary>
        /// Kijkt of er nog taken te doen zijn
        /// </summary>
        /// <param name="eersteVoorlichting">datum van de eerste voorlichting</param>
        private static void CheckVoorTaken(DateTime eersteVoorlichting)
        {
            //check hoeveel dagen het nog duurd tot de voorlichting
            int dagenTotVoorlichting = (int)(eersteVoorlichting - DateTime.Now).TotalDays + 1;
            Mail mail;
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            switch (dagenTotVoorlichting)
            {
                //7 weken van tevoren mail naar admin voor importeren leerlingen
                case 49:
                    SqlCommand getAdmin = new SqlCommand("SELECT email FROM Gebruiker WHERE rol = 'admin'", conn);
                    SqlDataReader readAdmin = getAdmin.ExecuteReader();
                    if (readAdmin.Read() && !readAdmin.IsDBNull(0))
                    {
                        mail = new Mail(MailInhoud.GeefMailInhoud(MailInhoud.Mailtjes.ImportHerinnering));
                        string adminEmail = readAdmin.GetString(0);
                        mail.SendMail(adminEmail, "Importeren leerlingen");
                    }
                    break;
                //5 weken voor de voorlichtingen mail naar decaan met dat ze actie moet ondernemen voor leerlingen
                //5 weken voor de voorlichtingen herinneringsmail naar leerling voor inschrijven ALLEEN NAAR DE LEERLINGEN DIE NIET INGESCHREVEN ZIJN
                case 35:
                    SqlCommand getDecanen = new SqlCommand("SELECT email FROM Gebruiker WHERE rol = 'decaan'", conn);
                    SqlDataReader readDecanen = getDecanen.ExecuteReader();
                    while (readDecanen.Read())
                    {
                        if (!readDecanen.IsDBNull(0))
                        {
                            //voor ieder emailadres van de decanen
                            string email = readDecanen.GetString(0);
                            mail = new Mail(MailInhoud.GeefMailInhoud(MailInhoud.Mailtjes.DecaanActieMail));
                            mail.SendMail(email, "Leerlingen zonder voorlichtingskeuze");
                        }
                    }
                    SqlCommand getNLeerlingen = new SqlCommand("SELECT email FROM gebruiker WHERE rol = 'leerling' AND gebruikerId NOT IN (SELECT GebruikerId FROM Keuze)", conn);
                    SqlDataReader readNLeerlingen = getNLeerlingen.ExecuteReader();
                    while (readNLeerlingen.Read())
                    {
                        if (!readNLeerlingen.IsDBNull(0))
                        {
                            SqlCommand herrineringsinhoud = new SqlCommand("SELECT inhoud, onderwerp FROM Mails WHERE mailId = 1", conn);
                            SqlDataReader readInhoud = herrineringsinhoud.ExecuteReader();
                            if (readInhoud.Read() && !readInhoud.IsDBNull(0) && !readInhoud.IsDBNull(1))
                            {
                                string emailNI = MailInhoud.begin + readInhoud.GetString(0) + MailInhoud.eind;
                                mail = new Mail(emailNI);
                                mail.SendMail(readNLeerlingen.GetString(0), readInhoud.GetString(1));
                            }
                        }
                    }
                    break;
                //16 dagen voor de voorlichtingen tweede herrineringsmail nog niet ingeschreven leerlingen
                case 16:
                    SqlCommand getNLeerlingen2 = new SqlCommand("SELECT email FROM gebruiker WHERE rol = 'leerling' AND gebruikerId NOT IN (SELECT GebruikerId FROM Keuze)", conn);
                    SqlDataReader readNLeerlingen2 = getNLeerlingen2.ExecuteReader();
                    while (readNLeerlingen2.Read())
                    {
                        if (!readNLeerlingen2.IsDBNull(0))
                        {
                            //voor iedere leerling
                            SqlCommand herrineringsinhoud = new SqlCommand("SELECT inhoud, onderwerp FROM Mails WHERE mailId = 2", conn);
                            SqlDataReader readInhoud = herrineringsinhoud.ExecuteReader();
                            if (readInhoud.Read() && !readInhoud.IsDBNull(0) && !readInhoud.IsDBNull(1))
                            {
                                string emailNI2 = MailInhoud.begin + readInhoud.GetString(0) + MailInhoud.eind;
                                mail = new Mail(emailNI2);
                                mail.SendMail(readNLeerlingen2.GetString(0), readInhoud.GetString(1));
                            }
                        }
                    }
                    break;
                //2 weken van tevoren mail voor informatie aanvullen zoals leraren bij les etc. 
                case 14:
                    SqlCommand getDecanen2 = new SqlCommand("SELECT email FROM Gebruiker WHERE rol = 'decaan'", conn);
                    SqlDataReader readDecanen2 = getDecanen2.ExecuteReader();
                    while (readDecanen2.Read())
                    {
                        if (!readDecanen2.IsDBNull(0))
                        {
                            //voor ieder emailadres van de decanen
                            string email = readDecanen2.GetString(0);
                            mail = new Mail(MailInhoud.GeefMailInhoud(MailInhoud.Mailtjes.DecaanAanvulling));
                            mail.SendMail(email, "informatieaanvulling");
                        }
                    }
                    break;
                //1 week voor de voorlichting: START ROOSTERSOFTWARE 
                case 7:
                    StartRooster();
                    break;
            }
            conn.Close();
        }

        /// <summary>
        /// Verzend mailtjes op de ingestelde datum
        /// </summary>
        private static void CheckGeplandeEmails() 
        {
            //check voor mailtjes die vandaag verzonden moeten worden
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //alle mails die nog niet verzonden zijn voor datetime.now
                SqlCommand mailsVoorVandaag = new SqlCommand("SELECT * FROM Mails WHERE Convert(datetime, Convert(int, GetDate())) >= Convert(datetime, Convert(int, verzendDatum)) AND verzonden IS NULL OR verzonden = 'false'", conn);
                using (SqlDataReader reader = mailsVoorVandaag.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //voor elke mail
                        if (!reader.IsDBNull(1))
                        {
                            string inhoud = reader.GetString(1);
                            string onderwerp;
                            if (!reader.IsDBNull(2))
                            {
                                onderwerp = reader.GetString(2);
                            }
                            else
                            {
                                onderwerp = "Geen Onderwerp";
                            }
                            string begin = @"<html>
                                <head></head>
                                <body>
                                    <div class=""container"">
                                        <style>
                                            .part{{
                                                border-bottom: 10px #a1a1a1 solid;
                                            }}
                                        </style>
                                        <table>
                                            <!--Content hier-->
                                            <tbody>
                                                <tr class=""part"" style=""border-bottom: 10px #a1a1a1 solid;"">
                                                    <td>
                                                        <img src=""http://www.csg-comenius.nl/themes/comenius2013/images/logo-comenius-new@2x.png"" >
                                                    </td>
                                                </tr>
                                                <tr class=""part"" style=""border-bottom: 10px #a1a1a1 solid;"">
                                                    <td>";
        //inhoud van de mail hier
                            string eind = @"                   </td>
                                                </tr>
                                                <tr class=""part"" style=""border-bottom: 10px #a1a1a1 solid;"">
                                                    <td><em>Dit mailtje is automatisch verstuurd via profielkeuze.csg-comenius.nl</em></td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </body>
                            </html>";
                            Mail geplandeEmail = new Mail(begin + inhoud + eind);
                            if (reader.IsDBNull(6))
                            {
                                //Maak ondersteuning voor verzenden van mails naar:
                                /*
                                 * Ouders van leerlingen
                                 * */
                                //het mailtje moet naar iedereen met de rol gestuurd worden
                                SqlCommand emailsMetRol = new SqlCommand("SELECT DISTINCT email FROM Gebruiker WHERE rol = @rol", conn);
                                if (reader.GetString(4) == "ouders")
                                {
                                    emailsMetRol = new SqlCommand("SELECT DISTINCT oudersemail FROM Leerling WHERE oudersemail IS NOT NULL", conn);
                                }
                                else
                                {
                                    emailsMetRol.Parameters.Add(new SqlParameter("rol", reader.GetString(4)));
                                }
                                using (SqlDataReader emailreader = emailsMetRol.ExecuteReader())
                                {
                                    while (emailreader.Read())
                                    {
                                        if (!emailreader.IsDBNull(0))
                                        {
                                            string emailadres = emailreader.GetString(0);
                                            geplandeEmail.SendMail(emailadres, onderwerp);
                                        }
                                    }
                                }
                                //de mail is verzonden naar iedereen
                                SqlCommand updateMailVerzonden = new SqlCommand("UPDATE Mails SET verzonden = 'true' WHERE mailId = @mailId", conn);
                                updateMailVerzonden.Parameters.Add(new SqlParameter("mailId", reader.GetInt32(0)));
                                //execute query
                                updateMailVerzonden.ExecuteNonQuery();
                            }
                            else
                            {
                                //het mailtje moet naar het emailadres gestuurd worden 
                                string emailNaar = reader.GetString(6);
                                geplandeEmail.SendMail(emailNaar, onderwerp);
                                //de mail is verzonden naar de persoon
                                SqlCommand updateMailVerzonden = new SqlCommand("UPDATE Mails SET verzonden = 'true' WHERE mailId = @mailId", conn);
                                updateMailVerzonden.Parameters.Add(new SqlParameter("mailId", reader.GetInt32(0)));
                                //execute query
                                updateMailVerzonden.ExecuteNonQuery();
                            }
                        }
                    }
                }
                conn.Close();
            }
        }
        /// <summary>
        /// Start de roostersoftware op
        /// </summary>
        private static void StartRooster()
        {
            //start de roostersoftware
            Process.Start(System.IO.Directory.GetCurrentDirectory() + "\\ProfielkeuzeRoosterSoftware.exe");
            Console.WriteLine("RoosterSoftware gestart");
        }

        /// <summary>
        /// Kijkt of de septembermail al verstuurd is. (staat los van de voorlichtingen
        /// </summary>
        private static void CheckSeptemberMail()
        {
            if ((int)DateTime.Now.Month == 9 && (int)DateTime.Now.Day == 1)
            {
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand getDecanen = new SqlCommand("SELECT email FROM Gebruiker WHERE rol = 'decaan'", conn);
                SqlDataReader readDecanen = getDecanen.ExecuteReader();
                while (readDecanen.Read())
                {
                    if (!readDecanen.IsDBNull(0))
                    {
                        //voor ieder emailadres van de decanen
                        string email = readDecanen.GetString(0);
                        Mail mail = new Mail(MailInhoud.GeefMailInhoud(MailInhoud.Mailtjes.SeptemberMail));
                        mail.SendMail(email, "Instellingen Profielkeuzewebsite");
                    }
                }
                conn.Close();
            }
        }
    }
}
