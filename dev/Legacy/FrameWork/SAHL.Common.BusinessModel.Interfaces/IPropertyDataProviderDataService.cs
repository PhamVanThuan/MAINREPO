using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.PropertyDataProviderDataService_DAO
    /// </summary>
    public partial interface IPropertyDataProviderDataService : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.PropertyDataProviderDataService_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.PropertyDataProviderDataService_DAO.DataProviderDataService
        /// </summary>
        IDataProviderDataService DataProviderDataService
        {
            get;
            set;
        }
    }
}