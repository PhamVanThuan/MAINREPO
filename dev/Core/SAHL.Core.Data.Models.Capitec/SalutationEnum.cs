using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class SalutationEnumDataModel :  IDataModel
    {
        public SalutationEnumDataModel(string name, bool isActive, int sAHLSalutationKey)
        {
            this.Name = name;
            this.IsActive = isActive;
            this.SAHLSalutationKey = sAHLSalutationKey;
		
        }

        public SalutationEnumDataModel(Guid id, string name, bool isActive, int sAHLSalutationKey)
        {
            this.Id = id;
            this.Name = name;
            this.IsActive = isActive;
            this.SAHLSalutationKey = sAHLSalutationKey;
		
        }		

        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public int SAHLSalutationKey { get; set; }

        public const string MR = "c3b80328-c104-4e39-9dcc-a2d500ab5a5b";

        public const string MRS = "413b379e-bce8-4141-9a79-a2d500ab5a5c";

        public const string PROF = "6cdf2382-60ef-4a73-a522-a2d500ab5a5c";

        public const string DR = "dbdacf00-6a42-4be9-bcfd-a2d500ab5a5d";

        public const string PAST = "70d84f7e-3b45-4f6d-aa37-a2d500ab5a5e";

        public const string CAPT = "7cfd1df6-833a-4d9e-bd2d-a2d500ab5a5e";

        public const string SIR = "73980bd1-f323-4128-a8cd-a2d500ab5a5f";

        public const string MISS = "6b63d4ed-e505-4f10-aaa1-a2d500ab5a5f";

        public const string MS = "08de5dde-1d76-4163-be9b-a2d500ab5a60";

        public const string LORD = "af9ac391-b052-49d5-9f00-a2d500ab5a61";

        public const string REV = "f10c8e7a-df28-4c48-9fa0-a2d500ab5a61";

        public const string ADVOCATE = "e7fb0f48-b3c1-486e-aa7b-a2d500ab5a62";
    }
}