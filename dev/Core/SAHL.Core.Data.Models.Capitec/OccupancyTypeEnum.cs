using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class OccupancyTypeEnumDataModel :  IDataModel
    {
        public OccupancyTypeEnumDataModel(string name, bool isActive, int sAHLOccupancyTypeKey)
        {
            this.Name = name;
            this.IsActive = isActive;
            this.SAHLOccupancyTypeKey = sAHLOccupancyTypeKey;
		
        }

        public OccupancyTypeEnumDataModel(Guid id, string name, bool isActive, int sAHLOccupancyTypeKey)
        {
            this.Id = id;
            this.Name = name;
            this.IsActive = isActive;
            this.SAHLOccupancyTypeKey = sAHLOccupancyTypeKey;
		
        }		

        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public int SAHLOccupancyTypeKey { get; set; }

        public const string OWNER_OCCUPIED = "81b73055-cfe4-4676-9b8e-a2d500ac88c8";

        public const string INVESTMENT_PROPERTY = "7fc97060-4748-4024-af69-a2d500ac88ca";
    }
}