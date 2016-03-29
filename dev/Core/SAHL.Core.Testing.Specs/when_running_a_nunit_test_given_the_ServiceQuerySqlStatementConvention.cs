﻿using Machine.Fakes;
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

namespace SAHL.Core.Testing.Specs
{
    public class when_running_a_nunit_test_given_the_ServiceQuerySqlStatementConvention : WithFakes
    {
        private static TestParamsProvider<SAHLSpecsAssemblyConvention, ServiceQuerySqlStatementConvention> testParamProvider;
        private static IEnumerable<ITestParams> testParams;

        Establish context = () =>
        {
            var container = TestingIoc.Initialise();
            container.Configure<SAHLSpecsAssemblyConvention, ServiceQuerySqlStatementConvention>();
            testParamProvider = container.GetInstance<TestParamsProvider<SAHLSpecsAssemblyConvention, ServiceQuerySqlStatementConvention>>();
        };

        Because of = () =>
        {
            testParams = testParamProvider.GetTestParams();
        };

        It should_return_sql_statement_types = () =>
        {
            testParams.Count().ShouldBeGreaterThan(0);

            foreach (var testParam in testParams)
            {
                testParam.UsedConvention.ShouldNotBeNull();
                var conventionInstance = Activator.CreateInstance(testParam.UsedConvention);
                conventionInstance.ShouldBeOfExactType<ServiceQuerySqlStatementConvention>();
            }
        };

        It should_be_able_to_create_an_instance_of_the_sql_statement = () =>
        {
            testParams.Count().ShouldBeGreaterThan(0);
            foreach (var testParam in testParams)
            {
                var instance = testParam.TestingIocContainer.GetInstance(testParam.TypeUnderTest);
                instance.ShouldNotBeNull();
                testParam.TypeUnderTest.ShouldNotBeNull();
                instance.ShouldBeOfExactType(testParam.TypeUnderTest);
            }
        };
    }
}
