using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationConditionToken_DAO
    /// </summary>
    public partial interface IApplicationConditionToken : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationConditionToken_DAO.Token
        /// </summary>
        IToken Token
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationConditionToken_DAO.TranslatableItem
        /// </summary>
        ITranslatableItem TranslatableItem
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationConditionToken_DAO.TokenValue
        /// </summary>
        System.String TokenValue
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationConditionToken_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationConditionToken_DAO.ApplicationCondition
        /// </summary>
        IApplicationCondition ApplicationCondition
        {
            get;
            set;
        }
    }
}