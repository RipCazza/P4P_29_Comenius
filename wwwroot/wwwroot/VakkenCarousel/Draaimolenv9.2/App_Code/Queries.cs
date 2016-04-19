using System;
using System.Collections.Generic;
using System.Web;

public static class Queries
{
    public const string naam = "SELECT voornaam, achternaam FROM Leerling WHERE gebruikerId = @0";
    public const string leerlingnummer = "SELECT leerlingnummer FROM Leerling WHERE gebruikerId = @0";
    public const string klas = "SELECT klas FROM Leerling WHERE gebruikerId = @0";
    public const string rolEmail = "SELECT rol FROM Gebruiker WHERE email = @0";
    public const string rolId = "SELECT rol FROM Gebruiker WHERE gebruikerId = @0";
    public const string niveau = "SELECT niveau FROM Leerling WHERE gebruikerId = @0";
    public const string email = "SELECT email FROM Gebruiker WHERE gebruikerId = @0";
}