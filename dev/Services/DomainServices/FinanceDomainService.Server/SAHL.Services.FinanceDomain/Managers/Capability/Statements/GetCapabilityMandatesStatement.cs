using SAHL.Core.Data;

namespace SAHL.Services.FinanceDomain.Managers.Capability.Statements
{
    public class GetCapabilityMandatesStatement : ISqlStatement<ApprovalMandateRanges>
    {
        public string GetStatement()
        {
            return @"SELECT 
                        CM.StartRange AS LowerBound,
                        CM.EndRange AS UpperBound,
                        C.Description AS Capability
                    FROM
                        [2AM].OrgStruct.CapabilityMandate CM
                    JOIN [2AM].OrgStruct.Capability C ON C.CapabilityKey = CM.CapabilityKey
                    WHERE CM.MandateTypeKey = 1";
        }
    }
}
