
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Shared.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SAHL.Common.Service.Interfaces
{
    public interface ILeadImportPublisherService
    {
        IBatchService PublishLeadsForImport<T>(Stream fileStream, string fileName) where T : class;
    }
}
