using System;

namespace SAHL.Core.Identity
{
    public interface ICombGuid
    {
        Guid Generate();

        string GenerateString();
    }
}