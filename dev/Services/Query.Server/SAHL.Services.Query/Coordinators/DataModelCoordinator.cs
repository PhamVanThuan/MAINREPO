using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SAHL.Services.Interfaces.Query.Coordinators;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Models.Core;

namespace SAHL.Services.Query.LinkCoordinator
{
    public class DataModelCoordinator : IDataModelCoordinator
    {
        public IEnumerable<IQueryDataModel> ResolveDataModelRelationships(IEnumerable<IQueryDataModel> datamodels)
        {
            foreach (IQueryDataModel datamodel in datamodels)
            {
                ResolveDataModelRelationships(datamodel);
            }

            return datamodels;
        }

        public IQueryDataModel ResolveDataModelRelationships(IQueryDataModel datamodel)
        {
            Type dataModelType = datamodel.GetType();
            PropertyInfo[] properties = dataModelType.GetProperties();

            foreach (var propertyInfo in properties)
            {
                if (propertyInfo.PropertyType == typeof(List<IRelationshipDefinition>))
                {
                    var relationships = GetModelRelationships(datamodel, properties, propertyInfo);
                    var incompleteRelationships = new List<IRelationshipDefinition>();
                    if (relationships != null)
                    {
                        foreach (var relationship in relationships)
                        {
                            if (!HandleAllRelatedFields(datamodel, properties, relationship))
                            {
                                incompleteRelationships.Add(relationship);
                            }
                        }
                    }

                    RemoveIncompleteRelationships(incompleteRelationships, relationships);
                }
            }

            return datamodel;
        }

        private static void RemoveIncompleteRelationships(List<IRelationshipDefinition> incompleteRelationships, List<IRelationshipDefinition> relationships)
        {
            if (incompleteRelationships.Count > 0)
            {
                foreach (var incompleteRelationship in incompleteRelationships)
                {
                    relationships.Remove(incompleteRelationship);
                }
            }
        }

        private bool HandleAllRelatedFields(IQueryDataModel datamodel, PropertyInfo[] properties, IRelationshipDefinition relationship)
        {
            foreach (var relatedField in relationship.RelatedFields)
            {
                string localKey = relatedField.LocalKey;
                var lookupId = ReadPropertyKeyValue(datamodel, localKey, properties);
                if (lookupId != null)
                {
                    SetValueField(relatedField, lookupId);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private void SetValueField(IRelatedField relatedField, object lookupId)
        {
            if (lookupId == null)
            {
                return;
            }

            Type relatedFieldType = relatedField.GetType();
            PropertyInfo[] fieldProperties = relatedFieldType.GetProperties();

            foreach (var fieldProperty in fieldProperties)
            {
                if (fieldProperty.Name == "Value")
                {
                    fieldProperty.SetValue(relatedField, lookupId.ToString());
                }
            }
        }

        private List<IRelationshipDefinition> GetModelRelationships(IQueryDataModel datamodel, PropertyInfo[] properties, PropertyInfo propertyInfo)
        {
            string property = propertyInfo.Name;
            return (List<IRelationshipDefinition>)ReadPropertyKeyValue(datamodel, property, properties);
        }

        private object ReadPropertyKeyValue(IQueryDataModel datamodel, string property, PropertyInfo[] properties)
        {
            PropertyInfo valueProperty = properties.FirstOrDefault(x => x.Name == property);
            return valueProperty.GetValue(datamodel, null);
        }
    }
}