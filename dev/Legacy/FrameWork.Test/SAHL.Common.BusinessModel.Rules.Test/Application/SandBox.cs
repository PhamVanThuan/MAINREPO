using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using NUnit.Framework;

namespace SAHL.Common.BusinessModel.Rules.Test.Application
{
    [TestFixture]
    public class SandBox : RuleBase
    {
        [NUnit.Framework.SetUp()]
        public void Setup()
        {
            base.Setup();
        }

        [TearDown]
        public void TearDown()
        {
            base.TearDown();
        }


        //[NUnit.Framework.Test] // What is this Test Testing???
        //public void ApplicationError()
        //{
        //    IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
        //    IApplication application = appRepo.GetApplicationByKey(528392);

        //    IApplicationProduct appProd = application.CurrentProduct;
        //}
    }
}
