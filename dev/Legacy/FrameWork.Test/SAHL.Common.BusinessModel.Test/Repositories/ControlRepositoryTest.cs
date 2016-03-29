using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Test;
using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class ControlRepositoryTest : TestBase
    {
        [NUnit.Framework.Test]
        public void GetControlByDescription_Pass()
        {
            using (new SessionScope())
            {
                // find the first Control table entry
                Control_DAO controlDAO = Control_DAO.FindFirst();
                string controlDescription = controlDAO.ControlDescription;

                //use the repo method to return the control entry
                IControlRepository CR = RepositoryFactory.GetRepository<IControlRepository>();
                IControl control = CR.GetControlByDescription(controlDescription);

                Assert.IsTrue(control.ControlDescription == controlDescription);
            }
        }

        [NUnit.Framework.Test]
        public void GetControlByDescription_Fail()
        {
            using (new SessionScope())
            {
                // make up a ficticious control description
                string controlDescription = "this will never return an valid IControl";

                //use the repo method to return the control entry
                IControlRepository CR = RepositoryFactory.GetRepository<IControlRepository>();
                IControl control = CR.GetControlByDescription(controlDescription);

                Assert.IsTrue(control==null);
            }
        }
    }
}
