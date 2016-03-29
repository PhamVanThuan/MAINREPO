using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.DocumentVersion_DAO
    /// </summary>
    public partial interface IDocumentVersion : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DocumentVersion_DAO.Version
        /// </summary>
        System.String Version
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DocumentVersion_DAO.EffectiveDate
        /// </summary>
        System.DateTime EffectiveDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DocumentVersion_DAO.ActiveIndicator
        /// </summary>
        System.Boolean ActiveIndicator
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DocumentVersion_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DocumentVersion_DAO.DocumentType
        /// </summary>
        IDocumentType DocumentType
        {
            get;
            set;
        }
    }
}