using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationCapitecDetail_DAO
    /// </summary>
    public partial interface IApplicationCapitecDetail : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationCapitecDetail_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationCapitecDetail_DAO.ApplicationKey
        /// </summary>
        System.Int32 ApplicationKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationCapitecDetail_DAO.Branch
        /// </summary>
        System.String Branch
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationCapitecDetail_DAO.Consultant
        /// </summary>
        System.String Consultant
        {
            get;
            set;
        }
    }
}