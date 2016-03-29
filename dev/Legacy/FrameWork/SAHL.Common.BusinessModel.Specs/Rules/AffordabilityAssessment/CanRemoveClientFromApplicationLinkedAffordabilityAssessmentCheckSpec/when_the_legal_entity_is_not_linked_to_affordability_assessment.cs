using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Rules.AffordabilityAssessment;
using System.Data;

namespace SAHL.Common.BusinessModel.Specs.Rules.AffordabilityAssessment.CanRemoveClientFromApplicationLinkedAffordabilityAssessmentCheckSpec
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.AffordabilityAssessment.CanRemoveClientFromApplicationLinkedAffordabilityAssessmentCheck))]
    public class when_the_legal_entity_is_not_linked_to_affordability_assessment : RulesBaseWithFakes<CanRemoveClientFromApplicationLinkedAffordabilityAssessmentCheck>
    {
        protected static ICastleTransactionsService castleTransactionService;
        protected static IUIStatementService uistatementService;

        private Establish context = () =>
        {
            castleTransactionService = An<ICastleTransactionsService>();
            uistatementService = An<IUIStatementService>();

            parameters = new object[2] { 0, 0 };
            DataTable dt = new DataTable();
            dt.Columns.Add("col1");
            dt.Rows.Add(new object[] { "1" });
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(dt);

            uistatementService.WhenToldTo(x => x.GetStatement(Param.IsAny<string>(), Param.IsAny<string>())).Return("some sql");
            castleTransactionService.WhenToldTo(x => x.ExecuteQueryOnCastleTran(Param.IsAny<string>(), Param.IsAny<SAHL.Common.Globals.Databases>(), Param.IsAny<DataAccess.ParameterCollection>())).Return(dataSet);

            businessRule = new CanRemoveClientFromApplicationLinkedAffordabilityAssessmentCheck(castleTransactionService, uistatementService);
            RulesBaseWithFakes<CanRemoveClientFromApplicationLinkedAffordabilityAssessmentCheck>.startrule.Invoke();
        };

        private Because of = () =>
        {
            RuleResult = businessRule.ExecuteRule(messages, parameters);
        };

        private It should_pass_the_rule = () =>
        {
            messages.Count.ShouldEqual(0);
            RuleResult.ShouldEqual(1);
        };
    }
}