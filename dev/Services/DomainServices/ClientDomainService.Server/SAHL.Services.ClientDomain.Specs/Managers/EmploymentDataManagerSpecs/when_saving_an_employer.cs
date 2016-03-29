using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.ClientDomain.Specs.Managers;

using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Core.Testing.Providers;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Core.Testing.Fakes;

namespace SAHL.Services.ClientDomain.Specs.Managers.EmploymentDataManagerSpecs
{
    public class when_saving_an_employer : WithFakes
    {
        private static IEmploymentDataManager employmentDataManager;
        private static int employerKey;
        private static FakeDbFactory dbFactory;
        private static EmployerModel employerModel;
        private static int result;

        Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            employmentDataManager = new EmploymentDataManager(dbFactory);
            employerKey = 111;
            EmploymentSector employmentSector = EmploymentSector.FinancialServices;
            employerModel = new SAHL.Services.Interfaces.ClientDomain.Models.EmployerModel(employerKey, "SAHL", "+2731", "5685300",
                                                                                           "Ingrid Sharp", "ingrid.sharp@sahomeloansc.om", 
                                                                                           Core.BusinessModel.Enums.EmployerBusinessType.Company, 
                                                                                           employmentSector);
            dbFactory.FakedDb.DbContext.WhenToldTo
              <IReadWriteSqlRepository>(x => x.Insert<EmployerDataModel>
                (Param.IsAny<EmployerDataModel>()))
                 .Callback<EmployerDataModel>(y => { y.EmployerKey = employerKey; });
        };

        Because of = () =>
        {
            result = employmentDataManager.SaveEmployer(employerModel);
        };

        It should_insert_employer_into_system = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo
             (x => x.Insert<EmployerDataModel>(Arg.Is<EmployerDataModel>
              (y => y.EmployerKey == employerModel.EmployerKey)));
        };

        It should_return_an_employer_key = () =>
        {
            result.ShouldEqual(employerKey);
        };

    }
}
