using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Services.LifeDomain.Managers.Statements;
using SAHL.Services.LifeDomain.QueryStatements;
using System;
using System.Collections.Generic;

namespace SAHL.Services.LifeDomain.Managers
{
    public class LifeDomainDataManager : ILifeDomainDataManager
    {
        private IDbFactory dbFactory;

        public LifeDomainDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }


        public int LodgeDisabilityClaim(int lifeAccountKey, int claimantLegalEntityKey)
        {
            var query = new LodgeDisabilityClaimStatement(lifeAccountKey, claimantLegalEntityKey);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                int disabilityClaimKey = db.SelectOne<int>(query);
                db.Complete();

                return disabilityClaimKey;
            }
        }


        public void UpdateDisabilityClaimStatus(int disabilityClaimKey, DisabilityClaimStatus disabilityClaimStatus)
        {
            var query = new UpdateDisabilityClaimStatusStatement(disabilityClaimKey, disabilityClaimStatus);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.ExecuteNonQuery(query);
                db.Complete();
            }
        }

        public void UpdateDisabilityClaimPaymentDates(int disabilityClaimKey, DateTime? paymentStartDate, int? numberOfInstalmentsAuthorised, DateTime? paymentEndDate)
        {
            var query = new UpdateDisabilityClaimPaymentDatesStatement(disabilityClaimKey, paymentStartDate, numberOfInstalmentsAuthorised, paymentEndDate);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.ExecuteNonQuery(query);
                db.Complete();
            }
        }

        public void UpdateApprovedDisabilityClaim(int disabilityClaimKey, int disabilityTypeKey, string otherDisabilityComments, string claimantOccupation, DateTime? expectedReturnToWorkDate)
        {
            var query = new UpdateApprovedDisabilityClaimStatement(disabilityClaimKey, disabilityTypeKey, otherDisabilityComments, claimantOccupation, expectedReturnToWorkDate);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.ExecuteNonQuery(query);
                db.Complete();
            }
        }

        public string CreateDisabilityClaimPaymentSchedule(int disabilityClaimKey, string adUserName)
        {
            var query = new CreateDisabilityClaimPaymentScheduleStatement(disabilityClaimKey, adUserName);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne<string>(query);
            }
        }

        public void DeleteDisabilityClaim(int disabilityClaimKey)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.DeleteByKey<DisabilityClaimDataModel, int>(disabilityClaimKey);
                db.Complete();
            }
        }

        public IEnumerable<DisabilityClaimModel> GetDisabilityClaimsByAccount(int accountNumber)
        {
            GetDisabilityClaimsByAccountStatement getDisabilityClaimsStatement = new GetDisabilityClaimsByAccountStatement(accountNumber);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.Select(getDisabilityClaimsStatement);
            }
        }

        public IEnumerable<DisabilityClaimDetailModel> GetDisabilityClaimHistory(int accountNumber)
        {
            GetDisabilityClaimHistoryStatement getDisabilityClaimHistoryStatement = new GetDisabilityClaimHistoryStatement(accountNumber);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.Select(getDisabilityClaimHistoryStatement);
            }
        }

        public string GetDisabilityClaimInstanceSubject(int disabilityClaimKey)
        {
            GetDisabilityClaimInstanceSubjectStatement getDisabilityClaimInstanceSubjectStatement = new GetDisabilityClaimInstanceSubjectStatement(disabilityClaimKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne(getDisabilityClaimInstanceSubjectStatement);
            }
        }

        public string GetDisabilityClaimStatusDescription(int disabilityClaimKey)
        {
            GetDisabilityClaimStatusDescriptionStatement query = new GetDisabilityClaimStatusDescriptionStatement(disabilityClaimKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne(query);
            }
        }

        public DisabilityClaimDetailModel GetDisabilityClaimDetailByKey(int disabilityClaimKey)
        {
            GetDisabilityClaimDetailByKeyStatement getDisabilityClaimByKeyStatement = new GetDisabilityClaimDetailByKeyStatement(disabilityClaimKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne(getDisabilityClaimByKeyStatement);
            }
        }

        public string TerminateDisabilityClaimPaymentSchedule(int disabilityClaimKey, string adUserName)
        {
            var query = new TerminateDisabilityClaimPaymentScheduleStatement(disabilityClaimKey, adUserName);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne<string>(query);
            }
        }

        public void UpdatePendingDisabilityClaim(int disabilityClaimKey, DateTime dateOfDiagnosis, int disabilityTypeKey, string otherDisabilityComments, string claimantOccupation,
            DateTime lastDateWorked, DateTime? expectedReturnToWorkDate)
        {
            var query = new UpdatePendingDisabilityClaimStatement(disabilityClaimKey, dateOfDiagnosis, disabilityTypeKey, otherDisabilityComments, 
                claimantOccupation, lastDateWorked, expectedReturnToWorkDate);

            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.ExecuteNonQuery(query);
                db.Complete();
            }
        }


        public string GetEmailAddressForUserWhoApprovedDisabilityClaim(int disabilityClaimKey)
        {
            var query = new GetEmailAddressForUserWhoApprovedDisabilityClaimStatement(disabilityClaimKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne<string>(query);
            }
        }

    }
}