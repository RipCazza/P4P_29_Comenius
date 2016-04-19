using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateChecker
{
    public class MailInhoud
    {
        public const string begin = @"<html>
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
        public const string eind = @"                   </td>
                                                </tr>
                                                <tr class=""part"" style=""border-bottom: 10px #a1a1a1 solid;"">
                                                    <td><em>Dit mailtje is automatisch verstuurd via profielkeuze.csg-comenius.nl</em></td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </body>
                            </html>";

        public enum Mailtjes
        {
            SeptemberMail, ImportHerinnering, DecaanActieMail, DecaanAanvulling
        };

        //private string septembermail;
        //private string importHerinnering;
        //private string decaanActieMail;
        //private string leerlingHerrineringsMail;
        //private string leerlingHerinneringsMail2;
        //private string decaanAanvulling;

        /// <summary>
        /// Geeft de inhoud terug van een standaardmail
        /// </summary>
        /// <param name="welkeMail"></param>
        /// <returns></returns>
        public static string GeefMailInhoud(Mailtjes welkeMail)
        {
            switch (welkeMail)
            {
                case Mailtjes.SeptemberMail:
                    return
                        begin +
                        @"Beste decaan,
                          <br><br>
                          Het is nu September.
                          <br>
                          De gegevens op de profielkeuzewebsite moeten weer worden aangepast. 
                          <br>
                          Denk hierbij aan bijvoorbeld:
                          <br>
                          - Het verwijderen/toevoegen van vakken
                          <br>
                          - Aanpassen van alle voorlichtingen (datum)
                          <br>
                          -Verwijderen van voorlichtingen waarbij het vak niet meer wordt gegegeven
                          <br>
                          - Toevoegen van voorlichtingen van nieuwe vakken.
                          <br>
                          Klik <a href=""carouselkeuze.csg-comenius.nl"">hier</a> om naar de website te gaan.
                          <br>
                          Als de bovenstaande link niet werkt, voer dan profielkeuze.csg-comenius.nl in uw browser in. 
                        "
                        + eind;
                case Mailtjes.ImportHerinnering:
                    return begin +
                         @"Beste admin,
                           <br><br>
                           De nieuwe leerlingen moeten weer geïmporteerd worden op de profielkeuze.
                           <br>
                           Klik <a href=""carouselkeuze.csg-comenius.nl"">hier</a> om naar de website te gaan.
                           <br>
                           Als de bovenstaande link niet werkt, voer dan profielkeuze.csg-comenius.nl in uw browser in. 
                        "
                         + eind;
                case Mailtjes.DecaanActieMail:
                    return begin +
                        @"Beste decaan,
                          <br><br>
                          Er is een herinneringsmail gestuurd naar de leerlingen die zich nog niet hebben ingeschreven.
                          <br>
                          De leerlingen die zich nog niet hebben ingeschreven kunt u bekijken op de website.
                          <br>
                          Klik <a href=""carouselkeuze.csg-comenius.nl"">hier</a> om naar de website te gaan.
                          <br>
                          Als de bovenstaande link niet werkt, voer dan profielkeuze.csg-comenius.nl in uw browser in.
                        "
                        + eind;
                case Mailtjes.DecaanAanvulling:
                    return begin +
                        @"Beste decaan,
                          <br><br>
                          De inschrijvingen zijn nu gesloten.
                          <br>
                          De informatie over beschikbare lokalen, ruimte, tijd en leraren moet weer up-to-date worden gemaakt.
                          <br>
                          Als er andere dingen zijn gewijzigd moet dit ook worden aangepast.
                          <br>
                          Op " + DateTime.Now.AddDays(7) + @" worden de roosters gemaakt. Het is dus belangrijk dat alle gegevens kloppen.
                        "
                        + eind;
                //case Mailtjes.LeerlingHerrineringsMail:
                //    return begin + "inhoud" + eind;
                //case Mailtjes.LeerlingHerinneringsMail2:
                //    return begin + "inhoud" + eind;
                //case Mailtjes.DecaanAanvulling:
                //    return begin + "inhoud" + eind;
                default:
                    return "";
            }
        }
    }
}
