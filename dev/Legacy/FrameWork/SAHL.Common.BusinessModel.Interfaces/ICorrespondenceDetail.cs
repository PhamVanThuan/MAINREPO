using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CorrespondenceDetail_DAO
    /// </summary>
    public partial interface ICorrespondenceDetail : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CorrespondenceDetail_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CorrespondenceDetail_DAO.Correspondence
        /// </summary>
        ICorrespondence Correspondence
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CorrespondenceDetail_DAO.CorrespondenceText
        /// </summary>
        System.String CorrespondenceText
        {
            get;
            set;
        }
    }
}