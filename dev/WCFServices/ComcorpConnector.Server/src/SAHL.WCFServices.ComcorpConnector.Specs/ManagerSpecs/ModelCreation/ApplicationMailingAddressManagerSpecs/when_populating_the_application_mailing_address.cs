using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Testing;
using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.ModelCreation.ApplicationMailingAddressManagerSpecs
{
    public class when_populating_the_application_mailing_address : WithCoreFakes
    {
        private static ApplicationMailingAddressModelManager modelManager;
        private static ApplicationMailingAddressModel applicationMailingAddress;
        private static ResidentialAddressModel address;
        private static int clientKey;

        private Establish context = () =>
        {
            clientKey = 5;
            address = new ResidentialAddressModel("21", "Somserset Drive", "9", "Camberwell",
                string.Empty, "Somerset Park", "Umhlanga", "Kwazulu-Natal", "4319", "South Africa",
                true);
            modelManager = new ApplicationMailingAddressModelManager();
        };

        private Because of = () =>
        {
            applicationMailingAddress = modelManager.PopulateApplicationMailingAddress(address, clientKey);
        };

        private It should_use_the_address_provided = () =>
        {
            applicationMailingAddress.Address.ShouldBeTheSameAs(address);
        };

        private It should_use_the_client_provided = () =>
        {
            applicationMailingAddress.ClientToUseForEmailCorrespondence.ShouldEqual(clientKey);
        };

        private It should_default_the_correspondence_mediuem_to_email = () =>
        {
            applicationMailingAddress.CorrespondenceMedium.ShouldEqual(CorrespondenceMedium.Email);
        };

        private It should_default_the_online_statement_required_to_false = () =>
        {
            applicationMailingAddress.OnlineStatementRequired.ShouldBeFalse();
        };

        private It should_default_the_online_statement_fornat_to_not_applicable = () =>
        {
            applicationMailingAddress.OnlineStatementFormat.ShouldEqual(OnlineStatementFormat.NotApplicable);
        };
    }
}