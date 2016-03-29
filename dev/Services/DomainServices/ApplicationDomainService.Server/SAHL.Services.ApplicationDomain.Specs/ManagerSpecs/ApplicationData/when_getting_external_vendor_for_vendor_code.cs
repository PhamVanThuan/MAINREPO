using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.ApplicationDomain.Managers.Application.Statements;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.ApplicationData
{
    public class when_getting_external_vendor_for_vendor_code : WithFakes
    {
        private static IApplicationDataManager applicationDataManager;
        private static string vendorCode;
        private static VendorModel result, expectedResult;
        private static FakeDbFactory dbFactory;
        

        Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            applicationDataManager = new ApplicationDataManager(dbFactory);
            vendorCode = "Test";
            expectedResult = new VendorModel(1111, vendorCode, 2222, 3333, (int)GeneralStatus.Active);
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.Select<VendorModel>(Param.IsAny<GetExternalVendorForVendorCodeStatement>())).Return(new List<VendorModel>() { expectedResult });
        };

        Because of = () =>
        {
            result = applicationDataManager.GetExternalVendorForVendorCode(vendorCode);
        };

        It should_get = () =>
        {
            result.ShouldBeLike(expectedResult);
        };
    }
}
