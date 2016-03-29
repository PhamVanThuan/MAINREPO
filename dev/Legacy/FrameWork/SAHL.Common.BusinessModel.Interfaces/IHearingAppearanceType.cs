using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.HearingAppearanceType_DAO
    /// </summary>
    public partial interface IHearingAppearanceType : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HearingAppearanceType_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HearingAppearanceType_DAO.HearingType
        /// </summary>
        IHearingType HearingType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HearingAppearanceType_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }
    }
}