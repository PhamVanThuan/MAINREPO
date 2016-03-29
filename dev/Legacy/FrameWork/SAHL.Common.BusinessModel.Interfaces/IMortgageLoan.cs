using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.MortgageLoan_DAO
    /// </summary>
    public partial interface IMortgageLoan : IEntityValidation, IBusinessModelObject, IFinancialService
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MortgageLoan_DAO.CreditMatrix
        /// </summary>
        ICreditMatrix CreditMatrix
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MortgageLoan_DAO.Property
        /// </summary>
        IProperty Property
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MortgageLoan_DAO.MortgageLoanPurpose
        /// </summary>
        IMortgageLoanPurpose MortgageLoanPurpose
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MortgageLoan_DAO.Bonds
        /// </summary>
        IEventList<IBond> Bonds
        {
            get;
        }
    }
}