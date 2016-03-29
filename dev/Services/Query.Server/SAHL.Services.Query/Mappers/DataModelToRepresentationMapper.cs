using System;
using System.Collections.Generic;
using SAHL.Core.Data;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Query.Core;
using SAHL.Services.Query.Interfaces;
using SAHL.Services.Query.Parsers.Elemets;
using SAHL.Services.Query.Resources;
using SAHL.Services.Query.Resources.Attorney;
using Omu.ValueInjecter;
using WebApi.Hal;

namespace SAHL.Services.Query.Mappers
{
    public class DataModelToRepresentationMapper<TDataModel, TRepresentation, TListRepresentation>: IDataModelToRepresentationMapper<IDataModel, IRepresentation, IListRepresentation> 
        where TListRepresentation : IListRepresentation 
        where TRepresentation : Representation
        where TDataModel : IDataModel
    {
         private readonly ILinkResolver linkResolver;

        public DataModelToRepresentationMapper(ILinkResolver linkResolver)
        {
            this.linkResolver = linkResolver;
        }

        public IListRepresentation MapModelsToRepresentation(IEnumerable<IDataModel> dataItems)
        {
            var representations = new List<Representation>();
            foreach (var dataItem in dataItems)
            {
                representations.Add(MapModelToRepresentation(dataItem));
            }

            IListRepresentation listRepresentation = (TListRepresentation)Activator.CreateInstance(typeof(TListRepresentation), linkResolver, representations);
            return listRepresentation;
        }

        public Representation MapModelToRepresentation(IDataModel dataModel)
        {
            Representation representation = (Representation) Activator.CreateInstance(typeof (TRepresentation), linkResolver);
            representation.InjectFrom(dataModel);
            return representation;
        }

    }

}