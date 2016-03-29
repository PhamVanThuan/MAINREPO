using SAHL.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.LifeDomain.Managers.Statements
{
    public class GetEmailAddressForUserWhoApprovedDisabilityClaimStatement : ISqlStatement<string>
    {
        public int DisabilityClaimKey { get; protected set; }
        public GetEmailAddressForUserWhoApprovedDisabilityClaimStatement(int disabilityClaimKey)
        {
            this.DisabilityClaimKey = disabilityClaimKey;
        }

        public string GetStatement()
        {
            return @"select top 1 le.EmailAddress --, * 
                from [2AM].[dbo].StageTransition st
                join [2AM].[dbo].StageDefinitionStageDefinitionGroup sdsdg on st.StageDefinitionStageDefinitionGroupKey = sdsdg.StageDefinitionStageDefinitionGroupKey
                join [2AM].[dbo].[ADUser] ad on st.ADUserKey = ad.ADUserKey
                JOIN  [2AM].[dbo].[LegalEntity] le ON ad.LegalEntityKey = le.LegalEntityKey
                where StageDefinitionKey = 4055 -- 'Disability Claim Approved'
                and st.GenericKey = @disabilityClaimKey
                order by TransitionDate desc";
        }
    }
}
