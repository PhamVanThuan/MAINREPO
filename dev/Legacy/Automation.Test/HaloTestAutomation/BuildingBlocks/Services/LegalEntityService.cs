using Automation.DataAccess;
using Automation.DataAccess.DataHelper;
using Automation.DataModels;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using WatiN.Core.Exceptions;

namespace BuildingBlocks.Services
{
    public class LegalEntityService : _2AMDataHelper, ILegalEntityService
    {
        private ICommonService commonService;
        private IAccountService accountService;

        public LegalEntityService(ICommonService commonService)
        {
            this.commonService = commonService;
            var transactionService = ServiceLocator.Instance.GetService<ILoanTransactionService>();
            var hocService = ServiceLocator.Instance.GetService<IHOCService>();
            var detailTypeService = ServiceLocator.Instance.GetService<IDetailTypeService>();
            this.accountService = new AccountService(this, transactionService, hocService, detailTypeService);
        }

        /// <summary>
        /// Gets a legalentity by idnumber, registrationNumber,registeredname or legalname, then populates an return
        /// Automation.DataModels.LegalEntity with the details of the legalentity.
        /// </summary>
        /// <param name="idNumber"></param>
        /// <param name="registrationNumber"></param>
        /// <param name="registeredname"></param>
        /// <param name="legalname"></param>
        /// <param name="legalentitykey"></param>
        /// <returns>Automation.DataModels.LegalEntity</returns>
        public Automation.DataModels.LegalEntity GetLegalEntity(string idNumber = "", string registrationNumber = "", string registeredname = "",
                string legalname = "", int legalentitykey = 0)
        {
            //Populate Automation.DataModels.LegalEntity
            return base.GetLegalEntity(idNumber, registrationNumber, registeredname, legalname, legalentitykey);
        }

