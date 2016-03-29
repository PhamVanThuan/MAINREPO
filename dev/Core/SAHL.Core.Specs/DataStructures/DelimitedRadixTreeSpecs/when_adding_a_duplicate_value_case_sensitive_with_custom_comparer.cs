using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.DataStructures;
using System;
using System.Linq;

namespace SAHL.Core.Specs.DataStructures.DelimitedRadixTreeSpecs
{
    public class when_adding_a_duplicate_value_case_sensitive_with_custom_comparer : WithFakes
    {
        private Establish that = () =>
        {
            keyDelimiter = ".";
            tree = new DelimitedRadixTree<int>(keyDelimiter, 0, StringComparer.Ordinal);
            firstKey = "process";
            firstValue = 1;
            secondKey = "PROCESS";
            secondValue = 2;
            thirdKey = "pRoCeSs";
            thirdValue = 3;

            tree.Add(firstKey, firstValue);
        };

        private Because of = () =>
        {
            tree.Add(secondKey, secondValue);
            tree.Add(thirdKey, thirdValue);
        };

        private It should_have_a_root_with_three_children = () =>
        {
            tree.Root.Children.Count.ShouldEqual(3);
        };

        private static DelimitedRadixTree<int> tree;
        private static string keyDelimiter;
        private static string firstKey;
        private static string secondKey;
        private static int firstValue;
        private static int secondValue;
        private static string thirdKey;
        private static int thirdValue;
    }
}