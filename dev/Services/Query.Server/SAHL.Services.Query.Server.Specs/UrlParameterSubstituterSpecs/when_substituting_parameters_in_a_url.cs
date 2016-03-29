using System;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Web.Mvc;
using SAHL.Core.Strings;
using SAHL.Services.Query.Url;
using StructureMap;
using Machine.Fakes;
using Machine.Specifications;

using SAHL.Core.Testing;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context.Configuration;

namespace SAHL.Services.Query.Server.Specs.UrlParameterSubstituterSpecs
{
    public class when_substituting_parameters_in_a_url : WithFakes
    {
        Establish that = () =>
        {
            url = "http://banana.url/somewhere/{over}/{the}/{rainbow}?a=1&b=2";

            replacements = Enumerable.Empty<KeyValuePair<string, string>>();

            stringReplacer = An<IStringReplacer>();
            stringReplacer
                .WhenToldTo(a => a.Replace(url, replacements, StringComparison.OrdinalIgnoreCase))
                .Return(" banana ");

            substituter = new UrlParameterSubstituter(stringReplacer);
        };


        private Because of = () =>
        {
            result = substituter.Replace(url, replacements);
        };

        private It should_have_called_the_case_insensitive_replace_on_the_string_replacer = () =>
        {
            stringReplacer
                .WasToldTo(a => a.Replace(url, replacements, StringComparison.OrdinalIgnoreCase))
                .OnlyOnce();
        };

        private It should_have_returned_the_value_retrieved_from_the_string_replacer = () =>
        {
            result.ShouldEqual(" banana ");
        };

        private static string url;
        private static IStringReplacer stringReplacer;
        private static UrlParameterSubstituter substituter;
        private static IEnumerable<KeyValuePair<string, string>> replacements;
        private static string result;
    }
}
