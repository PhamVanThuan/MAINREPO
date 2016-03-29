using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

using System;

namespace BuildingBlocks.Assertions
{
    public static class MaintenanceViewAssertions
    {
        private static ILegalEntityOrgStructureService leosService;
        private static ILegalEntityService leService;

        static MaintenanceViewAssertions()
        {
            leosService = ServiceLocator.Instance.GetService<ILegalEntityOrgStructureService>();
            leService = ServiceLocator.Instance.GetService<ILegalEntityService>();
        }

        public static void AssertLegalEntityOrganisationStructure(TestBrowser browser, Automation.DataModels.LegalEntityOrganisationStructure expectedLeos)
        {
            var onViewLeos = browser.Page<MaintenanceLegalEntity>().GetLegalEntityDetail();

            if (expectedLeos.LegalEntity.LegalEntityTypeKey == Common.Enums.LegalEntityTypeEnum.Company)
            {
                if (expectedLeos.OrganisationStructure.OrganisationTypeDescription == OrganisationType.Department
                    || expectedLeos.OrganisationStructure.OrganisationTypeDescription.Contains("Branch"))
                    StringAssert.AreEqualIgnoringCase(expectedLeos.LegalEntity.RegistrationNumber, onViewLeos.LegalEntity.RegistrationNumber, "Registration Number was not added/updated");
                StringAssert.AreEqualIgnoringCase(expectedLeos.LegalEntity.RegisteredName, onViewLeos.LegalEntity.RegisteredName, "Company Name was not added/updated");
            }
            else
            {
                StringAssert.AreEqualIgnoringCase(expectedLeos.LegalEntity.FirstNames, onViewLeos.LegalEntity.FirstNames, "FirstNames was not added/updated");
                StringAssert.AreEqualIgnoringCase(expectedLeos.LegalEntity.Surname, onViewLeos.LegalEntity.Surname, "Surname was not added/updated");
                StringAssert.AreEqualIgnoringCase(expectedLeos.LegalEntity.SalutationDescription, onViewLeos.LegalEntity.SalutationDescription, "Salutation was not added/updated");
                StringAssert.AreEqualIgnoringCase(expectedLeos.LegalEntity.Initials, onViewLeos.LegalEntity.Initials, "Initials was not added/updated");
                StringAssert.AreEqualIgnoringCase(expectedLeos.LegalEntity.IdNumber, onViewLeos.LegalEntity.IdNumber, "IdNumber was not added/updated");
                StringAssert.AreEqualIgnoringCase(expectedLeos.LegalEntity.PreferredName, onViewLeos.LegalEntity.PreferredName, "PreferredName was not added/updated");
                StringAssert.AreEqualIgnoringCase(expectedLeos.LegalEntity.GenderDescription, onViewLeos.LegalEntity.GenderDescription, "Gender was not added/updated");
                StringAssert.AreEqualIgnoringCase(expectedLeos.LegalEntity.LegalEntityStatusDescription, onViewLeos.LegalEntity.LegalEntityStatusDescription, "Gender was not added/updated");
            }
            StringAssert.AreEqualIgnoringCase(expectedLeos.LegalEntity.CellPhoneNumber, onViewLeos.LegalEntity.CellPhoneNumber, "Cellphone was not updated");
            StringAssert.AreEqualIgnoringCase(expectedLeos.LegalEntity.EmailAddress, onViewLeos.LegalEntity.EmailAddress, "Cellphone was not updated");
            if (expectedLeos.LegalEntity.HomePhoneCode != null && onViewLeos.LegalEntity.HomePhoneCode != null)
            {
                var expectedhome = String.Format("({0}) {1}", expectedLeos.LegalEntity.HomePhoneCode, expectedLeos.LegalEntity.HomePhoneNumber);
                var onViewhome = String.Format("({0}) {1}", onViewLeos.LegalEntity.HomePhoneCode, onViewLeos.LegalEntity.HomePhoneNumber);
                StringAssert.AreEqualIgnoringCase(expectedhome, onViewhome, "Home Phone Number was not updated");
            }
            if (expectedLeos.LegalEntity.WorkPhoneCode != null && onViewLeos.LegalEntity.WorkPhoneCode != null)
            {
                var expectedwork = String.Format("({0}) {1}", expectedLeos.LegalEntity.WorkPhoneCode, expectedLeos.LegalEntity.WorkPhoneNumber);
                var onViewwork = String.Format("({0}) {1}", onViewLeos.LegalEntity.WorkPhoneCode, onViewLeos.LegalEntity.WorkPhoneNumber);
                StringAssert.AreEqualIgnoringCase(expectedwork, onViewwork, "Work Phone Number was not updated");
            }
            if (expectedLeos.LegalEntity.FaxCode != null && onViewLeos.LegalEntity.FaxCode != null)
            {
                var expectedFax = String.Format("({0}) {1}", expectedLeos.LegalEntity.FaxCode, expectedLeos.LegalEntity.FaxNumber);
                var onViewFax = String.Format("({0}) {1}", onViewLeos.LegalEntity.FaxCode, onViewLeos.LegalEntity.FaxNumber);
                StringAssert.AreEqualIgnoringCase(expectedFax, onViewFax, "Fax r was not updated");
            }
        }

        public static void AssertLegalEntityOrganisationStructure(string registration_or_idNumber, LegalEntityTypeEnum leType)
        {
            var le = default(Automation.DataModels.LegalEntity);
            switch (leType)
            {
                case LegalEntityTypeEnum.NaturalPerson:
                    le = leService.GetLegalEntity(idNumber: registration_or_idNumber);
                    break;

                default:
                    le = leService.GetLegalEntity(registrationNumber: registration_or_idNumber);
                    break;
            }
            Assert.IsNotNull(le);
            var leos = leosService.GetLegalEntityOrganisationStructure(le.LegalEntityKey);
            Assert.IsNotNull(leos);
        }
    }
}