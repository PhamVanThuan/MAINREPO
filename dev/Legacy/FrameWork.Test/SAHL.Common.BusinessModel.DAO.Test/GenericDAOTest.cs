using NUnit.Framework;
using SAHL.Common.CacheData;
using SAHL.Common.Security;
using SAHL.Test;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    [TestFixture]
    public class GenericDAOTest : TestBase
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test, TestCaseSource(typeof(DAOProvider), "GetDaoTypesForFindFirst")]
        public void GenericFindFirst(Type daoType)
        {
            RunFindFirstTestOnDAO(daoType);
        }

        [Test, TestCaseSource(typeof(DAOProvider), "GetDaoTypesForLoadSaveLoad")]
        public void GenericLoadSaveLoad(Type daoType)
        {
            RunLoadSaveLoadTestOnDAO(daoType);
        }

        [Ignore]
        [Test]
        public void TestSpecific()
        {
            RunLoadSaveLoadTestOnDAO(typeof(ACBBranch_DAO));
        }

        private static void RunFindFirstTestOnDAO(Type daoType)
        {
            List<string> failedAssertions = new List<string>();
            List<string> listOfDAOsTested = new List<string>();
            List<string> ignoredDAOs = new List<string>();
            string keyColumn = "Key";
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            spc.DomainMessages.Clear();
            object daoInstance = Activator.CreateInstance(daoType);
            try
            {
                listOfDAOsTested.Add("FindFirst() \t: " + daoType.Name);
                Debug.WriteLine(string.Format("FindFirst DAO - {0}", daoType.Name));
                SAHL.Common.BusinessModel.DAO.Test.DAODataConsistencyChecker.FindFirst(daoInstance, keyColumn);
                SAHL.Common.BusinessModel.DAO.Test.DAODataConsistencyChecker.CleanUp();
                Assert.Pass(string.Format("Tested ({0})", daoType.Name));
            }
            catch (NUnit.Framework.SuccessException)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("Test Passed ({0})", daoType.Name));
            }
            catch (Exception ex)
            {
                SAHL.Common.BusinessModel.DAO.Test.DAODataConsistencyChecker.CleanUp();
                Debug.WriteLine(String.Format("FindFirst() Failed \t: {0}, Exception: {1}", daoType.UnderlyingSystemType.Name.ToString(), ex.Message));
                failedAssertions.Add(String.Format("FindFirst() Failed \t: {0}, Exception: {1}", daoType.UnderlyingSystemType.Name.ToString(), ex.Message));
            }
            Assert.That(failedAssertions.Count == 0, string.Join(" | ", failedAssertions.ToArray()));
        }

        private static void RunLoadSaveLoadTestOnDAO(Type daoType)
        {
            List<string> failedAssertions = new List<string>();
            List<string> listOfDAOsTested = new List<string>();
            string keyColumn = "Key";
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            spc.DomainMessages.Clear();
            object daoInstance = Activator.CreateInstance(daoType);
            try
            {
                listOfDAOsTested.Add("LoadSaveLoad() \t: " + daoType.Name);
                Debug.WriteLine(string.Format("Load Save Load DAO - {0}", daoType.Name));
                SAHL.Common.BusinessModel.DAO.Test.DAODataConsistencyChecker.LoadSaveLoad(daoInstance, keyColumn);
                SAHL.Common.BusinessModel.DAO.Test.DAODataConsistencyChecker.CleanUp();
                Assert.Pass(string.Format("Tested ({0})", daoType.Name));
            }
            catch (NUnit.Framework.SuccessException)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("Test Passed ({0})", daoType.Name));
            }
            catch (Exception ex)
            {
                SAHL.Common.BusinessModel.DAO.Test.DAODataConsistencyChecker.CleanUp();
                Debug.WriteLine(String.Format("LoadSaveLoad() Failed \t: {0}, Exception: {1}", daoType.UnderlyingSystemType.Name.ToString(), ex.Message));
                failedAssertions.Add(String.Format("LoadSaveLoad() Failed \t: {0}, Exception: {1}", daoType.UnderlyingSystemType.Name.ToString(), ex.Message));
            }
            Assert.That(failedAssertions.Count == 0, string.Join(" | ", failedAssertions.ToArray()));
        }
    }
}