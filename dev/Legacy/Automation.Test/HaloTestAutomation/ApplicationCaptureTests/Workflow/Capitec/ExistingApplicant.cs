using System.Linq;
using Automation.Framework;
using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using BuildingBlocks.Services.Contracts;
using Automation.DataModels;
using System;
using System.Xml.Linq;
using SAHL.Services.Capitec.Models.Shared;
using System.Collections.Generic;
using ApplicationCaptureTests.Workflow.Capitec;

namespace ApplicationCaptureTests.Workflow.Capitec
{
    [RequiresSTA]
    public class ExistingApplicant : CapitecBase
    {
        /// <summary>
        /// http://sahl-fb01/fogbugz/default.asp?W174
        /// Asserts that when creating applications for existing clients, the application should be linked to a new application and the client information should be updated.
        /// </summary>
        [Test]
        public void ExistingApplicant_WhenCreatingNewApplication_ShouldLinkAndUpdateExistingClient()
        {
            //Setup Test Pack
            var streetName_insert = String.Format("street name {0}", randomizer.Next());
            var streetName_update = String.Format("street name update {0}", randomizer.Next());
            var idnumber = IDNumbers.GetNextIDNumber();
            base.CreateApplication(MortgageLoanPurposeEnum.Newpurchase,
                                    includeITC: true,
                                    isITCLinkedToLegalEntity: false,
                                    streetName: streetName_insert,
                                    idNumber: idnumber,
                                    isMainContact: true,
                                    firstNames: "ExistingClient");
            var results = base.Assertable.Copy();

            //Assert Precondition
            base.AssertApplication();

            //Execute Test
            base.CreateApplication(MortgageLoanPurposeEnum.Newpurchase,
                                    includeITC: true,
                                    isITCLinkedToLegalEntity: true,
                                    emailAddress: "capitecAppUpdate@gmail.com",
                                    buildingName: "building name updated",
                                    streetName: streetName_update,
                                    idNumber: idnumber,
                                    employmentType: EmploymentTypeEnum.SelfEmployed,
                                    grossIncome: 56000M,
                                    isMainContact: true,
                                    cellPhoneNumber: "0791234567",
                                    workPhoneNumber: "0217654321",
                                    homePhoneNumber: "0111234567",
                                    firstNames: "ExistingClient");

            //Assert Links
            Assert.AreEqual(results.ExpectedLegalEntityKey, base.Assertable.ExpectedLegalEntityKey, "Not the same legalentity");//make sure same client.
            Assert.AreNotEqual(results.ExpectedLegalEntityAddressKey, base.Assertable.ExpectedLegalEntityAddressKey, "legal entity address link is the same");//make sure not same address link.
            Assert.AreNotEqual(results.ExpectedAddressKey, base.Assertable.ExpectedAddressKey, "The address is the same");//make sure not same address.
            Assert.AreNotEqual(results.ExpectedEmploymentKey, base.Assertable.ExpectedEmploymentKey, "employment is the same");//make sure new client employment record.

            //Assert
            base.AssertApplication();//assert that information has changed
        }

        [Test]
        public void ExistingApplicant_WhenClientNotOpenAccountAndClientSurnameInvalid_ShouldUpdateWithNewClientInformation()
        {
            //Setup Test Pack
            var legalentity = Service<ILegalEntityService>().GetDirtyNaturalPeopleOnAccounts(GeneralStatusEnum.Active)
                                                    .Where(x => String.IsNullOrEmpty(x.Surname)).FirstOrDefault();
            var expectedSurname = "UpdatedSurname";
            var expectedFirstNames = "ExistingClientInvalidSurname";
            var expectedSalutationType = SalutationTypeEnum.Prof;
            var expectedDOB = DateTime.Parse("1986-06-18");

            //Assert Precondition

            //Execute Test
            base.CreateApplication(MortgageLoanPurposeEnum.Switchloan, false,
                                        idNumber: legalentity.IdNumber,
                                        isMainContact: true,
                                        firstNames: expectedFirstNames,
                                        isDirtyLegalEntity:true,
                                        surname: expectedSurname,
                                        salutation: expectedSalutationType,
                                        dateOfBirth: expectedDOB
                                    );

            //Assert
            base.AssertApplication();
        }

