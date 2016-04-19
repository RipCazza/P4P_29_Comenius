using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

/// <summary>
/// Summary description for XMLUpload
/// </summary>
public class XMLUpload
{
	public XMLUpload()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static XmlDocument StripNamespace(XmlDocument doc)
    {
        if (doc.DocumentElement.NamespaceURI.Length > 0)
        {
            doc.DocumentElement.SetAttribute("xmlns", "");
            // must serialize and reload for this to take effect
            XmlDocument newDoc = new XmlDocument();
            newDoc.LoadXml(doc.OuterXml);
            return newDoc;
        }
        else
        {
            return doc;
        }
    }
}