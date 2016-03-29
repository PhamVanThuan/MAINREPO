using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Configuration;
using SAHL.Common.Collections.Interfaces;
using SAHL.Test;
using SAHL.Common.Collections;
//using Rhino.Mocks;
using Castle.ActiveRecord;
using SAHL.Common.Factories;
using SAHL.Common.Configuration;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.CorrespondenceGeneration.CorrespondenceStrategies;
using SAHL.Common.BusinessModel.CorrespondenceGeneration.CorrespondenceBuilders;

namespace SAHL.Common.Web.UI.Test.Correspondence
{
    [TestFixture]
    public class CorrespondenceTest : TestBase
    {
        public CorrespondenceTest()
        {
            SetRepositoryStrategy(TypeFactoryStrategy.Default);
        }
        [NUnit.Framework.Test]
        public void CanLoadConfiguration()
        {
            CorrespondenceSection cs = (CorrespondenceSection)ConfigurationManager.GetSection(SAHL.Common.Constants.WebConfig.CorrespondenceSection);

            Assert.IsTrue(cs.Views.Count >= 1);

        }

        [NUnit.Framework.Test]
        public void CanGetReportData()
        {
            using (new SessionScope())
            {
                List<ReportData> rd = CorrespondenceStrategyWorker.GetReportData("Correspondence_Test", 4);

                Assert.IsTrue(rd.Count > 0);
                Assert.IsTrue(rd[0].GenericKeyParameterName == "AccountKey");
                Assert.IsTrue(rd[0].LegalEntityParameterName == "LegalEntityKey");
                Assert.IsTrue(rd[0].AddressParameterName == "AddressKey");
                Assert.IsTrue(rd[0].MailingTypeParameterName == "MailingType");
                Assert.IsTrue(rd[0].LanguageKeyParameterName == "LanguageKey");

                Assert.IsTrue(rd[0].ReportParameters.Count == 4);
                Assert.IsTrue(rd[0].ReportParameters[0].ParameterName == rd[0].GenericKeyParameterName);
                Assert.IsTrue(rd[0].ReportParameters[1].ParameterName == rd[0].MailingTypeParameterName);
                Assert.IsTrue(rd[0].ReportParameters[2].ParameterName == rd[0].LegalEntityParameterName);
                Assert.IsTrue(rd[0].ReportParameters[3].ParameterName == rd[0].AddressParameterName);
            }
        }

        [NUnit.Framework.Test]
        public void CanGetReportCorrespondenceMediums()
        {
            using (new SessionScope())
            {
                List<ReportData> rd = CorrespondenceStrategyWorker.GetReportData("Correspondence_Test", 4);

                Assert.IsTrue(rd.Count > 0);
                Assert.IsTrue(rd[0].CorrespondenceMediums.Count == 3);
                Assert.IsTrue(rd[0].CorrespondenceMediums[0].CorrespondenceMediumKey == (int)SAHL.Common.Globals.CorrespondenceMediums.Post);
                Assert.IsTrue(rd[0].CorrespondenceMediums[1].CorrespondenceMediumKey == (int)SAHL.Common.Globals.CorrespondenceMediums.Email);
                Assert.IsTrue(rd[0].CorrespondenceMediums[2].CorrespondenceMediumKey == (int)SAHL.Common.Globals.CorrespondenceMediums.Fax);
            }
        }

        [Test]
        public void GetCorrespondenceByKey()
        {
            using (new SessionScope())
            {
                ICorrespondenceRepository correspondenceRepo = RepositoryFactory.GetRepository<ICorrespondenceRepository>();
                // get first correspondence record
                Correspondence_DAO correspondenceFirst = Correspondence_DAO.FindFirst();
                int reportStatementKey = correspondenceFirst.ReportStatement.Key;
                int genericKey = correspondenceFirst.GenericKey;
                DateTime? completedDate = correspondenceFirst.CompletedDate;

                // get the correspondence records using the repo method
                ICorrespondence correspondence = correspondenceRepo.GetCorrespondenceByKey(correspondenceFirst.Key);

                Assert.IsFalse(correspondence == null);
                Assert.IsTrue(correspondence.ReportStatement.Key == reportStatementKey);
                Assert.IsTrue(correspondence.GenericKey == genericKey);
            }
        }

