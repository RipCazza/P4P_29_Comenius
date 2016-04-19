using System;
using System.Collections.Generic;
using System.Web;

public class decaan
{
    public static bool ValidDatum(string datum, out string errormsg)
    {
        //controleert of er iets is ingevuld
        if(!string.IsNullOrEmpty(datum))
        {
            //controleert datum met slashes is opgedeeld
            if(datum.Contains("/") && datum.Length <= 10)
            {
                int n;
                int m;
                int s;
                string[] datum1 = datum.Split('/');
                //controleert of de dag en maand uit 2 getallen bestaan
                if(datum1[0].Length == 2 && datum1[1].Length == 2 && datum1[2].Length == 4 && Int32.TryParse(datum1[0], out n) && Int32.TryParse(datum1[1], out m) && Int32.TryParse(datum1[2], out s))
                {
                    if(Convert.ToInt32(datum1[0]) > 0 && Convert.ToInt32(datum1[0]) <= 31 && Convert.ToInt32(datum1[1]) > 0 && Convert.ToInt32(datum1[1]) <= 12)
                    {
                        errormsg = "";
                        return true;
                    }
                    else
                    {
                        errormsg = "Voer een geldige dag, maand en jaar in, gescheiden door een /";
                        return false;
                    }
                }
                else
                {
                    errormsg = "Gebruik format dd/mm/yyyy";
                    return false;
                }
            }
            else
            {
                errormsg = "Gebruik format dd/mm/yyyy";
                return false;
            }
        }
        else
        {
            errormsg = "Voer een datum in";
            return false;
        }
    }

    public static bool ValidLokaal(string lokaal, out string errormsg)
    {
        int n;
        //controleert of er een getal is ingevuld
        if(lokaal.Length == 3 && Int32.TryParse(lokaal, out n))
        {
            //controleert of er een positief getal is ingevuld
            if(n > 0)
            {
                errormsg = "";
                return true;
            }
            else
            {
                errormsg = "Voer een geldig lokaal in, bestaand uit 3 getallen";
                return false;
            }
        }
        else
        {
            errormsg = "Voer een geldig lokaal in, bestaand uit 3 getallen";
            return false;
        }
    }

    public static bool ValidVak(string vak, out string errormsg)
    {
        //controleert of er iets is ingevuld en dat het niet teveel karakters bevat
        if(vak.Length > 0 && vak.Length <= 50)
        {
            errormsg = "";
            return true;
        }
        else
        {
            errormsg = "Voer een vak in";
            return false;
        }
    }

    public static bool ValidRuimte(string maxPersonen, out string errormsg)
    {
        int n;
        //controleert of er een geldig getal is ingevuld
        if(Int32.TryParse(maxPersonen, out n))
        {
            if(n >= 0)
            {
                errormsg = "";
                return true;
            }
            else
            {
                errormsg = "voer een geldig getal in";
                return false;
            }            
        }
        else
        {
            errormsg = "voer een geldig getal in";
            return false;
        }
    }
    public static bool ValidEmail(string email, out string errormsg)
    {
        //controleert of de ingevulde waarde de layout heeft van een email adres
        if(email.Contains("@") && email[0] !=  '@' && (email.Substring(email.Length - 3).Contains(".nl") || email.Substring(email.Length - 4).Contains(".com")))
        {
            errormsg = "";
            return true;
        }
        else
        {
            errormsg = "Voer een geldig e-mail adres in";
            return false;
        }
    }
    public static bool ValidNaam(string voornaam, string achternaam, out string errormsg)
    {
        string naam = voornaam + " " + achternaam;
        //controleert of de naam alleen uit letters bestaat
        foreach(char c in naam)
        {
            if(!Char.IsLetter(c) && !Char.IsSeparator(c))
            {
                errormsg = "De voornaam en achternaam kunnen alleen uit letters bestaan.";
                return false;
            }
        }
        errormsg = "";
        return true;
    }
}
