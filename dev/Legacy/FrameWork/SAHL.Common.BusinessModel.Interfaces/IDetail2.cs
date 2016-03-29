using System;
using SAHL.Common.BusinessModel.Validation;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public partial interface IDetail : IEntityValidation
    {
        /// <summary>
        ///
        /// </summary>
        System.DateTime DetailDate
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        Double? Amount
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
        Int32? LinkID
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.String UserID
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.DateTime? ChangeDate
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        IAccount Account
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        IDetailType DetailType
        {
            get;
            set;
        }
    }
}