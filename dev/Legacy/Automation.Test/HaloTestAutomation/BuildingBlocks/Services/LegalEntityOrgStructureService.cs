using Automation.DataAccess.DataHelper;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using System;
using System.Linq;

namespace BuildingBlocks.Services
{
    public class LegalEntityOrgStructureService : _2AMDataHelper, ILegalEntityOrgStructureService
    {
        /// <summary>
        /// Inserts a new Debt Counsellor
        /// </summary>
        public void InsertNewDebtCounsellor()
        {
            var r = new Random();
            var name = string.Format(@"Test DC {0}-{1}", r.Next(0, 10000), r.Next(0, 100));
            string regNumber = string.Format(@"a{0}c{1}", r.Next(0, 10000), r.Next(0, 100));
            string ncrNumber = string.Format(@"NCR{0}DC{1}", r.Next(0, 10000), r.Next(0, 100));
            base.InsertNewTestDC(name, regNumber, ncrNumber);
        }

        /// <summary>
        /// Inserts a new PDA
        /// </summary>
        public void InsertNewPDA()
        {
            var r = new Random();
            string name = string.Format(@"Test PDA {0}-{1}", r.Next(0, 10000), r.Next(0, 100));
            string regNumber = string.Format(@"pd{0}a{1}", r.Next(0, 10000), r.Next(0, 100));
            base.InsertNewTestPDA(name, regNumber);
        }

        /// <summary>
        /// Get the leos and pupolate the model.
        /// </summary>
        /// <param name="legalentitykey"></param>
        /// <returns></returns>
        public Automation.DataModels.LegalEntityOrganisationStructure GetLegalEntityOrganisationStructure(int legalentitykey)
        {
            var orgStructures = base.GetOrganisationStructure(legalentitykey);
            var le = base.GetLegalEntity(null, null, null, null, legalentitykey: legalentitykey);
            return new Automation.DataModels.LegalEntityOrganisationStructure() { LegalEntity = le, OrganisationStructure = (from orgStructure in orgStructures select orgStructure).FirstOrDefault() };
        }

        public Automation.DataModels.LegalEntityOrganisationStructure GetCompanyLegalEntityOrganisationStructureTestData(string registeredname, string legalentitytype,
            string organisationtype)
        {
            var r = new Random();
            return new Automation.DataModels.LegalEntityOrganisationStructure()
            {
                LegalEntity = new Automation.DataModels.LegalEntity()
                {
                    RegisteredName = registeredname,
                    LegalEntityTypeDescription = legalentitytype,
                    RegistrationNumber = string.Format(@"ABC{0}/DEF{1}", r.Next(0, 99999), r.Next(0, 100)),
                    WorkPhoneCode = "031",
                    WorkPhoneNumber = "1234567",
                    FaxCode = "012",
                    FaxNumber = "2222222",
                    EmailAddress = "test@test.com",
                    HomePhoneCode = "013",
                    HomePhoneNumber = "1111111",
                    CellPhoneNumber = "0795242322"
                },
                OrganisationStructure = new Automation.DataModels.OrganisationStructure()
                {
                    OrganisationTypeDescription = organisationtype
                }
            };
        }

        public Automation.DataModels.LegalEntityOrganisationStructure GetNaturalLegalEntityOrganisationStructureAddTestData()
        {
            return new Automation.DataModels.LegalEntityOrganisationStructure()
            {
                LegalEntity = new Automation.DataModels.LegalEntity()
                {
                    SalutationDescription = "Mr",
                    GenderDescription = "Male",
                    FirstNames = "Contact",
                    Surname = "Added",
                    LegalEntityTypeDescription = LegalEntityType.NaturalPerson,
                    IdNumber = IDNumbers.GetNextIDNumber(),
                    HomePhoneCode = "031",
                    HomePhoneNumber = "7654321",
                    WorkPhoneCode = "031",
                    WorkPhoneNumber = "1234567",
                    CellPhoneNumber = "1231234567",
                    FaxCode = "",
                    FaxNumber = "",
                    EmailAddress = "test@test.co.za",
                    Initials = "CA",
                    PreferredName = "Contact"
                },
                OrganisationStructure = new Automation.DataModels.OrganisationStructure()
                {
                    OrganisationTypeDescription = "Contact"
                }
            };
        }

        public Automation.DataModels.LegalEntityOrganisationStructure GetNaturalLegalEntityOrganisationStructureUpdateTestData()
        {
            return new Automation.DataModels.LegalEntityOrganisationStructure()
            {
                LegalEntity = new Automation.DataModels.LegalEntity()
                {
                    SalutationDescription = "Mrs",
                    GenderDescription = "Female",
                    FirstNames = "Contact",
                    Surname = "Updated",
                    LegalEntityTypeDescription = LegalEntityType.NaturalPerson,
                    IdNumber = IDNumbers.GetNextIDNumber(),
                    HomePhoneCode = "999",
                    HomePhoneNumber = "9999999",
                    WorkPhoneCode = "999",
                    WorkPhoneNumber = "9999999",
                    CellPhoneNumber = "9999999999",
                    FaxCode = "",
                    FaxNumber = "",
                    EmailAddress = "testupdated@test.co.za",
                    Initials = "CAU",
                    PreferredName = "Contact"
                },
                OrganisationStructure = new Automation.DataModels.OrganisationStructure()
                {
                    OrganisationTypeDescription = "Contact"
                }
            };
        }
    }
}