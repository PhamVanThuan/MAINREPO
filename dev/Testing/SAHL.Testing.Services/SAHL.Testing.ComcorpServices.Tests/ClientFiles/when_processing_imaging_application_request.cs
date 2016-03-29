using NUnit.Framework;
using SAHL.Core.Data.Models.FETest;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using SAHL.Testing.ComcorpServices.Tests.Connector;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Testing.ComcorpServices.Tests.ClientFiles
{
    internal class when_processing_imaging_application_request : DocumentServiceTestBase<ImagingApplicationRequest>
    {
        [Test]
        [Ignore]
        public void should_send_request_successful_status_and_create_client_files_records()
        {
            var query = new GetComcorpApplicationsWithMultipleApplicantsQuery();
            base.PerformQuery(query);
            var application = query.Result.Results.FirstOrDefault();

            if (application == null)
                Assert.Fail("Warning: Unable to find comcorpApplication");

            var getapplicantsQuery = new GetApplicantsDetailsQuery(application.ApplicationNumber);
            base.PerformQuery(getapplicantsQuery);
            var applicants = getapplicantsQuery.Result.Results;
            var firstApplicant = applicants.OrderBy(x => x.LegalEntityKey).First();

            base.GenerateRequestFromTemplateXML("Xml\\ClientFiles\\ImagingApplicationRequestTemplate.xml");
            documentRequest.RequestHeader.ApplicationReference = application.Reference;
            documentRequest.RequestHeader.BankReference = application.ApplicationNumber.ToString();
            base.AddMacToRequest();
            var response = base.SubmitApplicationDocs(documentRequest, "", true);
            Assert.AreEqual(1M, response.ReplyHeader.RequestStatus);
            IEnumerable<DataDataModel> applicantDocuments = TestApiClient.Get<DataDataModel>(new { key1 = firstApplicant.IDNumber });
            Assert.AreEqual(base.documentRequest.SupportingDocuments.ApplicationDocuments.Count(), applicantDocuments.Count(x => x.msgReceived > base.testStartDate));
        }
    }
}