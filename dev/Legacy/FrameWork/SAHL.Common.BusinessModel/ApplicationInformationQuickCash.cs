using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// ApplicationInformationQuickCash_DAO is instantiated in order to retrieve information regarding the Quick Cash Application
    /// associated to the Mortgage Loan Application.
    /// </summary>
    public partial class ApplicationInformationQuickCash : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationInformationQuickCash_DAO>, IApplicationInformationQuickCash
    {
        public ApplicationInformationQuickCash(SAHL.Common.BusinessModel.DAO.ApplicationInformationQuickCash_DAO ApplicationInformationQuickCash)
            : base(ApplicationInformationQuickCash)
        {
            this._DAO = ApplicationInformationQuickCash;
        }

        /// <summary>
        /// Primary Key. This is also a foreign key reference to the OfferInformation table.
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// The total amount of Quick Cash which Credit has approved the client for.
        /// </summary>
        public Double CreditApprovedAmount
        {
            get { return _DAO.CreditApprovedAmount; }
            set { _DAO.CreditApprovedAmount = value; }
        }

        /// <summary>
        /// The Term of the Quick Cash loan.
        /// </summary>
        public Int32 Term
        {
            get { return _DAO.Term; }
            set { _DAO.Term = value; }
        }

        /// <summary>
        /// The amount that Credit has approved as the maximum allowed for the Up Front payment to the Client.
        /// </summary>
        public Double CreditUpfrontApprovedAmount
        {
            get { return _DAO.CreditUpfrontApprovedAmount; }
            set { _DAO.CreditUpfrontApprovedAmount = value; }
        }

        /// <summary>
        /// A OfferInformationQuickCash record has many records associated to it in the OfferInformationQuickCashDetails table.
        /// </summary>
        private DAOEventList<ApplicationInformationQuickCashDetail_DAO, IApplicationInformationQuickCashDetail, ApplicationInformationQuickCashDetail> _ApplicationInformationQuickCashDetails;

        /// <summary>
        /// A OfferInformationQuickCash record has many records associated to it in the OfferInformationQuickCashDetails table.
        /// </summary>
        public IEventList<IApplicationInformationQuickCashDetail> ApplicationInformationQuickCashDetails
        {
            get
            {
                if (null == _ApplicationInformationQuickCashDetails)
                {
                    if (null == _DAO.ApplicationInformationQuickCashDetails)
                        _DAO.ApplicationInformationQuickCashDetails = new List<ApplicationInformationQuickCashDetail_DAO>();
                    _ApplicationInformationQuickCashDetails = new DAOEventList<ApplicationInformationQuickCashDetail_DAO, IApplicationInformationQuickCashDetail, ApplicationInformationQuickCashDetail>(_DAO.ApplicationInformationQuickCashDetails);
                    _ApplicationInformationQuickCashDetails.BeforeAdd += new EventListHandler(OnApplicationInformationQuickCashDetails_BeforeAdd);
                    _ApplicationInformationQuickCashDetails.BeforeRemove += new EventListHandler(OnApplicationInformationQuickCashDetails_BeforeRemove);
                    _ApplicationInformationQuickCashDetails.AfterAdd += new EventListHandler(OnApplicationInformationQuickCashDetails_AfterAdd);
                    _ApplicationInformationQuickCashDetails.AfterRemove += new EventListHandler(OnApplicationInformationQuickCashDetails_AfterRemove);
                }
                return _ApplicationInformationQuickCashDetails;
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationQuickCash_DAO.ApplicationInformation
        /// </summary>
        public IApplicationInformation ApplicationInformation
        {
            get
            {
                if (null == _DAO.ApplicationInformation) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplicationInformation, ApplicationInformation_DAO>(_DAO.ApplicationInformation);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ApplicationInformation = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ApplicationInformation = (ApplicationInformation_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _ApplicationInformationQuickCashDetails = null;
        }
    }
}