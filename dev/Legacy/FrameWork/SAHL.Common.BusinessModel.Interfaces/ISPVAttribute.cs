using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.SPVAttribute_DAO
    /// </summary>
    public partial interface ISPVAttribute : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPVAttribute_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPVAttribute_DAO.SPV
        /// </summary>
        ISPV SPV
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPVAttribute_DAO.SPVAttributeType
        /// </summary>
        ISPVAttributeType SPVAttributeType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPVAttribute_DAO.Value
        /// </summary>
        System.String Value
        {
            get;
            set;
        }
    }
}