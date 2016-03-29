using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class ApplicantDeclarationDataModel :  IDataModel
    {
        public ApplicantDeclarationDataModel(Guid applicantId, Guid declarationId)
        {
            this.ApplicantId = applicantId;
            this.DeclarationId = declarationId;
		
        }

        public ApplicantDeclarationDataModel(Guid iD, Guid applicantId, Guid declarationId)
        {
            this.ID = iD;
            this.ApplicantId = applicantId;
            this.DeclarationId = declarationId;
		
        }		

        public Guid ID { get; set; }

        public Guid ApplicantId { get; set; }

        public Guid DeclarationId { get; set; }
    }
}