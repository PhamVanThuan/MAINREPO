using BuildingBlocks.Services.Contracts;
using Machine.Specifications;
using Machine;
using System.Data;
using NUnit.Framework;
using System.Xml.Linq;
using Common.Enums;
namespace Automation.Services.Specs.Valuations
{
    public class when_completing_a_valuation
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
            valuationService.SubmitCompletedEzVal(1534328, HOCRoofEnum.Conventional, conventionalAmount: 0.0f, thatchAmount: 800000, valuationAmount: 1200000f);
            validResponse = true;
        };

        It should_complete_valuation = () =>
        {
            Assert.True(validResponse);
        };
    }
}
