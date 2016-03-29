using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.DataStructures;
using System;
using System.Linq;

namespace SAHL.Core.Specs.DataStructures.DelimitedRadixTreeSpecs
{
    public class when_retrieving_an_item_from_an_empty_tree : WithFakes
    {
        private Establish that = () =>
        {
            rootValue = "rootValue";

            tree = new DelimitedRadixTree<string>(".", rootValue);
        };

        private Because of = () =>
        {
            value = tree.GetValue("unicorn");
        };

        private It should_return_the_root_value = () =>
        {
            value.ShouldEqual(rootValue);
        };

        private static DelimitedRadixTree<string> tree;
        private static string value;
        private static string rootValue;
    }
}