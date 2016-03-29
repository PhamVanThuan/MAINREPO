using SAHL.Core.Services;
using SAHL.Services.Interfaces.Capitec.Models;
using SAHL.Services.Interfaces.Capitec.Queries;

namespace SAHL.Services.Capitec.QueryStatements
{
    public class GetApplicationByIdentityNumberQueryStatement : IServiceQuerySqlStatement<GetApplicationByIdentityNumberQuery, GetApplicationByIdentityNumberQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT        
                        TOP 1 [Application].ApplicationNumber
                    FROM
                        [Capitec].[dbo].Person 
                    JOIN
                        [Capitec].[dbo].Applicant 
                    ON 
                        Applicant.PersonID = Person.Id
                    JOIN 
                        [Capitec].[dbo].[ApplicationApplicant]
                    ON
                         [ApplicationApplicant].ApplicantId = Applicant.Id
                    JOIN 
                        [Capitec].[dbo].[Application]
                    ON 
                        [Application].Id = [ApplicationApplicant].ApplicationId 
                    JOIN 
                        [Capitec].[dbo].ApplicationStatusEnum 
                    ON 
                        [Application].ApplicationStatusEnumId = ApplicationStatusEnum.Id 

                    WHERE
                        Person.IdentityNumber = @IdentityNumber
                    AND
                        ApplicationStatusEnum.Name = @StatusTypeName
                    ORDER BY 
                        [Application].Id DESC";
        }
    }
}