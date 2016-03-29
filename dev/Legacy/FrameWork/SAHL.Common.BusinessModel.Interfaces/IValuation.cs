using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Valuation_DAO stores the information on a valuation for a property. It is discriminated using the ValuationDataProviderDataService
    /// which allows currently:
    ///
    /// SAHL Client Estimate
    ///
    /// SAHL Manual Valuation
    ///
    /// Lightstone Automated Valuation
    ///
    /// AdCheck Desktop Valuation
    ///
    /// AdCheck Physical Valuation
    /// </summary>
    public partial interface IValuation : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The date on which the Valuation is completed.
        /// </summary>
        System.DateTime ValuationDate
        {
            get;
            set;
        }

        /// <summary>
        /// The Total Valuation Amount.
        /// </summary>
        Double? ValuationAmount
        {
            get;
            set;
        }

        /// <summary>
        /// The replacement HOC Total Valuation Amount
        /// </summary>
        Double? ValuationHOCValue
        {
            get;
            set;
        }

        /// <summary>
        /// The Municipal Valuation as provided by manual municipal enquiry.
        /// </summary>
        Double? ValuationMunicipal
        {
            get;
            set;
        }

        /// <summary>
        /// The Valuation User who captured the Valuation.
        /// </summary>
        System.String ValuationUserID
        {
            get;
            set;
        }

        /// <summary>
        /// The HOC Thatch Roof Type Total value.
        /// </summary>
        Double? HOCThatchAmount
        {
            get;
            set;
        }

        /// <summary>
        /// The HOC Conventional Roof Type Total value.
        /// </summary>
        Double? HOCConventionalAmount
        {
            get;
            set;
        }

        /// <summary>
        /// The HOC Shingle Roof Type Total value.
        /// </summary>
        Double? HOCShingleAmount
        {
            get;
            set;
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// The foreign reference to the Property table. Each Valuation belongs to a single Property.
        /// </summary>
        IProperty Property
        {
            get;
            set;
        }

        /// <summary>
        /// The foreign reference to the Valuator table. Each Valuation is carried out by a single Valuator. The Valuator details
        /// are stored as Legal Entity details.
        /// </summary>
        IValuator Valuator
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        IValuationClassification ValuationClassification
        {
            get;
            set;
        }

        /// <summary>
        /// The HOC escalation percentage applied to the HOC Valuation portions of this Valuation.
        /// </summary>
        Double? ValuationEscalationPercentage
        {
            get;
            set;
        }

        /// <summary>
        /// The foreign key reference to the ValuationStatus table. Each Valuation is assigned a status of pending or complete. The
        /// status is updated via the X2 workflow.
        /// </summary>
        IValuationStatus ValuationStatus
        {
            get;
            set;
        }

        /// <summary>
        /// This is the XML data detailing the valuation. Its structure is specific to each of the discriminations and in the case
        /// of Lightstone and AdCheck are the resulting XML documents received via the respective web services for a completed Valuation.
        /// </summary>
        System.String Data
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Valuation_DAO.ValuationDataProviderDataService
        /// </summary>
        IValuationDataProviderDataService ValuationDataProviderDataService
        {
            get;
            set;
        }

        /// <summary>
        /// This is a Valuation flag which is set by business rules (can be overridden manually). A property can have many completed
        /// valuations against it, from many sources, but only one active Valuation which is what this flag indicates.
        /// </summary>
        System.Boolean IsActive
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Valuation_DAO.HOCRoof
        /// </summary>
        IHOCRoof HOCRoof
        {
            get;
            set;
        }
    }
}