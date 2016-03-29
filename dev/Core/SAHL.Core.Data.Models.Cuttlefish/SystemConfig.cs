using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Cuttlefish
{
    [Serializable]
    public partial class SystemConfigDataModel :  IDataModel, IDataModelWithIdentitySeed, IDataModelWithPrimaryKeyId 
    {
        public SystemConfigDataModel(string cuttleFishServerName)
        {
            this.CuttleFishServerName = cuttleFishServerName;
		
        }

        public SystemConfigDataModel(int id, string cuttleFishServerName)
        {
            this.Id = id;
            this.CuttleFishServerName = cuttleFishServerName;
		
        }		

        public int Id { get; set; }

        public string CuttleFishServerName { get; set; }

        public void SetKey(int key)
        {
            this.Id =  key;
        }
    }
}