using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Test;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using System.Data;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.DAO;


namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class CampaignRepositoryTest : TestBase
    {
        private ICampaignRepository _campaignRepo = RepositoryFactory.GetRepository<ICampaignRepository>();

        [Test]
        public void SaveCampaign()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                LegalEntityNaturalPerson_DAO legalEntity = LegalEntityNaturalPerson_DAO.FindFirst();
                Account_DAO account = Account_DAO.FindFirst();
                ADUser_DAO adUser = ADUser_DAO.FindFirst();
                MarketingOption_DAO marketingOption = MarketingOption_DAO.FindFirst();
                OrganisationStructure_DAO orgStructure = OrganisationStructure_DAO.FindFirst();

                // Setup CampaignDefinition
                ICampaignDefinition campaignDefinition = _campaignRepo.CreateEmptyCampaignDefinition();
                campaignDefinition.CampaignName = "CAP TEST";
                campaignDefinition.CampaignReference = "TEST";
                campaignDefinition.Startdate = DateTime.Now;
                campaignDefinition.EndDate = null;
                campaignDefinition.MarketingOptionKey = marketingOption.Key;
                campaignDefinition.OrganisationStructureKey = orgStructure.Key;
                campaignDefinition.GeneralStatusKey = (int)GeneralStatuses.Active;
                campaignDefinition.ParentCampaignDefinition = null;
                campaignDefinition.ReportStatement = null;
                campaignDefinition.ADUserKey = adUser.Key;
                campaignDefinition.DataProviderDataServiceKey = (int)DataProviderDataServices.AdCheckDesktopValuation;
                campaignDefinition.MarketingOptionRelevanceKey = (int)MarketingOptionRelevances.Relevant;

                // Setup CampaignTarget
                ICampaignTarget campaignTarget = _campaignRepo.CreateEmptyCampaignTarget();
                campaignTarget.CampaignDefinition = campaignDefinition;
                campaignTarget.GenericKey = account.Key;
                campaignTarget.GenericKeyTypeKey = (int)GenericKeyTypes.Account;
                campaignTarget.ADUserKey = adUser.Key;

                // Add CampaignTarget to CampaignDefinition
                campaignDefinition.CampaignTargets.Add(null, campaignTarget);

                // Setup CampaignTargetContact
                ICampaignTargetContact campaignTargetContact = _campaignRepo.CreateEmptyCampaignTargetContact();
                campaignTargetContact.CampaignTarget = campaignTarget;
                campaignTargetContact.LegalEntityKey = legalEntity.Key;
                campaignTargetContact.ChangeDate = DateTime.Now;
                campaignTargetContact.AdUserKey = adUser.Key;
                campaignTargetContact.CampaignTargetResponse = _campaignRepo.GetCampaignTargetResponseByKey((int)CampaignTargetResponses.Callbackrequest);

                // Add CampaignTargetContact to CampaignTarget
                campaignTarget.CampaignTargetContacts.Add(null, campaignTargetContact);

                // Save CampaignDefinition
                _campaignRepo.SaveCampaignDefinition(campaignDefinition);

                Assert.IsTrue(campaignDefinition.Key > 0);
                Assert.IsTrue(campaignDefinition.CampaignTargets[0].Key > 0);
                Assert.IsTrue(campaignDefinition.CampaignTargets[0].CampaignTargetContacts[0].Key > 0);
                Assert.IsTrue(campaignDefinition.CampaignTargets[0].CampaignTargetContacts[0].CampaignTargetResponse.Key > 0);

            }
        }


        [Test]
        public void GetCampaignDefinitionByName()
        {
            using ((new SessionScope()))
            {
                string query = @"SELECT TOP 1 CampaignName FROM [2AM].[dbo].[CampaignDefinition] cd (nolock)";
                DataTable DT = base.GetQueryResults(query);

                if (DT.Rows.Count == 0)
                    Assert.Ignore("No data available");

                string CampaignName = Convert.ToString(DT.Rows[0][0]);

                ICampaignRepository repo = RepositoryFactory.GetRepository<ICampaignRepository>();
                IList<ICampaignDefinition> CampaignList = repo.GetCampaignDefinitionByName(CampaignName);
                Assert.IsNotNull(CampaignList);
                //Assert.That(CampaignList[0].CampaignName == CampaignName);
            }
        }

        [Test]
        public void GetCampaignDefinitionByNameAndConfiguration()
        {
            using ((new SessionScope()))
            {
                string query = @"SELECT TOP 1 CampaignName, CampaignReference, StartDate, EndDate FROM [2AM].[dbo].[CampaignDefinition] cd (nolock)" +
                                " Where StartDate is not null and EndDate is not null and CampaignReference is not null";
                DataTable DT = base.GetQueryResults(query);

                if (DT.Rows.Count == 0)
                    Assert.Ignore("No data available");

                string CampaignName = Convert.ToString(DT.Rows[0][0]);
                string CampaignReference = Convert.ToString(DT.Rows[0][1]);
                DateTime StartDate = Convert.ToDateTime(DT.Rows[0][2]);
                DateTime EndDate = Convert.ToDateTime(DT.Rows[0][3]);

                ICampaignRepository repo = RepositoryFactory.GetRepository<ICampaignRepository>();
                IList<ICampaignDefinition> CampaignList = repo.GetCampaignDefinitionByNameAndConfiguration(CampaignName,StartDate, 
                                                                                                        EndDate, CampaignReference);
                Assert.IsNotNull(CampaignList);
            }
        }
    }
}
