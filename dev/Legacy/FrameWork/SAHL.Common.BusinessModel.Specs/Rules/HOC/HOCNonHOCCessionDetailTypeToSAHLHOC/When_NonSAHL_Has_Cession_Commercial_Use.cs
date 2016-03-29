using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;


namespace SAHL.Common.BusinessModel.Specs.Rules.HOC.HOCNonHOCCessionDetailTypeToSAHLHOC
{
    internal class When_NonSAHL_Has_Cession_Commercial_Use : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.HOC.HOCNonHOCCessionDetailTypeToSAHLHOC>
    {
        internal class When_NonSAHL_Has_Cession_Policy : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.HOC.HOCNonHOCCessionDetailTypeToSAHLHOC>
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
                IDetail detail = An<IDetail>();
                IDetailType detailType = An<IDetailType>();

                detailType.WhenToldTo(x => x.Key).Return((int)DetailTypes.HOCCessionCommercialUse);
                detail.WhenToldTo(x => x.DetailType).Return(detailType);
                IEventList<IDetail> details = new EventList<IDetail>(new IDetail[] { detail });
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

            It message_collection_shouldbe_be_one = () =>
            {
                messages.Count.ShouldEqual(1);
            };

            It result_should_be_0 = () =>
            {
                result.ShouldEqual(0);
            };
        }
    }
}
