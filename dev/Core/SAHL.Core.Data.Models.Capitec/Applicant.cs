using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class ApplicantDataModel :  IDataModel
    {
        public ApplicantDataModel(Guid personID, bool mainContact)
        {
            this.PersonID = personID;
            this.MainContact = mainContact;
		
        }

        public ApplicantDataModel(Guid id, Guid personID, bool mainContact)
        {
            this.Id = id;
            this.PersonID = personID;
            this.MainContact = mainContact;
		
        }		

        public Guid Id { get; set; }

        public Guid PersonID { get; set; }

        public bool MainContact { get; set; }
    }
}