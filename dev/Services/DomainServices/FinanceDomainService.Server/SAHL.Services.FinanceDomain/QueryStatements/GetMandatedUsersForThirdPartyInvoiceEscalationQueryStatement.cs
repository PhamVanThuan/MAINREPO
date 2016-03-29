using SAHL.Core.Services;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using SAHL.Services.Interfaces.FinanceDomain.Queries;

namespace SAHL.Services.FinanceDomain.QueryStatements
{
    public class GetMandatedUsersForThirdPartyInvoiceEscalationQueryStatement :
        IServiceQuerySqlStatement<GetMandatedUsersForThirdPartyInvoiceEscalationQuery, GetMandatedUsersForThirdPartyInvoiceEscalationQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT 
                        AD.ADUserName AS UserName, 
                        UOS.UserOrganisationStructureKey, 
                        LE.FirstNames + ' ' + LE.Surname AS FullName,
                        C.description AS 'Description',
                        le.FirstNames + ' ' + le.Surname + ' ( ' + SUBSTRING(c.Description, CHARINDEX('Approver', c.Description), LEN(c.Description)) + ' ) ' as ApproverDescription
                    FROM [2am].OrgStruct.CapabilityMandate CM
                        JOIN [2am].OrgStruct.Capability C 
                            ON CM.CapabilityKey = C.CapabilityKey
                        JOIN [2am].OrgStruct.UserOrganisationStructureCapability UOSC 
                            ON C.CapabilityKey = UOSC.CapabilityKey
                        JOIN [2am].dbo.UserOrganisationStructure UOS 
                            ON UOSC.UserOrganisationStructureKey = UOS.UserOrganisationStructureKey
                        JOIN [2am].dbo.AdUser AD 
                            ON UOS.ADUserKey = AD.ADUserKey
                        JOIN [2am].dbo.LegalEntity LE 
                            ON AD.LegalEntityKey = LE.LegalEntityKey
                        LEFT JOIN [2am].dbo.ThirdPartyInvoice TPI 
                            ON TPI.ThirdPartyInvoiceKey = @ThirdPartyInvoiceKey
                    WHERE 
                        endRange > TPI.TotalAmountIncludingVAT
                    ORDER BY EndRange ASC";
        }
    }
}