using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Feature_DAO
    /// </summary>
    public partial interface IFeature : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Feature_DAO.ShortName
        /// </summary>
        System.String ShortName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Feature_DAO.LongName
        /// </summary>
        System.String LongName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Feature_DAO.HasAccess
        /// </summary>
        System.Boolean HasAccess
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Feature_DAO.Sequence
        /// </summary>
        System.Int32 Sequence
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Feature_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Feature_DAO.ChildFeatures
        /// </summary>
        IEventList<IFeature> ChildFeatures
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Feature_DAO.ParentFeature
        /// </summary>
        IFeature ParentFeature
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Feature_DAO.FeatureGroups
        /// </summary>
        IEventList<IFeatureGroup> FeatureGroups
        {
            get;
        }
    }
}