using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class PhoneNumberContactDetailDataModel :  IDataModel
    {
        public PhoneNumberContactDetailDataModel(Guid phoneNumberContactDetailTypeEnumId, string phoneCode, string phoneNumber)
        {
            this.PhoneNumberContactDetailTypeEnumId = phoneNumberContactDetailTypeEnumId;
            this.PhoneCode = phoneCode;
            this.PhoneNumber = phoneNumber;
		
        }

        public PhoneNumberContactDetailDataModel(Guid id, Guid phoneNumberContactDetailTypeEnumId, string phoneCode, string phoneNumber)
        {
            this.Id = id;
            this.PhoneNumberContactDetailTypeEnumId = phoneNumberContactDetailTypeEnumId;
            this.PhoneCode = phoneCode;
            this.PhoneNumber = phoneNumber;
		
        }		

        public Guid Id { get; set; }

        public Guid PhoneNumberContactDetailTypeEnumId { get; set; }

        public string PhoneCode { get; set; }

        public string PhoneNumber { get; set; }
    }
}