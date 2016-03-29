using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.FETest;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using System;
using System.Linq;

namespace SAHL.Testing.Services.Tests.ApplicationDomain
{
    public class when_adding_applicant_declarations : ServiceTestBase<IApplicationDomainServiceClient>
    {
        private ActiveNewBusinessApplicantsDataModel _newBusinessApplicant;

        [SetUp]
        public void Setup()
        {
            _newBusinessApplicant = TestApiClient.Get<ActiveNewBusinessApplicantsDataModel>(new { hasdeclarations = 0 }, limit: 1).First();
        }

        [TearDown]
        new public void OnTestTearDown()
        {
            if (_newBusinessApplicant != null)
            {
                var command = new RemoveApplicantFromActiveNewBusinessApplicantsCommand(_newBusinessApplicant.OfferRoleKey);
                base.PerformCommand(command);
                _newBusinessApplicant = null;
            }
            base.OnTestTearDown();
        }

        [Test]
        public void when_successful()
        {
            var expectedApplicantDeclarations = new ApplicantDeclarationsModel(_newBusinessApplicant.LegalEntityKey, _newBusinessApplicant.OfferKey, new DateTime(2014, 09, 12),
                new DeclaredInsolventDeclarationModel(OfferDeclarationAnswer.Yes, DateTime.Now.AddYears(-2)),
                new UnderAdministrationOrderDeclarationModel(OfferDeclarationAnswer.Yes, DateTime.Now.AddYears(-2)),
                new CurrentlyUnderDebtCounsellingReviewDeclarationModel(OfferDeclarationAnswer.Yes, true),
                new PermissionToConductCreditCheckDeclarationModel(OfferDeclarationAnswer.Yes));
            var command = new AddApplicantDeclarationsCommand(expectedApplicantDeclarations);
            base.Execute<AddApplicantDeclarationsCommand>(command);
            var declarations = TestApiClient.Get<OfferDeclarationDataModel>(new { offerrolekey = _newBusinessApplicant.OfferRoleKey });
            Assert.AreEqual(7, declarations.Count());
        }

        [Test]
        public void when_unsuccessful()
        {
            var expectedApplicantDeclarations = new ApplicantDeclarationsModel(_newBusinessApplicant.LegalEntityKey, _newBusinessApplicant.OfferKey, DateTime.Now,
                new DeclaredInsolventDeclarationModel(OfferDeclarationAnswer.Yes, null),
                new UnderAdministrationOrderDeclarationModel(OfferDeclarationAnswer.No, null),
                new CurrentlyUnderDebtCounsellingReviewDeclarationModel(OfferDeclarationAnswer.No, null),
                new PermissionToConductCreditCheckDeclarationModel(OfferDeclarationAnswer.Yes));
            var command = new AddApplicantDeclarationsCommand(expectedApplicantDeclarations);
            base.Execute<AddApplicantDeclarationsCommand>(command)
                .AndExpectThatErrorMessagesContain("Client answered 'Yes' to the 'Have you ever been declared insolvent?' question. A date rehabilitated answer is required.");
            var _declarations = TestApiClient.Get<OfferDeclarationDataModel>(new { offerrolekey = _newBusinessApplicant.OfferRoleKey });
            Assert.AreEqual(0, _declarations.Count());
        }
    }
}