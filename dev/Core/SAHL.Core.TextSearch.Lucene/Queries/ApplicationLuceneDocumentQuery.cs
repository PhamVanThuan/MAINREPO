using SAHL.Core.Data;
using SAHL.Core.TextSearch.Lucene.Models;

namespace SAHL.Core.TextSearch.Lucene.Queries
{
    public class ApplicationLuceneDocumentQuery : ISqlStatement<ApplicationLuceneDocumentModel>
    {
        public string GetStatement()
        {
            return @"select ap.ApplicationNumber,
                        [Capitec].[dbo].[GetIdentityNumberListForApplication](ap.Id) IdentityNumberList,
                        ap.ApplicationDate,
                        sta.[Name] ApplicationStage,
                        stu.[Name] ApplicationStatus,
                        [Capitec].[dbo].[CapitecApplicantsToJson](ap.ApplicationNumber) ApplicantsJson,
                        ConsultantName,
                        ConsultantContactNumber
                    from [Capitec].[dbo].[Application] ap (nolock)
                    join (select distinct apa.ApplicationId
                            from [Capitec].[dbo].[ApplicationApplicant] apa (nolock)
                            join [Capitec].[dbo].[Applicant] apl (nolock) on apl.Id = apa.ApplicantId
                            join [Capitec].[dbo].[Person] p (nolock) on p.Id = apl.PersonID) apl on apl.ApplicationId = ap.Id
                    join [Capitec].[dbo].[ApplicationStatusEnum] stu (nolock) on stu.Id = ap.ApplicationStatusEnumId
                    join [Capitec].[dbo].[ApplicationStageTypeEnum] sta (nolock) on sta.Id = ap.ApplicationStageTypeEnumId
                    where ap.ApplicationNumber IN (select ApplicationNumber from [staging].[ApplicationIndex] (nolock))";
        }
    }
}