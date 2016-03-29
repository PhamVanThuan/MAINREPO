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
    public class when_adding_a_string : WithFakes
    {
        static SAHL.Core.Cache<string> cache;
        static string inputKey;
        static string inputString;

        Establish context = () =>
        {
            inputKey = "Key";
            inputString = "Test";

            cache = new Cache<string>();
        };

        Because of = () =>
        {
            cache.Add(inputKey, inputString);
        };

        It should_only_have_one_item = () =>
        {
            cache.GetAll().Count().ShouldEqual(1);
        };

        It should_not_be_empty = () =>
        {
            cache.IsEmpty.ShouldBeFalse();
        };

        It should_now_contain_key = () =>
        {
            cache.Exists(inputKey);
        };

        It should_return_expected_value_for_key = () =>
        {
            cache.Get(inputKey).ShouldEqual(inputString);
        };
    }
}
