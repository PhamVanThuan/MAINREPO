using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord.Queries;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(ICampaignRepository))]
    public class CampaignRepository : AbstractRepositoryBase, ICampaignRepository
    {
        public CampaignRepository()
        {

        }

        public ICampaignDefinition CreateEmptyCampaignDefinition()
        {
            return base.CreateEmpty<ICampaignDefinition, CampaignDefinition_DAO>();          
        }

        public ICampaignTarget CreateEmptyCampaignTarget()
        {
            return base.CreateEmpty<ICampaignTarget, CampaignTarget_DAO>();   
        }

        public ICampaignTargetContact CreateEmptyCampaignTargetContact()
        {
            return base.CreateEmpty<ICampaignTargetContact, CampaignTargetContact_DAO>(); 
        }

        public ICampaignTargetResponse CreateEmptyCampaignTargetResponse()
        {
            return base.CreateEmpty<ICampaignTargetResponse, CampaignTargetResponse_DAO>(); 
        }

        public ICampaignDefinition GetCampaignDefinitionByKey(int key)
        {
            return base.GetByKey<ICampaignDefinition, CampaignDefinition_DAO>(key);
        }


        public IList<ICampaignDefinition> GetCampaignDefinitionByName(string CampaignName)
        {
            string HQL = "from CampaignDefinition_DAO camp where camp.CampaignName = ? ";
            SimpleQuery<CampaignDefinition_DAO> q = new SimpleQuery<CampaignDefinition_DAO>(HQL, CampaignName);
            CampaignDefinition_DAO[] res = q.Execute();

            IList<ICampaignDefinition> retval = new List<ICampaignDefinition>();

            for (int i = 0; i < res.Length; i++)
            {
                retval.Add(new CampaignDefinition(res[i]));
            }
            return retval;
        }


        public IList<ICampaignDefinition> GetCampaignDefinitionByNameAndConfiguration(string CampaignName, DateTime StartDate,
                                                                DateTime EndDate, string CampaignReference)
        {
            string HQL = "from CampaignDefinition_DAO camp where camp.CampaignName = ? And camp.Startdate = ? And " +
                         "camp.EndDate = ? And camp.CampaignReference = ?";
            SimpleQuery<CampaignDefinition_DAO> q = new SimpleQuery<CampaignDefinition_DAO>(HQL, CampaignName, StartDate, EndDate, CampaignReference);
            CampaignDefinition_DAO[] res = q.Execute();

            IList<ICampaignDefinition> retval = new List<ICampaignDefinition>();

            for (int i = 0; i < res.Length; i++)
            {
                retval.Add(new CampaignDefinition(res[i]));
            }
            return retval;
        }

        public ICampaignTargetResponse GetCampaignTargetResponseByKey(int key)
        {
            return base.GetByKey<ICampaignTargetResponse, CampaignTargetResponse_DAO>(key);
        }

        public void SaveCampaignDefinition(ICampaignDefinition campaignDefinition)
        {
            base.Save<ICampaignDefinition, CampaignDefinition_DAO>(campaignDefinition);
        }
    }
}
