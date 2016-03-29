using SAHL.Core.Data;
using SAHL.Services.EventProjection.Projections.CapitecApplicationDaily.Models;

namespace SAHL.Services.EventProjection.Projections.CapitecApplicationDaily.Statements
{
    public class GetConsultantLegalEntityDetailsStatement : ISqlStatement<ConsultantInfoDataModel>
    {
        public string ADUserName { get; protected set; }

        public GetConsultantLegalEntityDetailsStatement(string adUsername)
        {
            this.ADUserName = adUsername;
        }

        public string GetStatement()
        {
            return @"SELECT
	case
		when len(isnull(ltrim(rtrim(le.PreferredName)),'')) = 0
			then isnull(ltrim(rtrim(le.FirstNames))+' ', '') + isnull(ltrim(rtrim(le.Surname)), '')
			else isnull(ltrim(rtrim(le.PreferredName)),'')
		end as Name,
	case
		when len(isnull(ltrim(rtrim(le.WorkPhoneNumber)),'')) = 0
			then isnull(ltrim(rtrim(le.CellPhoneNumber)),'')
			else isnull(ltrim(rtrim(le.WorkPhoneCode)),'') + isnull(ltrim(rtrim(le.WorkPhoneNumber)),'')
		end as ContactNumber
FROM [2AM].dbo.ADUser au
JOIN [2AM].dbo.LegalEntity le ON au.LegalEntityKey = le.LegalEntityKey
WHERE ADUserName = @ADUserName";
        }
    }
}