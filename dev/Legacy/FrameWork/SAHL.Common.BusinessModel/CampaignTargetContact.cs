using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CampaignTargetContact_DAO
    /// </summary>
    public partial class CampaignTargetContact : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CampaignTargetContact_DAO>, ICampaignTargetContact
    {
        public CampaignTargetContact(SAHL.Common.BusinessModel.DAO.CampaignTargetContact_DAO CampaignTargetContact)
            : base(CampaignTargetContact)
        {
            this._DAO = CampaignTargetContact;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignTargetContact_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignTargetContact_DAO.LegalEntityKey
        /// </summary>
        public Int32 LegalEntityKey
        {
            get { return _DAO.LegalEntityKey; }
            set { _DAO.LegalEntityKey = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignTargetContact_DAO.ChangeDate
        /// </summary>
        public DateTime ChangeDate
        {
            get { return _DAO.ChangeDate; }
            set { _DAO.ChangeDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignTargetContact_DAO.AdUserKey
        /// </summary>
        public Int32 AdUserKey
        {
            get { return _DAO.AdUserKey; }
            set { _DAO.AdUserKey = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignTargetContact_DAO.CampaignTarget
        /// </summary>
        public ICampaignTarget CampaignTarget
        {
            get
            {
                if (null == _DAO.CampaignTarget) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ICampaignTarget, CampaignTarget_DAO>(_DAO.CampaignTarget);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.CampaignTarget = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.CampaignTarget = (CampaignTarget_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignTargetContact_DAO.CampaignTargetResponse
        /// </summary>
        public ICampaignTargetResponse CampaignTargetResponse
        {
            get
            {
                if (null == _DAO.CampaignTargetResponse) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ICampaignTargetResponse, CampaignTargetResponse_DAO>(_DAO.CampaignTargetResponse);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.CampaignTargetResponse = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.CampaignTargetResponse = (CampaignTargetResponse_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}