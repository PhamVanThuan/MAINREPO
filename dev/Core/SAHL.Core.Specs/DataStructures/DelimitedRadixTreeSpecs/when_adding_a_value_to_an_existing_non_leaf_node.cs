using System;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.DataStructures;
using SAHL.Core.Testing;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context.Configuration;

namespace SAHL.Core.Specs.DataStructures.DelimitedRadixTreeSpecs
{
    public class when_adding_a_value_to_an_existing_non_leaf_node_by_shortest_first : WithFakes
    {
        Establish that = () =>
        {
            keyDelimiter = ".";
            tree = new DelimitedRadixTree<int>(keyDelimiter);
            firstKey = "process";
            firstValue = 1;
            secondKey = "process.workflow";
            secondValue = 2;
            thirdKey = "process.workflow.state";
            thirdValue = 3;
        };

        private Because of = () =>
        {
            tree.Add(firstKey, firstValue);
            tree.Add(secondKey, secondValue);
            tree.Add(thirdKey, thirdValue);
        };

        private It should_have_a_non_default_value_at_the_non_leaf_nodes = () =>
        {
            NodeHelpers.GetNodeByKey(tree, "process").NodeValue.ShouldEqual(firstValue);
            NodeHelpers.GetNodeByKey(tree, "workflow").NodeValue.ShouldEqual(secondValue);
            NodeHelpers.GetNodeByKey(tree, "state").NodeValue.ShouldEqual(thirdValue);
        };

        private static DelimitedRadixTree<int> tree;
        private static string keyDelimiter;
        private static string firstKey;
        private static string secondKey;
        private static int firstValue;
        private static int secondValue;
        private static string thirdKey;
        private static int thirdValue;
    }
}
