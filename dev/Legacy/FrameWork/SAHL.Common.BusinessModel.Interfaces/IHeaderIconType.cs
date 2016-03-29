using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.HeaderIconType_DAO
    /// </summary>
    public partial interface IHeaderIconType : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HeaderIconType_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HeaderIconType_DAO.Icon
        /// </summary>
        System.String Icon
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HeaderIconType_DAO.StatementName
        /// </summary>
        System.String StatementName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HeaderIconType_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HeaderIconType_DAO.HeaderIconDetails
        /// </summary>
        IEventList<IHeaderIconDetails> HeaderIconDetails
        {
            get;
        }
    }
}