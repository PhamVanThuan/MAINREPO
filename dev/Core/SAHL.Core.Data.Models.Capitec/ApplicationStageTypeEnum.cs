using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class ApplicationStageTypeEnumDataModel :  IDataModel
    {
        public ApplicationStageTypeEnumDataModel(string name, int reference, bool isActive)
        {
            this.Name = name;
            this.Reference = reference;
            this.IsActive = isActive;
		
        }

        public ApplicationStageTypeEnumDataModel(Guid id, string name, int reference, bool isActive)
        {
            this.Id = id;
            this.Name = name;
            this.Reference = reference;
            this.IsActive = isActive;
		
        }		

        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Reference { get; set; }

        public bool IsActive { get; set; }

        public const string GATHERING_DOCUMENTATION = "c667c1e8-5347-4762-a11e-a2ee00f526b5";

        public const string VALUATION_COMPLETE = "c564a140-c1b5-4a95-a082-a2ee00f526b6";

        public const string CREDIT_ASSESSMENT = "dbaf7506-486d-45c8-b97d-a2ee00f526b7";

        public const string ATTORNEY_INSTRUCTED = "0600bd09-d89c-4ae1-b0f2-a2ee00f526b8";

        public const string LOAN_APPROVED = "3cb752bb-8b47-4228-b16f-a2ee00f526b8";

        public const string LOAN_REGISTERED = "84509e86-6c7e-48c6-a77b-a2ee00f526b9";

        public const string APPLICATION_SUBMITTED = "4db78a8e-fc04-41a5-a66c-a3e600f81338";
    }
}