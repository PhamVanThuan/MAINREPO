using SAHL.Core.Attributes;
using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    [NolockConventionExclude]
    public class GetApplicantDeclarationByDeclarationText : ISqlStatement<ApplicantDeclarationDataModel>
    {
        public Guid ApplicantID { get; protected set; }

        public string DeclarationText { get; protected set; }

        public GetApplicantDeclarationByDeclarationText(Guid applicantID, string declarationText)
        {
            this.ApplicantID = applicantID;
            this.DeclarationText = declarationText;
        }

        public string GetStatement()
        {
            return @"SELECT
	                    ad.ID, ad.applicantId, ad.declarationId
                    FROM
	                    Capitec.dbo.ApplicantDeclaration ad (nolock)
                    JOIN
	                    Capitec.dbo.Declaration d (nolock) ON ad.DeclarationId = d.ID
                    JOIN
	                    Capitec.dbo.DeclarationDefinition dd (nolock) ON d.DeclarationDefinitionId = dd.ID
                    WHERE
	                    dd.DeclarationText = @DeclarationText
                    AND ad.ApplicantID = @ApplicantID";
        }
    }
}