using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using SAHL.Test;
using SAHL.Common.BusinessModel.DAO;

using Castle.ActiveRecord;
using NUnit.Framework;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    /// <summary>
    /// Class for testing the <see cref="LegalEntityMarketingOption_DAO"/> entity.
    /// </summary>
    [TestFixture]
    public class LegalEntityMarketingOption_DAOTest : TestBase
    {
        [Test]
        public void LoadSaveLoad()
        {
            using (new SessionScope())
            {
                LegalEntityNaturalPerson_DAO lenp = DAODataConsistencyChecker.GetDAO<LegalEntityNaturalPerson_DAO>();
                lenp.SaveAndFlush();


                LegalEntityMarketingOption_DAO errRep = DAODataConsistencyChecker.GetDAO<LegalEntityMarketingOption_DAO>();
                errRep.LegalEntity = lenp;
                errRep.CreateAndFlush();

                // now try and load it
                LegalEntityMarketingOption_DAO errRep2 = LegalEntityMarketingOption_DAO.Find(errRep.Key);
                Assert.IsNotNull(errRep2);

                // delete all the data
                errRep2.DeleteAndFlush();
                lenp.DeleteAndFlush();
            }
        }
    }
}
