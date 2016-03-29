namespace SAHL.Core.BusinessModel.Enums
{
    public enum CancellationReason
    {
        CapTermExpired = 1,
        LoanClosed = 2,
        OptintoNewProductwhichdoesntpermitCAP = 3,
        ClientelectedtocancelCAP = 4,
        ErrorinCreation_Cappingofloan = 5,
        ClienthasproductwhichdoesntpermitCAP = 6,
        OptintoNewProduct = 7,
        TermExpired = 8,
        CancelDefendingDiscount = 9,
        CancelSuperLo = 10,
        SAHLStaff = 11,
        CancelNonPerfoming = 12,
        EdgeOptOutforDC = 13,
        IOOptoutforDC = 14,
        StaffoptoutforDC = 15,
        SuperLooptoutforDC = 16,
        DefendingDiscountoptoutforDC = 17,
        DebtCounsellingOptOut = 18,
        DiscountedLinkRateOptOut = 19,
        Staffoptout_nolongeramemberofstaff = 20,
        InterestOnlyOptOut = 21,
        TransferedoutofDebtCounselling = 22,
        CancelFixedRateAdjustment = 23,
        VarifixOptOutforDC = 24,
        NonPerformingOptoutforDC = 25
    }
}