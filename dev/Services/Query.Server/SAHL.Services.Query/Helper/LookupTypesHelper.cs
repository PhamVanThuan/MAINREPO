using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;
using SAHL.Services.Interfaces.Query.DataManagers;
using SAHL.Services.Interfaces.Query.Helpers;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Query.Models;
using SAHL.Services.Query.Models.Lookup;
using SAHL.Services.Query.Resources;

namespace SAHL.Services.Query.Helper
{
    public class LookupTypesHelper : ILookupTypesHelper
    {

        private ISupportedLookupDataManager DataManager;

        public Dictionary<string, ISupportedLookup> ValidLookupTypes { get; private set; }

        public LookupTypesHelper(ISupportedLookupDataManager dataManager)
        {
            DataManager = dataManager;
            ValidLookupTypes = new Dictionary<string, ISupportedLookup>(StringComparer.OrdinalIgnoreCase);
        }

        public bool IsValidLookupType(string lookupType)
        {
            return ValidLookupTypes.ContainsKey(lookupType);
        }

        public ILookupMetaDataModel FindLookupMetaData(string lookupType)
        {
            ILookupMetaDataModel lookupMetaData;
            if (IsValidLookupType(lookupType))
            {
                lookupMetaData = ValidLookupTypes[lookupType].MetaData;
                if (lookupMetaData == null)
                {
                    lookupMetaData = DataManager.GetLookupSchema(lookupType);
                    ValidLookupTypes[lookupType].MetaData = lookupMetaData;
                }
            }
            else
            {
                throw new ArgumentException("Invalid lookup type supplied");
            }

            return lookupMetaData;
        }

        public void LoadValidLookupTypes()
        {
            
            List<SupportedLookupModel> supportedLookups = (List<SupportedLookupModel>)DataManager.GetSupportedLookups();
            ValidLookupTypes.Clear();
            foreach (var supportedLookup in supportedLookups)
            {
                SupportedLookup lookup = new SupportedLookup() { Lookup = supportedLookup.LookupTable };
                ValidLookupTypes.Add(supportedLookup.LookupKey, lookup);
            }

        }

    }

}