using SAHL.Common.Collections.Interfaces;
using SAHL.Common.X2.BusinessModel.Validation;

namespace SAHL.Common.X2.BusinessModel.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public partial interface IExternalActivity : IEntityValidation
    {
        /// <summary>
        ///
        /// </summary>
        System.Int32 WorkFlowID
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.String Name
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.Int32 ID
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        IEventList<IActiveExternalActivity> ActiveExternalActivities
        {
            get;
        }

        /// <summary>
        ///
        /// </summary>
        IEventList<IActivity> Activities
        {
            get;
        }
    }
}