using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.RuleParameter_DAO
    /// </summary>
    public partial interface IRuleParameter : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RuleParameter_DAO.Name
        /// </summary>
        System.String Name
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RuleParameter_DAO.Value
        /// </summary>
        System.String Value
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RuleParameter_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RuleParameter_DAO.RuleItem
        /// </summary>
        IRuleItem RuleItem
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RuleParameter_DAO.RuleParameterType
        /// </summary>
        IParameterType RuleParameterType
        {
            get;
            set;
        }
    }
}