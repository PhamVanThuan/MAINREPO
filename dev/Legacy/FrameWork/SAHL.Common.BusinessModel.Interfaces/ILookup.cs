using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Lookup_DAO
    /// </summary>
    public partial interface ILookup : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Lookup_DAO.STORid
        /// </summary>
        System.Decimal STORid
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Lookup_DAO.Field
        /// </summary>
        System.String Field
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Lookup_DAO.Text
        /// </summary>
        System.String Text
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Lookup_DAO.Key
        /// </summary>
        System.Decimal Key
        {
            get;
            set;
        }
    }
}