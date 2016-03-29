using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class ApplicationStatusEnumDataModel :  IDataModel
    {
        public ApplicationStatusEnumDataModel(string name, int sAHLOfferStatusKey, bool isActive)
        {
            this.Name = name;
            this.SAHLOfferStatusKey = sAHLOfferStatusKey;
            this.IsActive = isActive;
		
        }

        public ApplicationStatusEnumDataModel(Guid id, string name, int sAHLOfferStatusKey, bool isActive)
        {
            this.Id = id;
            this.Name = name;
            this.SAHLOfferStatusKey = sAHLOfferStatusKey;
            this.IsActive = isActive;
		
        }		

        public Guid Id { get; set; }

        public string Name { get; set; }

        public int SAHLOfferStatusKey { get; set; }

        public bool IsActive { get; set; }

        public const string NTU = "3d0b9a49-c69d-43b6-80ac-a2ee00f78b7b";

        public const string IN_PROGRESS = "f14b51ae-d633-454b-8f3f-a2ee00f78b7b";

        public const string DECLINE = "0a22a0cc-6933-4746-8032-a2ee00f78b7c";

        public const string PORTAL_DECLINED = "dc304e67-5792-4f8c-8a25-b87c000de96c";
    }
}