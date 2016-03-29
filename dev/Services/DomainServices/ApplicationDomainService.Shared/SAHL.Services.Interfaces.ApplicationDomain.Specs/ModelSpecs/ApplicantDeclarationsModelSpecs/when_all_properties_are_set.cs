using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.ApplicationDomain.Specs.ModelSpecs.ApplicantDeclarationsModelSpecs
{
    public class when_all_properties_are_set : WithFakes
    {
        private static ApplicantDeclarationsModel model;
        private static DeclaredInsolventDeclarationModel insolvencyDeclaration;
        private static CurrentlyUnderDebtCounsellingReviewDeclarationModel debtCounsellingDeclaration;
        private static UnderAdministrationOrderDeclarationModel adminOrderDeclaration;
        private static PermissionToConductCreditCheckDeclarationModel creditCheckDeclaration;
        private static Exception ex;
        private static int clientKey;
        private static int applicationNumber;

        private Establish context = () =>
        {
            insolvencyDeclaration = new DeclaredInsolventDeclarationModel(OfferDeclarationAnswer.No, null);
            debtCounsellingDeclaration = new CurrentlyUnderDebtCounsellingReviewDeclarationModel(OfferDeclarationAnswer.No, null);
            adminOrderDeclaration = new UnderAdministrationOrderDeclarationModel(OfferDeclarationAnswer.No, null);
            creditCheckDeclaration = new PermissionToConductCreditCheckDeclarationModel(OfferDeclarationAnswer.Yes);
            clientKey = 1;
            applicationNumber = 1408582;
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() =>
            {
                model = new ApplicantDeclarationsModel(clientKey, applicationNumber, DateTime.Now, insolvencyDeclaration, adminOrderDeclaration, debtCounsellingDeclaration, creditCheckDeclaration);
            });
        };

        private It should_not_throw_a_validation_exception = () =>
        {
            ex.ShouldBeNull();
        };

        private It should_construct_the_model = () =>
        {
            model.ShouldNotBeNull();
        };
    }
}
