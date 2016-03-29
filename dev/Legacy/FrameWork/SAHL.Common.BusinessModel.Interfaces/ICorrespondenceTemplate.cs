using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CorrespondenceTemplate_DAO
    /// </summary>
    public partial interface ICorrespondenceTemplate : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CorrespondenceTemplate_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CorrespondenceTemplate_DAO.Name
        /// </summary>
        System.String Name
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CorrespondenceTemplate_DAO.Subject
        /// </summary>
        System.String Subject
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CorrespondenceTemplate_DAO.Template
        /// </summary>
        System.String Template
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CorrespondenceTemplate_DAO.DefaultEmail
        /// </summary>
        System.String DefaultEmail
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CorrespondenceTemplate_DAO.ContentType
        /// </summary>
        IContentType ContentType
        {
            get;
            set;
        }
    }
}