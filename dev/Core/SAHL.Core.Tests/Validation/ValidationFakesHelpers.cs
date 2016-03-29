using SAHL.Core.Tests.Validation.Fakes;
using SAHL.Core.Validation;

namespace SAHL.Core.Tests.Validation
{
    public static class ValidationFakesHelpers
    {
        public const string MatchSingleDigitRegularExpressionPattern = @"^\d$";
        public const string MatchSingleLetterRegularExpressionPattern = @"^[A-Za-z]$";
        public const string MatchAnyCharacterRegularExpressionPattern = @"^.*$";

        public static IValidateCommand CreateValidateCommand(ITypeMetaDataLookup lookup = null)
        {
            var metaDataLookup = lookup ?? new TypeMetaDataLookup();
            return new ValidateCommand(metaDataLookup, new ValidateStrategy(metaDataLookup));
        }

        public static FakeValidationCommandComposite CreateNewFakeValidationCommand(string id = "", string key = "", string commandSingleKey = "")
        {
            return new FakeValidationCommandComposite()
            {
                Id = id,
                Key = key,
                Composite = new FakeValidationCommandSingleProperty
                {
                    Key = commandSingleKey
                }
            };
        }
    }
}