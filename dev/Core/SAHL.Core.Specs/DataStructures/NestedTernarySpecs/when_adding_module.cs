using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.DataStructures;

namespace SAHL.Core.Specs.DataStructures.NestedTernarySpecs
{
    [Subject("SAHL.Core.DataStructures.NestedTernary.Add")]
    public class when_adding_module : WithFakes
    {
        static NestedTernary tree;
        static NestedTernaryNode result;

        Establish context = () =>
        {
            result = new NestedTernaryNode('a')
            {
                Modules = new NestedTernaryNode('b')
                {
                    IsLast = true
                }
            };

            tree = new NestedTernary();
        };

        Because of = () =>
        {
            tree.Add("a::b");
        };

        It should_set_root = () =>
        {
            tree.Root.ShouldNotBeNull();
        };

        It should_equal_expected_result_at_top_node = () =>
        {
            tree.Root.Value.ShouldEqual(result.Value);
        };

        It should_set_is_last_true_on_root_node = () =>
        {
            tree.Root.IsLast.ShouldEqual(true);
        };

        It should_equal_expected_result_at_modules_node = () =>
        {
            tree.Root.Modules.Value.ShouldEqual(result.Modules.Value);
        };

        It should_set_is_last_true_on_modules_node = () =>
        {
            tree.Root.Modules.IsLast.ShouldEqual(true);
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