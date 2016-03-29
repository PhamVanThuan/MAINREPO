using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.CATS.ConfigExtension;
using SAHL.Services.CATS.Managers;
using SAHL.Services.CATS.Managers.CATS;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;

namespace SAHL.Services.CATS.Server.Specs.Managers.CatsManagerSpecs
{
    public class when_grouping_payments : WithFakes
    {
        private static ICATSManager catsManager;
        private static IEnumerable<CATSPaymentBatchItemDataModel> batchPayments;
        private static IDictionary<int, IEnumerable<CATSPaymentBatchItemDataModel>> results;
        private static IDictionary<int, IEnumerable<CATSPaymentBatchItemDataModel>> expectedResults;
        private static ICATSDataManager catsDataManager;
        private static IFileSystem fileSystem;
        private static ICatsAppConfigSettings catsConfigSettings;

        private Establish context = () =>
         {
             catsConfigSettings = An<ICatsAppConfigSettings>();
             fileSystem = An<IFileSystem>();
             catsDataManager = An<ICATSDataManager>();
             catsManager = new CATSManager(catsDataManager, fileSystem, catsConfigSettings);

             batchPayments = new List<CATSPaymentBatchItemDataModel>{
                        new CATSPaymentBatchItemDataModel(34, 14008, (int) GenericKeyType.ThirdPartyInvoice, 42006,
                            200m, 32109, 8409, 96,
                            "SAHL/02/08/20015-23", "SAHL SPV", "Strauss Daly", "", "strauss@sd.co.za", 54007, true)
                     };

             expectedResults = new Dictionary<int, IEnumerable<CATSPaymentBatchItemDataModel>>();
             expectedResults.Add(2005, batchPayments);
         };

        private Because of = () =>
         {
             results = catsManager.GroupBatchPaymentsByRecipient(batchPayments);
         };

        private It should_return_grouped_payments = () =>
         {
             results.First().Value.ShouldEqual(batchPayments);
             results.First().Key.ShouldEqual(batchPayments.First().LegalEntityKey);
         };
    }
}