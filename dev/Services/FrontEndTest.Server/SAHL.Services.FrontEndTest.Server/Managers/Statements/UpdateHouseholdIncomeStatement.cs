using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.FrontEndTest.Managers.Statements
{
    public class UpdateHouseholdIncomeStatement : ISqlStatement<OfferInformationVariableLoanDataModel>
    {
        public int ApplicationNumber { get; protected set; }

        public double HouseholdIncome { get; protected set; }

        public UpdateHouseholdIncomeStatement(int applicationNumber, double householdIncome)
        {
            this.ApplicationNumber = applicationNumber;
            this.HouseholdIncome = householdIncome;
        }

        public string GetStatement()
        {
            return @"Update oivl set HouseholdIncome = @HouseholdIncome
                    from (select max(OfferInformationKey) as OfferInformationKey 
                    from [2am].dbo.OfferInformation where OfferKey = @ApplicationNumber) as oi
                    join [2am].dbo.OfferInformationVariableLoan oivl on oi.OfferInformationKey = oivl.OfferInformationKey";
        }
    }
}
