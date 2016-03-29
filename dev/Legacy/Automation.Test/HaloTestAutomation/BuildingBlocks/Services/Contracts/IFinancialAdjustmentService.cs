using Automation.DataAccess;
using Common.Enums;
using System.Collections.Generic;

namespace BuildingBlocks.Services.Contracts
{
    public interface IFinancialAdjustmentService
    {
        QueryResults GetFinAdjustmentByAccountFinAdjustmentTypeAndStatus(int AccountKey, FinancialAdjustmentTypeSourceEnum financialAdjustmentTypeSource, FinancialAdjustmentStatusEnum status);

        void StartFinancialAdjustment(int accountKey, FinancialAdjustmentTypeSourceEnum finAdjustmentTypeSource, double discount);

        void CancelFinancialAdjustments(int accountKey);

        QueryResults GetFinancialAdjustmentsByTypeAndStatus(FinancialAdjustmentTypeSourceEnum fAdjTypeSource, FinancialAdjustmentStatusEnum fAdjStatus);

        List<FinancialAdjustmentTypeSourceEnum> GetAccountFinancialAdjustmentsByStatus(FinancialAdjustmentStatusEnum status, int accountKey, params FinancialAdjustmentTypeSourceEnum[] sources);

        QueryResultsRow GetAccountWithFinancialAdjustmentNotUnderDebtCounselling(FinancialAdjustmentTypeSourceEnum fats, FinancialAdjustmentStatusEnum status);

        QueryResults GetOfferInformationFinancialAdjustmentByOfferAndType(int offerKey, FinancialAdjustmentTypeSourceEnum finAdjustmentTypeSource);

        QueryResultsRow GetAccountWithFutureDatedFinancialAdjustment(FinancialAdjustmentTypeSourceEnum fats, FinancialAdjustmentStatusEnum status);

        QueryResults GetAppliedOfferInformationRateAdjustment(int offerKey, double expectedAdjustment, string expectedRateAdjustmentElement);
    }
}