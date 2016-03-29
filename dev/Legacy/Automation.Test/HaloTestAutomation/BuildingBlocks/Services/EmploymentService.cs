using Automation.DataAccess.DataHelper;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using System;

namespace BuildingBlocks.Services
{
    public class EmploymentService : _2AMDataHelper, IEmploymentService
    {
        /// <summary>
        /// Returns an employment class that can be used to add a subsidised employment record.
        /// </summary>
        /// <returns></returns>
        public Automation.DataModels.Employment GetSubsidisedEmployment()
        {
            Automation.DataModels.Employment employment =
                new Automation.DataModels.Employment
                {
                    Employer = "STANDARD BANK",
                    MonthlyIncomeRands = 20000,
                    EmploymentType = EmploymentType.SalariedWithDeductions,
                    RemunerationType = RemunerationType.Salaried,
                    StartDate = "01/01/2010",
                    SubsidyProvider = "Department of Labour",
                    SalaryNumber = "123456",
                    PayPoint = "Durban",
                    Rank = "Manager",
                    Notch = "Seven",
                    StopOrderAmount = 5000
                };
            return employment;
        }

        //Inserts a record into the database with ConfirmedIncomeFlag = 0
        public void InsertUnconfirmedSalariedEmployment(int legalentitykey)
        {
            var employment =
                new Automation.DataModels.Employment
                {
                    EmployerKey = 6453,
                    EmploymentTypeKey = EmploymentTypeEnum.Salaried,
                    RemunerationTypeKey = RemunerationTypeEnum.Salaried,
                    EmploymentStatusKey = EmploymentStatusEnum.Current,
                    LegalEntityKey = legalentitykey,
                    EmploymentStartDate = DateTime.Now,
                    BasicIncome = 11000,
                    //Confirmed income and employment fields need to be Null not 0, so the below are commented out
                    //ConfirmedEmploymentFlag = false,
                    //ConfirmedIncomeFlag = false,
                };
            InsertEmployment(employment);
        }

        //Inserts a record into the database with ConfirmedIncomeFlag = 1
        public void ConfirmAllEmployment(int offerKey)
        {
            var legalEntities = GetActiveOfferRolesByOfferRoleType(offerKey, OfferRoleTypeEnum.MainApplicant);
            foreach (var legalentity in legalEntities)
            {
                var employmentRecords = GetEmploymentByGenericKey(legalentity.Column("LegalEntityKey").GetValueAs<int>(), true, false);
                foreach (var employmentRecord in employmentRecords)
                {
                    var employment = new Automation.DataModels.Employment
                    {
                        EmploymentKey = employmentRecord.Column("EmploymentKey").GetValueAs<int>(),
                        ContactPerson = "Test Contact Person",
                        ContactPhoneNumber = "1111111",
                        ContactPhoneCode = "111",
                        ConfirmedBy = TestUsers.NewBusinessProcessor,
                        ConfirmedDate = DateTime.Now,
                        ConfirmedBasicIncome = employmentRecord.Column("BasicIncome").GetValueAs<double>(),
                        ConfirmedEmploymentFlag = true,
                        ConfirmedIncomeFlag = true,
                        EmploymentConfirmationSourceKey = EmploymentConfirmationSourceEnum.ElectronicYellowPagesDirectory,
                        SalaryPaymentDay = 26
                    };
                    ConfirmEmployment(employment);
                }
            }
        }

        public void ConfirmSuretorEmployment(int offerKey)
        {
            var legalEntities = GetActiveOfferRolesByOfferRoleType(offerKey, OfferRoleTypeEnum.Suretor);
            foreach (var legalentity in legalEntities)
            {
                var employmentRecords = GetEmploymentByGenericKey(legalentity.Column("LegalEntityKey").GetValueAs<int>(), true, false);
                foreach (var employmentRecord in employmentRecords)
                {
                    var employment = new Automation.DataModels.Employment
                    {
                        EmploymentKey = employmentRecord.Column("EmploymentKey").GetValueAs<int>(),
                        ContactPerson = "Test Contact Person",
                        ContactPhoneNumber = "1111111",
                        ContactPhoneCode = "111",
                        ConfirmedBy = TestUsers.NewBusinessProcessor,
                        ConfirmedDate = DateTime.Now,
                        ConfirmedBasicIncome = employmentRecord.Column("BasicIncome").GetValueAs<double>(),
                        ConfirmedEmploymentFlag = true,
                        ConfirmedIncomeFlag = true,
                        EmploymentConfirmationSourceKey = EmploymentConfirmationSourceEnum.ElectronicYellowPagesDirectory,
                        SalaryPaymentDay = 26
                    };
                    ConfirmEmployment(employment);
                }
            }
        }

        public Automation.DataModels.Employer GetFullyPopulatedEmployer()
        {
            var random = new Random();
            return new Automation.DataModels.Employer()
            {
                Name = string.Format(@"test employer {0}", random.Next(0, 999999999)),
                TelephoneNumber = "1234567",
                TelephoneCode = "012",
                ContactPerson = "Contact Person Test",
                ContactEmail = "mail@testmail.com",
                AccountantName = "account name test",
                AccountantContactPerson = "accountant contact person",
                AccountantTelephoneCode = "013",
                AccountantTelephoneNumber = "7654321",
                AccountantEmail = "Accountant@mail.com",
                EmployerBusinessTypeKey = EmployerBusinessTypeEnum.Company,
                UserID = @"SAHL\HaloUser",
                ChangeDate = DateTime.Now,
                EmploymentSectorKey = EmploymentSectorEnum.Legal,
                EmployerBusinessTypeDescription = "Company",
                EmploymentSectorDescription = "Legal"
            };
        }

        /// <summary>
        /// Returns a subsidy provider
        /// </summary>
        /// <returns></returns>
        public Automation.DataModels.SubsidyProvider GetFullyPopulatedSubsidyProvider()
        {
            var address = new Automation.DataModels.Address()
            {
                AddressFormatKey = AddressFormatEnum.Box,
                AddressFormatDescription = AddressFormat.Box,
                BoxNumber = "3275",
                RRR_SuburbDescription = "Kaarkloof",
                RRR_CityDescription = "Kaarkloof",
                RRR_ProvinceDescription = "Kwazulu-Natal",
                RRR_PostalCode = "3275",
                RRR_CountryDescription = "South Africa",
                PostOfficeDescription = "Kaarkloof"
            };
            var legalEntityaddress = new Automation.DataModels.LegalEntityAddress()
            {
                AddressTypeKey = AddressTypeEnum.Postal,
                GeneralStatusKey = GeneralStatusEnum.Active,
                Address = address
            };
            var legalentity = new Automation.DataModels.LegalEntity()
            {
                RegisteredName = "ProviderNameTest",
                WorkPhoneCode = "012",
                WorkPhoneNumber = "1234567",
                IntroductionDate = DateTime.Now,
                DateOfBirth = DateTime.Now,
                ChangeDate = DateTime.Now,
                LegalEntityTypeKey = LegalEntityTypeEnum.Company,
                DocumentLanguageKey = LanguageEnum.English,
                HomeLanguageKey = LanguageEnum.English,
                LegalEntityAddress = legalEntityaddress,
                EmailAddress = "test@test.co.za"
            };
            return new Automation.DataModels.SubsidyProvider()
            {
                SubsidyProviderTypeKey = Common.Enums.SubsidyProviderTypeEnum.CivilServants,
                SubsidyProviderTypeDescription = "Civil Servants",
                ContactPerson = "ContactPersonTest",
                LegalEntity = legalentity
            };
        }
    }
}