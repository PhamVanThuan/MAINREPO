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
    /// SAHL.Common.BusinessModel.DAO.CampaignTarget_DAO
    /// </summary>
    public partial class CampaignTarget : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CampaignTarget_DAO>, ICampaignTarget
    {
        public CampaignTarget(SAHL.Common.BusinessModel.DAO.CampaignTarget_DAO CampaignTarget)
            : base(CampaignTarget)
        {
            this._DAO = CampaignTarget;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignTarget_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignTarget_DAO.GenericKey
        /// </summary>
        public Int32 GenericKey
        {
            get { return _DAO.GenericKey; }
            set { _DAO.GenericKey = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignTarget_DAO.ADUserKey
        /// </summary>
        public Int32 ADUserKey
        {
            get { return _DAO.ADUserKey; }
            set { _DAO.ADUserKey = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignTarget_DAO.GenericKeyTypeKey
        /// </summary>
        public Int32 GenericKeyTypeKey
        {
            get { return _DAO.GenericKeyTypeKey; }
            set { _DAO.GenericKeyTypeKey = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignTarget_DAO.CampaignTargetContacts
        /// </summary>
        private DAOEventList<CampaignTargetContact_DAO, ICampaignTargetContact, CampaignTargetContact> _CampaignTargetContacts;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignTarget_DAO.CampaignTargetContacts
        /// </summary>
        public IEventList<ICampaignTargetContact> CampaignTargetContacts
        {
            get
            {
                if (null == _CampaignTargetContacts)
                {
                    if (null == _DAO.CampaignTargetContacts)
                        _DAO.CampaignTargetContacts = new List<CampaignTargetContact_DAO>();
                    _CampaignTargetContacts = new DAOEventList<CampaignTargetContact_DAO, ICampaignTargetContact, CampaignTargetContact>(_DAO.CampaignTargetContacts);
                    _CampaignTargetContacts.BeforeAdd += new EventListHandler(OnCampaignTargetContacts_BeforeAdd);
                    _CampaignTargetContacts.BeforeRemove += new EventListHandler(OnCampaignTargetContacts_BeforeRemove);
                    _CampaignTargetContacts.AfterAdd += new EventListHandler(OnCampaignTargetContacts_AfterAdd);
                    _CampaignTargetContacts.AfterRemove += new EventListHandler(OnCampaignTargetContacts_AfterRemove);
                }
                return _CampaignTargetContacts;
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignTarget_DAO.CampaignDefinition
        /// </summary>
        public ICampaignDefinition CampaignDefinition
        {
            get
            {
                if (null == _DAO.CampaignDefinition) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ICampaignDefinition, CampaignDefinition_DAO>(_DAO.CampaignDefinition);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.CampaignDefinition = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.CampaignDefinition = (CampaignDefinition_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _CampaignTargetContacts = null;
        }
    }
}