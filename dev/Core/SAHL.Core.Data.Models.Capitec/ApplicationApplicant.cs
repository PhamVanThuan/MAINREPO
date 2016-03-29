using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class ApplicationApplicantDataModel :  IDataModel
    {
        public ApplicationApplicantDataModel(Guid applicationId, Guid applicantId)
        {
            this.ApplicationId = applicationId;
            this.ApplicantId = applicantId;
		
        }

        public ApplicationApplicantDataModel(Guid id, Guid applicationId, Guid applicantId)
        {
            this.Id = id;
            this.ApplicationId = applicationId;
            this.ApplicantId = applicantId;
		
        }		

        public Guid Id { get; set; }

        public Guid ApplicationId { get; set; }

        public Guid ApplicantId { get; set; }
    }
}