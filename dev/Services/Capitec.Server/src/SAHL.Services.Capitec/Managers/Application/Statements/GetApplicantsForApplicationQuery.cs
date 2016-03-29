using SAHL.Core.Data;
using SAHL.Services.Capitec.Managers.Application.Models;
using System;
using SAHL.Services.Interfaces.Capitec.Common;

namespace SAHL.Services.Capitec.Managers.Application.Statements
{
    public class GetApplicantsForApplicationQuery : ISqlStatement<ApplicantModel>
    {
        public Guid ApplicationID { get; protected set; }
        public string IncomeContributorDeclarationText { get;protected set; }

        public GetApplicantsForApplicationQuery(Guid applicationID)
        {
            this.ApplicationID = applicationID;
            this.IncomeContributorDeclarationText = DeclarationDefinitions.IncomeContributor;
        }

        public string GetStatement()
        {
            return @"SELECT (p.FirstName + ' ' + p.Surname) AS Name, app.Id AS ID, 
                        CASE WHEN dd.DeclarationTypeEnumID = 'F54495A4-AAEE-4031-8099-A2D500AB5A75'
                        THEN 1 ELSE 0 END AS IncomeContributor
                        FROM [Capitec].dbo.ApplicationApplicant aa
                        JOIN [Capitec].dbo.Applicant app ON app.ID = aa.ApplicantId
                        JOIN [Capitec].dbo.Person p ON app.PersonID = p.ID
                        JOIN [Capitec].dbo.ApplicantDeclaration ad ON ad.ApplicantId = aa.ApplicantId
                        JOIN [Capitec].dbo.Declaration d ON d.ID = ad.DeclarationId
                        JOIN [Capitec].dbo.DeclarationDefinition dd ON d.DeclarationDefinitionId = dd.ID
                        WHERE aa.ApplicationId = @ApplicationID
                        AND dd.DeclarationText = @IncomeContributorDeclarationText";
        }
    }
}