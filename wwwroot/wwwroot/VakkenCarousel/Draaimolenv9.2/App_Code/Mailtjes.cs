using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for Mailtjes
/// </summary>
public static class Mailtjes
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
                                                        <img src=""http://www.csg-comenius.nl/themes/comenius2013/images/logo-comenius-new@2x.png"" alt=""csg-comenius logo"">
                                                    </td>
                                                </tr>
                                                <tr class=""part"" style=""border-bottom: 10px #a1a1a1 solid;"">
                                                    <td>";
                                                    //inhoud van de mail hier
    public const string eind = @"                   </td>
                                                </tr>
                                                <tr class=""part"" style=""border-bottom: 10px #a1a1a1 solid;"">
                                                    <td><em>Dit mailtje is automatisch verstuurd via carrouselkeuze.csg-comenius.nl</em></td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </body>
                            </html>";
    public const string accountgegevens = "SELECT inhoud, subject FROM mails WHERE mailId = 3";
    public const string leerlingentoevoegenadmin = "SELECT inhoud, subject FROM mails WHERE mailId = 4";
    public const string inschrijvinggeopendmailhavo3 = "SELECT inhoud, subject FROM mails WHERE mailId = 5";
    public const string inschrijvinggeopendmailath3 = "SELECT inhoud, subject FROM mails WHERE mailId = 6";
    public const string herinneringdecaanvoorlichtingenwijzigen = "SELECT inhoud, subject FROM mails WHERE mailId = 7";
    public const string herringsleerling5week = "SELECT inhoud, subject FROM mails WHERE mailId = 8";
    public const string herinneringsleerling2dag = "SELECT inhoud, subject FROM mails WHERE mailId = 9";
    public const string herinneringsdecaanaanpassenvoorlichtingen = "SELECT inhoud, subject FROM mails WHERE mailId = 10";
    public const string herinneringsdecaanactieondernemen = "SELECT inhoud, subject FROM mails WHERE mailId = 11";
}
