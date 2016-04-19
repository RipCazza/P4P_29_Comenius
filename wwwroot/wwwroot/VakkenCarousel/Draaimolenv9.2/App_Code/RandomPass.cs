using System;
using System.Collections.Generic;
using System.Web;
using System.IO;

public class RandomPass
{
    public static string GeneratePass(byte length, Random random) 
    {
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        string pass = "";
        for(int i = 0; i < length; i++)
        {
            pass += chars[random.Next(chars.Length)];
        }
        return pass;
    }
}