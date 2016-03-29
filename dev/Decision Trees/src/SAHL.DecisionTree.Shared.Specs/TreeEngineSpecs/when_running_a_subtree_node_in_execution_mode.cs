using Machine.Fakes;
using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Tools.ObjectFromJsonGenerator.Lib;
using SAHL.DecisionTree.Shared.Interfaces;
using SAHL.DecisionTree.Shared.Helpers;
using SAHL.DecisionTree.Shared.Core;
using SAHL.Core.SystemMessages;
using Newtonsoft.Json;
using System.Reflection;

namespace SAHL.DecisionTree.Shared.Specs
{
    public class when_running_a_subtree_node_in_execution_mode : WithFakes
    {
        private static Node node;        
        private static TreeProcessingContext debugger;
        private static int nextNodeId;

        private Establish context = () =>
        {
            Dictionary<string, object> inputValues = new Dictionary<string, object>();
            inputValues.Add("ApplicationType","Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch");
            inputValues.Add("PropertyOccupancyType","Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied");
            inputValues.Add("HouseholdIncomeType","Enumerations::sAHomeLoans::householdIncomeType.Salaried");
            inputValues.Add("HouseholdIncome",30000);
            inputValues.Add("PropertyPurchasePrice",0.0);
            inputValues.Add("DepositAmount",0.0);
            inputValues.Add("CashAmountRequired",100000);
            inputValues.Add("CurrentMortgageLoanBalance",150000);
            inputValues.Add("EstimatedMarketValueofProperty",650000);
            inputValues.Add("EldestApplicantAge",35);
            inputValues.Add("YoungestApplicantAge",34);
            inputValues.Add("TermInMonth",240);
            inputValues.Add("FirstIncomeContributorApplicantEmpirica",550);
            inputValues.Add("FirstIncomeContributorApplicantIncome",22500);
            inputValues.Add("SecondIncomeContributorApplicantEmpirica",610);
            inputValues.Add("SecondIncomeContributorApplicantIncome",7500);
            inputValues.Add("EligibleBorrower",true);
            inputValues.Add("Fees",0);
            inputValues.Add("InterimInterest",0);
            inputValues.Add("CapitaliseFees",true);

            debugger = new TreeProcessingContext(inputValues, "CapitecOriginationCreditPricing", "1", JsonConvert.DeserializeObject<QueryGlobalsVersion>("{  'VariablesVersion': 2,  'MessagesVersion': 2,  'EnumerationsVersion': 2,  '_name': 'SAHL.DecisionTree.Shared.QueryGlobalsVersion,SAHL.DecisionTree.Shared'}"), new SystemMessageCollection(), "");
        };

        private Because of = () =>
        {
            node = debugger.TreeStep(-4);
            Type objType = debugger.GetType();
            FieldInfo propInfo = objType.GetField("nextNodeId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            nextNodeId = (int)propInfo.GetValue(debugger);   
        };

        private It should_move_to_the_next_node_on_successful_processing_of_the_subtree_node = () =>
        {
            node.id.ShouldEqual(nextNodeId);     
        };


    }
}
