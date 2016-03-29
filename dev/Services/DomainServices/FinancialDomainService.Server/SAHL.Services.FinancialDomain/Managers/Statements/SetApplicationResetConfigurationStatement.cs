using SAHL.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.FinancialDomain.Managers.Statements
{
    public class SetApplicationResetConfigurationStatement: ISqlStatement<int>
    {
        public int ApplicationNumber { get; protected set; }

        public int SPVKey { get; protected set; }

        public int ProductKey { get; protected set; }

        public SetApplicationResetConfigurationStatement(int ApplicationNumber, int SPVKey, int ProductKey)
        {
            this.ApplicationNumber = ApplicationNumber;
            this.SPVKey = SPVKey;
            this.ProductKey = ProductKey;
        }

        public string GetStatement()
        {
            return @"
                    Update [2AM].dbo.OfferMortgageLoan 
                    set ResetConfigurationKey = (SELECT [2AM].[dbo].[fResetConfigurationDetermine] (@SPVKey, @ProductKey))
                    where OfferKey = @ApplicationNumber
                    ";
        }
    }
}
