using System.Linq;
using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.FinanceDomain;
using SAHL.Services.Interfaces.FinanceDomain.Queries;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Testing.Common.Models;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Testing.Services.Tests.Tests.FinanceDomain
{
    [TestFixture]
    public class when_querying_for_attorney_invoices_monhtly_break_down : ServiceTestBase<IFinanceDomainServiceClient>
    {
        [Test]
        public void when_successful()
        {
            var command = new GetAttorneyInvoiceMonthlyBreakDownQuery();
            base.Execute(command).WithoutErrors();
        }
    }
}