using SAHL.Core.Attributes;
using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    [NolockConventionExclude]
    public class GetDeclarationTypeEnumForApplicantDeclarationByDeclarationText : ISqlStatement<DeclarationTypeEnumDataModel>
    {
        public Guid ApplicantID { get; protected set; }

        public string DeclarationText { get; protected set; }

        public GetDeclarationTypeEnumForApplicantDeclarationByDeclarationText(Guid applicantID, string declarationText)
        {
            this.ApplicantID = applicantID;
            this.DeclarationText = declarationText;
        }

        public string GetStatement()
        {
            return @"SELECT
                        dte.ID, dte.Name, dte.IsActive
                    FROM
                        Capitec.dbo.ApplicantDeclaration ad (nolock)
                    JOIN
                        Capitec.dbo.Declaration d (nolock) ON ad.DeclarationId = d.ID
                    JOIN
                        Capitec.dbo.DeclarationDefinition dd (nolock) ON d.DeclarationDefinitionId = dd.ID
                    JOIN
                        Capitec.dbo.DeclarationTypeEnum dte (nolock) ON dd.DeclarationTypeEnumId = dte.ID
                    WHERE
                        dd.DeclarationText = @DeclarationText
                    AND
                        ad.ApplicantID = @ApplicantID
                    ORDER BY
                        d.DeclarationDate DESC";
        }
    }
}