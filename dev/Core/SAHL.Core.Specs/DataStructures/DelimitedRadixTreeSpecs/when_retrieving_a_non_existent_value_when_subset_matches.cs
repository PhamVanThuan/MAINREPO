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
    public class when_retrieving_a_non_existent_value_when_subset_matches : WithFakes
    {
        Establish that = () =>
        {
            var root = new DelimitedRadixTree<string>.Node<string, string>(string.Empty, null);

            firstKey = "first";
            firstValue = "1st";
            var first = new DelimitedRadixTree<string>.Node<string, string>(firstKey, firstValue);
            root.Children.Add(firstKey, first);

            secondKey = "second";
            secondValue = "2nd";
            var second = new DelimitedRadixTree<string>.Node<string, string>(secondKey, secondValue);
            first.Children.Add("second", second);

            thirdKey = "third";
            thirdValue = "3rd";
            var third = new DelimitedRadixTree<string>.Node<string, string>(thirdKey, thirdValue);
            second.Children.Add("third", third);

            tree = new DelimitedRadixTree<string>(".", root); 
        };

        private Because of = () =>
        {
            value = tree.GetValue("first.second.third.fourth.fifth");
        };

        private It should_find_the_last_common_prefix_value = () =>
        {
            value.ShouldEqual(thirdValue);
        };

        private static DelimitedRadixTree<string> tree;
        private static string value;
        private static string secondValue;
        private static string firstValue;
        private static string thirdValue;
        private static string firstKey;
        private static string secondKey;
        private static string thirdKey;
    }
}
