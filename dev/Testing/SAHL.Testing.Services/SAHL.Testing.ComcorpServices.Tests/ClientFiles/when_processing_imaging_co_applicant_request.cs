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
    internal class when_processing_imaging_co_applicant_request : DocumentServiceTestBase<ImagingCoApplicantRequest>
    {
        private IEnumerable<LegalEntityDataModel> comcorpApplicants;
        private IEnumerable<DataDataModel> coApplicantDocuments;
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
                base.GenerateRequestFromTemplateXML("Xml\\ClientFiles\\ImagingCoApplicantRequestTemplate.xml");
                base.documentRequest.RequestHeader.ApplicationReference = comcorpApplication.Reference;
                base.documentRequest.RequestHeader.BankReference = comcorpApplication.ApplicationNumber.ToString();

                base.documentRequest.SupportingDocuments.CoApplicantDocuments.ApplicantFirstName = comcorpApplicants.ElementAt(0).FirstNames;
                base.documentRequest.SupportingDocuments.CoApplicantDocuments.ApplicantSurname = comcorpApplicants.ElementAt(0).Surname;
                base.documentRequest.SupportingDocuments.CoApplicantDocuments.ApplicantIdentityNumber = comcorpApplicants.ElementAt(0).IDNumber;

                if (comcorpApplicants.Count() > 1)
                {
                    base.documentRequest.SupportingDocuments.CoApplicantDocuments.SpouseDocuments.ApplicantFirstName = comcorpApplicants.ElementAt(1).FirstNames;
                    base.documentRequest.SupportingDocuments.CoApplicantDocuments.SpouseDocuments.ApplicantSurname = comcorpApplicants.ElementAt(1).Surname;
                    base.documentRequest.SupportingDocuments.CoApplicantDocuments.SpouseDocuments.ApplicantIdentityNumber = comcorpApplicants.ElementAt(1).IDNumber;
                }
                else
                {
                    base.documentRequest.SupportingDocuments.CoApplicantDocuments.SpouseDocuments = null;
                }

                base.AddMacToRequest();
            }
            else
            {
                Assert.Fail("Warning: Unable to find comcorpApplication");
            }

            var response = base.SubmitCoApplicantDocs(base.documentRequest, "", true);
            Assert.AreEqual(1M, response.ReplyHeader.RequestStatus);
            coApplicantDocuments = TestApiClient.Get<DataDataModel>(new { key1 = comcorpApplicants.ElementAt(0).IDNumber });
            Assert.AreEqual(base.documentRequest.SupportingDocuments.CoApplicantDocuments.SupportingDocument.Count(), coApplicantDocuments.Count(x => x.msgReceived > base.testStartDate));

            if (comcorpApplicants.Count() > 1)
            {
                spouseDocuments = TestApiClient.Get<DataDataModel>(new { key1 = comcorpApplicants.ElementAt(1).IDNumber });
                Assert.AreEqual(base.documentRequest.SupportingDocuments.CoApplicantDocuments.SpouseDocuments.SupportingDocument.Count(), spouseDocuments.Count(x => x.msgReceived > base.testStartDate));
            }
        }
    }
}