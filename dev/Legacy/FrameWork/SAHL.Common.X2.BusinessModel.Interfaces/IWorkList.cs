using SAHL.Common.X2.BusinessModel.Validation;

namespace SAHL.Common.X2.BusinessModel.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public partial interface IWorkList : IEntityValidation
    {
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
        System.String ADUserName
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.DateTime ListDate
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.String Message
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        IInstance Instance
        {
            get;
            set;
        }
    }
}