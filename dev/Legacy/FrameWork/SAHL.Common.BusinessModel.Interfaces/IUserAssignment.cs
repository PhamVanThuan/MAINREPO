using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.UserAssignment_DAO
    /// </summary>
    public partial interface IUserAssignment : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UserAssignment_DAO.FinancialServiceKey
        /// </summary>
        System.Int32 FinancialServiceKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UserAssignment_DAO.OriginationSourceProductKey
        /// </summary>
        System.Int32 OriginationSourceProductKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UserAssignment_DAO.AssignmentDate
        /// </summary>
        System.DateTime AssignmentDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UserAssignment_DAO.AssigningUser
        /// </summary>
        System.String AssigningUser
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UserAssignment_DAO.AssignedUser
        /// </summary>
        System.String AssignedUser
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UserAssignment_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}