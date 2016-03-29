using SAHL.Core.Data;
using System;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Managers.Application.Statements
{
    public class DoesSuppliedVendorExistStatement : ISqlStatement<int>
    {
        public string VendorCode { get; set; }

        public DoesSuppliedVendorExistStatement(string vendorCode)
        {
            this.VendorCode = vendorCode;
        }

        public string GetStatement()
        {
            var sql = @"SELECT 
                            Count([VendorKey]) as Total
                        FROM
                            [2AM].[dbo].[Vendor]
                        WHERE
                           VendorCode = @VendorCode
                        AND 
                           GeneralStatusKey = 1";
            return sql;
        }
    }
}