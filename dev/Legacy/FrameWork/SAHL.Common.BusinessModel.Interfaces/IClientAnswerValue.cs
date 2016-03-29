using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ClientAnswerValue_DAO
    /// </summary>
    public partial interface IClientAnswerValue : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientAnswerValue_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientAnswerValue_DAO.Value
        /// </summary>
        System.String Value
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientAnswerValue_DAO.ClientAnswer
        /// </summary>
        IClientAnswer ClientAnswer
        {
            get;
            set;
        }
    }
}