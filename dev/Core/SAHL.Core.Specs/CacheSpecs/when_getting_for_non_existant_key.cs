using Machine.Fakes;
using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.Specs.CacheSpecs
{
    [Subject("SAHL.Core.Cache.Add")]
    public class when_getting_for_non_existant_key : WithFakes
    {
        static SAHL.Core.Cache<string> cache;
        static string inputKey;
        static Exception exception;

        Establish context = () =>
        {
            inputKey = "Key";
            cache = new Cache<string>();
        };

        Because of = () =>
        {
            exception = Catch.Exception(() => cache.Get(inputKey));
        };

        It should_only_have_one_item = () =>
        {
            cache.GetAll().Count().ShouldEqual(0);
        };

        It should_not_be_empty = () =>
        {
            cache.IsEmpty.ShouldBeTrue();
        };

        It should_throw_error = () =>
        {
            exception.ShouldNotBeNull();
        };
    }
}
