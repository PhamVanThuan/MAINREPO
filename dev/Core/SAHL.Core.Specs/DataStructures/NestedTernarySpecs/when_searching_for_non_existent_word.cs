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
    public class when_searching_for_non_existent_word : WithFakes
    {
        static NestedTernary tree;
        static IEnumerable<string> result;

        Establish context = () =>
        {
            tree = new NestedTernary();
            tree.Add("Supercalifragilistic");
            tree.Add("Antidisestablishmentarianism");
        };

        Because of = () =>
        {
            result = tree.StartsWith("Pseudopseudohypoparathyroidism");
        };

        It should_return_no_results = () =>
        {
            result.Count().ShouldEqual(0);
        };
    }
}
