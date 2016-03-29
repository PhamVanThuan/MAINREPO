using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.ApplicationDomain.Managers.Applicant.Statements;
using SAHL.Services.ApplicationDomain.Managers.Application.Statements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Managers.Applicant
{
    public class ApplicantDataManager : IApplicantDataManager
    {
        private IDbFactory dbFactory;

        public ApplicantDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public int SaveApplicant(LegalEntityDataModel legalEntity)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<LegalEntityDataModel>(legalEntity);
                db.Complete();
                return legalEntity.LegalEntityKey;
            }
        }

        public void UpdateApplicant(LegalEntityDataModel legalEntity)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Update<LegalEntityDataModel>(legalEntity);
                db.Complete();
            }
        }

        public LegalEntityDataModel GetApplicantByIDNumber(string idnumber)
        {
            LegalEntityDataModel legalEntity = null;
            GetApplicantByIDNumberStatement legalEntityQuery = new GetApplicantByIDNumberStatement(idnumber);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                legalEntity = db.SelectOne<LegalEntityDataModel>(legalEntityQuery);
            }
            return legalEntity;
        }

        public int AddApplicantRole(OfferRoleDataModel offerRole)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<OfferRoleDataModel>(offerRole);
                db.Complete();
                return offerRole.OfferRoleKey;
            }
        }

        public void SaveApplicantDeclarations(IEnumerable<OfferDeclarationDataModel> offerDeclarations)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<OfferDeclarationDataModel>(offerDeclarations);
                db.Complete();
            }
        }

        public bool DoesBankAccountBelongToApplicantOnApplication(int applicationNumber, int clientBankAccountKey)
        {
            bool response;
            var query = new DoesBankAccountBelongToApplicantOnApplicationStatement(applicationNumber, clientBankAccountKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var result = db.SelectOne<int>(query);
                response = (result == 1);
            }

            return response;
        }

        public int LinkDomiciliumAddressToApplicant(OfferRoleDomiciliumDataModel offerRoleDomiciliumDataModel)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<OfferRoleDomiciliumDataModel>(offerRoleDomiciliumDataModel);
                db.Complete();
                return offerRoleDomiciliumDataModel.OfferRoleDomiciliumKey;
            }
        }

        public bool DoesClientAddressBelongToApplicant(int clientAddressKey, int applicationNumber)
        {
            var doesClientAddressBelongToApplicant = new DoesClientAddressBelongToApplicantStatement(clientAddressKey, applicationNumber);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var results = db.Select(doesClientAddressBelongToApplicant);
                return results.Any();
            }
        }

        public bool CheckClientIsAnApplicantOnTheApplication(int clientKey, int applicationNumber)
        {
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                CheckClientIsAnApplicantOnTheApplicationStatement checkClientIsAnApplicantOnTheApplicationStatement =
                    new CheckClientIsAnApplicantOnTheApplicationStatement(clientKey, applicationNumber);
                var results = db.SelectOne<int>(checkClientIsAnApplicantOnTheApplicationStatement);
                return results > 0;
            }
        }

        public IEnumerable<EmploymentDataModel> GetIncomeContributorApplicantsCurrentEmployment(int applicationNumber)
        {
            IEnumerable<EmploymentDataModel> employment = new List<EmploymentDataModel>();
            GetIncomeContributorApplicantsCurrentEmploymentStatement getApplicantsCurrentEmploymentStatement
                = new GetIncomeContributorApplicantsCurrentEmploymentStatement(applicationNumber);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                employment = db.Select<EmploymentDataModel>(getApplicantsCurrentEmploymentStatement);
            }
            return employment;
        }

        public IEnumerable<OfferRoleDataModel> GetActiveClientRoleOnApplication(int applicationNumber, int clientKey)
        {
            IEnumerable<OfferRoleDataModel> offerRoleDataModel = Enumerable.Empty<OfferRoleDataModel>();
            GetActiveClientRoleOnApplicationStatement clientRoleOnApplicationStatement = new GetActiveClientRoleOnApplicationStatement(applicationNumber, clientKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                offerRoleDataModel = db.Select<OfferRoleDataModel>(clientRoleOnApplicationStatement);
            }
            return offerRoleDataModel;
        }

        public OfferRoleDataModel GetActiveApplicationRole(int applicationNumber, int clientKey)
        {
            OfferRoleDataModel applicationRoleDataModel;
            GetActiveApplicationRoleStatement query = new GetActiveApplicationRoleStatement(applicationNumber, clientKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                applicationRoleDataModel = db.SelectOne<OfferRoleDataModel>(query);
            }
            return applicationRoleDataModel;
        }

        public bool DoesApplicantHavePendingDomiciliumOnApplication(int ClientKey, int ApplicationNumber)
        {
            bool response;
            DoesApplicantHavePendingDomiciliumOnApplicationStatement query = new DoesApplicantHavePendingDomiciliumOnApplicationStatement(ClientKey, ApplicationNumber);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var results = db.SelectOne<int>(query);
                response = (results > 0);
            }
            return response;
        }

        public void AddOfferRoleAttribute(OfferRoleAttributeDataModel offerRoleAttribute)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<OfferRoleAttributeDataModel>(offerRoleAttribute);
                db.Complete();
            }
        }

        public bool ApplicantHasCurrentSAHLBusiness(int applicantKey)
        {
            bool hasCurrentBusiness = false;
            ClientHasOpenAccountOrOfferStatement clientHasOpenAccountOrOfferStatement = new ClientHasOpenAccountOrOfferStatement(applicantKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                hasCurrentBusiness = db.SelectOne<bool>(clientHasOpenAccountOrOfferStatement);
            }
            return hasCurrentBusiness;
        }

        public IEnumerable<OfferRoleAttributeDataModel> GetApplicantAttributes(int applicantRoleKey)
        {
            GetApplicantAttributesStatement statement = new GetApplicantAttributesStatement(applicantRoleKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.Select(statement);
            }
        }


        public IEnumerable<OfferDeclarationDataModel> GetApplicantDeclarations(int applicationNumber, int clientKey)
        {
            GetApplicantDeclarationsStatement statement = new GetApplicantDeclarationsStatement(applicationNumber, clientKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.Select(statement);
            }
        }


        public System.DateTime? GetClientDateOfBirth(int clientKey)
        {
            GetClientDateOfBirthStatement statement = new GetClientDateOfBirthStatement(clientKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne(statement);
            }
        }

    }
}