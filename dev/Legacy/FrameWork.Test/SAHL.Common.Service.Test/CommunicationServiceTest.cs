using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Service.Interfaces;
using SAHL.Test;
using SAHL.V3.Framework;
using SAHL.V3.Framework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Common.Service.Test
{
    [TestFixture]
    public class CommunicationServiceTest : TestBase
    {
        [Test]
        [Ignore("Use this as an integration test")]
        public void SendComcorpLiveReply()
        {
            try
            {
                IV3ServiceManager serviceManager = V3ServiceManager.Instance;
                ICommunicationService communicationService = serviceManager.Get<ICommunicationService>();
                communicationService.SendComcorpLiveReply(Guid.NewGuid(), "reference", "bondAccountNo", "compcorpReference", "eventComment", DateTime.Now, 1, 1, 1, 1);

            }
            catch (Exception ex)
            {
                var s = ex.ToString();
                throw;
            }

        }
    }
}