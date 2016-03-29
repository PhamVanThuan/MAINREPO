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
    public class when_searching_word_before_property : WithFakes
    {
        static NestedTernary tree;
        static string expctedResult;
        static IEnumerable<string> result;
        Establish context = () =>
        {
            expctedResult = "Pseudopseudohypoparathyroidism";
            tree = new NestedTernary();
            tree.Add("Pseudopseudohypoparathyroidism.expialidocious");
            tree.Add("Supercalifragilistic.expialidocious");
            tree.Add("Antidisestablishmentarianism.expialidocious");
        };

        Because of = () =>
        {
            result = tree.StartsWith("Pseud");
        };

        It should_return_expected_result = () =>
        {
            result.First().ShouldEqual(expctedResult);
        };
    }
}
