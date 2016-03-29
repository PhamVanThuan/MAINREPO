using SAHL.Core.Data;
using SAHL.Services.Interfaces.ApplicationDomain.Models;

namespace SAHL.Services.ApplicationDomain.Managers.Application.Statements
{
    public class GetExternalVendorForVendorCodeStatement : ISqlStatement<VendorModel>
    {
        public string VendorCode { get; protected set; }

        public GetExternalVendorForVendorCodeStatement(string vendorCode)
        {
            this.VendorCode = vendorCode;
        }

        public string GetStatement()
        {
            var sql = @"SELECT VendorKey, VendorCode, OrganisationStructureKey,
                            LegalEntityKey, GeneralStatusKey
                        FROM [2AM].[dbo].[Vendor]
                        WHERE VendorCode = @VendorCode";
            return sql;
        }
    }
}