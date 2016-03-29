using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// ApplicationInformationSuperLoLoan_DAO is instantiated in order to retrieve those details specific to a Super Lo
    /// Application.
    /// </summary>
    public partial interface IApplicationInformationSuperLoLoan : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// Primary Key. This is also a foreign key reference to the OfferInformation table.
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// The date on which the client elected to take the Super Lo Product option.
        /// </summary>
        DateTime? ElectionDate
        {
            get;
            set;
        }

        /// <summary>
        /// The Prepayment Threshold for the 1st year of the loyalty period. A client is opted out of Super Lo should they
        /// breach this threshold.
        /// </summary>
        System.Double PPThresholdYr1
        {
            get;
            set;
        }

        /// <summary>
        /// The Prepayment Threshold for the 2nd year of the loyalty period. A client is opted out of Super Lo should they
        /// breach this threshold.
        /// </summary>
        System.Double PPThresholdYr2
        {
            get;
            set;
        }

        /// <summary>
        /// The Prepayment Threshold for the 3rd year of the loyalty period. A client is opted out of Super Lo should they
        /// breach this threshold.
        /// </summary>
        System.Double PPThresholdYr3
        {
            get;
            set;
        }

        /// <summary>
        /// The Prepayment Threshold for the 4th year of the loyalty period. A client is opted out of Super Lo should they
        /// breach this threshold.
        /// </summary>
        System.Double PPThresholdYr4
        {
            get;
            set;
        }

        /// <summary>
        /// The Prepayment Threshold for the 5th year of the loyalty period. A client is opted out of Super Lo should they
        /// breach this threshold.
        /// </summary>
        System.Double PPThresholdYr5
        {
            get;
            set;
        }

        /// <summary>
        /// The Status.
        /// </summary>
        System.Int32 Status
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationSuperLoLoan_DAO.ApplicationInformation
        /// </summary>
        IApplicationInformation ApplicationInformation
        {
            get;
            set;
        }
    }
}