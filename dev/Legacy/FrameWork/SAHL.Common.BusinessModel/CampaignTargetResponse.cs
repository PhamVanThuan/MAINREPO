using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CampaignTargetResponse_DAO
    /// </summary>
    public partial class CampaignTargetResponse : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CampaignTargetResponse_DAO>, ICampaignTargetResponse
    {
        public CampaignTargetResponse(SAHL.Common.BusinessModel.DAO.CampaignTargetResponse_DAO CampaignTargetResponse)
            : base(CampaignTargetResponse)
        {
            this._DAO = CampaignTargetResponse;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignTargetResponse_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignTargetResponse_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignTargetResponse_DAO.CampaignTargetContacts
        /// </summary>
        private DAOEventList<CampaignTargetContact_DAO, ICampaignTargetContact, CampaignTargetContact> _CampaignTargetContacts;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignTargetResponse_DAO.CampaignTargetContacts
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

        public override void Refresh()
        {
            base.Refresh();
            _CampaignTargetContacts = null;
        }
    }
}