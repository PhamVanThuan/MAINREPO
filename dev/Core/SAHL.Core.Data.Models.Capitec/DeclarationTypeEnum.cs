using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class DeclarationTypeEnumDataModel :  IDataModel
    {
        public DeclarationTypeEnumDataModel(string name, bool isActive)
        {
            this.Name = name;
            this.IsActive = isActive;
		
        }

        public DeclarationTypeEnumDataModel(Guid iD, string name, bool isActive)
        {
            this.ID = iD;
            this.Name = name;
            this.IsActive = isActive;
		
        }		

        public Guid ID { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public const string YES = "f54495a4-aaee-4031-8099-a2d500ab5a75";

        public const string NO = "5be6c2e7-9ec3-44a8-a572-a2d500ab5a76";
    }
}