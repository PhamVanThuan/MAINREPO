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
    public class when_retrieving_an_item_from_a_multi_branch_tree : WithFakes
    {
        Establish that = () =>
        {
            var root = new DelimitedRadixTree<string>.Node<string, string>(string.Empty, null);

            firstKey = "Origination";
            firstValue = "SELECT 1";
            var first = new DelimitedRadixTree<string>.Node<string, string>(firstKey, firstValue);
            root.Children.Add(firstKey, first);

            secondKey = "ApplicationCapture";
            secondValue = "SELECT 2";
            var second = new DelimitedRadixTree<string>.Node<string, string>(secondKey, secondValue);
            first.Children.Add(secondKey, second);

            firstChildOfSecondKey = "State1";
            firstChildOfSecondValue = "SELECT 3a";
            var firstChildOfSecond = new DelimitedRadixTree<string>.Node<string, string>(firstChildOfSecondKey, firstChildOfSecondValue);
            second.Children.Add(firstChildOfSecondKey, firstChildOfSecond);

            secondChildOfSecondKey = "State2";
            secondChildOfSecondValue = "SELECT 3b";
            var secondChildOfSecond = new DelimitedRadixTree<string>.Node<string, string>(secondChildOfSecondKey, secondChildOfSecondValue);
            second.Children.Add(secondChildOfSecondKey, secondChildOfSecond);

            tree = new DelimitedRadixTree<string>(".", root); 
        };

        private Because of = () =>
        {
            value = tree.GetValue("Origination.ApplicationCapture.State2");
        };

        private It should_retrieve_the_associated_value = () =>
        {
            value.ShouldEqual(secondChildOfSecondValue);
        };

        private static DelimitedRadixTree<string> tree;
        private static string value;
        private static string firstValue;
        private static string secondValue;
        private static string firstChildOfSecondValue;
        private static string secondChildOfSecondValue;
        private static string firstKey;
        private static string secondKey;
        private static string firstChildOfSecondKey;
        private static string secondChildOfSecondKey;
    }
}
