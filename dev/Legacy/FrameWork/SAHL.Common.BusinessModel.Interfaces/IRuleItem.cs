using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.RuleItem_DAO
    /// </summary>
    public partial interface IRuleItem : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RuleItem_DAO.Name
        /// </summary>
        System.String Name
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RuleItem_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RuleItem_DAO.GeneralStatusReasonDescription
        /// </summary>
        System.String GeneralStatusReasonDescription
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RuleItem_DAO.AssemblyName
        /// </summary>
        System.String AssemblyName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RuleItem_DAO.TypeName
        /// </summary>
        System.String TypeName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RuleItem_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// A list of parameters that apply to the rule.
        /// </summary>
        IEventList<IRuleParameter> RuleParameters
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RuleItem_DAO.EnforceRule
        /// </summary>
        System.Boolean EnforceRule
        {
            get;
            set;
        }

        /// <summary>
        /// Foreign Key reference to the GeneralStatus table. Rules that are marked as Inactive should not be executed by
        /// the domain.
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
            set;
        }
    }
}