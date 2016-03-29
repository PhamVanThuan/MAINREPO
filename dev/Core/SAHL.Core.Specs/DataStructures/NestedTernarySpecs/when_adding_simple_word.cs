﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.DataStructures;

namespace SAHL.Core.Specs.DataStructures.NestedTernarySpecs
{
    [Subject("SAHL.Core.DataStructures.NestedTernary.Add")]
    public class when_adding_simple_word : WithFakes
    {
        static NestedTernary tree;
        static NestedTernaryNode result;

        Establish context = () =>
        {
            result = new NestedTernaryNode('a')
            {
                Center = new NestedTernaryNode('b')
                {
                    IsLast = true
                }
            };
            tree = new NestedTernary();
        };

        Because of = () =>
        {
            tree.Add("ab");
        };

        It should_set_root = () =>
        {
            tree.Root.ShouldNotBeNull();
        };

        It should_equal_expected_result_at_top_node = () =>
        {
            tree.Root.Value.ShouldEqual(result.Value);
        };

        It should_equal_expected_result_at_last_node = () =>
        {
            tree.Root.Center.Value.ShouldEqual(result.Center.Value);
        };

        It should_set_is_last_true_on_last_node = () =>
        {
            tree.Root.Center.IsLast.ShouldEqual(true);
        };

        It should_have_null_node_on_root_left = () =>
        {
            tree.Root.Left.ShouldBeNull();
        };

        It should_have_null_node_on_root_right = () =>
        {
            tree.Root.Right.ShouldBeNull();
        };
    }
}