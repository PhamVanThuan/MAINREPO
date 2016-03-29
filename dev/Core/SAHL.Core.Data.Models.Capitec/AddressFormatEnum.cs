using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class AddressFormatEnumDataModel :  IDataModel
    {
        public AddressFormatEnumDataModel(string name, bool isActive, int sAHLAddressFormatKey)
        {
            this.Name = name;
            this.IsActive = isActive;
            this.SAHLAddressFormatKey = sAHLAddressFormatKey;
		
        }

        public AddressFormatEnumDataModel(Guid id, string name, bool isActive, int sAHLAddressFormatKey)
        {
            this.Id = id;
            this.Name = name;
            this.IsActive = isActive;
            this.SAHLAddressFormatKey = sAHLAddressFormatKey;
		
        }		

        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public int SAHLAddressFormatKey { get; set; }

        public const string STREET = "8ebd6557-3d03-452a-a88d-a2d500ab5a6d";

        public const string BOX = "f9b76c82-f6bf-4f3e-9507-a2d500ab5a6e";

        public const string POSTNET_SUITE = "170e2647-359d-4437-b23d-a2d500ab5a6e";

        public const string PRIVATE_BAG = "f59052d3-a129-49b9-a4dc-a2d500ab5a6f";

        public const string FREE_TEXT = "5e5f1378-b735-486b-b112-a2d500ab5a6f";

        public const string CLUSTER_BOX = "cfdb1ef2-44df-4121-9a1b-a2d500ab5a70";
    }
}