        /// <summary>
        /// Gets a random legalentity provided LegalEntityType an return
        /// Automation.DataModels.LegalEntity with the details of the legalentity.
        /// </summary>
        /// <param name="leType">i.e LegalEntityType.NaturalPerson</param>
        /// <returns>Automation.DataModels.LegalEntity</returns>
        public Automation.DataModels.LegalEntity GetRandomLegalEntityRecord(LegalEntityTypeEnum leType)
        {
            //Populate Automation.DataModels.LegalEntity
            QueryResultsRow legalentityRow = base.GetRandomLegalEntityRecord(leType);

            #region PopulateLegalEntity

            var legalentity = new Automation.DataModels.LegalEntity();
            string legalentitytypekey = legalentityRow.Column("legalentitytypekey").Value;
            legalentity.LegalEntityTypeKey = (LegalEntityTypeEnum)Enum.Parse(typeof(LegalEntityTypeEnum), legalentitytypekey);
            string maritalstatuskey = legalentityRow.Column("maritalstatuskey").Value;
            if (!String.IsNullOrEmpty(maritalstatuskey))
                legalentity.MaritalStatusKey = (MaritalStatusEnum)Enum.Parse(typeof(MaritalStatusEnum), maritalstatuskey);
            string genderkey = legalentityRow.Column("genderkey").Value;
            if (!String.IsNullOrEmpty(genderkey))
                legalentity.GenderKey = (GenderTypeEnum)Enum.Parse(typeof(GenderTypeEnum), genderkey);
            string populationGroupKey = legalentityRow.Column("populationGroupKey").Value;
            if (!String.IsNullOrEmpty(populationGroupKey))
                legalentity.PopulationGroupKey = (PopulationGroupEnum)Enum.Parse(typeof(PopulationGroupEnum), populationGroupKey);
            string salutationkey = legalentityRow.Column("Salutationkey").Value;
            if (!String.IsNullOrEmpty(salutationkey))
                legalentity.SalutationKey = (SalutationTypeEnum)Enum.Parse(typeof(SalutationTypeEnum), salutationkey);
            legalentity.FirstNames = legalentityRow.Column("firstnames").Value;
            legalentity.Surname = legalentityRow.Column("surname").Value;
            legalentity.Initials = legalentityRow.Column("Initials").Value;
            legalentity.PreferredName = legalentityRow.Column("PreferredName").Value;
            legalentity.IdNumber = legalentityRow.Column("idnumber").Value;
            legalentity.PassportNumber = legalentityRow.Column("passportnumber").Value;
            legalentity.TaxNumber = legalentityRow.Column("TaxNumber").Value;
            legalentity.RegistrationNumber = legalentityRow.Column("RegistrationNumber").Value;
            legalentity.RegisteredName = legalentityRow.Column("RegisteredName").Value;
            legalentity.TradingName = legalentityRow.Column("TradingName").Value;
            string dob = legalentityRow.Column("DateOfBirth").Value;
            if (!String.IsNullOrEmpty(dob))
                legalentity.DateOfBirth = DateTime.Parse(legalentityRow.Column("DateOfBirth").Value);
            legalentity.HomePhoneCode = legalentityRow.Column("HomePhoneCode").Value;
            legalentity.HomePhoneNumber = legalentityRow.Column("HomePhoneNumber").Value;
            legalentity.WorkPhoneCode = legalentityRow.Column("WorkPhoneCode").Value;
            legalentity.WorkPhoneNumber = legalentityRow.Column("WorkPhoneNumber").Value;
            legalentity.CellPhoneNumber = legalentityRow.Column("CellPhoneNumber").Value;
            legalentity.EmailAddress = legalentityRow.Column("EmailAddress").Value;
            legalentity.FaxCode = legalentityRow.Column("FaxCode").Value;
            legalentity.FaxNumber = legalentityRow.Column("FaxNumber").Value;
            string citizenshipTypekey = legalentityRow.Column("citizenTypekey").Value;
            if (!String.IsNullOrEmpty(citizenshipTypekey))
                legalentity.CitizenTypeKey = (CitizenTypeEnum)Enum.Parse(typeof(CitizenTypeEnum), citizenshipTypekey);
            string generalstatuskey = legalentityRow.Column("legalentitystatuskey").Value;
            if (!String.IsNullOrEmpty(generalstatuskey))
                legalentity.LegalEntityStatusKey = (LegalEntityStatusEnum)Enum.Parse(typeof(GeneralStatusEnum), generalstatuskey);
            string educationkey = legalentityRow.Column("educationkey").Value;
            if (!String.IsNullOrEmpty(educationkey))
                legalentity.EducationKey = (EducationEnum)Enum.Parse(typeof(EducationEnum), educationkey);
            string homelanguagekey = legalentityRow.Column("homelanguagekey").Value;
            if (!String.IsNullOrEmpty(homelanguagekey))
                legalentity.HomeLanguageKey = (LanguageEnum)Enum.Parse(typeof(LanguageEnum), homelanguagekey);
            string documentLanguageKey = legalentityRow.Column("documentLanguageKey").Value;
            if (!String.IsNullOrEmpty(documentLanguageKey))
                legalentity.DocumentLanguageKey = (LanguageEnum)Enum.Parse(typeof(LanguageEnum), documentLanguageKey);
            legalentity.LegalEntityKey = legalentityRow.Column("LegalEntityKey").GetValueAs<int>();

            return legalentity;

            #endregion PopulateLegalEntity
        }

        /// <summary>
        /// This currently only caters for Application Management
        /// Some assumptions have been made based on the stage in WorkFlow
        ///     1. An Application Mailing Address exists for the offer
        /// </summary>
        /// <param name="offerKey">OfferKey</param>
        /// <param name="ort">OfferRoleType</param>
        /// <param name="hasApplicationMailingAddress">TRUE = Add Mailing Address, FALSE = Do not add Mailing Address</param>
        /// <param name="newLEKey"></param>
        /// <param name="originalMaAddressKey"></param>
        /// <returns></returns>
        public string AddNewLegalEntity(int offerKey, OfferRoleTypeEnum ort, bool hasApplicationMailingAddress, out int newLEKey,
            out int originalMaAddressKey)
        {
            var idNumber = IDNumbers.GetNextIDNumber();
            originalMaAddressKey = -1;
            //get/insert le
            int leKey = base.CreateNewLegalEntity("test@test.co.za", idNumber);
            newLEKey = leKey;
            //Create the offerrole
            bool created = base.CreateOfferRole(leKey, offerKey, ort, GeneralStatusEnum.Active);
            if (!created)
            {
                throw new WatiNException("OfferRole record not created");
            }
            //do the application mailing address stuff
            if (hasApplicationMailingAddress)
            {
                originalMaAddressKey = base.AddAppMailingAddressToLE(leKey, offerKey);
            }
            //return LE Name
            return base.GetLegalEntityLegalNameByLegalEntityKey(leKey).SQLScalarValue;
        }

