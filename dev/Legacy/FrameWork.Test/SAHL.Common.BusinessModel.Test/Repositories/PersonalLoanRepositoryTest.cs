using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Globals;
using SAHL.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class PersonalLoanRepositoryTest : TestBase
    {
        [Test]
        public void GetBatchServiceResultsTest()
        {
            IPersonalLoanRepository plRepo = new PersonalLoanRepository();
            plRepo.GetBatchServiceResults(BatchServiceTypes.PersonalLoanLeadImport);
        }
    }
}
