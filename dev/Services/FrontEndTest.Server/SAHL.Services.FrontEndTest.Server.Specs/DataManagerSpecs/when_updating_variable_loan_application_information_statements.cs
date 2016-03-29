using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.FrontEndTest.Managers.Statements;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_updating_variable_loan_application_information_statements : WithFakes
    {
        private static TestDataManager testDataManager;
        private static FakeDbFactory fakeDb;
        private static OfferInformationVariableLoanDataModel offerInformationLoanVariableDataModel;
        private static int ApplicationInformationKey;
        private static double LoanAmountNoFees;
        private static double LoanAgreementAmount;

        private Establish context = () =>
        {
            fakeDb = new FakeDbFactory();
            testDataManager = new TestDataManager(fakeDb);
            ApplicationInformationKey = 1492075;
            LoanAmountNoFees = 4500000;
            LoanAgreementAmount = 1000000;
            offerInformationLoanVariableDataModel = new OfferInformationVariableLoanDataModel
                                                   (ApplicationInformationKey, Param.IsAny<int>(), Param.IsAny<int>()
                                                   , Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<int>()
                                                   , Param.IsAny<double>(), Param.IsAny<double>(), Param.IsAny<double>(), Param.IsAny<double>()
                                                   , Param.IsAny<double>(), Param.IsAny<double>(), Param.IsAny<double>(), Param.IsAny<double>()
                                                   , Param.IsAny<double>(), Param.IsAny<double>(), Param.IsAny<double>(), LoanAmountNoFees
                                                   , Param.IsAny<double>(), LoanAgreementAmount, Param.IsAny<double>()
                                                   , Param.IsAny<int>(), Param.IsAny<double>(), Param.IsAny<int>(), Param.IsAny<int>()
                                                   , Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<int>());
        };

        private Because of = () =>
        {
            testDataManager.UpdateVariableLoanApplicationInformationStatement(offerInformationLoanVariableDataModel);
        };

        private It should_perform_the_update_using_the_correct_data = () =>
        {
            fakeDb.FakedDb.InAppContext()
                .WasToldTo(x => x.Update<OfferInformationVariableLoanDataModel>
                    (Arg.Is<UpdateVariableLoanApplicationInformationStatement>
                       (y => y.ApplicationInformationKey == offerInformationLoanVariableDataModel.OfferInformationKey
                          && y.LoanAmountNoFees == offerInformationLoanVariableDataModel.LoanAmountNoFees
                            && y.LoanAgreementAmount == offerInformationLoanVariableDataModel.LoanAgreementAmount)));
        };

        private It should_complete_the_db_context = () =>
        {
            fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Complete());
        };
    }
}