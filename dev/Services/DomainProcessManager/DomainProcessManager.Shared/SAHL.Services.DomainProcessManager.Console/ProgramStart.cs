using SAHL.Config.Services.Client;
using SAHL.Config.Services.DomainProcessManager.Client;
using SAHL.Core;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.DomainProcessManager;
using System;
using System.Collections.Generic;

namespace SAHL.Services.DomainProcessManager
{
    public class ProgramStart
    {
        private static IIocContainer _iocContainer;
        private static IDomainProcessManagerClient _domainProcessManagerClient;

        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Starting the Domain Process Manager console test application");

                var clientBootstrapper = new ClientBootstrapper();
                _iocContainer = clientBootstrapper.Initialise();
                if (_iocContainer == null) { throw new NullReferenceException("Ioc Container not found"); }

                _domainProcessManagerClient = _iocContainer.GetInstance<IDomainProcessManagerClient>();

                if (_domainProcessManagerClient == null) { throw new NullReferenceException("Domain Process Manager Client not found"); }

                //var applicationCreationProcessResponse = StartApplicationCreationProcess();

                //var receiveThirdPartyInvoiceDomainProcessResponse = StartReceiveThirdPartyInvoiceDomainProcess();

                var thirdPartyInvoicePaymentDomainProcessResponse = StartPayThirdPartyInvoiceDomainProcess();
            }
            catch (Exception runtimeException)
            {
                Console.WriteLine("Runtime Exception occurred\n{0}", runtimeException);
            }

            Console.WriteLine("Domain Process Manager console test application completed");
            Console.ReadLine();
        }

        //private static IStartDomainProcessResponse StartApplicationCreationProcess()
        //{
        //    var applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
        //    applicationCreationModel.Applicants.First().ApplicantAssetLiabilities = ApplicationCreationTestHelper.PopulateApplicantAssetLiabilities();

        //    //....test error logging...
        //    //applicationCreationModel.Applicants.First().BankAccounts = new System.Collections.Generic.List<BankAccountModel>{
        //    //    new BankAccountModel(string.Empty,"",string.Empty,ACBType.Bond,"","System",true)
        //    //};

        //    var domainProcessResponse = new DomainProcessManagerClientApi(_domainProcessManagerClient)
        //                                        .DataModel(applicationCreationModel)
        //                                        .EventToWaitFor(typeof(NewPurchaseApplicationAddedEvent).Name)
        //                                        .StartProcess().Result;

        //    return domainProcessResponse;
        //}

        private static IStartDomainProcessResponse StartReceiveThirdPartyInvoiceDomainProcess()
        {
            // start the required domain process
            var dataModel = new ReceiveAttorneyInvoiceProcessModel(
                                  1235894
                                , DateTime.Now
                                , "alexm@sahomeloans,com"
                                , "1235894- Testing Attorney Invoice Happy Case"
                                , "pdf-sample.pdf"
                                , ".pdf"
                                , string.Empty
                                , "VGhpcyBpcyBhIHRlc3QgYXR0YWNoZW1lbnQ="
                                );

            var domainProcessResponse = new DomainProcessManagerClientApi(_domainProcessManagerClient)
                                                .DataModel(dataModel)
                                                .EventToWaitFor("FireAndForget")
                                                .StartProcess().Result;

            return domainProcessResponse;
        }

        private static IStartDomainProcessResponse StartPayThirdPartyInvoiceDomainProcess()
        {
            // start the required domain process
            var invoices = new List<PayThirdPartyInvoiceModel> { new PayThirdPartyInvoiceModel(
                                              1127
                                            , 8917940
                                            , 1947879
                                            , "SAHL-2015/07/39"
                                            , PaymentProcessStep.PreparingWorkflowCase
                                     )};

            var dataModel = new PayThirdPartyInvoiceProcessModel(invoices, "SAHL\\HaloUser");

            var domainProcessResponse = new DomainProcessManagerClientApi(_domainProcessManagerClient)
                                                .DataModel(dataModel)
                                                .EventToWaitFor("FireAndForget")
                                                .StartProcess().Result;

            return domainProcessResponse;
        }
    }
}