using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class DeclarationDataModel :  IDataModel
    {
        public DeclarationDataModel(Guid declarationDefinitionId, DateTime? declarationDate)
        {
            this.DeclarationDefinitionId = declarationDefinitionId;
            this.DeclarationDate = declarationDate;
		
        }

        public DeclarationDataModel(Guid iD, Guid declarationDefinitionId, DateTime? declarationDate)
        {
            this.ID = iD;
            this.DeclarationDefinitionId = declarationDefinitionId;
            this.DeclarationDate = declarationDate;
		
        }		

        public Guid ID { get; set; }

        public Guid DeclarationDefinitionId { get; set; }

        public DateTime? DeclarationDate { get; set; }
    }
}