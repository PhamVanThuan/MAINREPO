using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.PropertyTitleDeed_DAO
    /// </summary>
    public partial interface IPropertyTitleDeed : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.PropertyTitleDeed_DAO.TitleDeedNumber
        /// </summary>
        System.String TitleDeedNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.PropertyTitleDeed_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.PropertyTitleDeed_DAO.Property
        /// </summary>
        IProperty Property
        {
            get;
            set;
        }

        /// <summary>
        /// Gets/sets the deeds office to which the title deed belongs.
        /// </summary>
        IDeedsOffice DeedsOffice
        {
            get;
            set;
        }
    }
}