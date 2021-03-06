﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.DataStructures;
using System.Linq;

namespace SAHL.Core.Specs.DataStructures.NestedTernarySpecs
{
    [Subject("SAHL.Core.DataStructures.NestedTernary.StartsWith")]
    public class when_searching_for_a_module : WithFakes
    {
        static NestedTernary tree;
        static string result;
        static string expected = "expialidocious";

        Establish context = () =>
        {
            tree = new NestedTernary();
            tree.Add("Pseudopseudohypoparathyroidism::expialidocious");
            tree.Add("Supercalifragilistic::expialidocious");
            tree.Add("Antidisestablishmentarianism::expialidocious");
        };

        Because of = () =>
        {
            result = tree.StartsWith("Supercalifragilistic::expia").First();
        };

        It should_have_first_result_equal_to_expected = () =>
        {
            result.ShouldEqual(expected);
        };
    }
}