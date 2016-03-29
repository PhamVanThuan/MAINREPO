using NUnit.Framework;
using SAHL.Common.Service.Interfaces;
using SAHL.Test;
using SAHL.V3.Framework;
using SAHL.V3.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Common.Service.Test
{
    [TestFixture]
    public class MortgageLoanServiceTest : TestBase
    {
        //[Test]
        public void GetDebitOrderDayByAccountTest()
        {
            try
            {
                IV3ServiceManager serviceManager = V3ServiceManager.Instance;
                IMortgageLoanDomainService mortgageLoanDomainService = serviceManager.Get<IMortgageLoanDomainService>();
                var accountKey = 2309733;
                var result = mortgageLoanDomainService.GetDebitOrderDayByAccount(accountKey);

            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
