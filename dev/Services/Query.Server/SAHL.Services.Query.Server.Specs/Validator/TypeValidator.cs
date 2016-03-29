using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SAHL.Services.Query.Resources;
using SAHL.Services.Query.Resources.OrganisationStructure;
using StructureMap.TypeRules;
using WebApi.Hal;

namespace SAHL.Services.Query.Server.Specs.Validator
{
    public static class TypeValidator
    {
        public static Exception CheckForAnIdProperty(Type type, string elementType)
        {

            if (TypeShouldIgnoreIdPropertyCheck(type)) { return null; }
            
            List<PropertyInfo> properties = GetAllTypeProperties(type);

            PropertyInfo idProperty = properties.FirstOrDefault(x => x.Name.ToLower() == "id");

            if (idProperty == null)
            {
                return new Exception("The " + elementType + " " + type.Name + " requires an id property");
            }

            return null;

        }

        private static bool TypeShouldIgnoreIdPropertyCheck(Type type)
        {
            return type.Name == "OrganisationRepresentation" 
                   || type.Name == "RootRepresentation"
                   || type.Name.Contains("ListRepresentation")
                   || type.Name.Contains("CountRepresentation");
        }

        public static Exception CheckThatAllTypesAreNullable(Type type, string elementType)
        {
            List<PropertyInfo> properties = GetAllTypeProperties(type);

            foreach (var property in properties)
            {
                Type currentType = property.PropertyType;
                if (!(property.Name.ToLower() == "id"))
                {

                    if (!(currentType == typeof(string)) && !currentType.IsClass && !currentType.IsInterface)
                    {
                        if (currentType.IsPrimitive)
                        {
                            return CreateNullableException(type, elementType, property);
                        }
                            
                        if (!currentType.IsNullable())
                        {
                            return CreateNullableException(type, elementType, property);
                        }    
                    }
                }
            }

            return null;
        }

        private static Exception CreateNullableException(Type type, string elementType, PropertyInfo property)
        {
            return new Exception(property.Name + " of the " + elementType + " " + type.Name + " is not nullable");
        }

        private static List<PropertyInfo> GetAllTypeProperties(Type type)
        {
            return type.GetProperties().ToList();
        }

    }
}