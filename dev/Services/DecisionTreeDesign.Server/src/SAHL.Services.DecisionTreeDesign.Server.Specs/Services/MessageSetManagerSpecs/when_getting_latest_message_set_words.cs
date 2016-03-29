using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.DecisionTree;
using SAHL.Services.DecisionTreeDesign.Managers.MessageSet;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.DecisionTreeDesign.Server.Specs.Services.MessageSetManagerSpecs
{
    [Subject("SAHL.Services.DecisionTree.Services.MessageSetManager.GetLatestMessageSetWords")]
    public class when_getting_latest_message_set_words : WithFakes
    {
        private static IMessageSetDataManager dataService;
        private static IMessageSetManager dataManager;
        private static IEnumerable<string> result;
        private static IUnitOfWorkFactory unitOfWorkFactory;
        private Establish context = () =>
        {
            dataService = An<IMessageSetDataManager>();
            unitOfWorkFactory = An<IUnitOfWorkFactory>();
            MessageSetDataModel model = new MessageSetDataModel(1, "{\"groups\":[{\"id\":1,\"name\":\"Capitec\",\"groups\":[{\"id\":1,\"name\":\"Credit\",\"groups\":[],\"messages\":[{\"name\":\"Applicant Minimum Empirica\",\"value\":\"The Empirica score is below SAHL policy minimum\",\"definition\":\"The minimum empirica score required is 575.\"},{\"name\":\"Applicant Maximum Judgements in Last 3 Years\",\"value\":\"There is record of multiple recent unpaid judgements in the last 3 years\",\"definition\":\"The maximum judgments within last three years allowed is four.\"},{\"name\":\"Maximum Aggregate Judgement Value with 3 Judgements in Last 3 Years\",\"value\":\"There is record of unpaid judgements with a material aggregated rand value\",\"definition\":\"The maximum aggregate judgement rand value with three judgments in last three years is R 10,000.00.\"},{\"name\":\"Maximum Aggregated Judgement Value Unsettled For Between 13 And 36 Months\",\"value\":\"There is record of an outstanding aggregated unpaid judgement of material value\",\"definition\":\"The maximum aggregated judgement rand value unsettled for between 13 and 36 months is R 15,000.00.\"},{\"name\":\"Maximum Number Of Unsettled Defaults Within Past 2 Years\",\"value\":\"There is record of numerous unsettled defaults within the past 2 years\",\"definition\":\"The maximum number of unsettled defaults within the past two years is four.\"},{\"name\":\"Notice Of Sequestration\",\"value\":\"There is a record of Sequestration\",\"definition\":\"Notice of sequestration.\"},{\"name\":\"Notice Of Administration Order\",\"value\":\"There is a record of an Administration Order\",\"definition\":\"Notice of administration order.\"},{\"name\":\"Notice Of Debt Counselling\",\"value\":\"There is a record of Debt Counselling\",\"definition\":\"Notice of debt counselling.\"},{\"name\":\"Notice Of Debt Review\",\"value\":\"There is a record of Debt Review\",\"definition\":\"Notice of debt review.\"},{\"name\":\"Notice Of Consumer Is Deceased\",\"value\":\"There is record that the consumer is deceased\",\"definition\":\"Notice of consumer is deceased.\"},{\"name\":\"Notice Of Credit Card Revoked\",\"value\":\"There is record of a revoked credit card\",\"definition\":\"Notice of credit card revoked.\"},{\"name\":\"Notice Of Absconded\",\"value\":\"There is record that the applicant has absconded\",\"definition\":\"Notice of absconded.\"},{\"name\":\"Notice Of Paid Out On Deceased Claim\",\"value\":\"There is record that a deceased claim has been paid out\",\"definition\":\"Notice of paid out on deceased claim.\"},{\"name\":\"No Credit Bureau Match Found\",\"value\":\"No credit bureau match found.\",\"definition\":\"No Credit Bureau Match Found\"},{\"name\":\"Loan to Value Above Credit Maximum\",\"value\":\"Insufficient property value for loan amount requested.\",\"definition\":\"The Loan to Value ratio is beyond credit limits. Insufficient property value for loan amount requested.\"}]}],\"messages\":[{\"name\":\"Insufficient information\",\"value\":\"The correct information is not available to continue.\",\"definition\":\"The correct information is not available to continue.\"}]}]}");
            dataService.WhenToldTo(x => x.GetLatestMessageSet()).Return(model);
            dataManager = new MessageSetManager(dataService, unitOfWorkFactory);
        };

        private Because of = () =>
        {
            result = dataManager.GetLatestMessageSetWords();
        };

        //terrible test i know, will make them more meaningful
        private It should_return_results = () =>
        {
            result.Count().ShouldNotEqual(0);
        };
    }
}