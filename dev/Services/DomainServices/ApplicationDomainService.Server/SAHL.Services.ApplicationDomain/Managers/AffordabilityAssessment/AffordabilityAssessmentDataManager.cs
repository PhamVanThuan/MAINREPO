using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment.Statements;
using SAHL.Services.ApplicationDomain.Managers.Application.Statements;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment
{
    public class AffordabilityAssessmentDataManager : IAffordabilityAssessmentDataManager
    {
        private IDbFactory dbFactory;

        public AffordabilityAssessmentDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public void InsertAffordabilityAssessment(AffordabilityAssessmentDataModel affordabilityAssessmentDataModel)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert(affordabilityAssessmentDataModel);
                db.Complete();
            }
        }

        public void UpdateAffordabilityAssessment(AffordabilityAssessmentDataModel affordabilityAssessmentDataModel)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Update(affordabilityAssessmentDataModel);
                db.Complete();
            }
        }

        public IEnumerable<AffordabilityAssessmentItemDataModel> GetAffordabilityAssessmentItems(int affordabilityAssessmentKey)
        {
            GetAffordabilityAssessmentItemsStatement statement = new GetAffordabilityAssessmentItemsStatement(affordabilityAssessmentKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var results = db.Select<AffordabilityAssessmentItemDataModel>(statement);
                return results;
            }
        }

        public AffordabilityAssessmentSummaryModel GetAffordabilityAssessmentSummary(int affordabilityAssessmentKey)
        {
            GetAffordabilityAssessmentSummaryStatement affordabilityAssessmentSummaryStatement = new GetAffordabilityAssessmentSummaryStatement(affordabilityAssessmentKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var result = db.SelectOne<AffordabilityAssessmentSummaryModel>(affordabilityAssessmentSummaryStatement);
                return result;
            }
        }

        public IEnumerable<int> GetAffordabilityAssessmentContributors(int affordabilityAssessmentKey)
        {
            GetAffordabilityAssessmentContributorsStatement affordabilityAssessmentContributorsStatement = new GetAffordabilityAssessmentContributorsStatement(affordabilityAssessmentKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var result = db.Select<int>(affordabilityAssessmentContributorsStatement);
                return result;
            }
        }

        public AffordabilityAssessmentDataModel GetAffordabilityAssessmentByKey(int affordabilityAssessmentKey)
        {
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.GetByKey<AffordabilityAssessmentDataModel, int>(affordabilityAssessmentKey);
            }
        }

        public AffordabilityAssessmentDataModel GetLatestConfirmedAffordabilityAssessmentByGenericKey(int genericKey, int genericKeyTypeKey)
        {
            GetLatestConfirmedAffordabilityAssessmentByGenericKeyStatement queryStatement = new GetLatestConfirmedAffordabilityAssessmentByGenericKeyStatement(genericKey, genericKeyTypeKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne<AffordabilityAssessmentDataModel>(queryStatement);
            }
        }

        public void DeleteUnconfirmedAffordabilityAssessment(int affordabilityAssessmentKey)
        {
            using (IDbContext db = this.dbFactory.NewDb().InAppContext())
            {
                object affordabilityAssessmentKeyParam = new { AffordabilityAssessmentKey = affordabilityAssessmentKey };
                db.DeleteWhere<AffordabilityAssessmentLegalEntityDataModel>("AffordabilityAssessmentKey = @AffordabilityAssessmentKey", affordabilityAssessmentKeyParam);
                db.DeleteWhere<AffordabilityAssessmentItemDataModel>("AffordabilityAssessmentKey = @AffordabilityAssessmentKey", affordabilityAssessmentKeyParam);
                db.DeleteByKey<AffordabilityAssessmentDataModel, int>(affordabilityAssessmentKey);
                db.Complete();
            }
        }

        public int GetAffordabilityAssessmentStressFactorKeyByStressFactorPercentage(string percentage)
        {
            GetAssessmentStressFactorByPercentageStatement query = new GetAssessmentStressFactorByPercentageStatement(percentage);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var results = db.SelectOne<AffordabilityAssessmentStressFactorDataModel>(query);
                return results.AffordabilityAssessmentStressFactorKey;
            }
        }

        public void InsertAffordabilityAssessmentLegalEntity(AffordabilityAssessmentLegalEntityDataModel affordabilityAssessmentLegalEntityDataModel)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert(affordabilityAssessmentLegalEntityDataModel);
                db.Complete();
            }
        }

        public void InsertAffordabilityAssessmentItem(AffordabilityAssessmentItemDataModel itemDataModel)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert(itemDataModel);
                db.Complete();
            }
        }

        public void DeleteAffordabilityAssessmentLegalEntity(int affordabilityAssessmentKey, int legalEntityKey)
        {
            using (IDbContext db = this.dbFactory.NewDb().InAppContext())
            {
                object fields = new { AffordabilityAssessmentKey = affordabilityAssessmentKey, LegalEntityKey = legalEntityKey };
                db.DeleteWhere<AffordabilityAssessmentLegalEntityDataModel>("AffordabilityAssessmentKey = @AffordabilityAssessmentKey AND LegalEntityKey = @LegalEntityKey", fields);
                db.Complete();
            }
        }

        public void UpdateAffordabilityAssessmentItem(AffordabilityAssessmentItemDataModel affordabilityAssessmentItemDataModel)
        {
            using (IDbContext db = this.dbFactory.NewDb().InAppContext())
            {
                db.Update<AffordabilityAssessmentItemDataModel>(affordabilityAssessmentItemDataModel);
                db.Complete();
            }
        }

        public double? GetBondInstalmentForNewBusinessApplication(int applicationKey)
        {
            GetBondInstalmentForNewBusinessApplicationStatement query = new GetBondInstalmentForNewBusinessApplicationStatement(applicationKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                double? results = db.SelectOne<double?>(query);
                return results;
            }
        }

        public double? GetBondInstalmentForFurtherLendingApplication(int applicationKey)
        {
            GetBondInstalmentForFurtherLendingApplicationStatement query = new GetBondInstalmentForFurtherLendingApplicationStatement(applicationKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                double? results = db.SelectOne<double?>(query);
                return results;
            }
        }

        public double? GetHocInstalmentForNewBusinessApplication(int applicationKey)
        {
            GetHocInstalmentForNewBusinessApplicationStatement query = new GetHocInstalmentForNewBusinessApplicationStatement(applicationKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                double? results = db.SelectOne<double?>(query);
                return results;
            }
        }

        public double? GetHocInstalmentForFurtherLendingApplication(int applicationKey)
        {
            GetHocInstalmentForFurtherLendingApplicationStatement query = new GetHocInstalmentForFurtherLendingApplicationStatement(applicationKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                double? results = db.SelectOne<double?>(query);
                return results;
            }
        }

        public void ArchiveAffordabilityAssessment(int affordabilityAssessmentKey, int _ADUserKey)
        {
            ArchiveAffordabilityAssessmentStatement archiveAffordabilityAssessmentStatement = new ArchiveAffordabilityAssessmentStatement(affordabilityAssessmentKey, _ADUserKey);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Update<AffordabilityAssessmentDataModel>(archiveAffordabilityAssessmentStatement);
                db.Complete();
            }
        }

        public void ConfirmAffordabilityAssessment(int affordabilityAssessmentKey)
        {
            var query = new ConfirmAffordabilityAssessmentStatement(affordabilityAssessmentKey);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Update<AffordabilityAssessmentDataModel>(query);
                db.Complete();
            }
        }

        public IEnumerable<AffordabilityAssessmentDataModel> GetActiveAffordabilityAssessmentsForApplication(int applicationKey)
        {
            GetActiveAffordabilityAssessmentsByGenericKeyStatement statement = new GetActiveAffordabilityAssessmentsByGenericKeyStatement(applicationKey, (int)GenericKeyType.Offer);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var results = db.Select<AffordabilityAssessmentDataModel>(statement);
                return results;
            }
        }

        public Tuple<decimal, string> GetAffordabilityAssessmentStressFactorByKey(int affordabilityAssessmentStressFactorKey)
        {
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                object fields = new { AffordabilityAssessmentStressFactorKey = affordabilityAssessmentStressFactorKey };
                AffordabilityAssessmentStressFactorDataModel results =
                    db.SelectOneWhere<AffordabilityAssessmentStressFactorDataModel>("AffordabilityAssessmentStressFactorKey = @AffordabilityAssessmentStressFactorKey", fields);
                return new Tuple<decimal, string>(results.PercentageIncreaseOnRepayments, results.StressFactorPercentage);
            }
        }
    }
}