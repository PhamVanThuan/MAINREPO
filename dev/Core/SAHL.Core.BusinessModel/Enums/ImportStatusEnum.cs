namespace SAHL.Core.BusinessModel.Enums
{
    public enum ImportStatus
    {
        Pending = 1,
        Rejected_Duplication = 2,
        Rejected_ExistingLoan = 3,
        Rejected_OpenOffer = 4,
        Rejected_General = 5,
        Rejected_InvalidIDNumber = 6,
        Accepted = 7,
        Rejected_ExistingProspect = 8
    }
}