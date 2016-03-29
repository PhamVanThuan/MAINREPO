using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.Managers.EmploymentDataManagerSpecs
{
    public class when_finding_an_employer_that_is_in_our_system : WithFakes
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
            employerModel = new EmployerModel(employerKey, "SAHL", "+2731", "5685300", "Ingrid Sharp", 
                                              "ingrid.sharp@sahomeloansc.om", EmployerBusinessType.Company, 
                                              EmploymentSector.FinancialServices);
            existingEmployerDataModel = new List<EmployerDataModel>
            {
                new EmployerDataModel("SAHL", "56858300", "+2731", "Ingrid Sharp", "ingrid.sharp@sahomeloans.com", 
                                      "John Doe", "Red John", "+2730", "5685322", "johndoe@accountantsa.com", 
                                      (int)EmployerBusinessType.Company, "5050", DateTime.Now, (int)EmploymentSector.FinancialServices)
            };

            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.Select<EmployerDataModel>(Param.IsAny<ISqlStatement<EmployerDataModel>>()))
                .Return(existingEmployerDataModel);
        };

        Because of = () =>
        {
            results = employmentDataManager.FindExistingEmployer(employerModel);
        };

        It should_query_for_an_existing_employer = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo
                (x => x.Select<EmployerDataModel>
                    (Param.IsAny<ISqlStatement<EmployerDataModel>>()));
        };

        It should_return_the_employer_details = () =>
        {
            results.First().AccountantName.ShouldEqual(existingEmployerDataModel.First().AccountantName);
        };
    }
}
