using System;

namespace SAHL.Services.Capitec.Managers.CapitecApplication.Models
{
    public class CapitecGuidToSAHL_KeyMappingModel
    {
        public Guid CapitecKey { get; private set; }

        public int SAHL_Key { get; private set; }

        public CapitecGuidToSAHL_KeyMappingModel()
        {
        }

        public CapitecGuidToSAHL_KeyMappingModel(Guid CapitecKey, int SAHL_Key)
        {
            this.CapitecKey = CapitecKey;
            this.SAHL_Key = SAHL_Key;
        }
    }
}