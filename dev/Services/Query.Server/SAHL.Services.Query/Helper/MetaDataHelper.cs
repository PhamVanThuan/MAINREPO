using System;
using SAHL.Services.Query.Models;
using SAHL.Services.Query.Models.Lookup;

namespace SAHL.Services.Query.Helper
{
    public class MetaDataHelper
    {
        public LookupMetaDataModel GenerateMetaDataLookup(string lookUpMetaData)
        {
            string[] metaData = lookUpMetaData.Split(",".ToCharArray());

            ValidateMetaData(metaData);
            
            LookupMetaDataModel lookupMetaDataModel = new LookupMetaDataModel();
            lookupMetaDataModel.LookupType = metaData[0];
            lookupMetaDataModel.Db = metaData[1];
            lookupMetaDataModel.Schema = metaData[2];
            lookupMetaDataModel.LookupTable = metaData[3];
            lookupMetaDataModel.LookupKey = metaData[4];
            lookupMetaDataModel.LookupDescription = metaData[5];

            return lookupMetaDataModel;

        }

        private void ValidateMetaData(string[] metaData)
        {
            if (metaData.Length != 6)
            {
                throw new ArgumentException("Invalid metadata supplied to LookupDataModel Meta Data object");
            }
        }
    }
}