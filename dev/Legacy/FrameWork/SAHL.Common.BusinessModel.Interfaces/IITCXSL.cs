using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ITCXSL_DAO
    /// </summary>
    public partial interface IITCXSL : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ITCXSL_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ITCXSL_DAO.EffectiveDate
        /// </summary>
        System.DateTime EffectiveDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ITCXSL_DAO.StyleSheet
        /// </summary>
        System.String StyleSheet
        {
            get;
            set;
        }
    }
}