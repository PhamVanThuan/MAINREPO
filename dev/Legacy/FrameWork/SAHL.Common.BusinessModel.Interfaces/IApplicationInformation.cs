using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// ApplicationInformation_DAO is used to link the Application with the various Application Information tables which
    /// hold product specific information regarding the Application.
    /// </summary>
    public partial interface IApplicationInformation : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// Date when the Application Information records were inserted.
        /// </summary>
        System.DateTime ApplicationInsertDate
        {
            get;
            set;
        }

        /// <summary>
        /// The Application to which the ApplicationInformation belongs.
        /// </summary>
        IApplication Application
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
        /// Each Application Information record belongs to a particular product.
        /// </summary>
        IProduct Product
        {
            get;
            set;
        }

        /// <summary>
        /// An Application Information record can have many FinancialAdjustmentTypeSources associated to it.
        /// These FinancialAdjustmentTypeSources can be found in the OfferInformationFinancialAdjustmentTypeSource table.
        /// </summary>
        IEventList<IApplicationInformationFinancialAdjustment> ApplicationInformationFinancialAdjustments
        {
            get;
        }

        /// <summary>
        /// Each Application Information record has a particular type, which is used to control the revisions which the Application
        /// Information records will undergo. This is defined in the OfferInformationType table and holds values such as Original Offer
        /// /Revised Offer/Accepted Offer
        /// </summary>
        IApplicationInformationType ApplicationInformationType
        {
            get;
            set;
        }
    }
}