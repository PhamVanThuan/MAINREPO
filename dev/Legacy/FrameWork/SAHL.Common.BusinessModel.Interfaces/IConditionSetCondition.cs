using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ConditionSetCondition_DAO
    /// </summary>
    public partial interface IConditionSetCondition : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionSetCondition_DAO.RequiredCondition
        /// </summary>
        System.Boolean RequiredCondition
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionSetCondition_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionSetCondition_DAO.ConditionSet
        /// </summary>
        IConditionSet ConditionSet
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionSetCondition_DAO.Condition
        /// </summary>
        ICondition Condition
        {
            get;
            set;
        }
    }
}