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
using NSubstitute;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.Application
{
    public class when_saving_application : WithCoreFakes
    {
        private static IApplicationManager applicationManager;
        private static IApplicationDataManager applicationDataManager; 
        private static OfferType offerType;
        private static OfferStatus offerStatus;
        private static DateTime openDate;
        private static int applicationSourceKey;
        private static int reservedAccountKey;
        private static OriginationSource originationSource;
        private static int applicantCount;

        Establish context = () =>
        {
            offerType = OfferType.NewPurchaseLoan; 
            offerStatus = OfferStatus.Open;
            openDate = DateTime.Now;
            applicationSourceKey = 1;
            reservedAccountKey = 1;
            originationSource = OriginationSource.SAHomeLoans;
            applicantCount = 1;

            applicationDataManager = An<IApplicationDataManager>();

            applicationManager = new ApplicationManager(applicationDataManager);
        };

        Because of = () =>
        {
            applicationManager.SaveApplication(offerType, offerStatus, openDate, applicationSourceKey, reservedAccountKey, originationSource, "reference1", applicantCount);
        };

        It should_ask_the_application_data_manager_to_save_the_application_model = () =>
        {
            applicationDataManager.WasToldTo(x => x.SaveApplication(Arg.Is<OfferDataModel>(o =>
                                                                        (o.OfferTypeKey == (int) offerType) &&
                                                                        (o.OfferStatusKey == (int) offerStatus) &&
                                                                        (o.OfferSourceKey == applicationSourceKey) &&
                                                                        (o.OriginationSourceKey == (int)OriginationSource.SAHomeLoans) &&
                                                                        (o.EstimateNumberApplicants == applicantCount)
                                                                    )));

            
        };
    }
}
