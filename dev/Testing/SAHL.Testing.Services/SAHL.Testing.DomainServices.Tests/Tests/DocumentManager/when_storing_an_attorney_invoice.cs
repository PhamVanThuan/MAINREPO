using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.DocumentManager;
using SAHL.Services.Interfaces.DocumentManager.Commands;
using SAHL.Services.Interfaces.DocumentManager.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.IO;
using System.Linq;

namespace SAHL.Testing.Services.Tests.DocumentManager
{
    public class when_storing_an_attorney_invoice : ServiceTestBase<IDocumentManagerServiceClient>
    {
        [Test]
        public void when_successful1()
        {
            Random rnd = new Random();
            var randomEmailSubjectToken = rnd.Next(1, 1000000);
            var emailSubject = "NewTest";
            var invoice = TestApiClient.Get<ThirdPartyInvoiceDataModel>(new { invoicestatuskey = (int)InvoiceStatus.Received }).First();
            var image = Convert.ToBase64String(File.ReadAllBytes("Tests\\DocumentManager\\SampleTiff\\ComcorpSampleDoc.tiff"));
            var document = new ThirdPartyInvoiceDocumentModel(invoice.AccountKey.ToString(), DateTime.Now, DateTime.Now,
                "vishavp@sahomeloans.com", emailSubject += randomEmailSubjectToken.ToString(), "NewFileName", "TIFF", "SomeOtherThirdCategory", image);
            var command = new StoreAttorneyInvoiceCommand(document, invoice.ThirdPartyInvoiceKey.ToString());

            Execute(command).WithoutErrors();
            var query = new GetAttorneyInvoiceQuery(emailSubject);
            PerformQuery(query);
            var queryResult = query.Result.Results.FirstOrDefault();

            Assert.That(queryResult.Category == document.Category
            && queryResult.FromEmailAddress == document.FromEmailAddress
            && queryResult.EmailSubject == document.EmailSubject
            && queryResult.LoanNumber == document.LoanNumber
            && queryResult.Extension == document.InvoiceFileExtension
            && queryResult.InvoiceFileName == document.InvoiceFileName
            && queryResult.ThirdPartyInvoiceKey.Length > 0);
        }
    }
}