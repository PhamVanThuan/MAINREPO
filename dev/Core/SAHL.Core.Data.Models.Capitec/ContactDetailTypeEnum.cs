using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class ContactDetailTypeEnumDataModel :  IDataModel
    {
        public ContactDetailTypeEnumDataModel(string name, bool isActive)
        {
            this.Name = name;
            this.IsActive = isActive;
		
        }

        public ContactDetailTypeEnumDataModel(Guid id, string name, bool isActive)
        {
            this.Id = id;
            this.Name = name;
            this.IsActive = isActive;
		
        }		

        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public const string EMAIL = "c3e9f603-b6fc-49a5-93ab-a2d500ab5a62";

        public const string PHONE = "61858b2a-77ee-415f-8c8e-a2d500ab5a63";

        public const string SMS = "0adaeec9-e9d5-44c2-a8a1-a2d500ab5a64";
    }
}