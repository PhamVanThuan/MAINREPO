using Machine.Specifications;
using Machine.Fakes;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.Managers.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.Data.Models._2AM;
using NSubstitute;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.Application
{
    public class when_saving_application_information : WithCoreFakes
    {
        private static IApplicationManager applicationManager;
        private static IApplicationDataManager applicationDataManager;

        private static DateTime offerInsertDate;
        private static int applicationNumber;
        private static OfferInformationType offerInformationType;
        private static Product product;

        Establish context = () =>
        {
            offerInsertDate = DateTime.Now;
            applicationNumber = 1;
            offerInformationType = OfferInformationType.OriginalOffer;
            product = Product.NewVariableLoan;
            
            applicationDataManager = An<IApplicationDataManager>();
            applicationManager = new ApplicationManager(applicationDataManager);
        };

        Because of = () =>
        {
            applicationManager.SaveApplicationInformation(offerInsertDate, applicationNumber, offerInformationType, product);
        };

        It should_ask_the_application_data_manager_to_save_the_application_information_model = () =>
        {
            applicationDataManager.WasToldTo(x => x.SaveApplicationInformation(Arg.Is<OfferInformationDataModel>(y =>
                    y.OfferInsertDate == offerInsertDate &&
                    y.OfferKey == applicationNumber &&
                    y.OfferInformationTypeKey == (int)offerInformationType &&
                    y.ProductKey == (int) product
                )));
        };
    }
}
