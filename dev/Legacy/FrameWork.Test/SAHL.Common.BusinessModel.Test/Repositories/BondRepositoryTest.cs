using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Test;
using System;
using System.Data;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class BondRepositoryTest : TestBase
    {
        [Test]
        public void GetBondByRegistrationNumber()
        {
            using (new SessionScope())
            {
                string query = "select top 1 * from Bond where BondRegistrationNumber != '' and BondRegistrationNumber is not null";

                DataTable DT = base.GetQueryResults(query);

                Assert.That(DT.Rows.Count == 1);

                IBondRepository repo = RepositoryFactory.GetRepository<IBondRepository>();

                IReadOnlyEventList<IBond> bonds = repo.GetBondByRegistrationNumber(DT.Rows[0]["BondRegistrationNumber"].ToString());

                bool foundKey = false;
                foreach (IBond b in bonds)
                {
                    if (b.Key.ToString() == DT.Rows[0]["BondKey"].ToString())
                    {
                        foundKey = true;
                        break;
                    }
                }
                Assert.That(foundKey == true);
            }
        }

        [Test]
        public void GetBondByApplicationKey()
        {
            using (new SessionScope())
            {
                string query = "select top 1 * from Bond where BondRegistrationNumber != '' and BondRegistrationNumber is not null and OfferKey is not null";

                DataTable DT = base.GetQueryResults(query);

                Assert.That(DT.Rows.Count == 1);

                IBondRepository repo = RepositoryFactory.GetRepository<IBondRepository>();

                IBond bond = repo.GetBondByApplicationKey(Convert.ToInt32(DT.Rows[0]["OfferKey"].ToString()));

                Assert.That(bond.Application.Key.ToString() == DT.Rows[0]["OfferKey"].ToString());
            }
        }

        [Test]
        public void GetBondByKey()
        {
            using (new SessionScope())
            {
                string query = "select top 1 * from Bond";

                DataTable DT = base.GetQueryResults(query);

                Assert.That(DT.Rows.Count == 1);

                IBondRepository repo = RepositoryFactory.GetRepository<IBondRepository>();

                IBond bond = repo.GetBondByKey(Convert.ToInt32(DT.Rows[0]["BondKey"].ToString()));

                repo.SaveBond(bond);

                Assert.That(bond.Key.ToString() == DT.Rows[0]["BondKey"].ToString());
            }
        }
    }
}