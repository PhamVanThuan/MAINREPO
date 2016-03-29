using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// ApplicationInformationInterestOnly_DAO is instantiated in order to retrieve those details specific to an Interest Only
    /// Application.
    /// </summary>
    public partial interface IApplicationInformationInterestOnly : IEntityValidation, IBusinessModelObject
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
        /// The Interest Only Instalment which does not take into account the client repaying any capital as part of their
        /// monthly instalment.
        /// </summary>
        Double? Installment
        {
            get;
            set;
        }

        /// <summary>
        /// The Interest Only Maturity Date is the date at which the client is required to repay any outstanding capital.
        /// </summary>
        DateTime? MaturityDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationInterestOnly_DAO.ApplicationInformation
        /// </summary>
        IApplicationInformation ApplicationInformation
        {
            get;
            set;
        }
    }
}