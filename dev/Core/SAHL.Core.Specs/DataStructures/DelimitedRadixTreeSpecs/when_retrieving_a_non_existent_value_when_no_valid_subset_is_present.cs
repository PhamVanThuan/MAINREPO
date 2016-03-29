using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.DataStructures;
using System;
using System.Linq;

namespace SAHL.Core.Specs.DataStructures.DelimitedRadixTreeSpecs
{
    public class when_retrieving_a_non_existent_value_when_no_valid_subset_is_present : WithFakes
    {
        private Establish that = () =>
        {

            rootValue = "rootValue";

            var root = new DelimitedRadixTree<string>.Node<string, string>(string.Empty, rootValue);
            var firstKey = "first";
            var first = new DelimitedRadixTree<string>.Node<string, string>(firstKey, "1st");
            root.Children.Add(firstKey, first);

            var secondKey = "second";
            var second = new DelimitedRadixTree<string>.Node<string, string>(secondKey, "2nd");
            first.Children.Add(secondKey, second);

            tree = new DelimitedRadixTree<string>(".", root); //tree is: first (1st) -> second (2nd)
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