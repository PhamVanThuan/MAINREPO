using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment
{
    public class AffordabilityAssessmentManager : IAffordabilityAssessmentManager
    {
        private IAffordabilityAssessmentDataManager affordabilityAssessmentDataManager;

        public AffordabilityAssessmentManager(IAffordabilityAssessmentDataManager affordabilityAssessmentDataManager)
        {
            this.affordabilityAssessmentDataManager = affordabilityAssessmentDataManager;
        }

        public void ConfirmAffordabilityAssessments(IEnumerable<AffordabilityAssessmentModel> affordabilityAssessmentModels)
        {
            foreach (AffordabilityAssessmentModel affordabilityAssessmentModel in affordabilityAssessmentModels)
            {
                affordabilityAssessmentDataManager.ConfirmAffordabilityAssessment(affordabilityAssessmentModel.Key);
            }
        }

        public void CopyAndArchiveAffordabilityAssessmentWithNewAffordabilityAssessmentItems(AffordabilityAssessmentModel affordabilityAssessment, int copiedByUserId)
        {
            AffordabilityAssessmentDataModel affordabilityAssessmentDataModel = affordabilityAssessmentDataManager.GetAffordabilityAssessmentByKey(affordabilityAssessment.Key);
            affordabilityAssessmentDataModel.GeneralStatusKey = (int)GeneralStatus.Inactive;
            affordabilityAssessmentDataManager.UpdateAffordabilityAssessment(affordabilityAssessmentDataModel);

            DateTime now = DateTime.Now;
            affordabilityAssessmentDataModel.AffordabilityAssessmentKey = 0;
            affordabilityAssessmentDataModel.ModifiedDate = now;
            affordabilityAssessmentDataModel.ModifiedByUserId = copiedByUserId;
            affordabilityAssessmentDataModel.AffordabilityAssessmentStatusKey = (int)AffordabilityAssessmentStatus.Unconfirmed;
            affordabilityAssessmentDataModel.GeneralStatusKey = (int)GeneralStatus.Active;
            affordabilityAssessmentDataModel.ConfirmedDate = null;
            affordabilityAssessmentDataManager.InsertAffordabilityAssessment(affordabilityAssessmentDataModel);

            AffordabilityAssessmentIncomeDetailModel incomeDetailModel = affordabilityAssessment.AffordabilityAssessmentDetail.Income;
            AffordabilityAssessmentIncomeDeductionsDetailModel incomeDeductionsDetailModel = affordabilityAssessment.AffordabilityAssessmentDetail.IncomeDeductions;
            AffordabilityAssessmentNecessaryExpensesDetailModel necessaryExpensesDetailModel = affordabilityAssessment.AffordabilityAssessmentDetail.NecessaryExpenses;
            AffordabilityAssessmentOtherExpensesDetailModel otherExpensesDetailModel = affordabilityAssessment.AffordabilityAssessmentDetail.OtherExpenses;
            AffordabilityAssessmentPaymentObligationDetailModel paymentObligationDetailModel = affordabilityAssessment.AffordabilityAssessmentDetail.PaymentObligations;
            AffordabilityAssessmentSAHLPaymentObligationDetailModel sahlPaymentObligationDetailModel = affordabilityAssessment.AffordabilityAssessmentDetail.SAHLPaymentObligations;

            List<AffordabilityAssessmentItemDataModel> affordabilityAssessmentItems = new List<AffordabilityAssessmentItemDataModel>();
            MapAffordabilityAssessment<AffordabilityAssessmentIncomeDetailModel, AffordabilityAssessmentItemDataModel, AffordabilityAssessmentItemModel>(incomeDetailModel,
                                                                                                                                                         affordabilityAssessmentItems,
                                                                                                                                                         MapAssessmentItemToDataModel);

            MapAffordabilityAssessment<AffordabilityAssessmentIncomeDeductionsDetailModel, AffordabilityAssessmentItemDataModel, AffordabilityAssessmentItemModel>(incomeDeductionsDetailModel,
                                                                                                                                                                affordabilityAssessmentItems,
                                                                                                                                                                MapAssessmentItemToDataModel);

            MapAffordabilityAssessment<AffordabilityAssessmentNecessaryExpensesDetailModel, AffordabilityAssessmentItemDataModel, AffordabilityAssessmentItemModel>(necessaryExpensesDetailModel,
                                                                                                                                                                affordabilityAssessmentItems,
                                                                                                                                                                MapAssessmentItemToDataModel);

            MapAffordabilityAssessment<AffordabilityAssessmentOtherExpensesDetailModel, AffordabilityAssessmentItemDataModel, AffordabilityAssessmentItemModel>(otherExpensesDetailModel,
                                                                                                                                                                affordabilityAssessmentItems,
                                                                                                                                                                MapAssessmentItemToDataModel);

            MapAffordabilityAssessment<AffordabilityAssessmentPaymentObligationDetailModel, AffordabilityAssessmentItemDataModel, AffordabilityAssessmentConsolidatableItemModel>(
                                                                                                                                                                paymentObligationDetailModel,
                                                                                                                                                                affordabilityAssessmentItems,
                                                                                                                                                                MapAssessmentItemToDataModel);

            MapAffordabilityAssessment<AffordabilityAssessmentSAHLPaymentObligationDetailModel, AffordabilityAssessmentItemDataModel, AffordabilityAssessmentItemModel>(
                                                                                                                                                                sahlPaymentObligationDetailModel,
                                                                                                                                                                affordabilityAssessmentItems,
                                                                                                                                                                MapAssessmentItemToDataModel);

            foreach (AffordabilityAssessmentItemDataModel affordabilityAssessmentItem in affordabilityAssessmentItems)
            {
                affordabilityAssessmentItem.AffordabilityAssessmentItemKey = 0;
                affordabilityAssessmentItem.AffordabilityAssessmentKey = affordabilityAssessmentDataModel.AffordabilityAssessmentKey;
                affordabilityAssessmentItem.ModifiedByUserId = copiedByUserId;
                affordabilityAssessmentItem.ModifiedDate = now;
                affordabilityAssessmentDataManager.InsertAffordabilityAssessmentItem(affordabilityAssessmentItem);
            }

            IEnumerable<int> incomeContributors = affordabilityAssessmentDataManager.GetAffordabilityAssessmentContributors(affordabilityAssessment.Key);
            foreach (int legalEntityKey in incomeContributors)
            {
                AffordabilityAssessmentLegalEntityDataModel assessmentLegalEntity = new AffordabilityAssessmentLegalEntityDataModel(affordabilityAssessmentDataModel.AffordabilityAssessmentKey,
                                                                                                                                    legalEntityKey);
                affordabilityAssessmentDataManager.InsertAffordabilityAssessmentLegalEntity(assessmentLegalEntity);
            }
        }

        public void CopyAndArchiveAffordabilityAssessmentWithNewIncomeContributors(AffordabilityAssessmentModel affordabilityAssessment, int copiedByUserId)
        {
            AffordabilityAssessmentDataModel affordabilityAssessmentDataModel = affordabilityAssessmentDataManager.GetAffordabilityAssessmentByKey(affordabilityAssessment.Key);
            affordabilityAssessmentDataModel.GeneralStatusKey = (int)GeneralStatus.Inactive;
            affordabilityAssessmentDataManager.UpdateAffordabilityAssessment(affordabilityAssessmentDataModel);

            DateTime now = DateTime.Now;
            affordabilityAssessmentDataModel.AffordabilityAssessmentKey = 0;
            affordabilityAssessmentDataModel.ModifiedDate = now;
            affordabilityAssessmentDataModel.ModifiedByUserId = copiedByUserId;
            affordabilityAssessmentDataModel.NumberOfContributingApplicants = affordabilityAssessment.NumberOfContributingApplicants;
            affordabilityAssessmentDataModel.NumberOfHouseholdDependants = affordabilityAssessment.NumberOfHouseholdDependants;
            affordabilityAssessmentDataModel.AffordabilityAssessmentStatusKey = (int)AffordabilityAssessmentStatus.Unconfirmed;
            affordabilityAssessmentDataModel.GeneralStatusKey = (int)GeneralStatus.Active;
            affordabilityAssessmentDataModel.ConfirmedDate = null;
            affordabilityAssessmentDataManager.InsertAffordabilityAssessment(affordabilityAssessmentDataModel);

            IEnumerable<AffordabilityAssessmentItemDataModel> affordabilityAssessmentItems = affordabilityAssessmentDataManager.GetAffordabilityAssessmentItems(affordabilityAssessment.Key);
            foreach (AffordabilityAssessmentItemDataModel affordabilityAssessmentItem in affordabilityAssessmentItems)
            {
                affordabilityAssessmentItem.AffordabilityAssessmentItemKey = 0;
                affordabilityAssessmentItem.AffordabilityAssessmentKey = affordabilityAssessmentDataModel.AffordabilityAssessmentKey;
                affordabilityAssessmentItem.ModifiedByUserId = copiedByUserId;
                affordabilityAssessmentItem.ModifiedDate = now;
                affordabilityAssessmentDataManager.InsertAffordabilityAssessmentItem(affordabilityAssessmentItem);
            }

            foreach (int legalEntityKey in affordabilityAssessment.ContributingApplicantLegalEntities)
            {
                AffordabilityAssessmentLegalEntityDataModel assessmentLegalEntity = new AffordabilityAssessmentLegalEntityDataModel(affordabilityAssessmentDataModel.AffordabilityAssessmentKey,
                                                                                                                                    legalEntityKey);
                affordabilityAssessmentDataManager.InsertAffordabilityAssessmentLegalEntity(assessmentLegalEntity);
            }
        }

        public int CreateAffordabilityAssessment(AffordabilityAssessmentModel affordabilityAssessment, int createdByUserId)
        {
            string defaultStressFactorPercentage = "0.5%";

            int defaultStressFactor = affordabilityAssessmentDataManager.GetAffordabilityAssessmentStressFactorKeyByStressFactorPercentage(defaultStressFactorPercentage);

            var affordabilityAssessmentDataModel = new AffordabilityAssessmentDataModel(affordabilityAssessment.GenericKey,
                                                                                        (int)GenericKeyType.Offer,
                                                                                        (int)AffordabilityAssessmentStatus.Unconfirmed,
                                                                                        (int)GeneralStatus.Active,
                                                                                        defaultStressFactor, // STRESS FACTOR KEY
                                                                                        DateTime.Now,
                                                                                        createdByUserId,
                                                                                        affordabilityAssessment.NumberOfContributingApplicants,
                                                                                        affordabilityAssessment.NumberOfHouseholdDependants,
                                                                                        null,
                                                                                        null,
                                                                                        null);

            affordabilityAssessmentDataManager.InsertAffordabilityAssessment(affordabilityAssessmentDataModel);
            return affordabilityAssessmentDataModel.AffordabilityAssessmentKey;
        }

        public void CreateBlankAffordabilityAssessmentItems(int affodabilityAssessmentKey, int userKey)
        {
            var itemDataModels = new List<AffordabilityAssessmentItemDataModel>();

            var incomeDetailModel = new AffordabilityAssessmentIncomeDetailModel();
            var incomeDeductionsDetailModel = new AffordabilityAssessmentIncomeDeductionsDetailModel();
            var necessaryExpensesDetailModel = new AffordabilityAssessmentNecessaryExpensesDetailModel();
            var otherExpensesDetailModel = new AffordabilityAssessmentOtherExpensesDetailModel();
            var paymentObligationDetailModel = new AffordabilityAssessmentPaymentObligationDetailModel();
            var sahlPaymentObligationDetailModel = new AffordabilityAssessmentSAHLPaymentObligationDetailModel();

            MapAffordabilityAssessment<AffordabilityAssessmentIncomeDetailModel, AffordabilityAssessmentItemDataModel, AffordabilityAssessmentItemModel>(incomeDetailModel,
                                                                                                                                                         itemDataModels,
                                                                                                                                                         MapAssessmentItemToDataModel);

            MapAffordabilityAssessment<AffordabilityAssessmentIncomeDeductionsDetailModel, AffordabilityAssessmentItemDataModel, AffordabilityAssessmentItemModel>(incomeDeductionsDetailModel,
                                                                                                                                                                itemDataModels,
                                                                                                                                                                MapAssessmentItemToDataModel);

            MapAffordabilityAssessment<AffordabilityAssessmentNecessaryExpensesDetailModel, AffordabilityAssessmentItemDataModel, AffordabilityAssessmentItemModel>(necessaryExpensesDetailModel,
                                                                                                                                                                itemDataModels,
                                                                                                                                                                MapAssessmentItemToDataModel);

            MapAffordabilityAssessment<AffordabilityAssessmentOtherExpensesDetailModel, AffordabilityAssessmentItemDataModel, AffordabilityAssessmentItemModel>(otherExpensesDetailModel,
                                                                                                                                                                itemDataModels,
                                                                                                                                                                MapAssessmentItemToDataModel);

            MapAffordabilityAssessment<AffordabilityAssessmentPaymentObligationDetailModel, AffordabilityAssessmentItemDataModel, AffordabilityAssessmentConsolidatableItemModel>(
                                                                                                                                                                paymentObligationDetailModel,
                                                                                                                                                                itemDataModels,
                                                                                                                                                                MapAssessmentItemToDataModel);

            MapAffordabilityAssessment<AffordabilityAssessmentSAHLPaymentObligationDetailModel, AffordabilityAssessmentItemDataModel, AffordabilityAssessmentItemModel>(
                                                                                                                                                                sahlPaymentObligationDetailModel,
                                                                                                                                                                itemDataModels,
                                                                                                                                                                MapAssessmentItemToDataModel);

            foreach (var item in itemDataModels)
            {
                item.AffordabilityAssessmentKey = affodabilityAssessmentKey;
                item.ModifiedByUserId = userKey;
                item.ModifiedDate = DateTime.Now;
                affordabilityAssessmentDataManager.InsertAffordabilityAssessmentItem(item);
            }
        }

        public void DeleteUnconfirmedAffordabilityAssessment(int affordabilityAssessmentKey)
        {
            affordabilityAssessmentDataManager.DeleteUnconfirmedAffordabilityAssessment(affordabilityAssessmentKey);
        }

        public AffordabilityAssessmentModel GetAffordabilityAssessment(int affordabilityAssessmentKey)
        {
            var summary = this.affordabilityAssessmentDataManager.GetAffordabilityAssessmentSummary(affordabilityAssessmentKey);
            var affordabilityAssessmentItems = this.affordabilityAssessmentDataManager.GetAffordabilityAssessmentItems(affordabilityAssessmentKey).ToList();
            IEnumerable<int> contributors = this.affordabilityAssessmentDataManager.GetAffordabilityAssessmentContributors(affordabilityAssessmentKey);

            var income = new AffordabilityAssessmentIncomeDetailModel();
            MapAffordabilityAssessment<AffordabilityAssessmentIncomeDetailModel, AffordabilityAssessmentItemDataModel, AffordabilityAssessmentItemModel>(
                                                                                                                                                income,
                                                                                                                                                affordabilityAssessmentItems,
                                                                                                                                                MapAffordabilityAssessmentItem);

            var incomeDeductions = new AffordabilityAssessmentIncomeDeductionsDetailModel();
            MapAffordabilityAssessment<AffordabilityAssessmentIncomeDeductionsDetailModel, AffordabilityAssessmentItemDataModel, AffordabilityAssessmentItemModel>(
                                                                                                                                                incomeDeductions,
                                                                                                                                                affordabilityAssessmentItems,
                                                                                                                                                MapAffordabilityAssessmentItem);

            var necessaryExpenses = new AffordabilityAssessmentNecessaryExpensesDetailModel();
            MapAffordabilityAssessment<AffordabilityAssessmentNecessaryExpensesDetailModel, AffordabilityAssessmentItemDataModel, AffordabilityAssessmentItemModel>(
                                                                                                                                                necessaryExpenses,
                                                                                                                                                affordabilityAssessmentItems,
                                                                                                                                                MapAffordabilityAssessmentItem);

            var paymentObligations = new AffordabilityAssessmentPaymentObligationDetailModel();
            MapAffordabilityAssessment<AffordabilityAssessmentPaymentObligationDetailModel, AffordabilityAssessmentItemDataModel, AffordabilityAssessmentConsolidatableItemModel>(
                                                                                                                                                paymentObligations,
                                                                                                                                                affordabilityAssessmentItems,
                                                                                                                                                MapAffordabilityAssessmentConsolidatableItem);

            var sahlPaymentObligations = new AffordabilityAssessmentSAHLPaymentObligationDetailModel();
            MapAffordabilityAssessment<AffordabilityAssessmentSAHLPaymentObligationDetailModel, AffordabilityAssessmentItemDataModel, AffordabilityAssessmentItemModel>(
                                                                                                                                                sahlPaymentObligations,
                                                                                                                                                affordabilityAssessmentItems,
                                                                                                                                                MapAffordabilityAssessmentItem);

            var otherExpenses = new AffordabilityAssessmentOtherExpensesDetailModel();
            MapAffordabilityAssessment<AffordabilityAssessmentOtherExpensesDetailModel, AffordabilityAssessmentItemDataModel, AffordabilityAssessmentItemModel>(
                                                                                                                                                otherExpenses,
                                                                                                                                                affordabilityAssessmentItems,
                                                                                                                                                MapAffordabilityAssessmentItem);

            var detail = new AffordabilityAssessmentDetailModel(income, incomeDeductions, necessaryExpenses, paymentObligations, sahlPaymentObligations,
                                                                otherExpenses, summary.StressFactorPercentageDisplay, summary.PercentageIncreaseOnRepayments,
                                                                summary.MinimumMonthlyFixedExpenses.HasValue ? summary.MinimumMonthlyFixedExpenses.Value : 0);

            AffordabilityAssessmentStatus affordabilityAssessmentStatus = (AffordabilityAssessmentStatus)summary.AffordabilityAssessmentStatusKey;
            var afforabilityAssessment = new AffordabilityAssessmentModel(summary.Key, summary.GenericKey, affordabilityAssessmentStatus, summary.DateLastAmended,
                                                                          summary.HouseholdDependants, summary.ContributingApplicants, summary.StressFactorKey, contributors, detail,
                                                                          summary.DateConfirmed);

            return afforabilityAssessment;
        }

        public IEnumerable<AffordabilityAssessmentModel> GetApplicationAffordabilityAssessments(int applicationKey)
        {
            IEnumerable<AffordabilityAssessmentDataModel> affordabilityAssessmentDataModels = affordabilityAssessmentDataManager.GetActiveAffordabilityAssessmentsForApplication(applicationKey);
            IList<AffordabilityAssessmentModel> affordabilityAssessments = new List<AffordabilityAssessmentModel>(affordabilityAssessmentDataModels.Count());
            foreach (AffordabilityAssessmentDataModel affordabilityAssessmentDataModel in affordabilityAssessmentDataModels)
            {
                List<AffordabilityAssessmentItemDataModel> AffordabilityAssessmentItemDataModels =
                                this.affordabilityAssessmentDataManager.GetAffordabilityAssessmentItems(affordabilityAssessmentDataModel.AffordabilityAssessmentKey).ToList();

                IEnumerable<int> contributors = this.affordabilityAssessmentDataManager.GetAffordabilityAssessmentContributors(affordabilityAssessmentDataModel.AffordabilityAssessmentKey);

                AffordabilityAssessmentIncomeDetailModel income = new AffordabilityAssessmentIncomeDetailModel();
                MapAffordabilityAssessment<AffordabilityAssessmentIncomeDetailModel, AffordabilityAssessmentItemDataModel, AffordabilityAssessmentItemModel>(income,
                                                                                                                                                            AffordabilityAssessmentItemDataModels,
                                                                                                                                                            MapAffordabilityAssessmentItem);

                AffordabilityAssessmentIncomeDeductionsDetailModel incomeDeductions = new AffordabilityAssessmentIncomeDeductionsDetailModel();
                MapAffordabilityAssessment<AffordabilityAssessmentIncomeDeductionsDetailModel, AffordabilityAssessmentItemDataModel, AffordabilityAssessmentItemModel>(
                                                                                                                                                            incomeDeductions,
                                                                                                                                                            AffordabilityAssessmentItemDataModels,
                                                                                                                                                            MapAffordabilityAssessmentItem);

                AffordabilityAssessmentNecessaryExpensesDetailModel necessaryExpenses = new AffordabilityAssessmentNecessaryExpensesDetailModel();
                MapAffordabilityAssessment<AffordabilityAssessmentNecessaryExpensesDetailModel, AffordabilityAssessmentItemDataModel, AffordabilityAssessmentItemModel>(
                                                                                                                                                            necessaryExpenses,
                                                                                                                                                            AffordabilityAssessmentItemDataModels,
                                                                                                                                                            MapAffordabilityAssessmentItem);

                AffordabilityAssessmentPaymentObligationDetailModel paymentObligations = new AffordabilityAssessmentPaymentObligationDetailModel();
                MapAffordabilityAssessment<AffordabilityAssessmentPaymentObligationDetailModel, AffordabilityAssessmentItemDataModel, AffordabilityAssessmentConsolidatableItemModel>(
                                                                                                                                                   paymentObligations,
                                                                                                                                                   AffordabilityAssessmentItemDataModels,
                                                                                                                                                   MapAffordabilityAssessmentConsolidatableItem);

                AffordabilityAssessmentSAHLPaymentObligationDetailModel sahlPaymentObligations = new AffordabilityAssessmentSAHLPaymentObligationDetailModel();
                MapAffordabilityAssessment<AffordabilityAssessmentSAHLPaymentObligationDetailModel, AffordabilityAssessmentItemDataModel, AffordabilityAssessmentItemModel>(
                                                                                                                                                   sahlPaymentObligations,
                                                                                                                                                   AffordabilityAssessmentItemDataModels,
                                                                                                                                                   MapAffordabilityAssessmentItem);

                AffordabilityAssessmentOtherExpensesDetailModel otherExpenses = new AffordabilityAssessmentOtherExpensesDetailModel();
                MapAffordabilityAssessment<AffordabilityAssessmentOtherExpensesDetailModel, AffordabilityAssessmentItemDataModel, AffordabilityAssessmentItemModel>(
                                                                                                                                                   otherExpenses,
                                                                                                                                                   AffordabilityAssessmentItemDataModels,
                                                                                                                                                   MapAffordabilityAssessmentItem);

                Tuple<decimal, string> affordabilityAssessmentStressFactor =
                                        affordabilityAssessmentDataManager.GetAffordabilityAssessmentStressFactorByKey(affordabilityAssessmentDataModel.AffordabilityAssessmentStressFactorKey);

                AffordabilityAssessmentDetailModel detail =
                    new AffordabilityAssessmentDetailModel(
                                        income,
                                        incomeDeductions,
                                        necessaryExpenses,
                                        paymentObligations,
                                        sahlPaymentObligations,
                                        otherExpenses,
                                        affordabilityAssessmentStressFactor.Item2,
                                        (double)affordabilityAssessmentStressFactor.Item1,
                                        affordabilityAssessmentDataModel.MinimumMonthlyFixedExpenses.HasValue ? affordabilityAssessmentDataModel.MinimumMonthlyFixedExpenses.Value : 0);

                AffordabilityAssessmentStatus affordabilityAssessmentStatus = (AffordabilityAssessmentStatus)affordabilityAssessmentDataModel.AffordabilityAssessmentStatusKey;
                AffordabilityAssessmentModel afforabilityAssessment =
                    new AffordabilityAssessmentModel(affordabilityAssessmentDataModel.AffordabilityAssessmentKey,
                                                     affordabilityAssessmentDataModel.GenericKey,
                                                     affordabilityAssessmentStatus,
                                                     affordabilityAssessmentDataModel.ModifiedDate,
                                                     affordabilityAssessmentDataModel.NumberOfHouseholdDependants,
                                                     affordabilityAssessmentDataModel.NumberOfContributingApplicants,
                                                     affordabilityAssessmentDataModel.AffordabilityAssessmentStressFactorKey,
                                                     contributors,
                                                     detail,
                                                     affordabilityAssessmentDataModel.ConfirmedDate);
                affordabilityAssessments.Add(afforabilityAssessment);
            }
            return affordabilityAssessments;
        }

        public double? GetBondInstalmentForFurtherLendingApplication(int applicationKey)
        {
            return affordabilityAssessmentDataManager.GetBondInstalmentForFurtherLendingApplication(applicationKey);
        }

        public double? GetBondInstalmentForNewBusinessApplication(int applicationKey)
        {
            return affordabilityAssessmentDataManager.GetBondInstalmentForNewBusinessApplication(applicationKey);
        }

        public double? GetHocInstalmentForFurtherLendingApplication(int applicationKey)
        {
            return affordabilityAssessmentDataManager.GetHocInstalmentForFurtherLendingApplication(applicationKey);
        }

        public double? GetHocInstalmentForNewBusinessApplication(int applicationKey)
        {
            return affordabilityAssessmentDataManager.GetHocInstalmentForNewBusinessApplication(applicationKey);
        }

        public int LinkLegalEntityToAffordabilityAssessment(int affordabilityAssessmentKey, int applicantLegalEntityKey)
        {
            var assessmentLegalEntityDataModel = new AffordabilityAssessmentLegalEntityDataModel(affordabilityAssessmentKey, applicantLegalEntityKey);
            affordabilityAssessmentDataManager.InsertAffordabilityAssessmentLegalEntity(assessmentLegalEntityDataModel);
            return assessmentLegalEntityDataModel.AffordabilityAssessmentLegalEntityKey;
        }

        public void UpdateAffordabilityAssessmentAndAffordabilityAssessmentItems(AffordabilityAssessmentModel affordabilityAssessment, int updatedByUserId)
        {
            DateTime now = DateTime.Now;
            AffordabilityAssessmentDataModel affordabilityAssessmentDataModel = affordabilityAssessmentDataManager.GetAffordabilityAssessmentByKey(affordabilityAssessment.Key);
            affordabilityAssessmentDataModel.AffordabilityAssessmentStressFactorKey = affordabilityAssessment.StressFactorKey;
            affordabilityAssessmentDataModel.MinimumMonthlyFixedExpenses = affordabilityAssessment.AffordabilityAssessmentDetail.MinimumMonthlyFixedExpenses;
            affordabilityAssessmentDataModel.ModifiedDate = now;
            affordabilityAssessmentDataModel.ModifiedByUserId = updatedByUserId;
            affordabilityAssessmentDataManager.UpdateAffordabilityAssessment(affordabilityAssessmentDataModel);

            AffordabilityAssessmentIncomeDetailModel incomeDetailModel = affordabilityAssessment.AffordabilityAssessmentDetail.Income;
            AffordabilityAssessmentIncomeDeductionsDetailModel incomeDeductionsDetailModel = affordabilityAssessment.AffordabilityAssessmentDetail.IncomeDeductions;
            AffordabilityAssessmentNecessaryExpensesDetailModel necessaryExpensesDetailModel = affordabilityAssessment.AffordabilityAssessmentDetail.NecessaryExpenses;
            AffordabilityAssessmentOtherExpensesDetailModel otherExpensesDetailModel = affordabilityAssessment.AffordabilityAssessmentDetail.OtherExpenses;
            AffordabilityAssessmentPaymentObligationDetailModel paymentObligationDetailModel = affordabilityAssessment.AffordabilityAssessmentDetail.PaymentObligations;
            AffordabilityAssessmentSAHLPaymentObligationDetailModel sahlPaymentObligationDetailModel = affordabilityAssessment.AffordabilityAssessmentDetail.SAHLPaymentObligations;

            List<AffordabilityAssessmentItemDataModel> affordabilityAssessmentItems = new List<AffordabilityAssessmentItemDataModel>();
            MapAffordabilityAssessment<AffordabilityAssessmentIncomeDetailModel, AffordabilityAssessmentItemDataModel, AffordabilityAssessmentItemModel>(incomeDetailModel,
                                                                                                                                                         affordabilityAssessmentItems,
                                                                                                                                                         MapAssessmentItemToDataModel);

            MapAffordabilityAssessment<AffordabilityAssessmentIncomeDeductionsDetailModel, AffordabilityAssessmentItemDataModel, AffordabilityAssessmentItemModel>(incomeDeductionsDetailModel,
                                                                                                                                                                affordabilityAssessmentItems,
                                                                                                                                                                MapAssessmentItemToDataModel);

            MapAffordabilityAssessment<AffordabilityAssessmentNecessaryExpensesDetailModel, AffordabilityAssessmentItemDataModel, AffordabilityAssessmentItemModel>(necessaryExpensesDetailModel,
                                                                                                                                                                affordabilityAssessmentItems,
                                                                                                                                                                MapAssessmentItemToDataModel);

            MapAffordabilityAssessment<AffordabilityAssessmentOtherExpensesDetailModel, AffordabilityAssessmentItemDataModel, AffordabilityAssessmentItemModel>(otherExpensesDetailModel,
                                                                                                                                                                affordabilityAssessmentItems,
                                                                                                                                                                MapAssessmentItemToDataModel);

            MapAffordabilityAssessment<AffordabilityAssessmentPaymentObligationDetailModel, AffordabilityAssessmentItemDataModel, AffordabilityAssessmentConsolidatableItemModel>(
                                                                                                                                                                paymentObligationDetailModel,
                                                                                                                                                                affordabilityAssessmentItems,
                                                                                                                                                                MapAssessmentConsolidatableItemToDataModel);

            MapAffordabilityAssessment<AffordabilityAssessmentSAHLPaymentObligationDetailModel, AffordabilityAssessmentItemDataModel, AffordabilityAssessmentItemModel>(
                                                                                                                                                                sahlPaymentObligationDetailModel,
                                                                                                                                                                affordabilityAssessmentItems,
                                                                                                                                                                MapAssessmentItemToDataModel);

            foreach (AffordabilityAssessmentItemDataModel affordabilityAssessmentItem in affordabilityAssessmentItems)
            {
                affordabilityAssessmentItem.ModifiedByUserId = updatedByUserId;
                affordabilityAssessmentItem.ModifiedDate = now;
                affordabilityAssessmentDataManager.UpdateAffordabilityAssessmentItem(affordabilityAssessmentItem);
            }
        }

        public void UpdateAffordabilityAssessmentAndIncomeContributors(AffordabilityAssessmentModel affordabilityAssessment, int updatedByUserId)
        {
            AffordabilityAssessmentDataModel affordabilityAssessmentDataModel = affordabilityAssessmentDataManager.GetAffordabilityAssessmentByKey(affordabilityAssessment.Key);
            affordabilityAssessmentDataModel.NumberOfContributingApplicants = affordabilityAssessment.NumberOfContributingApplicants;
            affordabilityAssessmentDataModel.NumberOfHouseholdDependants = affordabilityAssessment.NumberOfHouseholdDependants;
            affordabilityAssessmentDataModel.ModifiedDate = DateTime.Now;
            affordabilityAssessmentDataModel.ModifiedByUserId = updatedByUserId;
            affordabilityAssessmentDataManager.UpdateAffordabilityAssessment(affordabilityAssessmentDataModel);

            IEnumerable<int> currentIncomeContributors = affordabilityAssessmentDataManager.GetAffordabilityAssessmentContributors(affordabilityAssessment.Key);
            foreach (int legalEntityKey in currentIncomeContributors)
            {
                if (!affordabilityAssessment.ContributingApplicantLegalEntities.Contains(legalEntityKey))
                {
                    affordabilityAssessmentDataManager.DeleteAffordabilityAssessmentLegalEntity(affordabilityAssessment.Key, legalEntityKey);
                }
            }

            foreach (int legalEntityKey in affordabilityAssessment.ContributingApplicantLegalEntities)
            {
                if (!currentIncomeContributors.Contains(legalEntityKey))
                {
                    affordabilityAssessmentDataManager.InsertAffordabilityAssessmentLegalEntity(new AffordabilityAssessmentLegalEntityDataModel(affordabilityAssessment.Key, legalEntityKey));
                }
            }
        }

        private static void MapAssessmentItemToDataModel(List<AffordabilityAssessmentItemDataModel> items, AffordabilityAssessmentItemModel model)
        {
            AffordabilityAssessmentItemDataModel affordabilityAssessmentItemDataModel = new AffordabilityAssessmentItemDataModel(
                            model.Key,
                            model.AffordabilityAssessmentKey,
                            (int)model.AffordabilityAssessmentItemType,
                            model.ModifiedDate,
                            model.ModifiedByUserId,
                            model.ClientValue,
                            model.CreditValue,
                            null,
                            model.ItemNotes);
            items.Add(affordabilityAssessmentItemDataModel);
        }

        private static void MapAssessmentConsolidatableItemToDataModel(List<AffordabilityAssessmentItemDataModel> items, AffordabilityAssessmentConsolidatableItemModel model)
        {
            AffordabilityAssessmentItemDataModel affordabilityAssessmentItemDataModel = new AffordabilityAssessmentItemDataModel(
                            model.Key,
                            model.AffordabilityAssessmentKey,
                            (int)model.AffordabilityAssessmentItemType,
                            model.ModifiedDate,
                            model.ModifiedByUserId,
                            model.ClientValue,
                            model.CreditValue,
                            model.ConsolidationValue,
                            model.ItemNotes);
            items.Add(affordabilityAssessmentItemDataModel);
        }

        private void MapAffordabilityAssessment<T, S, D>(T model, List<S> items, Action<List<S>, D> resolver)
        {
            Type myType = model.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

            foreach (PropertyInfo prop in props)
            {
                if (prop.PropertyType == typeof(D))
                {
                    var propValue = (D)prop.GetValue(model, null);
                    if (propValue != null)
                    {
                        resolver(items, propValue);
                    }
                }
            }
        }

        private void MapAffordabilityAssessmentConsolidatableItem(List<AffordabilityAssessmentItemDataModel> items, AffordabilityAssessmentConsolidatableItemModel model)
        {
            int itemTypeKey = (int)model.AffordabilityAssessmentItemType;
            var itemToMapTo = items.Where(x => x.AffordabilityAssessmentItemTypeKey == itemTypeKey).FirstOrDefault();
            if (itemToMapTo != null)
            {
                model.Key = itemToMapTo.AffordabilityAssessmentItemKey;
                model.AffordabilityAssessmentKey = itemToMapTo.AffordabilityAssessmentKey;
                model.ClientValue = itemToMapTo.ClientValue;
                model.CreditValue = itemToMapTo.CreditValue;
                model.ConsolidationValue = itemToMapTo.DebtToConsolidateValue;
                model.ItemNotes = itemToMapTo.ItemNotes;
            }
        }

        private void MapAffordabilityAssessmentItem(List<AffordabilityAssessmentItemDataModel> items, AffordabilityAssessmentItemModel model)
        {
            int itemTypeKey = (int)model.AffordabilityAssessmentItemType;
            var itemToMapTo = items.Where(x => x.AffordabilityAssessmentItemTypeKey == itemTypeKey).FirstOrDefault();
            if (itemToMapTo != null)
            {
                model.Key = itemToMapTo.AffordabilityAssessmentItemKey;
                model.AffordabilityAssessmentKey = itemToMapTo.AffordabilityAssessmentKey;
                model.ClientValue = itemToMapTo.ClientValue;
                model.CreditValue = itemToMapTo.CreditValue;
                model.ItemNotes = itemToMapTo.ItemNotes;
            }
        }
    }
}