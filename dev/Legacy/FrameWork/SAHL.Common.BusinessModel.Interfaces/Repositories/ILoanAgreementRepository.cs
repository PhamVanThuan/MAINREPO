using System;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    /// <summary>
    /// The Loan Agreement repository
    /// </summary>
    public interface ILoanAgreementRepository
    {
        ILoanAgreement CreateLoanAgreement(DateTime AgreementDate, double Amount, DateTime ChangeDate, IBond Bond, string UserName);
    }
}