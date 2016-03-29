using System;
using SAHL.Common.BusinessModel.Validation;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public partial interface ILoanAgreement : IEntityValidation
    {
        /// <summary>
        ///
        /// </summary>
        DateTime AgreementDate
        {
            get;
        }

        /// <summary>
        ///
        /// </summary>
        Double Amount
        {
            get;
        }

        /// <summary>
        ///
        /// </summary>
        String UserName
        {
            get;
        }

        /// <summary>
        ///
        /// </summary>
        DateTime ChangeDate
        {
            get;
        }

        /// <summary>
        ///
        /// </summary>
        Int32 Key
        {
            get;
        }

        /// <summary>
        ///
        /// </summary>
        IBond Bond
        {
            get;
        }
    }
}