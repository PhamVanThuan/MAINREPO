using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class ApplicantApplicationAddressDataModel :  IDataModel
    {
        public ApplicantApplicationAddressDataModel(Guid applicantAddressId, Guid applicationId)
        {
            this.ApplicantAddressId = applicantAddressId;
            this.ApplicationId = applicationId;
		
        }

        public ApplicantApplicationAddressDataModel(Guid id, Guid applicantAddressId, Guid applicationId)
        {
            this.Id = id;
            this.ApplicantAddressId = applicantAddressId;
            this.ApplicationId = applicationId;
		
        }		

        public Guid Id { get; set; }

        public Guid ApplicantAddressId { get; set; }

        public Guid ApplicationId { get; set; }
    }
}