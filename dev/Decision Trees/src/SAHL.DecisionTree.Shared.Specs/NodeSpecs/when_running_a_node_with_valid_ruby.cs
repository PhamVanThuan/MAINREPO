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
    public class when_running_a_node_with_valid_ruby : WithFakes
    {
        private static NodeType nodeType;
        private static int id;
        private static string rubyCode;
        private static Node node;
        private static bool result;
 
        private Establish context = () =>
        {
            id = 0;
            nodeType = NodeType.Process;
            rubyCode = "x = 2 / 1";
            result = false;

            node = new Node(id,"validNode", nodeType,rubyCode);
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

        private It should_return_true_after_processing = () =>
        {
            result.ShouldEqual(true);           
        };


    }
}
