using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Attorney_DAO
    /// </summary>
    public partial interface IAttorney : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Attorney_DAO.AttorneyContact
        /// </summary>
        System.String AttorneyContact
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Attorney_DAO.GeneralStatus
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Attorney_DAO.AttorneyMandate
        /// </summary>
        Double? AttorneyMandate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Attorney_DAO.AttorneyWorkFlowEnabled
        /// </summary>
        Int32? AttorneyWorkFlowEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Attorney_DAO.AttorneyLoanTarget
        /// </summary>
        Double? AttorneyLoanTarget
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Attorney_DAO.AttorneyFurtherLoanTarget
        /// </summary>
        Double? AttorneyFurtherLoanTarget
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Attorney_DAO.AttorneyLitigationInd
        /// </summary>
        Boolean? AttorneyLitigationInd
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Attorney_DAO.AttorneyRegistrationInd
        /// </summary>
        Boolean? AttorneyRegistrationInd
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Attorney_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Attorney_DAO.LegalEntity
        /// </summary>
        ILegalEntity LegalEntity
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Attorney_DAO.DeedsOffice
        /// </summary>
        IDeedsOffice DeedsOffice
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Attorney_DAO.OriginationSources
        /// </summary>
        IEventList<IOriginationSource> OriginationSources
        {
            get;
        }
    }
}