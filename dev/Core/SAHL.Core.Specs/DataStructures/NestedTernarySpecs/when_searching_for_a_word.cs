using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.DataStructures;
using System.Linq;

namespace SAHL.Core.Specs.DataStructures.NestedTernarySpecs
{
    [Subject("SAHL.Core.DataStructures.NestedTernary.StartsWith")]
    public class when_searching_for_a_word : WithFakes
    {
        static NestedTernary tree;
        static string result;
        static string expected = "Supercalifragilisticexpialidocious";

        Establish context = () =>
        {
            tree = new NestedTernary();
            tree.Add("Pseudopseudohypoparathyroidism");
            tree.Add(expected);
            tree.Add("Antidisestablishmentarianism");
        };

        Because of = () =>
        {
            result = tree.StartsWith("Super").First();
        };

        It should_have_first_result_equal_to_expected = () =>
        {
            result.ShouldEqual(expected);
        };
    }
}