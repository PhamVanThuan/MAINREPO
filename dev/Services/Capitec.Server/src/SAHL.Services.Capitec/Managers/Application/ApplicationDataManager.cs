using System;
using System.Collections.Generic;
using System.Linq;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Data.Models.Capitec.Statements;
using SAHL.Services.Capitec.Managers.Application.Models;
using SAHL.Services.Capitec.Managers.Application.Statements;

namespace SAHL.Services.Capitec.Managers.Application
{
    public class ApplicationDataManager : IApplicationDataManager
    {
        private IDbFactory dbFactory;

        public ApplicationDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public void AddApplication(Guid applicationID, DateTime dateTime, int applicationNumber, Guid applicationPurposeEnumId, Guid userId, DateTime captureStartTime, Guid branchId)
        {
            ApplicationDataModel application = new ApplicationDataModel(applicationID, dateTime, applicationPurposeEnumId, applicationNumber, userId, null, null,
                new Guid(ApplicationStatusEnumDataModel.IN_PROGRESS), dateTime, null, null, captureStartTime, null, branchId);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<ApplicationDataModel>(application);
                db.Complete();
            }
        }

        public int GetNextApplicationNumber()
        {
            GetNextApplicationNumberQuery query = new GetNextApplicationNumberQuery();
            ReservedApplicationNumberDataModel applicationNumberDataModel = null;
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                applicationNumberDataModel = db.SelectOne<ReservedApplicationNumberDataModel>(query);
            }
            if (applicationNumberDataModel != null)
            {
                ReservedApplicationNumberDataModel reservedApplicationNumberDataModel = new ReservedApplicationNumberDataModel(applicationNumberDataModel.ApplicationNumber, true);
                using (var db = this.dbFactory.NewDb().InAppContext())
                {
                    db.Update<ReservedApplicationNumberDataModel>(reservedApplicationNumberDataModel);
                    db.Complete();
                }
                return applicationNumberDataModel.ApplicationNumber;
            }
            throw new NullReferenceException("No reserved application numbers are available.");
        }

        public void AddApplicationLoanDetail(Guid applicationID, Guid applicationLoanDetailID, Guid employmentTypeID, Guid occupancyTypeEnumID, decimal householdIncome, decimal instalment, decimal interestRate, decimal loanAmount, decimal ltv, decimal pti, decimal fees, int termInMonths, bool capitaliseFees)
        {
            ApplicationLoanDetailDataModel applicationLoanDetail = new ApplicationLoanDetailDataModel(applicationLoanDetailID, applicationID, employmentTypeID, occupancyTypeEnumID, householdIncome, instalment, interestRate, loanAmount, ltv, pti, fees, termInMonths, capitaliseFees);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<ApplicationLoanDetailDataModel>(applicationLoanDetail);
                db.Complete();
            }
        }

        public void AddNewPurchaseApplicationLoanDetail(Guid applicationLoanDetailID, decimal purchasePrice, decimal deposit)
        {
            NewPurchaseApplicationLoanDetailDataModel applicationLoanDetail = new NewPurchaseApplicationLoanDetailDataModel(applicationLoanDetailID, deposit, purchasePrice);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<NewPurchaseApplicationLoanDetailDataModel>(applicationLoanDetail);
                db.Complete();
            }
        }

        public void AddSwitchApplicationLoanDetail(Guid applicationLoanDetailID, decimal cashRequired, decimal currentBalance, decimal estimatedMarketValueOfTheHome, decimal interimInterest)
        {
            SwitchApplicationLoanDetailDataModel applicationLoanDetail = new SwitchApplicationLoanDetailDataModel(applicationLoanDetailID, cashRequired, currentBalance, estimatedMarketValueOfTheHome, interimInterest);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<SwitchApplicationLoanDetailDataModel>(applicationLoanDetail);
                db.Complete();
            }
        }

        public List<ApplicantModel> GetApplicantsForApplication(Guid applicationID)
        {
            var query = new GetApplicantsForApplicationQuery(applicationID);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.Select<ApplicantModel>(query).ToList();
            }
        }

        public ApplicationDataModel GetApplicationByID(Guid guid)
        {
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.GetByKey<ApplicationDataModel, Guid>(guid);
            }
        }

        public void SetApplicationStatus(Guid applicationID, Guid applicationStatusID, DateTime statusChangeDate)
        {
            var sql = new SetApplicationStatusStatement(applicationID, applicationStatusID, statusChangeDate);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Update<ApplicationDataModel>(sql);
                db.Complete();
            }
        }

        public bool DoesApplicationExist(Guid applicationID)
        {
            var sql = new GetApplicationExistsQuery(applicationID);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var result = db.SelectOne(sql);
                bool exists = result == 1;
                return exists;
            }
        }

        public void UpdateCaptureEndTime(Guid applicationID, DateTime captureEndTime)
        {
            var sql = new SetApplicationCaptureEndTime(applicationID, captureEndTime);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Update<ApplicationDataModel>(sql);
                db.Complete();
            }
        }
    }
}