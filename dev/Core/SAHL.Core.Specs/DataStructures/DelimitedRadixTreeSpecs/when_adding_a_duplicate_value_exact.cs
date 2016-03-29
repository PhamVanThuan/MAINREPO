using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.DataStructures;
using System;
using System.Linq;

namespace SAHL.Core.Specs.DataStructures.DelimitedRadixTreeSpecs
{
    public class when_adding_a_duplicate_value_case_insensitive : WithFakes
    {
        private Establish that = () =>
        {
            keyDelimiter = ".";
            tree = new DelimitedRadixTree<int>(keyDelimiter);
            firstKey = "process";
            firstValue = 1;
            secondValue = 2;

            tree.Add(firstKey, firstValue);
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() => tree.Add(firstKey, secondValue));
        };

        private It should_prevent_adding_an_item_with_the_same_key = () =>
        {
            exception.ShouldBeAssignableTo<ArgumentException>();
        };

        private It should_have_an_appropriate_message = () =>
        {
            exception.Message.ShouldEqual(string.Format("An item with the same key '{0}' has already been added.", firstKey));
        };

        private It should_have_not_changed_the_value_of_the_item_already_in_the_tree = () =>
        {
            tree.Root.Children.First().Value.NodeValue.ShouldEqual(firstValue);
        };

        private static DelimitedRadixTree<int> tree;
        private static string keyDelimiter;
        private static string firstKey;
        private static int firstValue;
        private static int secondValue;
        private static Exception exception;
    }
}