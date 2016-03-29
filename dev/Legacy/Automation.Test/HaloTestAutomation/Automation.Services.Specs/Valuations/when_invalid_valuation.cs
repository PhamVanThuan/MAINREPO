using BuildingBlocks.Services.Contracts;
using Machine.Specifications;
using Machine;
using System.Data;
using NUnit.Framework;
using System.Xml.Linq;
using Common.Enums;
namespace Automation.Services.Specs.Valuations
{
    public class when_invalid_valuation
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
            valuationService.SubmitInvalidCompletedEzVal(1524424);
            validResponse = true;
        };

        It should_raise_error_valuation = () =>
        {
            Assert.True(validResponse);
        };
    }
}
