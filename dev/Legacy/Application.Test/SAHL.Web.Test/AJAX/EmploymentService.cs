using System;
using System.Collections.Generic;
using System.Text;
using AjaxControlToolkit;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using NUnit.Framework;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI.Controls;
using SAHL.Web.AJAX;

namespace SAHL.Web.Test.AJAX
{
    [TestFixture]
    public class EmploymentService : TestViewBase
    {
        private Employment _empService;
        private IEmploymentRepository EmpRepo = RepositoryFactory.GetRepository<IEmploymentRepository>();

        [SetUp]
        public void Setup()
        {
            _empService = new Employment();
        }

        [TearDown]
        public void TearDown()
        {
            _empService = null;
        }

        [NUnit.Framework.Test]
        public void GetEmployers()
        {
            Employer_DAO emp = Employer_DAO.FindFirst();
            string prefix = emp.Name.Substring(0, emp.Name.Length - 2).ToLower();

            SAHLAutoCompleteItem[] items = _empService.GetEmployers(prefix);
            bool found = false;

            foreach (SAHLAutoCompleteItem item in items)
            {
                if (item.Value == emp.Key.ToString())
                    found = true;
            }

            if (!found)
                Assert.Fail("Unable to find employer with prefix {0}", prefix);
        }

        [NUnit.Framework.Test]
        public void GetRemunerationTypesByEmploymentType()
        {
            ILookupRepository lookupRep = RepositoryFactory.GetRepository<ILookupRepository>();

            foreach (string empType in Enum.GetNames(typeof(EmploymentTypes)))
            {
                int key = (int)Enum.Parse(typeof(EmploymentTypes), empType);
                CascadingDropDownNameValue[] items = _empService.GetRemunerationTypesByEmploymentType(AjaxHelpers.ConvertToDictionaryEntry(key), String.Empty);
            }
        }

        [NUnit.Framework.Test]
        public void GetSubsidyProviders()
        {
            using (new SessionScope())
            {
                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran("select top 1 subsidyproviderkey from subsidyprovider", typeof(SubsidyProvider_DAO), null);

                ISubsidyProvider subsidyProvider = EmpRepo.GetSubsidyProviderByKey((int)obj);

                string prefix = GetRegisteredName(subsidyProvider.LegalEntity);
                prefix = prefix.Substring(0, prefix.Length - 2);

                SAHLAutoCompleteItem[] items = _empService.GetSubsidyProviders(prefix);
                bool found = false;
                foreach (SAHLAutoCompleteItem item in items)
                {
                    if (item.Value == subsidyProvider.Key.ToString())
                        found = true;
                }

                if (!found)
                    Assert.Fail("Unable to find subsidy provider with prefix {0}", prefix);
            }
        }

        //[NUnit.Framework.Test]
        //public void GetSubsidyProvidersByType()
        //{
        //    using (new SessionScope())
        //    {
        //        SubsidyProvider_DAO sp = SubsidyProvider_DAO.FindFirst();
        //        string prefix = GetRegisteredName(sp.LegalEntity);
        //        prefix = prefix.Substring(0, 1);
        //        int spType = sp.SubsidyProviderType.Key;

        //        SAHLAutoCompleteItem[] items = _empService.GetSubsidyProvidersByType(prefix, spType.ToString());
        //        foreach (SAHLAutoCompleteItem item in items)
        //        {
        //            int key = Int32.Parse(item.Value);
        //            sp = SubsidyProvider_DAO.Find(key);

        //            if (sp.SubsidyProviderType.Key != spType)
        //                Assert.Fail("Subsidy provider {0} does not match expected type {1}", sp.Key, spType);
        //        }
        //    }
        //}

        /// <summary>
        /// Helper method to get the registered name of a company, close corporation or trust.
        /// </summary>
        /// <param name="le"></param>
        /// <returns></returns>
        private string GetRegisteredName(ILegalEntity le)
        {
            ILegalEntityCompany lec = le as ILegalEntityCompany;
            if (lec != null)
                return lec.RegisteredName;

            ILegalEntityCloseCorporation lecc = le as ILegalEntityCloseCorporation;
            if (lecc != null)
                return lecc.RegisteredName;

            ILegalEntityTrust let = le as ILegalEntityTrust;
            if (let != null)
                return let.RegisteredName;

            throw new NotSupportedException("Unsupported legal entity");
        }
    }
}