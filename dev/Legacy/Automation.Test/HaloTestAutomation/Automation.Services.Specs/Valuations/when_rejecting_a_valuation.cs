using BuildingBlocks.Services.Contracts;
using Machine.Specifications;
using Machine;
using System.Data;
using NUnit.Framework;
using System.Xml.Linq;
using Common.Enums;
namespace Automation.Services.Specs.Valuations
{
    public class when_rejecting_a_valuation
    {
        private static IValuationService valuationService;
        private static bool validResponse;

        Establish context = () =>
        {
            validResponse = false;
            valuationService = BuildingBlocks.ServiceLocator.Instance.GetService<IValuationService>();
        };
        Because of = () =>
        {
            valuationService.SubmitRejectedEzVal(1524424);
            validResponse = true;
        };

        It should_reject_valuation = () =>
        {
            Assert.True(validResponse);
        };
    }
}
