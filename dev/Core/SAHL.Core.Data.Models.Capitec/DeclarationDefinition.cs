using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class DeclarationDefinitionDataModel :  IDataModel
    {
        public DeclarationDefinitionDataModel(Guid declarationTypeEnumId, string declarationText)
        {
            this.DeclarationTypeEnumId = declarationTypeEnumId;
            this.DeclarationText = declarationText;
		
        }

        public DeclarationDefinitionDataModel(Guid iD, Guid declarationTypeEnumId, string declarationText)
        {
            this.ID = iD;
            this.DeclarationTypeEnumId = declarationTypeEnumId;
            this.DeclarationText = declarationText;
		
        }		

        public Guid ID { get; set; }

        public Guid DeclarationTypeEnumId { get; set; }

        public string DeclarationText { get; set; }
    }
}