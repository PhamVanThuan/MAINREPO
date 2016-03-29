using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.SharedServices.Common.DoCreditScoreCommandHandlerSpecs
{
    [Subject(typeof(DoCreditScoreCommandHandler))]
    public class When_an_application_exists : DomainServiceSpec<DoCreditScoreCommand, DoCreditScoreCommandHandler>
    {
        protected static ICreditScoringRepository creditScoringRepository;
        protected static IApplicationRepository applicationRepository;
        protected const double highLTV = 0.90;

        Establish context = () =>
        {
            creditScoringRepository = An<ICreditScoringRepository>();
            applicationRepository = An<IApplicationRepository>();

            IApplication application = An<IApplication>();
            ICallingContext callingContext = An<ICallingContext>();
            IApplicationCreditScore applicationCreditScrore = An<IApplicationCreditScore>();

            // application product
            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);

            // risk matrix
            creditScoringRepository.WhenToldTo(x => x.GetCallingContextByKey(Param<int>.IsAnything)).Return(callingContext);
            creditScoringRepository.WhenToldTo(x => x.GenerateApplicationCreditScore(Param<IApplication>.IsAnything, Param<ICallingContext>.IsAnything, Param<string>.IsAnything)).Return(applicationCreditScrore);

            command = new DoCreditScoreCommand(Param<int>.IsAnything, Param<int>.IsAnything, Param<string>.IsAnything, Param<bool>.IsAnything);
            handler = new DoCreditScoreCommandHandler(creditScoringRepository, applicationRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_generate_credit_score = () =>
        {
            creditScoringRepository.WasToldTo(x => x.GenerateApplicationCreditScore(Param<IApplication>.IsAnything, Param<ICallingContext>.IsAnything, Param<string>.IsAnything));
        };
    }
}