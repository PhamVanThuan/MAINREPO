using SAHL.Core.Data;
using SAHL.Services.Interfaces.DocumentManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.DocumentManager.Managers.Document.Statements
{
    public class DeleteAttorneyInvoiceDataStatement : ISqlStatement<int>
    {

        public int AttorneyIvoiceKey { get; protected set; }

        public DeleteAttorneyInvoiceDataStatement(int attorneyInvoiceKey)
        {
            this.AttorneyIvoiceKey = attorneyInvoiceKey;
        }
        public string GetStatement()
        {
            return @"DELETE FROM [ImageIndex].[dbo].[Data]
                    WHERE STOR = 44 AND Key2 = @AttorneyIvoiceKey";
        }
    }
}
