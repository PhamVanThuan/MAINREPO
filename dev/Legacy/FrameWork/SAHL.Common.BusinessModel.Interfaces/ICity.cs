using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.City_DAO
    /// </summary>
    public partial interface ICity : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.City_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.City_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.City_DAO.PostOffices
        /// </summary>
        IEventList<IPostOffice> PostOffices
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.City_DAO.Suburbs
        /// </summary>
        IEventList<ISuburb> Suburbs
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.City_DAO.Province
        /// </summary>
        IProvince Province
        {
            get;
            set;
        }
    }
}