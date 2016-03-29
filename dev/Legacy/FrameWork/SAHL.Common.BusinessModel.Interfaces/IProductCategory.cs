using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// This class links a OriginationSourceProduct (OSP) for a given Category to a margin.
    /// </summary>
    public partial interface IProductCategory : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// This is the primary key, used to identify an instance of ProductCategory.
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// The category the OSP is linked to.
        /// </summary>
        ICategory Category
        {
            get;
            set;
        }

        /// <summary>
        /// The margin associated with this OSP for given Category.
        /// </summary>
        IMargin Margin
        {
            get;
            set;
        }

        /// <summary>
        /// The OSP.
        /// </summary>
        IOriginationSourceProduct OriginationSourceProduct
        {
            get;
            set;
        }
    }
}