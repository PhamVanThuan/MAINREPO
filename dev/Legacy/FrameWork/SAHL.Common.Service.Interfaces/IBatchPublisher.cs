
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Shared.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SAHL.Common.Service.Interfaces
{
    public interface IBatchPublisher
    {
        void Publish<T>(IEnumerable<T> messages, IBatchService batchService) where T : class;
    }
}
