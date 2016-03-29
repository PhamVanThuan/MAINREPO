using System.Collections.Generic;

namespace SAHL.Core.Tests.Validation.Fakes
{
    public class FakeValidationCommandWithDictionary<TKey, TValue>
    {
        public Dictionary<TKey, TValue> Validating { get; set; }
    }
}