        /// <summary>
        /// Removes a legal entity added to an application by an automated test.
        /// </summary>
        /// <param name="leKey">LegalEntityKey</param>
        /// <param name="offerKey">OfferKey</param>
        /// <param name="ort">OfferRoleType</param>
        /// <param name="maAddressKey">MailingAddressKey to revert back to.</param>
        public void CleanUpNewLegalEntity(int leKey, int offerKey, OfferRoleTypeEnum ort, int maAddressKey)
        {
            //reset the mailing address key if it is valid
            if (maAddressKey > 0)
            {
                base.SetMailingAddress(maAddressKey, offerKey);
            }

            //delete the legalentity and related dependant data
            base.DeleteLEAddress(leKey);
            base.DeleteOfferRole(leKey, offerKey);
            base.DeleteLegalEntity(leKey);
        }

        ///<summary>
        ///</summary>
        ///<param name="leKey"></param>
        ///<param name="offerKey"></param>
        ///<param name="bankAccountKey"></param>
        ///<param name="legalEntityBankAccountKey"></param>
        ///<param name="offerDebitOrderKey"></param>
        public void AddNewBankAccount(int leKey, int offerKey, out int bankAccountKey, out int legalEntityBankAccountKey,
            out int offerDebitOrderKey)
        {
            bankAccountKey = base.GetUnusedBankAccountKey();
            var legalEntityBankAccount = base.InsertLegalEntityBankAccount(leKey, bankAccountKey);
            legalEntityBankAccountKey = legalEntityBankAccount.LegalEntityBankAccountKey;
            offerDebitOrderKey = base.InsertOfferDebitOrder(offerKey, bankAccountKey);
        }

        /// <summary>
        /// Gets a random employment record from the database provided the parameters.
        /// </summary>
        /// <param name="employmenstatus"></param>
        /// <param name="remunType"></param>
        /// <param name="employType"></param>
        /// <returns></returns>
        public Automation.DataModels.LegalEntityEmployment GetRandomLegalEntityEmploymentRecord(EmploymentStatusEnum employmenstatus, RemunerationTypeEnum remunType,
                EmploymentTypeEnum employType)
        {
            var employmentRecordRow = base.GetLegalEntityEmploymentRecord(employmentType: employType, remunerationType: remunType,
                                                employmentStatus: employmenstatus);
            var legalEntityEmploymentRecord = new Automation.DataModels.LegalEntityEmployment();

            string employmentTypekey = employmentRecordRow.Column("employmentTypekey").Value;
            if (!String.IsNullOrEmpty(employmentTypekey))
                legalEntityEmploymentRecord.EmploymentType
                    = (EmploymentTypeEnum)Enum.Parse(typeof(EmploymentTypeEnum), employmentTypekey);

            string employmentStatusKey = employmentRecordRow.Column("EmploymentStatusKey").Value;
            if (!String.IsNullOrEmpty(employmentStatusKey))
                legalEntityEmploymentRecord.EmploymentStatus
                    = (EmploymentStatusEnum)Enum.Parse(typeof(EmploymentStatusEnum), employmentStatusKey);

            string remunerationTypeKey = employmentRecordRow.Column("RemunerationTypeKey").Value;
            if (!String.IsNullOrEmpty(remunerationTypeKey))
                legalEntityEmploymentRecord.RemunerationType
                    = (RemunerationTypeEnum)Enum.Parse(typeof(RemunerationTypeEnum), remunerationTypeKey);

            legalEntityEmploymentRecord.MonthlyIncome = employmentRecordRow.Column("MonthlyIncome").GetValueAs<float>();

            return legalEntityEmploymentRecord;
        }

