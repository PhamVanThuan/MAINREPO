using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SAHL.Config.Services.Query.Server;
using SAHL.Services.Query.Metadata;
using StructureMap;

namespace SAHL.Services.Query.Server.DynamicRoutePersister
{
    class Program
    {
        private static void Main(string[] args)
        {
            var allDynamicRootsContainer = new Container(new RouteMetadataRegistry());
            var links =
                allDynamicRootsContainer.GetInstance<IEnumerable<LinkMetadata>>(
                    RouteMetadataRegistry.RouteMetadataInstanceName);
            XDocument persistanceDocument = new XDocument();
            XElement rootLevel = new XElement("DynamicRoots");
            persistanceDocument.Add(rootLevel);

            rootLevel.Add(links.Select(x => new XElement("route", x.UrlTemplate.Replace("~", ""))));
            string fileName = "SAHL.Services.Query.Routes.xml";
            persistanceDocument.Save(fileName);
            Console.WriteLine("saved");
            if (args.Any())
            {
                try
                {
                    Console.WriteLine(string.Format("{0} ", "to file"));
                    string path = string.Format("{0}\\{1}", (new Uri(args[0])).LocalPath.Replace("\"", ""), fileName);
                    Console.WriteLine(string.Format("{0} -- {1}", "to file", path));
                    persistanceDocument.Save(path);
                }
                catch (Exception E)
                {
                    Console.WriteLine(E.Message);
                }
            }

        }
    }
}
