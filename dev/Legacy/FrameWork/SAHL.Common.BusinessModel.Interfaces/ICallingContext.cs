using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CallingContext_DAO
    /// </summary>
    public partial interface ICallingContext : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CallingContext_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CallingContext_DAO.CallingContextType
        /// </summary>
        ICallingContextType CallingContextType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CallingContext_DAO.CallingProcess
        /// </summary>
        System.String CallingProcess
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CallingContext_DAO.CallingMethod
        /// </summary>
        System.String CallingMethod
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CallingContext_DAO.CallingState
        /// </summary>
        System.String CallingState
        {
            get;
            set;
        }
    }
}