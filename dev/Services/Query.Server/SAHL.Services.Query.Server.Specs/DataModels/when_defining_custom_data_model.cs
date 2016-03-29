using System;
using System.Data;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using SAHL.Core.Data;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Query.Metadata;
using SAHL.Services.Query.Models.Core;
using SAHL.Services.Query.Server.Specs.Validator;
using StructureMap;
using Machine.Fakes;
using Machine.Specifications;

using SAHL.Core.Testing;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context.Configuration;
using StructureMap.TypeRules;

namespace SAHL.Services.Query.Server.Specs.DataModels
{
    public class when_defining_custom_data_model : WithFakes
    {
        private static List<Type> typesThatImplementDataModel;
        private static Type dataModelType;
        private static List<Exception> exceptions;
        private static Exception nullableTypeException;
        private static Exception idException;
        private static Exception relationshipException;
        
        Establish that = () =>
        {
            dataModelType = typeof(IQueryDataModel);

            typesThatImplementDataModel = Assembly.Load("SAHL.Services.Query")
                .GetTypes()
                .Where(b => dataModelType.IsAssignableFrom(b))
                .ToList();
        };
        
        private Because of = () =>
        {
            foreach (var type in typesThatImplementDataModel)
            {
                CheckForAnIdProperty(type);
                CheckThatAllTypesAreNullable(type);
                CheckThatAllRelationshipDefinitionsLinkToDataModelElements(type);
            }
        };

        private It should_have_all_fields_nullable = () =>
        {
            if (nullableTypeException != null)
            {
                throw nullableTypeException;
            }
        };

        private It should_have_a_property_called_id = () =>
        {
            if (idException != null)
            {
                throw idException;
            }
        };

        private It should_have_relationship_local_key_definded_on_the_data_model = () =>
        {
            if (relationshipException != null)
            {
                throw relationshipException;
            }
        };

        private static void CheckForAnIdProperty(Type type)
        {
            if (idException == null)
            {
                idException = TypeValidator.CheckForAnIdProperty(type, "Data Model");
            }
        }

        private static void CheckThatAllTypesAreNullable(Type type)
        {
            if (nullableTypeException == null)
            {
                nullableTypeException = TypeValidator.CheckThatAllTypesAreNullable(type, "Data Model");
            }
        }

        private static List<PropertyInfo> GetAllTypeProperties(Type type)
        {
            return type.GetProperties().ToList();
        }

        private static void CheckThatAllRelationshipDefinitionsLinkToDataModelElements(Type type)
        {
            List<PropertyInfo> properties = GetAllTypeProperties(type);

            var relationshipProperty = properties
                .Where(x => x.PropertyType == typeof(List<RelationshipDefinition>))
                .ToList()
                .FirstOrDefault();

            if (relationshipProperty == null)
            {
                return;
            }

            var dataModel = Activator.CreateInstance(type);

            List<RelationshipDefinition> relationshipDefinitions = (List<RelationshipDefinition>)relationshipProperty.GetValue(dataModel, null);

            foreach (var relationshipDefinition in relationshipDefinitions)
            {
                foreach (var relatedField in relationshipDefinition.RelatedFields)
                {
                    var localField = relatedField.LocalKey;
                    var relatedLocalItem = properties.FirstOrDefault(x => x.Name.Equals(localField, StringComparison.OrdinalIgnoreCase));

                    if (relatedLocalItem != null)
                    {
                        continue;
                    }
                    var message = string.Format("DataModel {0} contains a relationship ({1}) with a local key ({2}) " +
                        "which doesn't exist on the model.", type.Name, relationshipDefinition.RelatedEntity, relatedField.LocalKey);
                    relationshipException = new Exception(message);
                }
            }
        }
    }
}
