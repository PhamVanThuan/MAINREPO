using System.Collections.Generic;
using SAHL.Services.Interfaces.Query.DataManagers;
using SAHL.Services.Interfaces.Query.Helpers;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Query.Interfaces;
using SAHL.Services.Query.Models;
using SAHL.Services.Query.Resources;
using SAHL.Services.Query.Resources.Lookup;
using WebApi.Hal;

namespace SAHL.Services.Query.Helper
{
    public class LookupRepresentationHelper : ILookupRepresentationHelper
    {

        private ILookupTypesHelper LookupTypesHelper { get; set; }
        private ILookupDataManager LookupDataManager { get; set; }
        private ILinkResolver linkResolver { get; set; }

        public LookupRepresentationHelper(ILinkResolver linkResolver, ILookupTypesHelper lookupTypesHelper, ILookupDataManager lookupDataManager)
        {
            LookupTypesHelper = lookupTypesHelper;
            LookupDataManager = lookupDataManager;
            this.linkResolver = linkResolver;
        }

        public LookupTypeListRepresentation GetLookupTypesRepresentation()
        {

            IList<LookupTypeRepresentation> lookupTypeListRepresentations = new List<LookupTypeRepresentation>();

            foreach (var lookupType in LookupTypesHelper.ValidLookupTypes.Keys)
            {
                LookupTypeRepresentation lookupTypeRepresentation = new LookupTypeRepresentation()
                {
                    Id = lookupType,
                    Description = LookupTypesHelper.ValidLookupTypes[lookupType].Lookup
                };

                lookupTypeListRepresentations.Add(lookupTypeRepresentation);

            }
            
            return new LookupTypeListRepresentation(lookupTypeListRepresentations);

        }

        public LookupListRepresentation GetLookupsRepresentation(string lookupType, IFindQuery findManyQuery)
        {   
            ILookupMetaDataModel lookupMetaDataModel = LookupTypesHelper.FindLookupMetaData(lookupType);
            IEnumerable<ILookupDataModel> lookups = LookupDataManager.GetLookups(findManyQuery, lookupMetaDataModel.Db, lookupMetaDataModel.Schema,
                lookupMetaDataModel.LookupTable, lookupMetaDataModel.LookupKey, lookupMetaDataModel.LookupDescription);

            List<LookupRepresentation> lookupRepresentations = new List<LookupRepresentation>();

            foreach (var lookupItem in lookups)
            {
                lookupRepresentations.Add(new LookupRepresentation()
                {
                    Id = lookupItem.Id,
                    LookupType = lookupType,
                    Description = lookupItem.Description
                });
            }

            return new LookupListRepresentation(linkResolver, (IList<LookupRepresentation>)lookupRepresentations, lookupType);
            
        }

        public LookupRepresentation GetLookupRepresentation(string lookupType, int id, IFindQuery findManyQuery)
        {
            ILookupMetaDataModel lookupMetaDataModel = LookupTypesHelper.FindLookupMetaData(lookupType);
            ILookupDataModel lookup = LookupDataManager.GetLookup(findManyQuery, lookupMetaDataModel.Db, lookupMetaDataModel.Schema,
                lookupMetaDataModel.LookupTable, lookupMetaDataModel.LookupKey, lookupMetaDataModel.LookupDescription, id);

            return new LookupRepresentation()
            {
                Id = lookup.Id,
                Description = lookup.Description,
                LookupType = lookupType
            };

        }


    }
}