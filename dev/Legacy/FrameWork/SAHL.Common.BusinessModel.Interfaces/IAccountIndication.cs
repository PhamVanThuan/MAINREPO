using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AccountIndication_DAO
    /// </summary>
    public partial interface IAccountIndication : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountIndication_DAO.AccountIndicator
        /// </summary>
        System.Int32 AccountIndicator
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountIndication_DAO.Indicator
        /// </summary>
        System.Char Indicator
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountIndication_DAO.UserID
        /// </summary>
        System.String UserID
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountIndication_DAO.DateChange
        /// </summary>
        System.DateTime DateChange
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountIndication_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountIndication_DAO.AccountIndicationType
        /// </summary>
        IAccountIndicationType AccountIndicationType
        {
            get;
            set;
        }
    }
}