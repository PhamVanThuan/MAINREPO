using SAHL.Common.BusinessModel.Validation;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface IDisbursement : IEntityValidation
    {
        string GetBankDisplayName(BankAccountNameFormat Format);
    }
}