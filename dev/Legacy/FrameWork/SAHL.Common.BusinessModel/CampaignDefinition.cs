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
    /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO
    /// </summary>
    public partial class CampaignDefinition : BusinessModelBase<CampaignDefinition_DAO>, ICampaignDefinition
    {
        public CampaignDefinition(SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO CampaignDefinition)
            : base(CampaignDefinition)
        {
            this._DAO = CampaignDefinition;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.CampaignName
        /// </summary>
        public String CampaignName
        {
            get { return _DAO.CampaignName; }
            set { _DAO.CampaignName = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.CampaignReference
        /// </summary>
        public String CampaignReference
        {
            get { return _DAO.CampaignReference; }
            set { _DAO.CampaignReference = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.Startdate
        /// </summary>
        public DateTime? Startdate
        {
            get { return _DAO.Startdate; }
            set { _DAO.Startdate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.EndDate
        /// </summary>
        public DateTime? EndDate
        {
            get { return _DAO.EndDate; }
            set { _DAO.EndDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.MarketingOptionKey
        /// </summary>
        public Int32? MarketingOptionKey
        {
            get { return _DAO.MarketingOptionKey; }
            set { _DAO.MarketingOptionKey = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.OrganisationStructureKey
        /// </summary>
        public Int32 OrganisationStructureKey
        {
            get { return _DAO.OrganisationStructureKey; }
            set { _DAO.OrganisationStructureKey = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.GeneralStatusKey
        /// </summary>
        public Int32 GeneralStatusKey
        {
            get { return _DAO.GeneralStatusKey; }
            set { _DAO.GeneralStatusKey = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.ReportStatement
        /// </summary>
        public IReportStatement ReportStatement
        {
            get
            {
                if (null == _DAO.ReportStatement) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IReportStatement, ReportStatement_DAO>(_DAO.ReportStatement);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ReportStatement = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ReportStatement = (ReportStatement_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.ADUserKey
        /// </summary>
        public Int32 ADUserKey
        {
            get { return _DAO.ADUserKey; }
            set { _DAO.ADUserKey = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.DataProviderDataServiceKey
        /// </summary>
        public Int32 DataProviderDataServiceKey
        {
            get { return _DAO.DataProviderDataServiceKey; }
            set { _DAO.DataProviderDataServiceKey = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.MarketingOptionRelevanceKey
        /// </summary>
        public Int32 MarketingOptionRelevanceKey
        {
            get { return _DAO.MarketingOptionRelevanceKey; }
            set { _DAO.MarketingOptionRelevanceKey = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.CampaignTargets
        /// </summary>
        private DAOEventList<CampaignTarget_DAO, ICampaignTarget, CampaignTarget> _CampaignTargets;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.CampaignTargets
        /// </summary>
        public IEventList<ICampaignTarget> CampaignTargets
        {
            get
            {
                if (null == _CampaignTargets)
                {
                    if (null == _DAO.CampaignTargets)
                        _DAO.CampaignTargets = new List<CampaignTarget_DAO>();
                    _CampaignTargets = new DAOEventList<CampaignTarget_DAO, ICampaignTarget, CampaignTarget>(_DAO.CampaignTargets);
                    _CampaignTargets.BeforeAdd += new EventListHandler(OnCampaignTargets_BeforeAdd);
                    _CampaignTargets.BeforeRemove += new EventListHandler(OnCampaignTargets_BeforeRemove);
                    _CampaignTargets.AfterAdd += new EventListHandler(OnCampaignTargets_AfterAdd);
                    _CampaignTargets.AfterRemove += new EventListHandler(OnCampaignTargets_AfterRemove);
                }
                return _CampaignTargets;
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.ChildCampaignDefinitions
        /// </summary>
        private DAOEventList<CampaignDefinition_DAO, ICampaignDefinition, CampaignDefinition> _ChildCampaignDefinitions;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.ChildCampaignDefinitions
        /// </summary>
        public IEventList<ICampaignDefinition> ChildCampaignDefinitions
        {
            get
            {
                if (null == _ChildCampaignDefinitions)
                {
                    if (null == _DAO.ChildCampaignDefinitions)
                        _DAO.ChildCampaignDefinitions = new List<CampaignDefinition_DAO>();
                    _ChildCampaignDefinitions = new DAOEventList<CampaignDefinition_DAO, ICampaignDefinition, CampaignDefinition>(_DAO.ChildCampaignDefinitions);
                    _ChildCampaignDefinitions.BeforeAdd += new EventListHandler(OnChildCampaignDefinitions_BeforeAdd);
                    _ChildCampaignDefinitions.BeforeRemove += new EventListHandler(OnChildCampaignDefinitions_BeforeRemove);
                    _ChildCampaignDefinitions.AfterAdd += new EventListHandler(OnChildCampaignDefinitions_AfterAdd);
                    _ChildCampaignDefinitions.AfterRemove += new EventListHandler(OnChildCampaignDefinitions_AfterRemove);
                }
                return _ChildCampaignDefinitions;
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.ParentCampaignDefinition
        /// </summary>
        public ICampaignDefinition ParentCampaignDefinition
        {
            get
            {
                if (null == _DAO.ParentCampaignDefinition) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ICampaignDefinition, CampaignDefinition_DAO>(_DAO.ParentCampaignDefinition);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ParentCampaignDefinition = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ParentCampaignDefinition = (CampaignDefinition_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _CampaignTargets = null;
            _ChildCampaignDefinitions = null;
        }
    }
}