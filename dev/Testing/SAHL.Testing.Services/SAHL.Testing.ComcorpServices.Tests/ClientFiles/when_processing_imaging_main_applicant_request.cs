using NUnit.Framework;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.FETest;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using SAHL.Testing.ComcorpServices.Tests.Connector;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Testing.ComcorpServices.Tests.ClientFiles
{
    internal class when_processing_imaging_main_applicant_request : DocumentServiceTestBase<ImagingMainApplicantRequest>
    {
        private IEnumerable<LegalEntityDataModel> comcorpApplicants;
        private IEnumerable<DataDataModel> mainApplicantDocuments;
        private IEnumerable<DataDataModel> spouseDocuments;

        [Test]
        [Ignore]
        public void should_send_request_successful_status_and_create_client_files_records()
        {
            var query = new GetComcorpApplicationsWithMultipleApplicantsQuery();
            base.PerformQuery(query);
            var comcorpApplication = query.Result.Results.FirstOrDefault();

            if (comcorpApplication != null)
            {
                var getapplicantsQuery = new GetApplicantsDetailsQuery(comcorpApplication.ApplicationNumber);
                base.PerformQuery(getapplicantsQuery);
                comcorpApplicants = getapplicantsQuery.Result.Results;
                base.GenerateRequestFromTemplateXML("Xml\\ClientFiles\\ImagingMainApplicantRequestTamplate.xml");
                base.documentRequest.RequestHeader.ApplicationReference = comcorpApplication.Reference;
                base.documentRequest.RequestHeader.BankReference = comcorpApplication.ApplicationNumber.ToString();

                base.documentRequest.SupportingDocuments.MainApplicantDocuments.ApplicantFirstName = comcorpApplicants.ElementAt(0).FirstNames;
                base.documentRequest.SupportingDocuments.MainApplicantDocuments.ApplicantSurname = comcorpApplicants.ElementAt(0).Surname;
                base.documentRequest.SupportingDocuments.MainApplicantDocuments.ApplicantIdentityNumber = comcorpApplicants.ElementAt(0).IDNumber;

                if (comcorpApplicants.Count() > 1)
                {
                    base.documentRequest.SupportingDocuments.MainApplicantDocuments.SpouseDocuments.ApplicantFirstName = comcorpApplicants.ElementAt(1).FirstNames;
                    base.documentRequest.SupportingDocuments.MainApplicantDocuments.SpouseDocuments.ApplicantSurname = comcorpApplicants.ElementAt(1).Surname;
                    base.documentRequest.SupportingDocuments.MainApplicantDocuments.SpouseDocuments.ApplicantIdentityNumber = comcorpApplicants.ElementAt(1).IDNumber;
                }
                else
                {
                    base.documentRequest.SupportingDocuments.MainApplicantDocuments.SpouseDocuments = null;
                }

                base.AddMacToRequest();
            }
            else
            {
                Assert.Fail("Warning: Unable to find comcorpApplication");
            }

            var response = base.SubmitMainApplicantDocs(documentRequest, "", true);
            Assert.AreEqual(1M, response.ReplyHeader.RequestStatus);

            mainApplicantDocuments = TestApiClient.Get<DataDataModel>(new { key1 = comcorpApplicants.ElementAt(0).IDNumber });
            Assert.AreEqual(base.documentRequest.SupportingDocuments.MainApplicantDocuments.SupportingDocument.Count(), mainApplicantDocuments.Count(x => x.msgReceived > base.testStartDate));

            if (comcorpApplicants.Count() > 1)
            {
                spouseDocuments = TestApiClient.Get<DataDataModel>(new { key1 = comcorpApplicants.ElementAt(0).IDNumber });
                Assert.AreEqual(base.documentRequest.SupportingDocuments.MainApplicantDocuments.SpouseDocuments.SupportingDocument.Count(), spouseDocuments.Count(x => x.msgReceived > base.testStartDate));
            }
        }
    }
}