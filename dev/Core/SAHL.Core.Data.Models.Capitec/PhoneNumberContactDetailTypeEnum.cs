using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class PhoneNumberContactDetailTypeEnumDataModel :  IDataModel
    {
        public PhoneNumberContactDetailTypeEnumDataModel(string name, bool isActive)
        {
            this.Name = name;
            this.IsActive = isActive;
		
        }

        public PhoneNumberContactDetailTypeEnumDataModel(Guid id, string name, bool isActive)
        {
            this.Id = id;
            this.Name = name;
            this.IsActive = isActive;
		
        }		

        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public const string HOME = "81929b34-6398-4733-9699-a2d500ab5a64";

        public const string WORK = "5c12c4f9-95b5-4c25-ae2c-a2d500ab5a65";

        public const string FAX = "88841dcf-9535-4cd8-8170-a2d500ab5a66";

        public const string MOBILE = "091f4918-63bd-405a-96cc-a2d500ab5a66";
    }
}