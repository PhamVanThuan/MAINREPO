using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System.Collections.Generic;

namespace SAHL.Services.ClientDomain.Specs.Managers.EmploymentDataManagerSpecs
{
    public class when_finding_an_employer_that_is_not_in_our_system : WithFakes
    {
        private static IEmploymentDataManager employmentDataManager;
        private static IEnumerable<EmployerDataModel> existingEmployerDataModel;
        private static IEnumerable<EmployerDataModel> results;
        private static FakeDbFactory dbFactory;
        private static EmployerModel employerModel;

        Establish context = () =>
        {
            int employerKey = 1;
            dbFactory = new FakeDbFactory();
            employmentDataManager = new EmploymentDataManager(dbFactory);
            employerModel = new SAHL.Services.Interfaces.ClientDomain.Models.EmployerModel
              (employerKey, "SAHL", "+2731", "5685300", "Ingrid Sharp", "ingrid.sharp@sahomeloansc.om", 
              EmployerBusinessType.Company, EmploymentSector.FinancialServices);
            existingEmployerDataModel = null;

            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo
             (x => x.Select<EmployerDataModel>(Param.IsAny<ISqlStatement<EmployerDataModel>>()))
              .Return(existingEmployerDataModel);

        };

        Because of = () =>
        {
            results = employmentDataManager.FindExistingEmployer(employerModel);
        };

        It should_should_query_for_existing_employer = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo
             (x => x.Select<EmployerDataModel>
              (Param.IsAny<ISqlStatement<EmployerDataModel>>()));
        };

        It should_not_return_employer_details = () =>
        {
            results.ShouldBeNull();
        };

    }
}