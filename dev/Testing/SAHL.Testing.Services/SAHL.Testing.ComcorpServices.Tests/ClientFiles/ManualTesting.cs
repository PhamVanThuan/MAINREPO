using NUnit.Framework;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using SAHL.Testing.ComcorpServices.Tests.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAHL.Testing.ComcorpServices.Tests.ClientFiles
{
    [TestFixture]
    internal class ManualTesting : DocumentServiceTestBase<ImagingMainApplicantRequest>
    {
        private IEnumerable<GetComcorpApplicationsWithMultipleApplicantsQueryResult> _comcorpApplications;
        private Dictionary<int, string> _imagingReferences;

        [TestFixtureTearDown]
        public void Teardown()
        {
            if (_comcorpApplications != null)
            {
                foreach (var item in _comcorpApplications)
                {
                    Console.WriteLine("('{0}'),", item.ApplicationNumber);
                }
            }
        }

        [Test]
        [Ignore]
        public void _01_multiple_application_doc_requests()
        {
            Console.WriteLine(@"Test start time: {0} (Server time: {1})", DateTime.Now, base.testStartDate);
            var query = new GetComcorpApplicationsWithMultipleApplicantsQuery();
            base.PerformQuery(query);
            var _comcorpApplications = query.Result.Results.Take(10);
            //_comcorpApplications = (from t in testDataManager.GetComcorpApplicationsWithMultipleApplicants() where t.OfferKey == 1655268 select t);
            _imagingReferences = new Dictionary<int, string>();
            Parallel.ForEach<GetComcorpApplicationsWithMultipleApplicantsQueryResult>(_comcorpApplications, (comcorpApplication) =>
            {
                Console.WriteLine(@"Starting application request for application: {0}", comcorpApplication.ApplicationNumber);
                var applicationRequest = base.GenerateRequestFromTemplateXML<ImagingApplicationRequest>("Xml\\ClientFiles\\ImagingApplicationRequestTemplate.xml");

                applicationRequest.RequestHeader.ApplicationReference = comcorpApplication.Reference;
                applicationRequest.RequestHeader.BankReference = comcorpApplication.ApplicationNumber.ToString();
                applicationRequest.RequestHeader.RequestMessages = 2M;
                applicationRequest.RequestHeader.RequestDateTime = DateTime.Now;
                applicationRequest.RequestHeader.BankCustom = Guid.NewGuid().ToString();

                base.AddMacToRequest(applicationRequest);

                var applicationResponse = base.SubmitApplicationDocs(applicationRequest, "", false);
                _imagingReferences.Add(comcorpApplication.ApplicationNumber, applicationResponse.ReplyHeader.ImagingReference);
                Console.WriteLine(@"Completing application request for application: {0}. Status = {1}. Reference = {2}",
                    comcorpApplication.ApplicationNumber, applicationResponse.ReplyHeader.RequestStatus, applicationResponse.ReplyHeader.ImagingReference);
            });
        }

        [Test]
        [Ignore]
        public void _02_multiple_applicant_doc_requests()
        {
            Parallel.ForEach<GetComcorpApplicationsWithMultipleApplicantsQueryResult>(_comcorpApplications, (comcorpApplication) =>
                {
                    var getapplicantsQuery = new GetApplicantsDetailsQuery(comcorpApplication.ApplicationNumber);
                    base.PerformQuery(getapplicantsQuery);
                    var comcorpApplicants = getapplicantsQuery.Result.Results;
                    var applicantRequest = base.GenerateRequestFromTemplateXML<ImagingMainApplicantRequest>("Xml\\ClientFiles\\ImagingMainApplicantRequestTamplate.xml");
                    applicantRequest.RequestHeader.ApplicationReference = comcorpApplication.Reference;
                    applicantRequest.RequestHeader.BankReference = comcorpApplication.ApplicationNumber.ToString();
                    applicantRequest.RequestHeader.RequestMessages = 2M;
                    applicantRequest.RequestHeader.RequestDateTime = DateTime.Now;
                    applicantRequest.RequestHeader.BankCustom = Guid.NewGuid().ToString();
                    applicantRequest.RequestHeader.ImagingReference = (from a in _imagingReferences where a.Key == comcorpApplication.ApplicationNumber select a.Value).FirstOrDefault();

                    applicantRequest.SupportingDocuments.MainApplicantDocuments.ApplicantFirstName = comcorpApplicants.ElementAt(0).FirstNames;
                    applicantRequest.SupportingDocuments.MainApplicantDocuments.ApplicantSurname = comcorpApplicants.ElementAt(0).Surname;
                    applicantRequest.SupportingDocuments.MainApplicantDocuments.ApplicantIdentityNumber = comcorpApplicants.ElementAt(0).IDNumber;

                    if (comcorpApplicants.Count() > 1)
                    {
                        applicantRequest.SupportingDocuments.MainApplicantDocuments.SpouseDocuments.ApplicantFirstName = comcorpApplicants.ElementAt(1).FirstNames;
                        applicantRequest.SupportingDocuments.MainApplicantDocuments.SpouseDocuments.ApplicantSurname = comcorpApplicants.ElementAt(1).Surname;
                        applicantRequest.SupportingDocuments.MainApplicantDocuments.SpouseDocuments.ApplicantIdentityNumber = comcorpApplicants.ElementAt(1).IDNumber;
                    }
                    else
                    {
                        applicantRequest.SupportingDocuments.MainApplicantDocuments.SpouseDocuments = null;
                    }

                    base.AddMacToRequest(applicantRequest);
                    Console.WriteLine(@"Starting applicant request for application: {0}. Reference: {1}", comcorpApplication.ApplicationNumber, applicantRequest.RequestHeader.ImagingReference);
                    var mainApplicantResponse = base.SubmitMainApplicantDocs(applicantRequest, "", false);
                    Console.WriteLine(@"Completed applicant request for application: {0}. Status = {1}", comcorpApplication.ApplicationNumber, mainApplicantResponse.ReplyHeader.RequestStatus);
                });
        }
    }
}