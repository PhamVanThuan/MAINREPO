using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class EmailAddressContactDetailDataModel :  IDataModel
    {
        public EmailAddressContactDetailDataModel(Guid emailAddressContactDetailTypeEnumId, string emailAddress)
        {
            this.EmailAddressContactDetailTypeEnumId = emailAddressContactDetailTypeEnumId;
            this.EmailAddress = emailAddress;
		
        }

        public EmailAddressContactDetailDataModel(Guid id, Guid emailAddressContactDetailTypeEnumId, string emailAddress)
        {
            this.Id = id;
            this.EmailAddressContactDetailTypeEnumId = emailAddressContactDetailTypeEnumId;
            this.EmailAddress = emailAddress;
		
        }		

        public Guid Id { get; set; }

        public Guid EmailAddressContactDetailTypeEnumId { get; set; }

        public string EmailAddress { get; set; }
    }
}