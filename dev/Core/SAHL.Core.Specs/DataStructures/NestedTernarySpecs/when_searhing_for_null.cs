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
    public class when_searhing_for_null : WithFakes
    {
        static NestedTernary tree;
        static Exception exception;
        
        Establish context = () =>
        {
            
            tree = new NestedTernary();
            tree.Add("Pseudopseudohypoparathyroidism");
            tree.Add("Supercalifragilistic");
            tree.Add("Antidisestablishmentarianism");
        };

        Because of = () =>
        {
            exception = Catch.Exception(()=>tree.StartsWith(null));
        };

        It should_throw_error = () =>
        {
            exception.ShouldNotBeNull();
        };
    }
}
