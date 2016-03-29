using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Cuttlefish
{
    [Serializable]
    public partial class GenericStatusDataModel :  IDataModel
    {
        public GenericStatusDataModel(string description)
        {
            this.Description = description;
		
        }

        public GenericStatusDataModel(int iD, string description)
        {
            this.ID = iD;
            this.Description = description;
		
        }		

        public int ID { get; set; }

        public string Description { get; set; }
    }
}