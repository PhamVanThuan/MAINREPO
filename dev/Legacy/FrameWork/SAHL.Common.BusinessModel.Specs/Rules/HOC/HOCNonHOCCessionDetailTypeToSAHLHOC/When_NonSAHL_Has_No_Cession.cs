using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Specs.Rules.HOC.HOCNonHOCCessionDetailTypeToSAHLHOC
{
    internal class When_NonSAHL_Has_No_Cession : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.HOC.HOCNonHOCCessionDetailTypeToSAHLHOC>
    {
        protected static IHOC hocAccountInterface;
        static int result;
        Establish context = () =>
        {
            hocAccountInterface = An<IHOC>();
            IHOCInsurer insurer = An<IHOCInsurer>();
            IFinancialService hocFS = An<IFinancialService>();
            IFinancialService parentFS = An<IFinancialService>();
            IAccount parentAccount = An<IAccount>();
            
            IEventList<IDetail> details = new EventList<IDetail>();
            parentAccount.WhenToldTo(x => x.Details).Return(details);

            insurer.WhenToldTo(x => x.Key).Return((int)HOCInsurers.SAHLHOC);
            hocAccountInterface.WhenToldTo(x => x.HOCInsurer).Return(insurer);

            parentFS.WhenToldTo(x => x.Account).Return(parentAccount);
            hocFS.WhenToldTo(x => x.FinancialServiceParent).Return(parentFS);
            hocAccountInterface.WhenToldTo(x => x.FinancialService).Return(hocFS);

            businessRule = new SAHL.Common.BusinessModel.Rules.HOC.HOCNonHOCCessionDetailTypeToSAHLHOC();
            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.HOC.HOCNonHOCCessionDetailTypeToSAHLHOC>.startrule.Invoke();
        };

        Because of = () =>
        {
            result = businessRule.ExecuteRule(messages, hocAccountInterface);
        };

        It message_collection_shouldbe_be_zero = () =>
        {
            messages.Count.ShouldEqual(0);
        };

        It result_should_be_0 = () =>
        {
            result.ShouldEqual(1);
        };
    }
}
