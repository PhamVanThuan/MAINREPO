using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Web.AJAX;
using NUnit.Framework;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.DAO;

namespace SAHL.Web.Test.AJAX
{
    [TestFixture]
    public class ReasonService : TestViewBase
    {

        private Reason _reasonService;


        [SetUp]
        public void Setup()
        {
            _reasonService = new Reason();
        }

        [TearDown]
        public void TearDown()
        {
            _reasonService = null;
        }

        [NUnit.Framework.Test]
        public void GetReasonDefinitionDescriptions()
        {
            using (new SessionScope())
            {
                ReasonType_DAO reasonDef = ReasonType_DAO.FindFirst();

                List<CustomReasonDefinition> lstDef = _reasonService.GetReasonDefinitionDescriptions(reasonDef.Key, false);
                foreach (CustomReasonDefinition def in lstDef)
                {
                    ReasonDefinition_DAO rd = ReasonDefinition_DAO.Find(def.Key);
                    if (rd.ReasonType.Key != reasonDef.Key)
                    {
                        Assert.Fail("Description lookup for reason type of {0} failed!", reasonDef.Description);
                    }
                }
            }
        }

    }
}