        /// <summary>
        /// Returns the legal entity legal name
        /// </summary>
        /// <param name="legalentityKey"></param>
        /// <returns></returns>
        public string GetLegalEntityLegalName(int legalentityKey)
        {
            return base.GetLegalEntityLegalNameByLegalEntityKey(legalentityKey).SQLScalarValue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="legalEntityKey"></param>
        /// <returns></returns>
        public string GetLegalEntityIDNumber(int legalEntityKey)
        {
            var LegalEntityRecord = base.GetLegalEntityByLegalEntityKey(legalEntityKey);
            string idNumber = (from le in LegalEntityRecord select le.Column("IdNumber").GetValueAs<string>()).FirstOrDefault();
            return idNumber;
        }

        /// <summary>
        /// Get all the active legalentity roles against the account.
        /// </summary>
        /// <returns></returns>
        public List<Automation.DataModels.LegalEntityRole> GetActiveLegalEntityRoles(int accountkey, RoleTypeEnum roleTypeKey)
        {
            var leRoles = base.GetLegalEntityRoles(accountkey);
            return (from leRole in leRoles
                    where leRole.GeneralStatusKey == GeneralStatusEnum.Active && leRole.RoleTypeKey == roleTypeKey
                    select leRole).ToList();
        }

        /// <summary>
        /// Get all the active legalentity roles against the account.
        /// </summary>
        /// <returns></returns>
        public List<Automation.DataModels.LegalEntityRole> GetAllLegalEntityRoles(int accountkey)
        {
            var leRoles = base.GetLegalEntityRoles(accountkey);
            //add the bank accounts.
            foreach (var le in leRoles)
            {
                var leba = base.GetLegalEntityBankAccounts(le.LegalEntityKey).ToList();
                le.LegalEntityBankAccounts = leba[0] == null ? null : leba;
            }
            return (from leRole in leRoles
                    select leRole).ToList();
        }

        /// <summary>
        /// Get all the active legalentity roles.
        /// </summary>
        /// <returns></returns>
        public List<Automation.DataModels.LegalEntityRole> GetAllLegalEntityRolesByLegalEntityKey(int legalEntityKey)
        {
            var leRoles = base.GetAllLegalEntityRoles(legalEntityKey);
            //add the bank accounts.
            foreach (var le in leRoles)
            {
                var leba = base.GetLegalEntityBankAccounts(le.LegalEntityKey).ToList();
                le.LegalEntityBankAccounts = leba[0] == null ? null : leba;
            }
            return (from leRole in leRoles
                    select leRole).ToList();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public List<int> GetTwoOpenMortgageLoanAccountsForSameLegalEntity()
        {
            //find all LE's with more than one open mortgage loan account
            var results = base.LegalEntitiesWithMoreThanOneAccount();
            //we need his legalEntityKey
            var legalEntity = results.Rows(0).Column("legalentitykey").GetValueAs<int>();
            //this will be the one account
            var accountKey = results.Rows(0).Column("MaxAccountNumber").GetValueAs<int>();
            List<int> accountList = new List<int> { accountKey };
            //get roles

            var roles = GetAllLegalEntityRolesByLegalEntityKey(legalEntity);
            var relatedAccountKey = (from r in roles
                                     where r.AccountKey != accountKey && r.GeneralStatusKey == GeneralStatusEnum.Active
                                         && (r.ProductKey == (int)ProductEnum.NewVariableLoan || r.ProductKey == (int)ProductEnum.VariableLoan)
                                     select r.AccountKey).FirstOrDefault();
            //add this key
            accountList.Add(relatedAccountKey);
            return accountList;
        }

        /// <summary>
        /// Get all the active legalentity roles against the account.
        /// </summary>
        /// <returns></returns>
        public Automation.DataModels.Account GetLegalEntityLoanAccount(int legalentitykey)
        {
            var accounts = base.GetLegalEntityAccounts(legalentitykey);
            return (from acc in accounts
                    where acc.RRR_ProductKey != ProductEnum.HomeOwnersCover &&
                                acc.RRR_ProductKey != ProductEnum.LifePolicy
                    select acc).FirstOrDefault();
        }

        /// <summary>
        /// Fetches the first main applicant for the specified account.
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        /// <returns>LegalEntity</returns>
        public QueryResultsRow GetFirstLegalEntityMainApplicantOnAccount(int accountKey)
        {
            var legalEntities = base.GetLegalEntityNamesAndRoleByAccountKey(accountKey);
            var legalEntity = (from le in legalEntities
                               where le.Column("Role").Value == RoleType.MainApplicant
                               select le).FirstOrDefault();
            return legalEntity;
        }

        public Automation.DataModels.LegalEntity GetRandomLegalEntityRecord(LegalEntityTypeEnum legalEntityType,
            LanguageEnum documentLanguageKey, LegalEntityExceptionStatusEnum legalEntityExceptionStatusKey = LegalEntityExceptionStatusEnum.None,
            MaritalStatusEnum maritalStatusKey = MaritalStatusEnum.Unknown, CitizenTypeEnum citizenShipType = CitizenTypeEnum.SACitizen,
            string registrationNumberExclusion = "", string IdNumberExclusionExpression = "", string firstnamesExclusionExpression = "",
            string surnameExclusionExpression = "", string registeredNameExclusionExpression = "")
        {
            var legalEntities = base.GetManyLegalEntities(citizenShipType, legalEntityType, legalEntityExceptionStatusKey);

            legalEntities = from le in legalEntities
                            where le.DocumentLanguageKey == documentLanguageKey
                            select le;

            if (maritalStatusKey != MaritalStatusEnum.Unknown)
            {
                legalEntities = from le in legalEntities
                                where le.MaritalStatusKey == maritalStatusKey
                                select le;
            }
            switch (legalEntityType)
            {
                case LegalEntityTypeEnum.NaturalPerson:
                    {
                        if (!String.IsNullOrEmpty(surnameExclusionExpression))
                        {
                            legalEntities = from le in legalEntities
                                            where le.Surname != null
                                                && !le.Surname.Contains(surnameExclusionExpression)
                                            select le;
                        }
                        if (!String.IsNullOrEmpty(firstnamesExclusionExpression))
                        {
                            legalEntities = from le in legalEntities
                                            where le.FirstNames != null
                                                && !le.FirstNames.Contains(firstnamesExclusionExpression)
                                            select le;
                        }
                        if (!String.IsNullOrEmpty(IdNumberExclusionExpression))
                        {
                            legalEntities = from le in legalEntities
                                            where le.IdNumber != null
                                                && !le.IdNumber.Contains(IdNumberExclusionExpression)
                                            select le;
                        }
                        break;
                    }
                default:
                    {
                        if (!String.IsNullOrEmpty(registeredNameExclusionExpression))
                        {
                            legalEntities = from le in legalEntities
                                            where le.RegisteredName != null
                                                && !le.RegisteredName.Contains(registeredNameExclusionExpression)
                                            select le;
                        }
                        if (!String.IsNullOrEmpty(registrationNumberExclusion))
                        {
                            legalEntities = from le in legalEntities
                                            where le.RegistrationNumber != null
                                                && !le.RegistrationNumber.Contains(registrationNumberExclusion)
                                            select le;
                        }
                        break;
                    }
            }
            return legalEntities.FirstOrDefault();
        }

        /// <summary>
        /// Inserts a legal entity bank account record
        /// </summary>
        /// <param name="legalEntityKey"></param>
        public Automation.DataModels.LegalEntityBankAccount InsertLegalEntityBankAccount(int legalEntityKey)
        {
            int bankAccountKey = base.GetUnusedBankAccountKey();
            return base.InsertLegalEntityBankAccount(legalEntityKey, bankAccountKey);
        }

        /// <summary>
        /// Gets the latest legal entity asset liability, sorted by AssetLiabilityKey, for the legal entity provided.
        /// </summary>
        /// <param name="legalentitykey"></param>
        /// <returns></returns>
        public Automation.DataModels.LegalEntityAssetLiabilities GetLatestLegalEntityAssetLiabilityRecord(int legalentitykey)
        {
            return (from legalentityAssetsLiability in base.GetLegalEntityAssetLiabilities(legalentitykey: legalentitykey)
                    orderby legalentityAssetsLiability.AssetLiabilityKey descending
                    select legalentityAssetsLiability).FirstOrDefault();
        }

        public Automation.DataModels.LegalEntityOrganisationStructure GetEmptyLegalEntityOrganisationStructure()
        {
            return new Automation.DataModels.LegalEntityOrganisationStructure()
            {
                LegalEntity = new Automation.DataModels.LegalEntity()
                {
                    LegalEntityTypeDescription = "",
                    MaritalStatusDescription = "",
                    GenderDescription = "",
                    PopulationGroupDescription = "",
                    SalutationDescription = "",
                    CitizenTypeDescription = "",
                    LegalEntityStatusDescription = "",
                    EducationDescription = "",
                    HomeLanguageDescription = "",
                    DocumentLanguageDescription = "",
                    IdNumber = "",
                    Initials = "",
                    FirstNames = "",
                    Surname = "",
                    PreferredName = "",
                    PassportNumber = "",
                    RegistrationNumber = "",
                    RegisteredName = "",
                    TradingName = "",
                    HomePhoneCode = "",
                    HomePhoneNumber = "",
                    WorkPhoneCode = "",
                    WorkPhoneNumber = "",
                    FaxCode = "",
                    FaxNumber = "",
                    CellPhoneNumber = "",
                    EmailAddress = "",
                },
                OrganisationStructure = new Automation.DataModels.OrganisationStructure()
                {
                    OrganisationTypeDescription = ""
                }
            };
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="roleTypeEnum"></param>
        /// <param name="over18Years"></param>
        /// <returns></returns>
        public string GetLegalEntityIDNumberNotPlayRole(RoleTypeEnum roleTypeEnum, bool over18Years = true)
        {
            if (over18Years)
            {
                return (from le in base.GetLegalEntitiesNotPlayRole(roleTypeEnum)
                        where le.DateOfBirth < DateTime.Parse("1990-01-01")
                        select le.IdNumber).FirstOrDefault();
            }
            else
            {
                return (from le in base.GetLegalEntitiesNotPlayRole(roleTypeEnum)
                        where le.DateOfBirth > DateTime.Now
                        select le.IdNumber).FirstOrDefault();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="roleTypeEnum"></param>
        /// <param name="over18Years"></param>
        /// <returns></returns>
        public string GetLegalEntityIDNumberPlaySameRoleTwiceDifferentAccounts(RoleTypeEnum roleTypeEnum, bool over18Years = true, bool inclHOCAccounts = true)
        {
            if (over18Years)
            {
                return (from le in base.GetLegalEntityWithMoreThanOneSameRole(roleTypeEnum, inclHOCAccounts)
                        where le.DateOfBirth < DateTime.Now.AddYears(-18)
                        select le.IDNumber).FirstOrDefault();
            }
            else
            {
                return (from le in base.GetLegalEntityWithMoreThanOneSameRole(roleTypeEnum, inclHOCAccounts)
                        where le.DateOfBirth > DateTime.Now.AddYears(-18)
                        select le.IDNumber).FirstOrDefault();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="subsidyProviderToInsert"></param>
        /// <returns></returns>
        public Automation.DataModels.SubsidyProvider InsertSubsidyProvider(Automation.DataModels.SubsidyProvider subsidyProviderToInsert)
        {
            var legalEntityToInsert = subsidyProviderToInsert.LegalEntity;
            var legalEntityAddressToInsert = subsidyProviderToInsert.LegalEntity.LegalEntityAddress;
            var addressToInsert = subsidyProviderToInsert.LegalEntity.LegalEntityAddress.Address;
            var legalEntityKey = 0;
            var addresskey = 0;

            try
            {
                //Insert and Load the legalentity so that we can also get the keys for the inserted legalentity.
                var insertedLegalEntity = base.InsertLegalEntity(legalEntityToInsert);
                legalEntityKey = insertedLegalEntity.LegalEntityKey;

                //Insert and Load the address to get keys out.
                var insertedAddress = base.InsertAddress(addressToInsert);
                addresskey = insertedAddress.AddressKey;

                //Insert and Load the legalentityaddress so that we can also get the keys for the inserted legalentityaddress.
                //legalEntityAddressToInsert.LegalEntityKey = legalEntityKey;
                legalEntityAddressToInsert.LegalEntity.LegalEntityKey = legalEntityKey;
                legalEntityAddressToInsert.AddressKey = addresskey;
                var insertedLegalentityaddress = base.InsertLegalEntityAddress(legalEntityAddressToInsert);
                insertedLegalEntity.LegalEntityAddress = insertedLegalentityaddress;
                insertedLegalEntity.LegalEntityAddress.Address = insertedAddress;

                //Insert the subsidy provider detail.
                subsidyProviderToInsert.LegalEntity = insertedLegalEntity;
                var insertedSubsidyProvider = base.InsertSubsidyProvider(subsidyProviderToInsert);
                insertedSubsidyProvider.LegalEntity = insertedLegalEntity;

                return insertedSubsidyProvider;
            }
            catch
            {
                ////Cleanup everything that was inserted.
                DeleteSubsidyProvider(legalEntityKey, addresskey);
                throw;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="legalentitykey"></param>
        /// <param name="addresskey"></param>
        public void DeleteSubsidyProvider(int legalentitykey, int addresskey)
        {
            //Has to be deleted in this order due to foreign key contraints.
            base.DeleteLegalEntityAddress(legalentitykey);
            base.DeleteAddress(addresskey);
            base.DeleteSubsidyProvider(legalentitykey);
            base.DeleteLegalEntity(legalentitykey);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Automation.DataModels.LegalEntity GetNaturalPersonLegalEntity()
        {
            var random = new Random();
            string idNumber = IDNumbers.GetNextIDNumber();
            var legalentityDetailAdd = new Automation.DataModels.LegalEntity
            {
                MaritalStatusKey = MaritalStatusEnum.Single,
                GenderKey = GenderTypeEnum.Male,
                PopulationGroupKey = PopulationGroupEnum.White,
                SalutationKey = SalutationTypeEnum.Mr,
                FirstNames = string.Format(@"First{0}Name{1}", random.Next(0, 5000), random.Next(0, 5000)),
                Initials = "SA",
                Surname = string.Format(@"Surname{0}", random.Next(0, 5000)),
                PreferredName = "Test",
                TaxNumber = "1234",
                IdNumber = idNumber,
                PassportNumber = random.Next(100000, 10000000).ToString(),
                HomePhoneCode = "012",
                HomePhoneNumber = "1234567",
                WorkPhoneCode = "012",
                WorkPhoneNumber = "1234567",
                CellPhoneNumber = "0823456789",
                EmailAddress = "test@test.co.za",
                FaxCode = "123",
                FaxNumber = "999999",
                CitizenTypeKey = CitizenTypeEnum.SACitizen,
                RoleTypeKey = RoleTypeEnum.MainApplicant,
                DocumentLanguageKey = LanguageEnum.English,
                EducationKey = EducationEnum.Other,
                HomeLanguageKey = LanguageEnum.English,
                LegalEntityTypeKey = LegalEntityTypeEnum.NaturalPerson
            };
            return legalentityDetailAdd;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Automation.DataModels.LegalEntity GetCompanyLegalEntity()
        {
            var randomGen = new Random();
            var legalEntity = new Automation.DataModels.LegalEntity
            {
                HomePhoneCode = "012",
                HomePhoneNumber = "1234567",
                RoleTypeKey = RoleTypeEnum.MainApplicant,
                RegistrationNumber =
                    String.Format("ABC/{0}/D/{1}",
                                randomGen.Next(0, 999999999), randomGen.Next(0, 9999)),
                TradingName = "CompanyTest",
                RegisteredName = "CompanyTest",
                LegalEntityTypeKey = LegalEntityTypeEnum.Company,
                WorkPhoneCode = "115",
                WorkPhoneNumber = "1925314",
                TaxNumber = "A/R/2312311"
            };
            return legalEntity;
        }

        /// <summary>
        /// Get a legal entity key from 2am by legal entity trade name
        /// </summary>
        public int GetLegalEntityKeyByTradeName(string tradeName)
        {
            var key = base.GetLegalEntityKeyByTradingName(tradeName);
            return int.Parse(key);
        }

        /// <summary>
        /// Get a legal entity key from 2am by legal entity first name
        /// </summary>
        public int GetLegalEntityKeyByFirstNames(string firstNames)
        {
            var legalEntityKey = base.GetLegalEntityKeyByFirstNames(firstNames);
            return int.Parse(legalEntityKey);
        }

        /// <summary>
        /// Gets an existing relationship against a legal entity
        /// </summary>
        /// <param name="legalEntityKey"></param>
        /// <param name="existingRelationship"></param>
        /// <param name="idNumber"></param>
        /// <returns></returns>
        public void GetExistingRelationship(int legalEntityKey, string _legalEntityIDNumber, out string existingRelationship, out string idNumber)
        {
            //get the existing relationships so we can update them
            var r = base.GetLegalEntityRelationship(legalEntityKey);
            existingRelationship = string.Empty;
            idNumber = _legalEntityIDNumber;
            foreach (QueryResultsRow row in r.RowList)
            {
                if (row.Column(0).Value == _legalEntityIDNumber)
                {
                    existingRelationship = r.Rows(0).Column(1).Value;
                    break;
                }
            }
        }

        /// <summary>
        /// Returns attorney access users
        /// </summary>
        /// <returns></returns>
        public Automation.DataModels.LegalEntityLogin GetAttorneyAccessLogin(GeneralStatusEnum legalEntityLoginStatus, GeneralStatusEnum externalRoleStatus)
        {
            var legalEntityLogins = base.GetLegalEntityLogin(legalEntityLoginStatus, externalRoleStatus);
            if (legalEntityLogins.Count() > 0)
                return legalEntityLogins.FirstOrDefault();
            return default(Automation.DataModels.LegalEntityLogin);
        }

        /// <summary>
        /// Returns attorney access users
        /// </summary>
        /// <returns></returns>
        public Automation.DataModels.LegalEntityLogin GetAttorneyAccessLogin(GeneralStatusEnum legalEntityLoginStatus, GeneralStatusEnum externalRoleStatus,
            string emailAddress)
        {
            var legalEntityLogins = base.GetLegalEntityLogin(legalEntityLoginStatus, externalRoleStatus);
            return (from l in legalEntityLogins where l.Username == emailAddress select l).FirstOrDefault();
        }

        /// <summary>
        /// Returns the login details for an active user linked to the attorney provided.
        /// </summary>
        /// <param name="attorneyKey">attorneyKey</param>
        /// <param name="legalEntityLoginStatus">legalEntityLoginStatus</param>
        /// <param name="externalRoleStatus">externalRoleStatus</param>
        /// <returns></returns>
        public Automation.DataModels.LegalEntityLogin GetAttorneyAccessLoginLinkedToAttorney(int attorneyKey, GeneralStatusEnum legalEntityLoginStatus,
            GeneralStatusEnum externalRoleStatus)
        {
            var results = base.GetLegalEntityLogin(legalEntityLoginStatus, externalRoleStatus);
            var login = (from r in results where r.AttorneyKey == attorneyKey select r).FirstOrDefault();
            return login;
        }

        public IEnumerable<Automation.DataModels.LegalEntityReturningDiscountQualifyingData> GetLegalEntityReturningDiscountQualifyingDataWithValidIDNumber()
        {
            var results = (from r in base.GetLegalEntityReturningDiscountQualifyingData()
                           where r.IDNumber != null ? r.IDNumber.Length == 13 : false
                           select r).DefaultIfEmpty();
            return results;
        }

        public IEnumerable<Automation.DataModels.Account> GetLegalEntityLoanAccounts(int legalentitykey, AccountStatusEnum accountStatusKey)
        {
            var accounts = base.GetLegalEntityAccounts(legalentitykey);
            return (from acc in accounts
                    where acc.RRR_ProductKey != ProductEnum.HomeOwnersCover &&
                                acc.RRR_ProductKey != ProductEnum.LifePolicy &&
                                    acc.AccountStatusKey == accountStatusKey
                    select acc).DefaultIfEmpty();
        }

        public DateTime? GetLegalEntityDateOfBirth(string idNumber)
        {
            return base.GetLegalEntity(idNumber, string.Empty, string.Empty, string.Empty, 0).DateOfBirth;
        }
    }
}