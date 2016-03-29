using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.DocumentSetConfig_DAO
    /// </summary>
    public partial interface IDocumentSetConfig : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DocumentSetConfig_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DocumentSetConfig_DAO.DocumentSet
        /// </summary>
        IDocumentSet DocumentSet
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DocumentSetConfig_DAO.DocumentType
        /// </summary>
        IDocumentType DocumentType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DocumentSetConfig_DAO.RuleItem
        /// </summary>
        IRuleItem RuleItem
        {
            get;
            set;
        }
    }
}