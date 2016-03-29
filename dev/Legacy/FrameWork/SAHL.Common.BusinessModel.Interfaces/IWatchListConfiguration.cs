using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.WatchListConfiguration_DAO
    /// </summary>
    public partial interface IWatchListConfiguration : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.WatchListConfiguration_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.WatchListConfiguration_DAO.ProcessName
        /// </summary>
        System.String ProcessName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.WatchListConfiguration_DAO.WorkFlowName
        /// </summary>
        System.String WorkFlowName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.WatchListConfiguration_DAO.StatementName
        /// </summary>
        System.String StatementName
        {
            get;
            set;
        }
    }
}