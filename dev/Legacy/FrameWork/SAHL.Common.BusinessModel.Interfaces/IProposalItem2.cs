using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ProposalItem_DAO
    /// </summary>
    public partial interface IProposalItem : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The number of complete months
        /// </summary>
        int Period
        {
            get;
        }

        /// <summary>
        /// The sum of the selected MaretRate value and the InterestRate
        /// as a percentage
        /// </summary>
        double TotalInterestRate
        {
            get;
        }

        /// <summary>
        /// The sum of all amounts
        /// Amount + AdditionalAmount
        /// </summary>
        double TotalPayment
        {
            get;
        }
    }
}