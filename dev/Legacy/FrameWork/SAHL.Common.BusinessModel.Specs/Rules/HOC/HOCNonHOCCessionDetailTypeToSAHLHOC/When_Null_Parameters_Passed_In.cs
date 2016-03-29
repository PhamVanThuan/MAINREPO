using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel.Specs.Rules.HOC.HOCNonHOCCessionDetailTypeToSAHLHOC
{
    class When_Null_Parameters_Passed_In : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.HOC.HOCNonHOCCessionDetailTypeToSAHLHOC>
    {
        protected static Exception exception;
        protected static object[] paramsToPassIn;
        Establish context = () =>
        {
            paramsToPassIn = new object[0];
            businessRule = new SAHL.Common.BusinessModel.Rules.HOC.HOCNonHOCCessionDetailTypeToSAHLHOC();
            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.HOC.HOCNonHOCCessionDetailTypeToSAHLHOC>.startrule.Invoke();
        };

        Because of = () =>
        {
            exception = Catch.Exception(() => { businessRule.ExecuteRule(messages, paramsToPassIn); });
        };

        It should_throw_exception = () =>
        {
            exception.ShouldNotBeNull();
        };

        It should_be_argument_exception_type = () =>
        {
            exception.ShouldBeOfType<ArgumentException>();
        };
        
    }
}
