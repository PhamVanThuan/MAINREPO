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
    public class when_cache_is_empty : WithFakes
    {
        static SAHL.Core.Cache<string> cache;

        Establish context = () =>
        {

        };

        Because of = () =>
        {
            cache = new Cache<string>();
        };

        It should_only_have_one_item = () =>
        {
            cache.GetAll().Count().ShouldEqual(0);
        };

        It should_not_be_empty = () =>
        {
            cache.IsEmpty.ShouldBeTrue();
        };
    }
}
