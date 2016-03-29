using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using SAHL.Services.Query;
using SAHL.Services.Query.Controllers.Test;
using SAHL.Services.Query.Metadata;
using WebApi.Hal;

namespace SAHL.Config.Services.Query.Server
{
    public class RepresentationDataModelScanner
    {
        private readonly string assemblyToScan;

        public RepresentationDataModelScanner(string assemblyToScan)
        {
            this.assemblyToScan = assemblyToScan;
        }

        public IList<KeyValuePair<Type, Type>> GetMappings()
        {
            //modelType, representationType
            var result = new List<KeyValuePair<Type, Type>>();

            var modelTypes = GetDataModelTypes();
            var representationTypes = GetRepresentationTypes();

            AddMapping(representationTypes, result, modelTypes);

            return result;
        }

        private static void AddMapping(IEnumerable<Type> representationTypes, List<KeyValuePair<Type, Type>> result, IEnumerable<Type> modelTypes)
        {
            foreach (var representationType in representationTypes)
            {
                //representation maps to itself
                MapDataModelToRepresentation(result, representationType, representationType);

                var representationName = representationType.Name.Replace("Representation", string.Empty, StringComparison.OrdinalIgnoreCase);

                var modelType = modelTypes
                    .SingleOrDefault(a => a.Name.Replace("DataModel", string.Empty, StringComparison.OrdinalIgnoreCase)
                        .Equals(representationName));

                if (modelType == null)
                {
                    continue;
                }

                //add map for this data model to the representation
                MapDataModelToRepresentation(result, modelType, representationType);

                TryMapCollectionDataModelToListRepresentation(result, representationTypes, modelType, representationType);

            }
        }

        private static void TryMapCollectionDataModelToListRepresentation(List<KeyValuePair<Type, Type>> result, IEnumerable<Type> representationTypes, Type modelType, Type representationType)
        {
            foreach (var item in representationTypes)
            {
                //TODO: need to fix this one, as org structure doesn't have a list per-se
                if (item.Name.StartsWith("OrganisationStructure"))
                {
                    continue;
                }

                if (item.Name.EndsWith("ListRepresentation"))
                {
                    continue;
                }

                var representationName = item.Name.Replace("ListRepresentation", string.Empty, StringComparison.OrdinalIgnoreCase);

                if (!representationName.Equals(representationType.Name, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var listRepresentationName = representationName.Replace("Representation", "ListRepresentation", StringComparison.OrdinalIgnoreCase);

                var enumerableOfType = typeof (IEnumerable<>).MakeGenericType(modelType);

                var listRepresentationTypeString = item.AssemblyQualifiedName.Replace(item.Name, listRepresentationName);
                var listRepresentationType = Type.GetType(listRepresentationTypeString);

                if (listRepresentationType == null)
                {
                    throw new InvalidOperationException("Cannot add null list representation of " + listRepresentationName);
                }

                result.Add(new KeyValuePair<Type, Type>(enumerableOfType, listRepresentationType));
            }
        }

        private static void MapDataModelToRepresentation(List<KeyValuePair<Type, Type>> result, Type sourceType, Type targetType)
        {
            result.Add(new KeyValuePair<Type, Type>(sourceType, targetType));
        }

        private IEnumerable<Type> GetRepresentationTypes()
        {
            var representationType = typeof (Representation);
            return Assembly
                .Load(this.assemblyToScan)
                .GetTypes()
                .Where(a => a.BaseType != null && a.IsSubclassOf(representationType))
                ;
        }

        private IEnumerable<Type> GetDataModelTypes()
        {
            return Assembly
                .Load(this.assemblyToScan)
                .GetTypes()
                .Where(a => a.Name.EndsWith("DataModel")) //TODO: this really needs to be a common interface on all data models
                ;
        }
    }
}