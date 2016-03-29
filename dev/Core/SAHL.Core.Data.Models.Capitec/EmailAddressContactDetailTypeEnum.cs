using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class EmailAddressContactDetailTypeEnumDataModel :  IDataModel
    {
        public EmailAddressContactDetailTypeEnumDataModel(string name, bool isActive)
        {
            this.Name = name;
            this.IsActive = isActive;
		
        }

        public EmailAddressContactDetailTypeEnumDataModel(Guid id, string name, bool isActive)
        {
            this.Id = id;
            this.Name = name;
            this.IsActive = isActive;
		
        }		

        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public const string HOME = "99054934-7447-4b4f-8d19-a2d500ab5a67";

        public const string WORK = "fb0b9cf2-c576-4473-ba48-a2d500ab5a68";
    }
}