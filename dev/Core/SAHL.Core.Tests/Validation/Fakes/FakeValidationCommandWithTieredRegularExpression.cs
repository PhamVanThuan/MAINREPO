using SAHL.Core.Validation.Attributes;

namespace SAHL.Core.Tests.Validation.Fakes
{
    public class FakeValidationCommandWithTieredRegularExpression
    {
        [TieredReguarExpression(ValidationFakesHelpers.MatchSingleDigitRegularExpressionPattern, ValidationFakesHelpers.MatchSingleLetterRegularExpressionPattern)]
        public string Value { get; set; }
    }
}