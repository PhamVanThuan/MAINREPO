
using SAHL.Shared.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SAHL.Common.Service.Interfaces
{
    public interface ITextFileParser
    {
        IEnumerable<T> Parse<T>(Stream fileStream) where T : class;
    }
}
