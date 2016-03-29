using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.DataStructures;
using System;

namespace SAHL.Core.Specs.DataStructures.NestedTernarySpecs
{
    [Subject("SAHL.Core.DataStructures.NestedTernary.Add")]
    public class when_adding_empty_word : WithFakes
    {
        static NestedTernary tree;
        static Exception exc;

        Establish context = () =>
        {
            tree = new NestedTernary();
        };

        Because of = () =>
        {
            exc = Catch.Exception(() => tree.Add(""));
        };

        It should_not_set_root = () =>
        {
            tree.Root.ShouldBeNull();
        };
    }
}