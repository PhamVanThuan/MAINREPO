using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using Castle.ActiveRecord;
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
using SAHL.Test;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class LookupRepositoryTest : TestBase
    {
        private ILookupRepository _repo = RepositoryFactory.GetRepository<ILookupRepository>();

        /// <summary>
        /// Runs through all properties of the LookupRepository and ensures that they return
        /// This uses reflection to run through all the properties.
        /// </summary>
        [NUnit.Framework.Test]
        public void LoadAll()
        {
            // we need to use the actual repository class here for the reflection to work correctly
            LookupRepository repo = new LookupRepository();
            Type t = repo.GetType();
            PropertyInfo[] properties = t.GetProperties();
            foreach (PropertyInfo pi in properties)
            {
                object obj = t.InvokeMember(pi.Name, BindingFlags.GetProperty, null, repo, null);
                Assert.IsNotNull(obj);

                string msg = "Lookup " + pi.Name + " appears to be empty.";

                // do some type checking
                IDictionary dict = obj as IDictionary;
                IList list = obj as IList;
                IEnumerable enumerable = obj as IEnumerable;

                if (dict != null)
                    Assert.Greater(dict.Count, 0, msg);
                else if (list != null)
                    Assert.Greater(list.Count, 0, msg);
                else if (enumerable != null)
                {
                    IEnumerator enumerator = enumerable.GetEnumerator();
                    Assert.IsNotNull(enumerator);
                    Assert.IsTrue(enumerator.MoveNext(), msg);
                }
                else
                    Assert.Fail("Unhandled type: " + obj.GetType().FullName);

                Console.Out.WriteLine("Loaded lookup " + pi.Name);
            }
        }

        [Test]
        public void SortedDAOEventListTest()
        {
            CBOMenu_DAO[] test = CBOMenu_DAO.FindAll();

            SortedDAOEventList<CBOMenu_DAO, ICBOMenu, CBOMenu> _sortedChildMenus = new SortedDAOEventList<CBOMenu_DAO, ICBOMenu, CBOMenu>(test, "Key", "Sequence", true);
            foreach (ICBOMenu item in _sortedChildMenus)
            {
                System.Diagnostics.Debug.WriteLine("Main CBOMenu : ");
                System.Diagnostics.Debug.WriteLine(" -- " + item.Sequence);
                System.Diagnostics.Debug.WriteLine("    Child CBOMenu : ");
                foreach (ICBOMenu subitem in item.ChildMenus)
                {
                    System.Diagnostics.Debug.WriteLine("    ---- " + subitem.Sequence);
                }
                System.Diagnostics.Debug.WriteLine("    Child ContextMenu : ");
                foreach (IContextMenu subitem in item.ContextMenus)
                {
                    System.Diagnostics.Debug.WriteLine("    ---- " + subitem.Sequence);
                }
                System.Diagnostics.Debug.WriteLine("");
            }
        }

        /// <summary>
        /// Tests the PrioritiesByOSP method.
        /// </summary>
        [Test]
        public void PrioritiesByOSP()
        {
            using (new SessionScope())
            {
                int count = 0;

                // load up all the OSPs, and then run through them - make sure that the same Priority
                // doesn't appear for different OSPs otherwise there's a logic error
                OriginationSourceProduct_DAO[] list = OriginationSourceProduct_DAO.FindAll();

                List<int> priorityKeys = new List<int>();

                foreach (OriginationSourceProduct_DAO osp in list)
                {
                    IEventList<IPriority> priorities = _repo.PrioritiesByOSP(osp.Key);
                    Assert.IsNotNull(priorities);

                    foreach (IPriority p in priorities)
                    {
                        if (priorityKeys.Contains(p.Key))
                            Assert.Fail("Priority " + p.Key.ToString() + " appears for more than one OSP");

                        priorityKeys.Add(p.Key);
                    }

                    count += priorities.Count;
                }

                // make sure at least one collection was returned
                Assert.Greater(count, 0, "PrioritiesByOSP only returned 0");
            }
        }

        /// <summary>
        /// Tests the ProvincesByCountry method.
        /// </summary>
        [Test]
        public void ProvincesByCountry()
        {
            using (new SessionScope())
            {
                int count = 0;

                // load up all the countries, and then run through them - make sure that the same Priority
                // doesn't appear for different OSPs otherwise there's a logic error
                Country_DAO[] list = Country_DAO.FindAll();

                List<int> provinceKeys = new List<int>();

                foreach (Country_DAO c in list)
                {
                    IDictionary<int, string> provinces = _repo.ProvincesByCountry(c.Key);
                    Assert.IsNotNull(provinces);

                    foreach (KeyValuePair<int, string> kvp in provinces)
                    {
                        if (provinceKeys.Contains(kvp.Key))
                            Assert.Fail("Province " + kvp.Key.ToString() + " appears for more than one Country");

                        provinceKeys.Add(kvp.Key);
                    }

                    count += provinces.Count;
                }

                // make sure at least one collection was returned
                Assert.Greater(count, 0, "ProvincesByCountry only returned 0");
            }
        }

        [Test]
        public void AddressFormatsByAddressType()
        {
            IDictionary<int, string> residentialAddressTypes = _repo.AddressFormatsByAddressType(SAHL.Common.Globals.AddressTypes.Residential);
            Assert.AreEqual(residentialAddressTypes.Count, 2);
            IDictionary<int, string> postalAddressTypes = _repo.AddressFormatsByAddressType(SAHL.Common.Globals.AddressTypes.Postal);
            Assert.AreEqual(postalAddressTypes.Count, 5);
        }

        [Test]
        public void HelpDeskCategoriesActive()
        {
            IStageDefinitionRepository stageDefinitionRepository = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
            string sql = @"select count(HelpDeskCategorykey) as KeyCount from dbo.HelpDeskCategory where generalstatuskey = 1";
            int iHpCount = 0;
            using (new SessionScope())
            {
                ParameterCollection parameters = new ParameterCollection();
                DataSet dsHelpDesk = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), parameters);

                if (dsHelpDesk != null)
                {
                    if (dsHelpDesk.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsHelpDesk.Tables[0].Rows)
                        {
                            iHpCount = Convert.ToInt32(dr[0]);
                        }

                        IList<IHelpDeskCategory> helpdesks = _repo.HelpDeskCategoriesActive((int)SAHL.Common.Globals.GeneralStatuses.Active);

                        Assert.AreEqual(helpdesks.Count, iHpCount);
                    }
                }
            }
        }

        [Test]
        public void ResetAllTest()
        {
            IStageDefinitionRepository stageDefinitionRepository = RepositoryFactory.GetRepository<IStageDefinitionRepository>();

            try
            {
                _repo.ResetAll();
            }
            catch
            {
                Assert.Fail();
            }
        }

        [Test]
        public void ResetAllLookUpTest()
        {
            IStageDefinitionRepository stageDefinitionRepository = RepositoryFactory.GetRepository<IStageDefinitionRepository>();

            try
            {
                _repo.ResetLookup(LookupKeys.Countries);
                _repo.ResetLookup(LookupKeys.Languages);
                _repo.ResetLookup(LookupKeys.Priorities);
            }
            catch
            {
                Assert.Fail();
            }
        }
    }
}