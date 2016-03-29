using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.FrontEndTest.Managers.Statements
{
    public class SetOfferInformationSPVStatement: ISqlStatement<OfferInformationVariableLoanDataModel>
    {
        public int ApplicationInformationKey { get; protected set; }

        public int SPVKey { get; protected set; }

        public SetOfferInformationSPVStatement(int applicationNumberKey, int SPVKey)
        {
            this.ApplicationInformationKey = applicationNumberKey;
            this.SPVKey = SPVKey;
        }

        public string GetStatement()
        {
            return @"update [2am].dbo.OfferInformationVariableLoan set SpvKey = @SPVKey where OfferInformationKey = @ApplicationInformationKey";
        }
    }
}
