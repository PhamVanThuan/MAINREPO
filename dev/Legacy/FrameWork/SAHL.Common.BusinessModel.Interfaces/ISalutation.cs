using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Salutation_DAO
    /// </summary>
    public partial interface ISalutation : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Salutation_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Salutation_DAO.TranslatableItem
        /// </summary>
        ITranslatableItem TranslatableItem
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Salutation_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}