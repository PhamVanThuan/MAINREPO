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
    public class when_running_a_tree_node_in_execution_mode : WithFakes
    {
        private static Node node;        
        private static TreeProcessingContext debugger;
        private static int nextNodeId;

        private Establish context = () =>
        {
            Dictionary<string, object> inputValues = new Dictionary<string, object>();
            inputValues.Add("HouseholdIncome",0.0);
            inputValues.Add("Deposit",0.0);
            inputValues.Add("CalcRate",0.0);
            inputValues.Add("InterestRateQuery", true);

            debugger = new TreeProcessingContext(inputValues, "CapitecAffordabilityInterestRate", "1", JsonConvert.DeserializeObject<QueryGlobalsVersion>("{  'VariablesVersion': 2,  'MessagesVersion': 2,  'EnumerationsVersion': 2,  '_name': 'SAHL.DecisionTree.Shared.QueryGlobalsVersion,SAHL.DecisionTree.Shared'}"), new SystemMessageCollection(), "");
        };

        private Because of = () =>
        {
            node = debugger.TreeStep(1);
            Type objType = debugger.GetType();
            FieldInfo propInfo = objType.GetField("nextNodeId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            nextNodeId = (int)propInfo.GetValue(debugger);                        
        };

        private It should_move_to_the_next_node_on_successful_processing_of_the_start_node = () =>
        {
            node.id.ShouldEqual(nextNodeId);     
        };


    }
}
