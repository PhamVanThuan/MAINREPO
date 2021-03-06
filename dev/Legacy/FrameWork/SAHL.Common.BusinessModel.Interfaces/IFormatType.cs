using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.FormatType_DAO
    /// </summary>
    public partial interface IFormatType : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FormatType_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FormatType_DAO.Format
        /// </summary>
        System.String Format
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FormatType_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}