using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class EmploymentTypeEnumDataModel :  IDataModel
    {
        public EmploymentTypeEnumDataModel(string name, bool isActive, int sAHLEmploymentTypeKey)
        {
            this.Name = name;
            this.IsActive = isActive;
            this.SAHLEmploymentTypeKey = sAHLEmploymentTypeKey;
		
        }

        public EmploymentTypeEnumDataModel(Guid id, string name, bool isActive, int sAHLEmploymentTypeKey)
        {
            this.Id = id;
            this.Name = name;
            this.IsActive = isActive;
            this.SAHLEmploymentTypeKey = sAHLEmploymentTypeKey;
		
        }		

        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public int SAHLEmploymentTypeKey { get; set; }

        public const string SALARIED = "dbea5e1c-a711-48dc-9cb6-a2d500ab5a72";

        public const string SELF_EMPLOYED = "6795b5ce-2835-4675-8039-a2d500ab5a73";

        public const string SALARIED_WITH_HOUSING_ALLOWANCE = "de1590b5-25fd-4334-97cc-a2d500ab5a74";

        public const string SALARIED_WITH_COMMISSION = "199534ad-b097-48a2-a1a4-a2ed00f7d232";

        public const string UNEMPLOYED = "64dc7a68-a8a1-41de-a0a5-a32b010b64e5";
    }
}