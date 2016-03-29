using System.Collections.Generic;
using Mono.Cecil;

namespace SAHL.Tools.RestServiceRoutenator
{
    public interface IScanResult
    {
        bool UseContainer { get; }

        string Type { get; }

        string FakeType { get; }

        IList<TypeDefinition> FoundTypes { get; }

        IList<TypeDefinition> FoundResultTypes { get; }

        string[] AdditionalData { get; set; }

    }
}