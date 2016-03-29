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
    /// SAHL.Common.BusinessModel.DAO.Attorney_DAO
    /// </summary>
    public partial class Attorney : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Attorney_DAO>, IAttorney
    {
        public Attorney(SAHL.Common.BusinessModel.DAO.Attorney_DAO Attorney)
            : base(Attorney)
        {
            this._DAO = Attorney;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Attorney_DAO.AttorneyContact
        /// </summary>
        public String AttorneyContact
        {
            get { return _DAO.AttorneyContact; }
            set { _DAO.AttorneyContact = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Attorney_DAO.GeneralStatus
        /// </summary>
        public IGeneralStatus GeneralStatus
        {
            get
            {
                if (null == _DAO.GeneralStatus) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IGeneralStatus, GeneralStatus_DAO>(_DAO.GeneralStatus);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.GeneralStatus = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.GeneralStatus = (GeneralStatus_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Attorney_DAO.AttorneyMandate
        /// </summary>
        public Double? AttorneyMandate
        {
            get { return _DAO.AttorneyMandate; }
            set { _DAO.AttorneyMandate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Attorney_DAO.AttorneyWorkFlowEnabled
        /// </summary>
        public Int32? AttorneyWorkFlowEnabled
        {
            get { return _DAO.AttorneyWorkFlowEnabled; }
            set { _DAO.AttorneyWorkFlowEnabled = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Attorney_DAO.AttorneyLoanTarget
        /// </summary>
        public Double? AttorneyLoanTarget
        {
            get { return _DAO.AttorneyLoanTarget; }
            set { _DAO.AttorneyLoanTarget = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Attorney_DAO.AttorneyFurtherLoanTarget
        /// </summary>
        public Double? AttorneyFurtherLoanTarget
        {
            get { return _DAO.AttorneyFurtherLoanTarget; }
            set { _DAO.AttorneyFurtherLoanTarget = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Attorney_DAO.AttorneyLitigationInd
        /// </summary>
        public Boolean? AttorneyLitigationInd
        {
            get { return _DAO.AttorneyLitigationInd; }
            set { _DAO.AttorneyLitigationInd = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Attorney_DAO.AttorneyRegistrationInd
        /// </summary>
        public Boolean? AttorneyRegistrationInd
        {
            get { return _DAO.AttorneyRegistrationInd; }
            set { _DAO.AttorneyRegistrationInd = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Attorney_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Attorney_DAO.LegalEntity
        /// </summary>
        public ILegalEntity LegalEntity
        {
            get
            {
                if (null == _DAO.LegalEntity) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ILegalEntity, LegalEntity_DAO>(_DAO.LegalEntity);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.LegalEntity = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.LegalEntity = (LegalEntity_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Attorney_DAO.DeedsOffice
        /// </summary>
        public IDeedsOffice DeedsOffice
        {
            get
            {
                if (null == _DAO.DeedsOffice) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IDeedsOffice, DeedsOffice_DAO>(_DAO.DeedsOffice);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.DeedsOffice = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.DeedsOffice = (DeedsOffice_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Attorney_DAO.OriginationSources
        /// </summary>
        private DAOEventList<OriginationSource_DAO, IOriginationSource, OriginationSource> _OriginationSources;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Attorney_DAO.OriginationSources
        /// </summary>
        public IEventList<IOriginationSource> OriginationSources
        {
            get
            {
                if (null == _OriginationSources)
                {
                    if (null == _DAO.OriginationSources)
                        _DAO.OriginationSources = new List<OriginationSource_DAO>();
                    _OriginationSources = new DAOEventList<OriginationSource_DAO, IOriginationSource, OriginationSource>(_DAO.OriginationSources);
                    _OriginationSources.BeforeAdd += new EventListHandler(OnOriginationSources_BeforeAdd);
                    _OriginationSources.BeforeRemove += new EventListHandler(OnOriginationSources_BeforeRemove);
                    _OriginationSources.AfterAdd += new EventListHandler(OnOriginationSources_AfterAdd);
                    _OriginationSources.AfterRemove += new EventListHandler(OnOriginationSources_AfterRemove);
                }
                return _OriginationSources;
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _OriginationSources = null;
        }
    }
}