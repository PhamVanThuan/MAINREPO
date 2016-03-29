using Mono.Cecil;
using System.Collections.Generic;

namespace SAHL.Tools.Capitec.CSJsonifier
{
    public interface IScanResult
    {
        bool UseContainer { get; }

        string Type { get; }

        string FakeType { get; }

        IList<TypeDefinition> CommandTypes { get; }

        IList<TypeDefinition> CommandResultTypes { get; }
    }
}