        [Test]
        public void GetCorrespondenceByReportStatementAndGenericKey()
        {
            using (new SessionScope())
            {
                ICorrespondenceRepository correspondenceRepo = RepositoryFactory.GetRepository<ICorrespondenceRepository>();
                // get first correspondence record
                Correspondence_DAO correspondenceFirst = Correspondence_DAO.FindFirst();
                int reportStatementKey = correspondenceFirst.ReportStatement.Key;
                int genericKey = correspondenceFirst.GenericKey;
                int genericKeyTypeKey = correspondenceFirst.GenericKeyType.Key;
                DateTime? completedDate = correspondenceFirst.CompletedDate;

                // get the correspondence records using the repo method
                IEventList<ICorrespondence> correspondenceAll = correspondenceRepo.GetCorrespondenceByReportStatementAndGenericKey(reportStatementKey, genericKey, genericKeyTypeKey, false);
                IEventList<ICorrespondence> correspondenceUnprocessedOnly = correspondenceRepo.GetCorrespondenceByReportStatementAndGenericKey(reportStatementKey, genericKey,genericKeyTypeKey, true);

                Assert.IsTrue(correspondenceAll.Count > 0);
                Assert.IsTrue(correspondenceAll[0].ReportStatement.Key == reportStatementKey);
                Assert.IsTrue(correspondenceAll[0].GenericKey == genericKey);

                if (completedDate == null)
                {
                    Assert.IsTrue(correspondenceUnprocessedOnly.Count > 0);
                    Assert.IsTrue(correspondenceUnprocessedOnly[0].GenericKey == genericKey);
                    Assert.IsTrue(correspondenceUnprocessedOnly[0].ReportStatement.Key == reportStatementKey);
                }
            }
        }

        [Test]
        public void GetCorrespondenceParametersByCorrespondenceKey()
        {
            using (new SessionScope())
            {
                ICorrespondenceRepository correspondenceRepo = RepositoryFactory.GetRepository<ICorrespondenceRepository>();
                // get first correspondenceparameter record
                CorrespondenceParameters_DAO dao = CorrespondenceParameters_DAO.FindFirst();

                if (dao == null)
                    Assert.Ignore("No Correspondence Records Found");
                else
                {

                    int correspondenceKey = dao.Correspondence.Key;

                    // get the correspondence parameter records
                    IEventList<ICorrespondenceParameters> correspondenceParameters = correspondenceRepo.GetCorrespondenceParametersByCorrespondenceKey(correspondenceKey);

                    Assert.IsTrue(correspondenceParameters.Count > 0);
                    Assert.IsTrue(correspondenceParameters[0].Correspondence.Key == dao.Correspondence.Key);
                }
            }

        }

        /// <summary>
        /// Test the GetCorrespondenceByGenericKey repository method 
        /// </summary>
        [Test]
        public void GetCorrespondenceByGenericKey()
        {
            using (new SessionScope())
            {
                ICorrespondenceRepository Repo = RepositoryFactory.GetRepository<ICorrespondenceRepository>();

                Correspondence_DAO cs = Correspondence_DAO.FindFirst();

                IEventList<ICorrespondence> correspondences = Repo.GetCorrespondenceByGenericKey(cs.GenericKey,cs.GenericKeyType.Key,false);

                Assert.IsNotNull(correspondences);
                Assert.AreEqual(correspondences[0].GenericKey, cs.GenericKey);
            }
        }

        /// <summary>
        /// Test the GetCorrespondenceByGenericKeys repository method 
        /// </summary>
        [Test]
        public void GetCorrespondenceByGenericKeys()
        {
            using (new SessionScope())
            {
                ICorrespondenceRepository Repo = RepositoryFactory.GetRepository<ICorrespondenceRepository>();

                Correspondence_DAO cs = Correspondence_DAO.FindFirst();

                Dictionary<int,int> genericKeyValues = new Dictionary<int,int>();
                genericKeyValues.Add(cs.GenericKey, cs.GenericKeyType.Key);

                IEventList<ICorrespondence> correspondences = Repo.GetCorrespondenceByGenericKeys(genericKeyValues, false);

                Assert.IsNotNull(correspondences);
                Assert.AreEqual(correspondences[0].GenericKey, cs.GenericKey);
            }
        }

