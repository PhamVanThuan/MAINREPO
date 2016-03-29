using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.ClientDomain;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Testing.Services.Tests.ClientDomain
{
    [TestFixture]
    public class when_adding_an_employer : ServiceTestBase<IClientDomainServiceClient>
    {
        [Test]
        public void when_successful()
        {
            EmployerModel employer = new EmployerModel(null, string.Format("TestEmployer{0}", randomizer.Next(0, 100000)), "031", "5713036", "Bob",
                string.Empty, Core.BusinessModel.Enums.EmployerBusinessType.Company, Core.BusinessModel.Enums.EmploymentSector.Construction);
            AddEmployerCommand command = new AddEmployerCommand(this.linkedGuid, employer);
            base.Execute<AddEmployerCommand>(command);
            linkedKey = linkedKeyManager.RetrieveLinkedKey(this.linkedGuid);
            var newEmployer = TestApiClient.GetByKey<EmployerDataModel>(linkedKey);
            Assert.That(newEmployer != null, "No employer was added");
            Assert.That(newEmployer.Name == employer.EmployerName, "Incorrect employer details were added.");
        }

        [Test]
        public void when_unsuccessful()
        {
            var getEmployerQuery = new GetEmployerQuery();
            base.PerformQuery(getEmployerQuery);
            var existingEmployer = getEmployerQuery.Result.Results.First();
            EmployerModel employer = new EmployerModel(null, existingEmployer.Name, string.Empty, string.Empty, string.Empty, string.Empty,
                EmployerBusinessType.Company, EmploymentSector.Construction);
            AddEmployerCommand command = new AddEmployerCommand(this.linkedGuid, employer);
            string expectedMessage = String.Format("{0} is an existing employer and could not be added.", employer.EmployerName);
            base.Execute<AddEmployerCommand>(command)
                .AndExpectThatErrorMessagesContain(expectedMessage);
        }
    }
}