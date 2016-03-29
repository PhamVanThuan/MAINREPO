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
    public class when_adding_to_an_empty_tree : WithFakes
    {
        Establish that = () =>
        {
            keyDelimiter = ".";
            tree = new DelimitedRadixTree<object>(keyDelimiter);
            key = "somewhere.over.the.rainbow.way.up.high";
        };

        private Because of = () =>
        {
            tree.Add(key, 1);
        };

        private It should_have_the_specified_key_delimiter = () =>
        {
            tree.KeyDelimiter.ShouldEqual(new [] { keyDelimiter });
        };

        private It should_have_a_non_empty_root = () =>
        {
            tree.Root.ShouldNotBeNull();
        };

        private It should_have_children = () =>
        {
            tree.Root.Children.ShouldNotBeEmpty();
        };

        private It should_have_an_empty_root_node = () =>
        {
            tree.Root.NodeValue.ShouldBeNull();
        };

        private It should_have_a_node_chain_with_matching_node_keys = () =>
        {
            var tokens = key.Split(new[] { keyDelimiter }, StringSplitOptions.None);
            var sequentialKeys = new List<string>();
            var currentChildren = tree.Root.Children;
            while (currentChildren.Any())
            {
                var currentItem = currentChildren.Single();

                sequentialKeys.Add(currentItem.Value.NodeKey);

                currentChildren = currentItem.Value.Children;
            }
            tokens.ToList().ShouldEqual(sequentialKeys);
        };

        private It should_have_a_depth_equal_to_the_number_of_tokens_supplied = () =>
        {
            var depth = 0;
            var currentChildren = tree.Root.Children;
            while (currentChildren.Any())
            {
                depth++;
                currentChildren.Count.ShouldEqual(1);
                currentChildren = currentChildren.Single().Value.Children;
            }
            depth.ShouldEqual(key.Split(new [ ] { keyDelimiter }, StringSplitOptions.None).Length);
        };

        private It should_have_lookup_key_values_equal_to_each_token = () =>
        {
            var tokens = key.Split(new [ ] { keyDelimiter }, StringSplitOptions.None);
            var sequentialKeys = new List<string>();
            var currentChildren = tree.Root.Children;
            while (currentChildren.Any())
            {
                var currentItem = currentChildren.Single();

                sequentialKeys.Add(currentItem.Key);

                currentChildren = currentItem.Value.Children;
            }
            tokens.ToList().ShouldEqual(sequentialKeys);
        };

        private static DelimitedRadixTree<object> tree;
        private static string keyDelimiter;
        private static string key;
    }
}
