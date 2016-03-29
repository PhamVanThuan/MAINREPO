using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class YesNoDeclarationDataModel :  IDataModel
    {
        public YesNoDeclarationDataModel(string yesNoValue, DateTime? declarationDate)
        {
            this.YesNoValue = yesNoValue;
            this.DeclarationDate = declarationDate;
		
        }

        public YesNoDeclarationDataModel(Guid iD, string yesNoValue, DateTime? declarationDate)
        {
            this.ID = iD;
            this.YesNoValue = yesNoValue;
            this.DeclarationDate = declarationDate;
		
        }		

        public Guid ID { get; set; }

        public string YesNoValue { get; set; }

        public DateTime? DeclarationDate { get; set; }
    }
}