using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Managers.Applicant.Statements
{
    public class GetApplicantAttributesStatement : ISqlStatement<OfferRoleAttributeDataModel>
    {
        public int ApplicationRoleKey { get; protected set; }

        public GetApplicantAttributesStatement(int applicationRoleKey)
        {
            this.ApplicationRoleKey = applicationRoleKey;
        }

        public string GetStatement()
        {
            var query = @"select * from [2am].dbo.OfferRoleAttribute where OfferRoleKey = @ApplicationRoleKey";
            return query;
        }
    }
}