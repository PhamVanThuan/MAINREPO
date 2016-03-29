using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.WorkflowRuleSet_DAO
    /// </summary>
    public partial interface IWorkflowRuleSet : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.WorkflowRuleSet_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.WorkflowRuleSet_DAO.Name
        /// </summary>
        System.String Name
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.WorkflowRuleSet_DAO.Rules
        /// </summary>
        IEventList<IRuleItem> Rules
        {
            get;
        }
    }
}