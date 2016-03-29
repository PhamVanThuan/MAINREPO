using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Data.Models.Capitec.Statements;
using SAHL.Core.Identity;
using SAHL.Services.Interfaces.Capitec.Common;
using System;
using SAHL.Services.Capitec.Managers.Applicant;

namespace SAHL.Services.Capitec.Managers.Applicant
{
    public class ApplicantDataManager : IApplicantDataManager
    {
        private IDbFactory dbFactory;
        public ApplicantDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public void AddApplicantToApplication(Guid applicationID, Guid applicantID)
        {
            var applicationApplicantID = CombGuid.Instance.Generate();
            ApplicationApplicantDataModel applicationApplicant = new ApplicationApplicantDataModel(applicationApplicantID, applicationID, applicantID);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<ApplicationApplicantDataModel>(applicationApplicant);
                db.Complete();
            }
        }

        public void AddAddressToApplicant(Guid addressID, Guid applicantID, Guid addressType)
        {
            var applicantAddressID = CombGuid.Instance.Generate();
            ApplicantAddressDataModel applicantAddress = new ApplicantAddressDataModel(applicantAddressID, applicantID, addressID, addressType);

            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert(applicantAddress);
                db.Complete();
            }
        }

        public void AddDeclarationToApplicant(Guid applicantID, Guid declarationID)
        {
            var applicantDeclarationID = CombGuid.Instance.Generate();
            ApplicantDeclarationDataModel applicantDeclarationDataModel = new ApplicantDeclarationDataModel(applicantDeclarationID, applicantID, declarationID);

            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert(applicantDeclarationDataModel);
                db.Complete();
            }
        }

        public Guid GetDeclarationForApplicant(Guid applicantID, string declarationText)
        {
            var query = new GetApplicantDeclarationByDeclarationText(applicantID, declarationText);

            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var applicantDeclaration = db.SelectOne(query);
                if (applicantDeclaration != null)
                {
                    return applicantDeclaration.ID;
                }
            }
            throw new NullReferenceException(string.Format("Declaration {0} does not exist for applicant {1}", declarationText, applicantID));
        }

        public bool DidApplicantAnswerYesToDeclaration(Guid applicantID, string declarationText)
        {
            var query = new GetDeclarationTypeEnumForApplicantDeclarationByDeclarationText(applicantID, declarationText);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var declarationTypeEnum = db.SelectOne(query);
                if (declarationTypeEnum != null)
                {
                    if (declarationTypeEnum.Name == Enumerations.DeclarationTypeEnums.Yes.ToString())
                        return true;
                    else
                        return false;
                }
            }
            throw new NullReferenceException(string.Format("Declaration {0} does not exist for applicant {1}", declarationText, applicantID));
        }

        public void AddApplicant(Guid applicantID, Guid personID, bool mainContact)
        {
            ApplicantDataModel applicant = new ApplicantDataModel(applicantID, personID, mainContact);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<ApplicantDataModel>(applicant);
                db.Complete();
            }
        }

        public void AddPerson(Guid personID, Guid salutationEnumId, string firstName, string surname, string identityNumber)
        {
            PersonDataModel person = new PersonDataModel(personID, salutationEnumId, firstName, surname, identityNumber);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<PersonDataModel>(person);
                db.Complete();
            }
        }

        public bool DoesPersonExist(string identityNumber)
        {
            bool result = false;
            GetPersonFromIdentityNumberQuery query = new GetPersonFromIdentityNumberQuery(identityNumber);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var person = db.SelectOne<PersonDataModel>(query);
                if (person != null)
                {
                    result = true;
                }
            }
            return result;
        }

        public Guid GetPersonID(string identityNumber)
        {
            GetPersonFromIdentityNumberQuery query = new GetPersonFromIdentityNumberQuery(identityNumber);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var person = db.SelectOne<PersonDataModel>(query);
                if (person != null)
                {
                    return person.Id;
                }
            }
            throw new NullReferenceException("person does not exist");
        }
    }
}