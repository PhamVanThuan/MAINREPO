using System;
using SAHL.Core.Validation.Attributes;

namespace SAHL.Core.Tests.Validation.Fakes
{
    public class FakeValidationCommandWithIsoDateFormatOnDateTimeProperty
    {
        [ShouldBeInSahlDateFormat(ErrorMessage = "Please provide a valid date of birth")]
        public DateTime? Value { get; set; }
    }

    public class FakeValidationCommandWithIsoDateFormatOnStringProperty
    {
        [ShouldBeInSahlDateFormat(ErrorMessage = "Please provide a valid date of birth")]
        public string Value { get; set; }
    }
}