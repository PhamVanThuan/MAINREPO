using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using NUnit.Framework;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Test;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class StageDefinitionRepositoryTest : TestBase
    {
        [Test]
        public void GetStageDefinitionGroupByKey()
        {
            using (new SessionScope())
            {
                StageDefinitionGroup_DAO sdg = StageDefinitionGroup_DAO.FindFirst();

                IStageDefinitionRepository Repo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
                IStageDefinitionGroup Group = Repo.GetStageDefinitionGroupByKey(sdg.Key);

                Assert.AreEqual(sdg.Key, Group.Key);
            }
        }

        [Test]
        public void GetStageDefinitionStageDefinitionGroup()
        {
            using (new SessionScope())
            {
                IStageDefinitionRepository Repo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();

                StageDefinitionStageDefinitionGroup_DAO sdsdg = StageDefinitionStageDefinitionGroup_DAO.FindFirst();

                IStageDefinitionStageDefinitionGroup stageDefinitionStageDefinitionGroup = Repo.GetStageDefinitionStageDefinitionGroup(sdsdg.StageDefinitionGroup.Key, sdsdg.StageDefinition.Key);

                Assert.AreEqual(sdsdg.Key, stageDefinitionStageDefinitionGroup.Key);
            }
        }

        /// <summary>
        /// A very basic test to see if the GetStageTransitionsByGenericKey method executes.
        /// </summary>
        [Test]
        public void GetStageTransitionsByGenericKey()
        {
            using (new SessionScope())
            {
                IStageDefinitionRepository Repo = new StageDefinitionRepository();

                StageTransition_DAO st = StageTransition_DAO.FindFirst();

                IList<IStageTransition> Transitions = Repo.GetStageTransitionsByGenericKey(st.GenericKey);

                Assert.IsNotNull(Transitions);
                for (int i = 0; i < Transitions.Count; i++)
                {
                    Assert.AreEqual(Transitions[i].GenericKey, st.GenericKey);
                }
            }
        }

        /// <summary>
        /// Test the GetStageTransitionsByGenericKeys repository method
        /// </summary>
        [Test]
        public void GetStageTransitionsByGenericKeys()
        {
            using (new SessionScope())
            {
                IStageDefinitionRepository Repo = new StageDefinitionRepository();

                StageTransition_DAO st = StageTransition_DAO.FindFirst();

                IList<int> stageTransitions = new List<int>();
                stageTransitions.Add(st.GenericKey);

                IList<IStageTransition> Transitions = Repo.GetStageTransitionsByGenericKeys(stageTransitions);

                Assert.IsNotNull(Transitions);
                for (int i = 0; i < Transitions.Count; i++)
                {
                    Assert.AreEqual(Transitions[i].GenericKey, st.GenericKey);
                }
            }
        }

        [Test]
        public void SaveStageTransition()
        {
            int key = 0;
            string comments = string.Empty;

            IStageDefinitionRepository stageDefinitionRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();

            // create a new StageTransition object
            StageTransition_DAO stageTransition = DAODataConsistancyChecker.GetDAO<StageTransition_DAO>();

            using (new TransactionScope(TransactionMode.New, OnDispose.Rollback))
            {
                // make some changes and save using the Repository method
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IStageTransition st = BMTM.GetMappedType<StageTransition>(stageTransition);

                st.Comments = "My New Comments";

                stageDefinitionRepo.SaveStageTransition(st);

                key = st.Key;

                // retrieve a new version of the object and ensure the value has been changed
                StageTransition_DAO stageTransitionCheck = StageTransition_DAO.Find(key) as StageTransition_DAO;

                // compare the results
                Assert.AreEqual(stageTransitionCheck.Key, key);
                Assert.AreNotEqual(stageTransitionCheck.Comments, comments);
                Assert.IsTrue(stageTransitionCheck.Comments == "My New Comments");
            }
        }

        [Test]
        public void SaveStageTransitionWithParameters()
        {
            IStageDefinitionRepository stageDefinitionRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
            IOrganisationStructureRepository organisationStructureRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            IADUser adUser = organisationStructureRepo.GetAdUserForAdUserName("System");

            using (new TransactionScope(TransactionMode.New, OnDispose.Rollback))
            {
                StageDefinitionStageDefinitionGroup_DAO sdsdg = StageDefinitionStageDefinitionGroup_DAO.FindFirst();

                // Create a new stagetransition record using parameters
                IStageTransition stageTransition = stageDefinitionRepo.SaveStageTransition(9876789, sdsdg.StageDefinitionGroup.Key, sdsdg.StageDefinition.Description, "My Test Comments", adUser);

                // retrieve a new version of the object and ensure the value has been changed
                StageTransition_DAO st = StageTransition_DAO.Find(stageTransition.Key) as StageTransition_DAO;
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IStageTransition stageTransitionCheck = BMTM.GetMappedType<StageTransition>(st);

                // compare the results
                Assert.AreEqual(stageTransitionCheck.Key, stageTransition.Key);
                Assert.AreEqual(stageTransitionCheck.StageDefinitionStageDefinitionGroup.Key, stageTransition.StageDefinitionStageDefinitionGroup.Key);
                Assert.AreEqual(stageTransitionCheck.StageDefinitionStageDefinitionGroup.StageDefinition.Key, stageTransition.StageDefinitionStageDefinitionGroup.StageDefinition.Key);
                Assert.AreEqual(stageTransitionCheck.StageDefinitionStageDefinitionGroup.StageDefinitionGroup.Key, stageTransition.StageDefinitionStageDefinitionGroup.StageDefinitionGroup.Key);
            }
        }

        /// <summary>
        /// Get StageDefinition by StageDefinitonGroupKey and StageDefinition desc.
        /// </summary>
        [Test]
        public void GetStageDefinitionByDescription()
        {
            using (new SessionScope())
            {
                string stageDefinitionDescription = SAHL.Common.Constants.StageDefinitionConstants.ContactPersonChanged;

                IStageDefinitionRepository Repo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();

                IStageDefinition stageDefinition = Repo.GetStageDefinitionByDescription(stageDefinitionDescription);

                Assert.IsNotNull(stageDefinition);
                Assert.IsTrue(stageDefinition.Description == SAHL.Common.Constants.StageDefinitionConstants.ContactPersonChanged);
            }
        }

        [Test]
        public void CheckCompositeStageDefinition()
        {
            try
            {
                IStageDefinitionRepository stageDefinitionRepository = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
                string sql = @"Select top 1 StageTransitionKey, GenericKey, StageDefinitionStageDefinitionGroupKey"
                             + " From StageTransitionComposite (nolock)";

                using (new SessionScope())
                {
                    ParameterCollection parameters = new ParameterCollection();
                    DataSet dsCount = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), parameters);

                    int iGenericKey = 0;
                    int iStageDefinitionStageDefinitionGroupKey = 0;
                    if (dsCount != null)
                    {
                        if (dsCount.Tables.Count > 0)
                        {
                            foreach (DataRow dr in dsCount.Tables[0].Rows)
                            {
                                iGenericKey = Convert.ToInt32(dr["GenericKey"]);
                                iStageDefinitionStageDefinitionGroupKey = Convert.ToInt32(dr["StageDefinitionStageDefinitionGroupKey"]);
                            }
                        }
                    }
                    bool bCheck = stageDefinitionRepository.CheckCompositeStageDefinition(iGenericKey, iStageDefinitionStageDefinitionGroupKey);
                    Assert.IsNotNull(bCheck);

                    //Assert.IsTrue(stageDefinitionRepository.CountCompositeStageOccurance(AccountKey, StageDefinitionStageDefinitionGroupKey) == RowCount);
                }
            }
            catch (Exception E)
            {
                throw;
            }
        }

        [Test]
        public void CountCompositeStageOccurance()
        {
            try
            {
                IStageDefinitionRepository stageDefinitionRepository = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
                string sql = @"Select top 1 StageTransitionKey, GenericKey, StageDefinitionStageDefinitionGroupKey"
                             + " From StageTransitionComposite (nolock)";

                using (new SessionScope())
                {
                    ParameterCollection parameters = new ParameterCollection();
                    DataSet dsCount = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), parameters);

                    int iGenericKey = 0;
                    int iStageDefinitionStageDefinitionGroupKey = 0;
                    if (dsCount != null)
                    {
                        if (dsCount.Tables.Count > 0)
                        {
                            foreach (DataRow dr in dsCount.Tables[0].Rows)
                            {
                                iGenericKey = Convert.ToInt32(dr["GenericKey"]);
                                iStageDefinitionStageDefinitionGroupKey = Convert.ToInt32(dr["StageDefinitionStageDefinitionGroupKey"]);
                            }
                        }
                    }
                    int iCount = stageDefinitionRepository.CountCompositeStageOccurance(iGenericKey, iStageDefinitionStageDefinitionGroupKey);
                    Assert.IsNotNull(iCount);

                    //Assert.IsTrue(stageDefinitionRepository.CountCompositeStageOccurance(AccountKey, StageDefinitionStageDefinitionGroupKey) == RowCount);
                }
            }
            catch (Exception E)
            {
                throw;
            }
        }

        [Test]
        public void GetStageDefinitionStageDefinitionGroupKey()
        {
            IStageDefinitionRepository stageDefinitionRepository = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
            string sql = @"select top 1 sdsdg.StageDefinitionGroupKey, sdsdg.StageDefinitionKey
                            from StageDefinitionStageDefinitionGroup sdsdg (nolock)";

            using (new SessionScope())
            {
                ParameterCollection parameters = new ParameterCollection();
                DataSet dsGroupKey = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), parameters);

                int iStageDefinitionKey = 0;
                int iStageDefinitionGroupKey = 0;
                if (dsGroupKey != null)
                {
                    if (dsGroupKey.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsGroupKey.Tables[0].Rows)
                        {
                            iStageDefinitionKey = Convert.ToInt32(dr["StageDefinitionKey"]);
                            iStageDefinitionGroupKey = Convert.ToInt32(dr["StageDefinitionGroupKey"]);
                        }
                    }
                }
                int iGroupKey = stageDefinitionRepository.GetStageDefinitionStageDefinitionGroupKey(iStageDefinitionGroupKey, iStageDefinitionKey);
            }
        }

        [Test]
        public void GetLastStageTransitionComposite()
        {
            IStageDefinitionRepository stageDefinitionRepository = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
            string sql = @"select top 1 GenericKey, StageDefinitionStageDefinitionGroupKey " +
                            "From StageTransitionComposite (nolock)";

            using (new SessionScope())
            {
                ParameterCollection parameters = new ParameterCollection();
                DataSet dsComposite = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), parameters);

                int iGenericKey = 0;
                int iStageDefinitionStageDefinitionGroupKey = 0;
                IList<int> stageDefinitionStageDefinitionGroups = new List<int>();
                if (dsComposite != null)
                {
                    if (dsComposite.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsComposite.Tables[0].Rows)
                        {
                            iGenericKey = Convert.ToInt32(dr["GenericKey"]);
                            iStageDefinitionStageDefinitionGroupKey = Convert.ToInt32(dr["StageDefinitionStageDefinitionGroupKey"]);
                            stageDefinitionStageDefinitionGroups.Add(iStageDefinitionStageDefinitionGroupKey);
                        }
                    }

                    IStageTransitionComposite IComposite = stageDefinitionRepository.GetLastStageTransitionComposite(iGenericKey, stageDefinitionStageDefinitionGroups);
                    Assert.IsNotNull(IComposite);
                }
            }
        }

        [Test]
        public void CreateEmptyStageTransition()
        {
            IStageDefinitionRepository stageDefinitionRepository = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
            IStageTransition stageTransition = stageDefinitionRepository.CreateEmptyStageTransition();
            Assert.IsNotNull(stageTransition);
        }

        [Test]
        public void GetPreviousTransition()
        {
            IStageDefinitionRepository stageDefinitionRepository = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
            string sql = @"Select top 1 StageTransitionKey, GenericKey, StageDefinitionStageDefinitionGroupKey"
                          + " From StageTransition (nolock)";

            using (new SessionScope())
            {
                ParameterCollection parameters = new ParameterCollection();
                DataSet dsStageTransition = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), parameters);

                int iStageTransitionKey = 0;
                int iGenericKey = 0;
                int iStageDefinitionStageDefinitionGroupKey = 0;
                if (dsStageTransition != null)
                {
                    if (dsStageTransition.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsStageTransition.Tables[0].Rows)
                        {
                            iStageTransitionKey = Convert.ToInt32(dr["StageTransitionKey"]);
                            iGenericKey = Convert.ToInt32(dr["GenericKey"]);
                            iStageDefinitionStageDefinitionGroupKey = Convert.ToInt32(dr["StageDefinitionStageDefinitionGroupKey"]);
                        }
                    }

                    IStageTransition stageTransition = stageDefinitionRepository.GetPreviousTransition(iGenericKey, iStageDefinitionStageDefinitionGroupKey, iStageTransitionKey);
                    Assert.IsNotNull(stageTransition);
                }
            }
        }

        [Test]
        public void GetStageTransitionByKey()
        {
            IStageDefinitionRepository stageDefinitionRepository = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
            string sql = @"Select top 1 StageTransitionKey"
                        + " From StageTransition (nolock)";

            using (new SessionScope())
            {
                ParameterCollection parameters = new ParameterCollection();
                DataSet dsStageTransition = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), parameters);

                int iStageTransitionKey = 0;

                if (dsStageTransition != null)
                {
                    if (dsStageTransition.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsStageTransition.Tables[0].Rows)
                        {
                            iStageTransitionKey = Convert.ToInt32(dr["StageTransitionKey"]);
                        }
                    }

                    IStageTransition stageTransition = stageDefinitionRepository.GetStageTransitionByKey(iStageTransitionKey);
                    Assert.IsNotNull(stageTransition);
                }
            }
        }
    }
}