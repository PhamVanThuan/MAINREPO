using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.FeatureGroup_DAO
    /// </summary>
    public partial interface IFeatureGroup : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FeatureGroup_DAO.ADUserGroup
        /// </summary>
        System.String ADUserGroup
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FeatureGroup_DAO.Feature
        /// </summary>
        IFeature Feature
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FeatureGroup_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}