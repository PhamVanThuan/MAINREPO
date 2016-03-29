using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class ApplicantAddressDataModel :  IDataModel
    {
        public ApplicantAddressDataModel(Guid applicantId, Guid addressId, Guid addressTypeEnumId)
        {
            this.ApplicantId = applicantId;
            this.AddressId = addressId;
            this.AddressTypeEnumId = addressTypeEnumId;
		
        }

        public ApplicantAddressDataModel(Guid id, Guid applicantId, Guid addressId, Guid addressTypeEnumId)
        {
            this.Id = id;
            this.ApplicantId = applicantId;
            this.AddressId = addressId;
            this.AddressTypeEnumId = addressTypeEnumId;
		
        }		

        public Guid Id { get; set; }

        public Guid ApplicantId { get; set; }

        public Guid AddressId { get; set; }

        public Guid AddressTypeEnumId { get; set; }
    }
}