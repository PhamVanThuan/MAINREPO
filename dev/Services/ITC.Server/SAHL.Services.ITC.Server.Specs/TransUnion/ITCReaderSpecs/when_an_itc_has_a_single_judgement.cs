using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Specifications;
using SAHL.Services.Interfaces.ITC.Models;
using SAHL.Services.ITC.Tests.TransUnion;
using SAHL.Services.ITC.TransUnion;

namespace SAHL.Services.ITC.Server.Specs.TransUnion.ITCReaderSpecs
{
    class when_an_itc_has_a_single_judgement
    {
        private static ItcResponse response;
        private static ItcReader reader;
        private static ItcProfile itcProfile;

        Establish context = () =>
        {
            response = TestITCProvider.GetTestITC("Judgements 1 and value 10 000 ITC.xml");
        };

        Because of = () =>
        {
            reader = new ItcReader(response.Response);
            itcProfile = reader.GetITCProfile;
        };

        It should_contain_the_correct_judgement_value = () =>
        {
            itcProfile.Judgments.First().Amount.ShouldEqual(10000);
        };

        It should_contain_the_correct_number_of_judgements = () =>
        {
            itcProfile.Judgments.Count().ShouldEqual(1);
        };
    }
}
