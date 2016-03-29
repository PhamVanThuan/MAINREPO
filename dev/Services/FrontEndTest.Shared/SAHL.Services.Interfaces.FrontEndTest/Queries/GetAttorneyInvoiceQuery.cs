using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries
{
    public class GetAttorneyInvoiceQuery : ServiceQuery<GetAttorneyInvoiceQueryResult>, IFrontEndTestQuery, ISqlServiceQuery<GetAttorneyInvoiceQueryResult>
    {
        public GetAttorneyInvoiceQuery(string emailSubject)
        {
            this.EmailSubject = emailSubject;
        }

        public string EmailSubject { get; protected set; }

        }
}
