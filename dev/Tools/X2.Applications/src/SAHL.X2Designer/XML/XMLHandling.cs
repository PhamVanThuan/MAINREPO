using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace SAHL.X2Designer.XML
{
    public static class XMLHandling
    {
        public static string GetMapVersion(string fileName)
        {
            string Name = Path.GetFileNameWithoutExtension(fileName);

            if (MainForm.App.GetCurrentView() != null)
            {
                MainForm.App.GetCurrentView().Cursor = Cursors.WaitCursor;
                MainForm.App.GetCurrentView().Document.Location = fileName;
                MainForm.App.GetCurrentView().Name = Path.GetFileNameWithoutExtension(MainForm.App.GetCurrentView().Document.Location);
            }

            XmlDocument xdoc = new XmlDocument();

            xdoc.Load(fileName);

            XmlNode root = xdoc.DocumentElement;
            XmlAttribute a;
            a = root.Attributes["ProductVersion"];
            string docversion = "";
            if (a != null)
                docversion = a.Value.ToString();
            if (docversion != "0.1" && docversion != "0.2" && docversion != "0.3")
            {
                throw new NotSupportedException("For simplicity, this sample application does not handle different versions of saved documents");
            }

            a = root.Attributes["MapVersion"];
            if (a != null)
                if (a.Value.ToString().Length < 1)
                    return "UNKNOWN";
                else
                    return a.Value.ToString();
            else
                return "UNKNOWN";
        }

        public static void SetMapVersion(string fileName, string xmlFile, string version)
        {
            XmlDocument xdoc = new XmlDocument();

            string xmlName = Path.GetDirectoryName(fileName) + "\\" + Path.GetFileNameWithoutExtension(fileName) + ".xml";
            File.Copy(fileName, fileName + "cpy", true);

            xdoc.Load(fileName);

            XmlNode root = xdoc.DocumentElement;
            XmlAttribute a = root.Attributes["MapVersion"];
            if (a != null)
                root.Attributes["MapVersion"].Value = version;
            else
            {
                XmlAttribute mapVer;
                mapVer = xdoc.CreateAttribute("MapVersion");
                mapVer.Value = version;
                root.Attributes.Append(mapVer);
            }

            xdoc.Save(xmlName);

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }

        public static void FlagMapAsRetrieved(string fileName, string xmlFile)
        {
            XmlDocument xdoc = new XmlDocument();

            string xmlName = Path.GetDirectoryName(fileName) + "\\" + Path.GetFileNameWithoutExtension(fileName) + ".xml";
            xdoc.Load(fileName);

            XmlNode root = xdoc.DocumentElement;
            XmlAttribute a = root.Attributes["Retrieved"];

            if (a != null)
            {
                root.Attributes["Retrieved"].Value = "true";
            }

            xdoc.Save(xmlName);

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            File.Copy(xmlName, fileName);
            File.Delete(xmlName);
        }
    }
}