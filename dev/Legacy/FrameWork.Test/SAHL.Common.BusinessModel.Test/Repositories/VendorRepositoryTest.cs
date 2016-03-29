using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Test;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class VendorRepositoryTest : TestBase
    {
        private IVendorRepository _repo = RepositoryFactory.GetRepository<IVendorRepository>();

        [Test]
        public void GetVendorByLegalEntityKey_WhenVendorNotLinkedToLegalEntityKey()
        {
            using (new SessionScope())
            {
                IVendor vendor = _repo.GetVendorByLegalEntityKey(0);
                Assert.IsNull(vendor);
            }
        }

        [Test]
        public void GetVendorByLegalEntityKey_WhenVendorIsLinkedToLegalEntityKey()
        {
            using (new SessionScope())
            {
                string sql = @"select top 1 LegalEntityKey 
                           from [2AM].dbo.Vendor v
                           where GeneralStatusKey = 1
                           order by 1 desc";

                DataTable dt = base.GetQueryResults(sql);
                if (dt.Rows.Count == 0)
                    Assert.Ignore("No data found");
                int legalEntityKey = Convert.ToInt32(dt.Rows[0][0]);

                IVendor vendor = _repo.GetVendorByLegalEntityKey(legalEntityKey);
                Assert.IsNotNull(vendor);
            }
        }

    }
}
