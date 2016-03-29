using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO
    /// </summary>
    public partial interface ICampaignDefinition : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.CampaignName
        /// </summary>
        System.String CampaignName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.CampaignReference
        /// </summary>
        System.String CampaignReference
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.Startdate
        /// </summary>
        DateTime? Startdate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.EndDate
        /// </summary>
        DateTime? EndDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.MarketingOptionKey
        /// </summary>
        Int32? MarketingOptionKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.OrganisationStructureKey
        /// </summary>
        System.Int32 OrganisationStructureKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.GeneralStatusKey
        /// </summary>
        System.Int32 GeneralStatusKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.ReportStatement
        /// </summary>
        IReportStatement ReportStatement
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.ADUserKey
        /// </summary>
        System.Int32 ADUserKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.DataProviderDataServiceKey
        /// </summary>
        System.Int32 DataProviderDataServiceKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.MarketingOptionRelevanceKey
        /// </summary>
        System.Int32 MarketingOptionRelevanceKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.CampaignTargets
        /// </summary>
        IEventList<ICampaignTarget> CampaignTargets
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.ChildCampaignDefinitions
        /// </summary>
        IEventList<ICampaignDefinition> ChildCampaignDefinitions
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.ParentCampaignDefinition
        /// </summary>
        ICampaignDefinition ParentCampaignDefinition
        {
            get;
            set;
        }
    }
}