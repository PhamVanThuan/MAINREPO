using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SAHL.Common.Service
{
    public class LeadImportPublisherService : ILeadImportPublisherService
    {
        readonly IBatchPublisher batchPublisher;
        readonly ITextFileParser fileParser;
        readonly IBatchServiceRepository batchServiceRepo;

        public LeadImportPublisherService(IBatchPublisher batchPublisher, ITextFileParser fileParser, IBatchServiceRepository batchServiceRepo)
        {
            this.batchPublisher = batchPublisher;
            this.fileParser = fileParser;
            this.batchServiceRepo = batchServiceRepo;
        }

        public IBatchService PublishLeadsForImport<T>(Stream fileStream, string fileName) where T : class
        {
            IEnumerable<T> leads = this.fileParser.Parse<T>(fileStream);

            if (leads != null)
            {
                var batchService = CreateBatchService(leads.Count(), fileStream, fileName);
                this.batchPublisher.Publish(leads, batchService);
                return batchService;
            }
            return null;
        }

        private IBatchService CreateBatchService(int batchCount, Stream fileStream, string fileName)
        {
            var principal = SAHLPrincipal.GetCurrent();
            var batchService = batchServiceRepo.GetEmptyBatchService();
            batchService.RequestedBy = principal.Identity.Name;
            batchService.RequestedDate = DateTime.Now;
            batchService.BatchCount = batchCount;
            batchService.BatchServiceTypeKey = (int)BatchServiceTypes.PersonalLoanLeadImport;
            fileStream.Position = 0;
            byte[] fileContent = new byte[fileStream.Length];
            fileStream.Read(fileContent, 0, Convert.ToInt32(fileStream.Length));
            batchService.FileContent = fileContent;
            batchService.FileName = fileName;
            batchServiceRepo.SaveBatchService(batchService);
            return batchService;
        }
    }
}