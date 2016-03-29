using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Test;
using NUnit.Framework;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using Rhino.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel.Rules.Test.LegalEntity
{
    public class LegalEntityBase : RuleBase
    {
        protected ILegalEntityRepository LERepo = null;


        public override void Setup()
        {
            base.Setup();
            LERepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
        }

        public override void TearDown()
        {
            base.TearDown();
        }
    }
}
