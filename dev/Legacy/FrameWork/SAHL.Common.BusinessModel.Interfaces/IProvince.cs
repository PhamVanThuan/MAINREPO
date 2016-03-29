using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Province_DAO
    /// </summary>
    public partial interface IProvince : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Province_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Province_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Province_DAO.Cities
        /// </summary>
        IEventList<ICity> Cities
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Province_DAO.Country
        /// </summary>
        ICountry Country
        {
            get;
            set;
        }
    }
}