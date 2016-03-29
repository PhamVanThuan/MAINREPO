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
    public class when_adding_an_item_to_an_existing_partial_tree : WithFakes
    {
        Establish that = () =>
        {
            keyDelimiter = ".";
            tree = new DelimitedRadixTree<int>(keyDelimiter);
            firstKey = "somewhere.over.the.rainbow.way.up.high";
            firstValue = 1;
            secondKey = "somewhere.over.the.rainbow.way.down.low";
            secondValue = 2;
        };

        private Because of = () =>
        {
            tree.Add(firstKey, firstValue);
            tree.Add(secondKey, secondValue);
        };

        private It should_have_two_children_at_the_last_common_node = () =>
        {
            NodeHelpers.GetNodeByKey(tree, "way").Children.Count.ShouldEqual(2);
        };

        private It should_have_the_first_key_as_the_first_child = () =>
        {
            NodeHelpers.GetNodeByKey(tree, "way").Children.First().Key.ShouldEqual("up");
        };

        private It should_have_the_second_key_as_the_second_child = () =>
        {
            NodeHelpers.GetNodeByKey(tree, "way").Children.Skip(1).First().Key.ShouldEqual("down");
        };

        private It should_have_a_single_child_on_the_first_split = () =>
        {
            NodeHelpers.GetNodeByKey(tree, "way")
                .Children //children of way node
                .First() //up key-value-pair
                .Value //up node
                .Children //children of up node
                .First() //first child of up node
                .Key.ShouldEqual("high");
        };

        private It should_have_one_child_on_the_second_split = () =>
        {
            NodeHelpers.GetNodeByKey(tree, "way")
                .Children //children of way node
                .Skip(1).First() //down key-value-pair
                .Value //down node
                .Children.First() //first child of down node
                .Key.ShouldEqual("low");
        };

        private It should_have_a_matching_first_value = () =>
        {
            NodeHelpers.GetNodeByKey(tree, "way")
                .Children //children of way node
                .First() //up key-value-pair
                .Value //up node
                .Children //children of up node
                .First() //high key-value-pair
                .Value //high nodeValue
                .NodeValue.ShouldEqual(firstValue);
        };

        private It should_have_a_matching_second_value = () =>
        {
            NodeHelpers.GetNodeByKey(tree, "way")
                .Children //children of way node
                .Skip(1).First() //down key-value-pair
                .Value //down node
                .Children //children of down node
                .First() //low key-value-pair
                .Value //low nodeValue
                .NodeValue.ShouldEqual(secondValue);
        };

        private It should_have_non_leaf_nodes_with_default_values = () =>
        {
            var currentChildren = tree.Root.Children;
            CheckChildren(currentChildren);
        };

        private static void CheckChildren(IDictionary<string, DelimitedRadixTree<int>.Node<string, int>> currentChildren)
        {
            foreach (var item in currentChildren)
            {
                if (item.Value.Children.Any())
                {
                    //is a non-leaf node
                    item.Value.NodeValue.ShouldEqual(default(int));
                    CheckChildren(item.Value.Children);
                }
                else
                {
                    //is a leaf node
                    item.Value.ShouldNotBeNull();
                }
            }
        }
        
        private static DelimitedRadixTree<int> tree;
        private static string keyDelimiter;
        private static string firstKey;
        private static string secondKey;
        private static int firstValue;
        private static int secondValue;
    }
}
