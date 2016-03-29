using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.FinancialServiceCondition_DAO
    /// </summary>
    public partial interface IFinancialServiceCondition : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialServiceCondition_DAO.UserDefinedConditionText
        /// </summary>
        System.String UserDefinedConditionText
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialServiceCondition_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialServiceCondition_DAO.Condition
        /// </summary>
        ICondition Condition
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialServiceCondition_DAO.ConditionType
        /// </summary>
        IConditionType ConditionType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialServiceCondition_DAO.FinancialService
        /// </summary>
        IFinancialService FinancialService
        {
            get;
            set;
        }
    }
}