        [Test]
        public void PopulateCorrespondenceParametersTest()
        {
            using (new SessionScope())
            {
                ILookupRepository lRepo = RepositoryFactory.GetRepository<ILookupRepository>();
                IReportRepository reportRepo = RepositoryFactory.GetRepository<IReportRepository>();
                ICorrespondenceRepository correspRepo = RepositoryFactory.GetRepository<ICorrespondenceRepository>();

                int accountKey = 1;
                int legalEntityKey = 1; 
                int addressKey = 1; 
                int ospKey = 4;

                IReportStatement reportStatement = reportRepo.GetReportStatementByNameAndOSP("NTU Letter", ospKey);

                // insert the correspondence record
                ICorrespondence correspondence = correspRepo.CreateEmptyCorrespondence();
                correspondence.GenericKey = accountKey;
                correspondence.GenericKeyType = lRepo.GenericKeyType.ObjectDictionary[Convert.ToString((int)SAHL.Common.Globals.GenericKeyTypes.Account)];
                correspondence.ReportStatement = reportStatement;
                correspondence.CorrespondenceMedium = lRepo.CorrespondenceMediums.ObjectDictionary[Convert.ToString((int)SAHL.Common.Globals.CorrespondenceMediums.Post)];
                correspondence.DueDate = DateTime.Now;
                correspondence.ChangeDate = DateTime.Now;
                correspondence.UserID = "Test";

                // insert the correpondenceparameters records
                foreach (IReportParameter reportParameter in reportStatement.ReportParameters)
                {
                    ICorrespondenceParameters correspondenceParm = correspRepo.CreateEmptyCorrespondenceParameter();
                    correspondenceParm.Correspondence = correspondence;
                    correspondenceParm.ReportParameter = reportParameter; // this is the line that failing
                    if (reportParameter.ParameterName.ToLower() == "accountkey")
                        correspondenceParm.ReportParameterValue = accountKey.ToString();
                    else if (reportParameter.ParameterName.ToLower() == "mailingtype")
                        correspondenceParm.ReportParameterValue = Convert.ToString((int)SAHL.Common.Globals.CorrespondenceMediums.Post);
                    else if (reportParameter.ParameterName.ToLower() == "legalentitykey")
                        correspondenceParm.ReportParameterValue = legalEntityKey > 0 ? legalEntityKey.ToString() : null;
                    else if (reportParameter.ParameterName.ToLower() == "addresskey")
                        correspondenceParm.ReportParameterValue = addressKey > 0 ? addressKey.ToString() : null;

                    // add the correspondenceparameter object to the correspondence object
                    correspondence.CorrespondenceParameters.Add(new DomainMessageCollection(), correspondenceParm);
                }

                Assert.IsTrue(correspondence.CorrespondenceParameters.Count > 0);
                Assert.IsTrue(correspondence.CorrespondenceParameters[0].ReportParameter != null);
            }
        }

        #region Correspondence Detail

        [Test]
        public void CreateEmptyCorrespondenceDetail()
        {
            using (new SessionScope())
            {
                ICorrespondenceRepository correspRepo = RepositoryFactory.GetRepository<ICorrespondenceRepository>();
                ICorrespondenceDetail correspondenceDetail = correspRepo.CreateEmptyCorrespondenceDetail();
                Assert.IsNotNull(correspondenceDetail);
            }
        }

        [Test]
        public void GetCorrespondenceDetailByKey()
        {
            using (new SessionScope())
            {
                ICorrespondenceRepository correspRepo = RepositoryFactory.GetRepository<ICorrespondenceRepository>();
                CorrespondenceDetail_DAO correspondenceDetailFirst = CorrespondenceDetail_DAO.FindFirst();
                if (correspondenceDetailFirst != null)
                {
                    int CorrespondenceKey = correspondenceDetailFirst.Key;
                    ICorrespondenceDetail correspondenceDetail = correspRepo.GetCorrespondenceDetailByKey(CorrespondenceKey);
                    Assert.IsNotNull(correspondenceDetail);
                }
            }
        }

        [Test]
        public void SaveCorrespondenceDetail()
        {
            using (new SessionScope())
            {
                ICorrespondenceRepository correspRepo = RepositoryFactory.GetRepository<ICorrespondenceRepository>();
                CorrespondenceDetail_DAO correspondenceDetailFirst = CorrespondenceDetail_DAO.FindFirst();
                if (correspondenceDetailFirst != null)
                {
                    int CorrespondenceKey = correspondenceDetailFirst.Key;
                    ICorrespondenceDetail correspondenceDetail = correspRepo.GetCorrespondenceDetailByKey(CorrespondenceKey);
                    correspRepo.SaveCorrespondenceDetail(correspondenceDetail);
                    Assert.IsNotNull(correspondenceDetail);
                }
            }
        }
        #endregion

    }
}
