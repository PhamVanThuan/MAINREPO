using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing.Ioc;
using SAHL.Core.Testing.Providers;
using SAHL.Core.Testing;
using System;
using System.Collections.Generic;
using SAHL.Core.Data;
using System.Linq;
using SAHL.Core.Testing.Ioc.Registration;
using SAHL.Core.Testing.FileConventions;
using StructureMap;

namespace SAHL.Core.Testing.Specs
{
    public class when_registering_Ioc_testing_container : WithFakes
    {
        private static ITestingIocContainer testingIocContainer;
        private static ITestingIoc testingIoc;

        Establish context = () =>
        {
            testingIoc = TestingIoc.Initialise();
            testingIoc.Configure<SAHLSpecsAssemblyConvention, ServiceQuerySqlStatementConvention>();
        };

        Because of = () =>
        {
            testingIocContainer = testingIoc.GetInstance<ITestingIocContainer<SAHLSpecsAssemblyConvention, ServiceQuerySqlStatementConvention>>();
        };
     
        It should_only_have_types_that_belongs_to_assembly = () =>
        {
            testingIocContainer.GetRegisteredTypes().Count().ShouldBeGreaterThan(0);
        };
    }
}
