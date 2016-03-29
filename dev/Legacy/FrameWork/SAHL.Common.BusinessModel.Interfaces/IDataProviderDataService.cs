using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.DataProviderDataService_DAO
    /// </summary>
    public partial interface IDataProviderDataService : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DataProviderDataService_DAO.DataProvider
        /// </summary>
        IDataProvider DataProvider
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DataProviderDataService_DAO.DataService
        /// </summary>
        IDataService DataService
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DataProviderDataService_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}