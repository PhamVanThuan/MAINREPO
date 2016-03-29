using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.Specs.DataStructures.NestedTernarySpecs
{
    [Subject("SAHL.Core.DataStructures.NestedTernary.StartsWith")]
    public class when_searching_for_exact_word : WithFakes
    {
        static NestedTernary tree;
        static IEnumerable<string> result;
        static string expectedResult;

        Establish context = () =>
        {
            expectedResult = "Pseudopseudohypoparathyroidism";
            tree = new NestedTernary();
            tree.Add("Pseudopseudohypoparathyroidism");
            tree.Add("Supercalifragilistic");
            tree.Add("Antidisestablishmentarianism");
        };

        Because of = () =>
        {
            result = tree.StartsWith("Pseudopseudohypoparathyroidism");
        };

        It should_contain_expected_result = () =>
        {
            result.First().ShouldEqual(expectedResult);
        };
    }
}
