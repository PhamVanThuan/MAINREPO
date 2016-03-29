using Machine.Fakes;
using Machine.Specifications;
using SAHL.Tools.ObjectFromJsonGenerator.Lib;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.DecisionTree.Shared.Interfaces;
using SAHL.DecisionTree.Shared.Helpers;
using SAHL.DecisionTree.Shared.Core;

namespace SAHL.DecisionTree.Shared.Specs
{
    public class when_running_a_decision_node_with_a_false_condition : WithFakes
    {
        private static NodeType nodeType;
        private static int id;
        private static string nodeName;
        private static string rubyCode;
        private static Node node;
        private static bool result;
 
        private Establish context = () =>
        {
            id = 0;
            nodeType = NodeType.Decision;
            nodeName = "FalseNode";
            rubyCode = @"   x = false
                            if x then
	                            Variables::outputs.NodeResult = true
                            else
	                            Variables::outputs.NodeResult = false
                            end";
            result = false;

            node = new Node(id,nodeName, nodeType,rubyCode);
            Dictionary<string, string> classes = new Dictionary<string, string>();
            classes.Add("MessageSet", Shared.TestMessagesClass);
            classes.Add("EnumerationSet", Shared.TestEnumerationsClass);
            classes.Add("VariableSet", Shared.TestVariablesClass);
            IObjectGenerator objectGenerator = An<IObjectGenerator>();
            objectGenerator.WhenToldTo(x => x.GenerateLatestDebugClasses(Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>())).Return(classes);
            ISubTreeContextManager subTreeContextManager = An<ISubTreeContextManager>(); 
            TreeProcessingContext debugger = new TreeProcessingContext("", Shared.GlobalsVersion, new SAHL.Core.SystemMessages.SystemMessageCollection(), objectGenerator, "", subTreeContextManager);
            node.engine = debugger.engine;
        };

        private Because of = () =>
        {
            result = node.Process();
        };

        private It should_return_false_after_processing_the_decision = () =>
        {
            result.ShouldEqual(false);           
        };


    }
}
