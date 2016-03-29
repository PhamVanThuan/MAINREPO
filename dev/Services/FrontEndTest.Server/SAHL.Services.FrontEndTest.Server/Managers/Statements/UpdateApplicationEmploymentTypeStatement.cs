using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.FrontEndTest.Managers.Statements
{
    public class UpdateApplicationEmploymentTypeStatement : ISqlStatement<OfferInformationVariableLoanDataModel>
    {
        public int ApplicationNumber { get; protected set; }

        public int EmploymentTypeKey { get; protected set; }

        public UpdateApplicationEmploymentTypeStatement(int applicationNumber, int employmentTypeKey)
        {
            this.ApplicationNumber = applicationNumber;
            this.EmploymentTypeKey = employmentTypeKey;
        }

        public string GetStatement()
        {
            return @"Update oivl set EmploymentTypeKey = @EmploymentTypeKey
                    from (select max(OfferInformationKey) as OfferInformationKey 
                    from [2am].dbo.OfferInformation where OfferKey = @ApplicationNumber) as oi
                    join [2am].dbo.OfferInformationVariableLoan oivl on oi.OfferInformationKey = oivl.OfferInformationKey";
        }
    }
}
