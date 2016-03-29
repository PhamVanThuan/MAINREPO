using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class ApplicationPurposeEnumDataModel :  IDataModel
    {
        public ApplicationPurposeEnumDataModel(string name, bool isActive, int sAHLMortgageLoanPurposeKey)
        {
            this.Name = name;
            this.IsActive = isActive;
            this.SAHLMortgageLoanPurposeKey = sAHLMortgageLoanPurposeKey;
		
        }

        public ApplicationPurposeEnumDataModel(Guid id, string name, bool isActive, int sAHLMortgageLoanPurposeKey)
        {
            this.Id = id;
            this.Name = name;
            this.IsActive = isActive;
            this.SAHLMortgageLoanPurposeKey = sAHLMortgageLoanPurposeKey;
		
        }		

        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public int SAHLMortgageLoanPurposeKey { get; set; }

        public const string SWITCH = "d5688440-e327-4c63-b628-a2d500ab5a58";

        public const string NEW_PURCHASE = "2406d732-5017-47e8-985f-a2d500ab5a5b";
    }
}