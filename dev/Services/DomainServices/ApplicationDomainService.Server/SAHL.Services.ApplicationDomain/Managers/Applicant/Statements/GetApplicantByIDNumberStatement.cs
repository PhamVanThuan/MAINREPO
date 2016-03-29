using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.ApplicationDomain.Managers.Applicant.Statements
{
    public class GetApplicantByIDNumberStatement : ISqlStatement<LegalEntityDataModel>
    {
        public string IDNumber { get; protected set; }

        public GetApplicantByIDNumberStatement(string idnumber)
        {
            this.IDNumber = idnumber;
        }

        public string GetStatement()
        {
            var query = @"SELECT TOP 1 LE.* FROM [2am].dbo.LegalEntity LE WHERE IDNumber = @IDNumber";
            return query;
        }
    }
}