namespace SAHL.Core.BusinessModel.Enums
{
    public enum LifePolicyStatus
    {
        Prospect = 1,
        Accepted = 2,
        Inforce = 3,
        CancelledfromInception = 4,
        CancelledwithProrata = 5,
        Lapsed = 11,
        Closed = 12,
        Accepted_tocommenceon1st = 13,
        Closed_SystemError = 14,
        Cancelled_NoRefund = 15,
        NotTakenUp = 16,
        ExternalInsurer = 17,
        LapsePending = 18
    }
}