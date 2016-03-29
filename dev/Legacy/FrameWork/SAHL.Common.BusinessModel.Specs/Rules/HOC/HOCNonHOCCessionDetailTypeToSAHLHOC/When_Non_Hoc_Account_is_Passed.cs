using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel.Specs.Rules.HOC.HOCNonHOCCessionDetailTypeToSAHLHOC
{
    class When_Non_Hoc_Account_is_Passed : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.HOC.HOCNonHOCCessionDetailTypeToSAHLHOC>
    {
        protected static Exception exception;
        static IAccountLifePolicy iLifeAccount;
   
        Establish context = () =>
        {
            iLifeAccount = An<IAccountLifePolicy>();
            businessRule = new SAHL.Common.BusinessModel.Rules.HOC.HOCNonHOCCessionDetailTypeToSAHLHOC();
            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.HOC.HOCNonHOCCessionDetailTypeToSAHLHOC>.startrule.Invoke();
        };

        Because of = () =>
        {
            exception = Catch.Exception(() => { businessRule.ExecuteRule(messages, iLifeAccount, null); });
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