        [Test]
        public void ExistingApplicant_WhenClientNotOpenAccountAndClientFirstNamesInvalid_ShouldUpdateWithNewClientInformation()
        {
            //Setup Test Pack
            var legalentity = Service<ILegalEntityService>().GetDirtyNaturalPeopleOnAccounts(GeneralStatusEnum.Active)
                                                    .Where(x =>String.IsNullOrEmpty(x.FirstNames)).FirstOrDefault();
            var expectedSurname = "UpdatedSurname";
            var expectedFirstNames = "ExistingClientInvalidFirstNames";
            var expectedSalutationType = SalutationTypeEnum.Sir;
            var expectedDOB = DateTime.Parse("1986-06-18");

            //Assert Precondition

            //Execute Test
            base.CreateApplication(MortgageLoanPurposeEnum.Switchloan, false,
                                         idNumber: legalentity.IdNumber,
                                         isMainContact: true,
                                         isDirtyLegalEntity: true,
                                         firstNames: expectedFirstNames,
                                         surname: expectedSurname,
                                         salutation: expectedSalutationType,
                                         dateOfBirth: expectedDOB
                                     );

            //Assert
            base.AssertApplication();
        }

        [Test]
        public void ExistingApplicant_WhenClientNotOpenAccountAndClientSalutationInvalid_ShouldUpdateWithNewClientInformation()
        {
            //Setup Test Pack
            var legalentity = Service<ILegalEntityService>().GetDirtyNaturalPeopleOnAccounts(GeneralStatusEnum.Active)
                                                    .Where(x => x.SalutationKey == null).FirstOrDefault();
            var expectedSurname = "UpdatedSurname";
            var expectedFirstNames = "ExistingClientInvalidSalutation";
            var expectedSalutationType = SalutationTypeEnum.Past;
            var expectedDOB = DateTime.Parse("1986-06-18");

            //Assert Precondition

            //Execute Test
            base.CreateApplication(MortgageLoanPurposeEnum.Switchloan, false,
                                        idNumber: legalentity.IdNumber,
                                        isMainContact: true,
                                        isDirtyLegalEntity: true,
                                        firstNames: expectedFirstNames,
                                        surname: expectedSurname,
                                        salutation: expectedSalutationType,
                                        dateOfBirth: expectedDOB
                                    );
            //Assert
            base.AssertApplication();
        }

        [Test]
        public void ExistingApplicant_WhenClientNotOpenAccountAndClientDataOfBirthInvalid_ShouldUpdateWithNewClientInformation()
        {
            //Setup Test Pack
            var legalentity = Service<ILegalEntityService>().GetDirtyNaturalPeopleOnAccounts(GeneralStatusEnum.Active)
                                                    .Where(x =>  x.DateOfBirth == null).FirstOrDefault();
            var expectedSurname = "UpdatedSurname";
            var expectedFirstNames = "ExistingClientInvalidDataOfBirth";
            var expectedSalutationType = SalutationTypeEnum.Dr;
            var expectedDOB = DateTime.Parse("1986-06-18");

            //Assert Precondition

            //Execute Test
            base.CreateApplication(MortgageLoanPurposeEnum.Switchloan, false,
                                         idNumber: legalentity.IdNumber,
                                         isMainContact: true,
                                         isDirtyLegalEntity: true,
                                         firstNames: expectedFirstNames,
                                         surname: expectedSurname,
                                         salutation: expectedSalutationType,
                                         dateOfBirth: expectedDOB
                                     );
            //Assert
            base.AssertApplication();
        }

        [Test]
        public void ExistingApplicant_WhenClientHasOpenAccountAndClientDataOfBirthInvalid_ShouldNotUpdateClient()
        {
            //Setup Test Pack
            var legalentity = Service<ILegalEntityService>().GetDirtyNaturalPeopleOnAccounts(GeneralStatusEnum.Inactive)
                                                    .Where(x => x.DateOfBirth == null).FirstOrDefault();
            var expectedSurname = "UpdatedSurname";
            var expectedFirstNames = "ExistingClientInvalidDataOfBirth";
            var expectedSalutationType = SalutationTypeEnum.Lord;
            var expectedDOB = DateTime.Parse("1986-06-18");

            //Assert Precondition

            //Execute Test
            base.CreateApplication(MortgageLoanPurposeEnum.Switchloan, false,
                                         idNumber: legalentity.IdNumber,
                                         isMainContact: true,
                                         isDirtyLegalEntity: true,
                                         firstNames: expectedFirstNames,
                                         surname: expectedSurname,
                                         salutation: expectedSalutationType,
                                         dateOfBirth: expectedDOB,
                                         checkDirtyLegalEntityNotUpdated: true
                                     );
            //Assert
            base.AssertApplication();
        }
    }
}