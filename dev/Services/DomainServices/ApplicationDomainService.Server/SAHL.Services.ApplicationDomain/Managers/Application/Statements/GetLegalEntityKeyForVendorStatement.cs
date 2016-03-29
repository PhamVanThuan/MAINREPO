using SAHL.Core.Data;
using SAHL.Services.Interfaces.ApplicationDomain.Models;

namespace SAHL.Services.ApplicationDomain.Managers.Statements
{
    public class GetLegalEntityKeyForVendorStatement : ISqlStatement<VendorModel>
    {
        public string VendorCode { get; protected set; }

        public GetLegalEntityKeyForVendorStatement(string vendorCode)
        {
            this.VendorCode = vendorCode;
        }

        public string GetStatement()
        {
            var sql = @"SELECT
                            VendorKey,
                            VendorCode,
                            OrganisationStructureKey,
                            LegalEntityKey,
                            GeneralStatusKey
                        FROM [2AM].[dbo].[Vendor]
                        WHERE VendorCode = @VendorCode and GeneralStatusKey = 1";
            return sql;
        }
    }
}