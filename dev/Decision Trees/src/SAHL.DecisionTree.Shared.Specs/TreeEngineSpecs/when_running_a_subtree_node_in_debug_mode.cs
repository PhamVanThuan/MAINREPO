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

namespace SAHL.DecisionTree.Shared.Specs
{
    public class when_running_a_subtree_node_in_debug_mode : WithFakes
    {
        private static Node node;        
        private static TreeProcessingContext debugger;
        private static TreeProcessingContext subTreeDebugger;

        private Establish context = () =>
        {
            Dictionary<string, string> classes = new Dictionary<string, string>();
            classes.Add("MessageSet", Shared.TestMessagesClass);
            classes.Add("EnumerationSet", Shared.TestEnumerationsClass);
            classes.Add("VariableSet", Shared.TestVariablesClass);
            IObjectGenerator objectGenerator = An<IObjectGenerator>();
            objectGenerator.WhenToldTo(x => x.GenerateLatestDebugClasses(Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>())).Return(classes);
            ISubTreeContextManager subTreeContextManager = An<ISubTreeContextManager>();
            subTreeDebugger = new TreeProcessingContext(Shared.TestSubTreeJSON, Shared.GlobalsVersion, new SAHL.Core.SystemMessages.SystemMessageCollection(), objectGenerator, "", subTreeContextManager);
            subTreeContextManager.WhenToldTo(x => x.GetSubTreeDebugContext(Param.IsAny<string>(), Param.IsAny<int>())).Return(subTreeDebugger);
            debugger = new TreeProcessingContext(Shared.TestMainTreeCallingSubTreeJSON, Shared.GlobalsVersion, new SAHL.Core.SystemMessages.SystemMessageCollection(), objectGenerator, "", subTreeContextManager);
            debugger.Variables.inputs.newvarstr1 = "hello";
        };

        private Because of = () =>
        {
            node = debugger.TreeStep(-4);
        };

        private It should_move_to_the_next_node_on_successful_processing_of_the_subtree = () =>
        {
            node.id.ShouldEqual(-1);     
        };


    }
}
