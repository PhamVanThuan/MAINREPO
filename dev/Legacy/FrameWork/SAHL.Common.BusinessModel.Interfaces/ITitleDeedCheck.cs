using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.TitleDeedCheck_DAO
    /// </summary>
    public partial interface ITitleDeedCheck : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TitleDeedCheck_DAO.Key
        /// </summary>
        System.String Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TitleDeedCheck_DAO.TitleDeedIndicator
        /// </summary>
        System.String TitleDeedIndicator
        {
            get;
            set;
        }
    }
}