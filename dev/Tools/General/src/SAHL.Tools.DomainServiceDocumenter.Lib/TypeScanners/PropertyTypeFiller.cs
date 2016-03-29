using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Server;
using Mono.Cecil;
using SAHL.Tools.DomainServiceDocumenter.Lib.Helpers;
using SAHL.Tools.DomainServiceDocumenter.Lib.Models;
using System.Linq;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.TypeScanners
{
    public static class PropertyTypeFiller
    {
        public static Property FillProperty(Mono.Cecil.PropertyDefinition property)
        {
            Property prop = new Property();
            prop.Name = property.Name;
            
            string propertyType = "";
            string collectionPropertyType = "";
            bool isPropertyCollection = false;

            GetPropertyDetails(property, out propertyType, out collectionPropertyType, out isPropertyCollection);

            prop.Type = propertyType;
            prop.IsCollection = isPropertyCollection;

            prop.FullType = property.PropertyType.FullName;
            prop.CollectionPropertyType = collectionPropertyType;

            PropertyUtility.GetPropertyValidation(property, prop);
            GetPropertiesForComposite(property, prop);

            return prop;
        }
        
        private static void GetPropertiesForComposite(PropertyDefinition property, Property prop)
        {
            if (prop.FullType.StartsWith("SAHL."))
            {
                TypeDefinition type = property.PropertyType.Resolve();

                foreach (var innerProperty in type.Properties)
                {
                    prop.Properties.Add(PropertyTypeFiller.FillProperty(innerProperty));
                }
            }
        }

        private static void GetPropertyDetails(PropertyDefinition property, out string propertyType, out string collectionPropertyType, out bool isPropertyCollection)
        {

            isPropertyCollection = false;
            collectionPropertyType = "";

            if (property.PropertyType.IsGenericInstance)
            {
                string collectionType = GetGenericType(property);
                propertyType = GetInstanceType(property);
                if (collectionType.Length > 0)
                {
                    isPropertyCollection = true;
                    collectionPropertyType = propertyType;
                    propertyType = collectionType + "(" + propertyType + ")";
                }
            }
            else
            {
                propertyType =  property.PropertyType.Name;    
            }
            
        }

        private static string GetInstanceType(PropertyDefinition property)
        {
            string propertyFullName = property.PropertyType.FullName;
            Regex r = new Regex("[<>]");
            propertyFullName = r.Replace(propertyFullName, "");

            string[] items = propertyFullName.Split(".".ToCharArray());
            return items[items.Count() - 1];
        }

        private static string GetGenericType(PropertyDefinition property)
        {
            if (property.PropertyType.Name.Contains("IEnumerable")) { return "IEnumerable"; }
            if (property.PropertyType.Name.Contains("List")) { return "List"; }
            if (property.PropertyType.Name.Contains("Nullable")) { return ""; }
            return "Document Error";
        }
    }
}