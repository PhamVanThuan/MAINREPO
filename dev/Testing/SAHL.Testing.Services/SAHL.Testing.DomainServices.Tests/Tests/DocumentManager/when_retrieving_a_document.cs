using NUnit.Framework;
using SAHL.Services.Interfaces.DocumentManager;
using SAHL.Services.Interfaces.DocumentManager.Models;
using SAHL.Services.Interfaces.DocumentManager.Queries;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Testing.Services.Tests.DocumentManager
{
    public class when_retrieving_a_document : ServiceTestBase<IDocumentManagerServiceClient>
    {
        [Test]
        public void when_successful()
        {
            var getImageIndexDataQueryResultSet = new GetFirstThirdPartyInvoiceWhereFileExistsQuery();
            PerformQuery(getImageIndexDataQueryResultSet);
            string guid = getImageIndexDataQueryResultSet.Result.Results.First().ToString();
            var getDocumentFromStorByGuidQuery = new GetDocumentFromStorByDocumentGuidQuery(DocumentStorEnum.LossControlAttorneyInvoices, Guid.Parse(guid));
            Execute(getDocumentFromStorByGuidQuery);
            Assert.That(getDocumentFromStorByGuidQuery.Result.Results.First().FileContentAsBase64.Length > 0);
        }

        [Test]
        public void when_unsuccessful()
        {
            var guid = Guid.NewGuid();
            var query = new GetDocumentFromStorByDocumentGuidQuery(DocumentStorEnum.LossControlAttorneyInvoices, guid);
            Execute(query).AndExpectThatErrorMessagesContain("The requested document does not exist.");
            Assert.IsNullOrEmpty(query.Result.Results.FirstOrDefault().FileContentAsBase64);
        }
    }
}