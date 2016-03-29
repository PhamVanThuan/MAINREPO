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
    /// ApplicationInformation_DAO is used to link the Application with the various Application Information tables which
    /// hold product specific information regarding the Application.
    /// </summary>
    public partial class ApplicationInformation : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationInformation_DAO>, IApplicationInformation
    {
        public ApplicationInformation(SAHL.Common.BusinessModel.DAO.ApplicationInformation_DAO ApplicationInformation)
            : base(ApplicationInformation)
        {
            this._DAO = ApplicationInformation;
        }

        /// <summary>
        /// Date when the Application Information records were inserted.
        /// </summary>
        public DateTime ApplicationInsertDate
        {
            get { return _DAO.ApplicationInsertDate; }
            set { _DAO.ApplicationInsertDate = value; }
        }

        /// <summary>
        /// The Application to which the ApplicationInformation belongs.
        /// </summary>
        public IApplication Application
        {
            get
            {
                if (null == _DAO.Application) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplication, Application_DAO>(_DAO.Application);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Application = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Application = (Application_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// Each Application Information record belongs to a particular product.
        /// </summary>
        public IProduct Product
        {
            get
            {
                if (null == _DAO.Product) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IProduct, Product_DAO>(_DAO.Product);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Product = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Product = (Product_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// An Application Information record can have many FinancialAdjustmentTypeSources associated to it.
        /// These FinancialAdjustmentTypeSources can be found in the OfferInformationFinancialAdjustmentTypeSource table.
        /// </summary>
        private DAOEventList<ApplicationInformationFinancialAdjustment_DAO, IApplicationInformationFinancialAdjustment, ApplicationInformationFinancialAdjustment> _ApplicationInformationFinancialAdjustments;

        /// <summary>
        /// An Application Information record can have many FinancialAdjustmentTypeSources associated to it.
        /// These FinancialAdjustmentTypeSources can be found in the OfferInformationFinancialAdjustmentTypeSource table.
        /// </summary>
        public IEventList<IApplicationInformationFinancialAdjustment> ApplicationInformationFinancialAdjustments
        {
            get
            {
                if (null == _ApplicationInformationFinancialAdjustments)
                {
                    if (null == _DAO.ApplicationInformationFinancialAdjustments)
                        _DAO.ApplicationInformationFinancialAdjustments = new List<ApplicationInformationFinancialAdjustment_DAO>();
                    _ApplicationInformationFinancialAdjustments = new DAOEventList<ApplicationInformationFinancialAdjustment_DAO, IApplicationInformationFinancialAdjustment, ApplicationInformationFinancialAdjustment>(_DAO.ApplicationInformationFinancialAdjustments);
                    _ApplicationInformationFinancialAdjustments.BeforeAdd += new EventListHandler(OnApplicationInformationFinancialAdjustments_BeforeAdd);
                    _ApplicationInformationFinancialAdjustments.BeforeRemove += new EventListHandler(OnApplicationInformationFinancialAdjustments_BeforeRemove);
                    _ApplicationInformationFinancialAdjustments.AfterAdd += new EventListHandler(OnApplicationInformationFinancialAdjustments_AfterAdd);
                    _ApplicationInformationFinancialAdjustments.AfterRemove += new EventListHandler(OnApplicationInformationFinancialAdjustments_AfterRemove);
                }
                return _ApplicationInformationFinancialAdjustments;
            }
        }

        /// <summary>
        /// Each Application Information record has a particular type, which is used to control the revisions which the Application
        /// Information records will undergo. This is defined in the OfferInformationType table and holds values such as Original Offer
        /// /Revised Offer/Accepted Offer
        /// </summary>
        public IApplicationInformationType ApplicationInformationType
        {
            get
            {
                if (null == _DAO.ApplicationInformationType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplicationInformationType, ApplicationInformationType_DAO>(_DAO.ApplicationInformationType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ApplicationInformationType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ApplicationInformationType = (ApplicationInformationType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _ApplicationInformationFinancialAdjustments = null;
        }
    }
}