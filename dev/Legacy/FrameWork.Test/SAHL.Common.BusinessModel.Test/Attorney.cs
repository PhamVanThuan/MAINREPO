using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Test;
using System.Collections.Generic;
using System.Data;

namespace SAHL.Common.BusinessModel.Test
{
    [TestFixture]
    public class Attorney : TestBase
    {
        [Test]
        public void GetContacts()
        {
            string sql = @"
                select top 10 GenericKey, er.ExternalRoleTypeKey, GeneralStatusKey, count(1) as items
                --, *
                from ExternalRole er
                join ExternalRoleType ert on er.ExternalRoleTypeKey = ert.ExternalRoleTypeKey
                where ert.ExternalRoleTypeGroupKey = 3
                and GenericKeyTypeKey = 35
                group by GenericKey, er.ExternalRoleTypeKey, GeneralStatusKey
                ";

            DataTable dt = GetQueryResults(sql);
            if (dt.Rows.Count == 0)
                Assert.Ignore("No data");
            else
            {
                ILegalEntityRepository lerepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
                IAttorney att = null;
                foreach (DataRow r in dt.Rows)
                {
                    int attKey = (int)r["GenericKey"];
                    if (att == null || att.Key != attKey)
                    {
                        att = lerepo.GetAttorneyByKey(attKey);
                    }

                    IList<ILegalEntity> list = att.GetContacts((Globals.ExternalRoleTypes)r["ExternalRoleTypeKey"], (Globals.GeneralStatuses)r["GeneralStatusKey"]);

                    Assert.AreEqual(list.Count, (int)r["items"]);
                }
            }
        }
    }
}