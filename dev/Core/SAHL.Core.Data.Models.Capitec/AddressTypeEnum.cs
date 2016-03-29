using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class AddressTypeEnumDataModel :  IDataModel
    {
        public AddressTypeEnumDataModel(string name, bool isActive, int sAHLAddressTypeKey)
        {
            this.Name = name;
            this.IsActive = isActive;
            this.SAHLAddressTypeKey = sAHLAddressTypeKey;
		
        }

        public AddressTypeEnumDataModel(Guid id, string name, bool isActive, int sAHLAddressTypeKey)
        {
            this.Id = id;
            this.Name = name;
            this.IsActive = isActive;
            this.SAHLAddressTypeKey = sAHLAddressTypeKey;
		
        }		

        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public int SAHLAddressTypeKey { get; set; }

        public const string RESIDENTIAL = "4484b1c9-b8f1-4612-bb26-a2d500ab5a70";

        public const string POSTAL = "6315bbb9-2408-4ed6-ae9f-a2d500ab5a72";
    }
}