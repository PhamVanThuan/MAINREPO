using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class ApplicantContactDetailDataModel :  IDataModel
    {
        public ApplicantContactDetailDataModel(Guid applicantId, Guid contactDetailId)
        {
            this.ApplicantId = applicantId;
            this.ContactDetailId = contactDetailId;
		
        }

        public ApplicantContactDetailDataModel(Guid id, Guid applicantId, Guid contactDetailId)
        {
            this.Id = id;
            this.ApplicantId = applicantId;
            this.ContactDetailId = contactDetailId;
		
        }		

        public Guid Id { get; set; }

        public Guid ApplicantId { get; set; }

        public Guid ContactDetailId { get; set; }
    }
}