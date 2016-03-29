using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Service.Interfaces;
using SAHL.Shared.Messages.PersonalLoanLead;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SAHL.Common.Service.Specs
{
    public class when_given_a_personal_loan_lead_file : WithFakes
    {
        static ILeadImportPublisherService leadImportService;
        static ITextFileParser fileParser;
        static IEnumerable<PersonalLoanLead> leads;
        static IBatchServiceRepository batchServiceRepo;

        static IBatchPublisher batchPublisher;
        static Stream stream;
        static IBatchService batchService;

        Establish context = () =>
        {
            batchService = An<IBatchService>();
            batchPublisher = An<IBatchPublisher>();
            fileParser = An<ITextFileParser>();
            batchServiceRepo = An<IBatchServiceRepository>();
            leadImportService = new LeadImportPublisherService(batchPublisher, fileParser, batchServiceRepo);
            leads = new List<PersonalLoanLead>();

            stream = An<Stream>();
            batchServiceRepo.WhenToldTo(x => x.GetEmptyBatchService()).Return(batchService);
            fileParser.WhenToldTo(x => x.Parse<PersonalLoanLead>(stream)).Return(leads);
        };

        Because of = () =>
        {
            leadImportService.PublishLeadsForImport<PersonalLoanLead>(stream, string.Empty);
        };

        It should_parse_the_file = () =>
        {
            fileParser.WasToldTo(x => x.Parse<PersonalLoanLead>(stream));
        };

        It should_publish_each_the_leads_to_the_queue = () =>
        {
            batchPublisher.WasToldTo(x => x.Publish<PersonalLoanLead>(leads, batchService));
        };
    }
}