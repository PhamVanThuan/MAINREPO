using System.Linq;
using System;
using NUnit.Framework;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Identity;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.ITC;
using SAHL.Services.Interfaces.ITC.Commands;
using SAHL.Services.Interfaces.ITC.Models;

namespace SAHL.Testing.Services.Tests.ITC
{
    [TestFixture]
    public class when_performing_a_capitec_ITC_request : ServiceTestBase<IItcServiceClient>
    {
        [Test]
        public void when_succesful()
        {
            ApplicantITCRequestDetailsModel applicantITCRequestDetails = new ApplicantITCRequestDetailsModel(
                    "Clinton", "Speed", DateTime.Now.AddYears(-32), "8211045229080", "Mr", "0315713036", "0315713036", "0714033383", "clintonspeed@gmail.com",
                    "9 Camberwell", "21 Somerset Drive", "Somerset Park", "La Lucia Ridge", "4319"
                );
            CombGuid cguid = new CombGuid();
            var guid = cguid.Generate();
            var command = new PerformCapitecITCCheckCommand(guid, applicantITCRequestDetails);
            base.Execute(command).WithoutErrors();
            var itcResponse = TestApiClient.Get<ITCRequestDataModel>(new { id = guid.ToString() }).First();
            Assert.That(itcResponse.ITCData.Length > 0, string.Format("No ITC record could be found for Id = '{0}'", guid));
        }

        [Test]
        public void when_unsuccessful()
        {
            ApplicantITCRequestDetailsModel applicantITCRequestDetails = null;
            CombGuid cguid = new CombGuid();
            var guid = cguid.Generate();
            var command = new PerformCapitecITCCheckCommand(guid, applicantITCRequestDetails);
            base.Execute(command).AndExpectThatErrorMessagesContain("There was a validation error: [The ApplicantITCRequestDetails field is required. {in: root}], processing has been halted");
        }
    }
}