using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData.Statements;
using SAHL.Services.Interfaces.FinanceDomain.Model;

namespace SAHL.Services.FinanceDomain.Specs.Managers.ThirdPartyInvoiceData
{
    public class when_getting_third_party_email_subject : WithFakes
    {
        private static FakeDbFactory dbFactory;
        private static IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;

        private static int thirdPartyInvoiceKey;
        private static string emailSubject;
        private static ThirdPartyEmailSubjectModel expectedEmailSubject;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            thirdPartyInvoiceDataManager = new ThirdPartyInvoiceDataManager(dbFactory);
            emailSubject = "this is the subject";
            expectedEmailSubject = new ThirdPartyEmailSubjectModel() { EmailSubject = "this is the subject" };
            thirdPartyInvoiceKey = 123;

            dbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(y => y.SelectOne(Param.IsAny<GetThirdPartyInvoiceEmailSubjectStatement>()))
                .Return(expectedEmailSubject);
        };

        private Because of = () =>
        {
            emailSubject = thirdPartyInvoiceDataManager.GetThirdPartyInvoiceEmailSubject(thirdPartyInvoiceKey);
        };

        private It should_get_a_third_party_invoice_email_subject = () =>
        {
            emailSubject.ShouldEqual(expectedEmailSubject.EmailSubject);
        };
    }
}