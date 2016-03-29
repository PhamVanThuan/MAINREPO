using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using offerRole = SAHL.Common.BusinessModel.Interfaces.Repositories.OfferRole;

namespace SAHL.Common.BusinessModel.Specs.Repositories.WorkflowAssignment.IsUserActive
{
    internal class when_user_is_inactive : WorkflowAssignmentRepositoryWithFakesBase
    {
        static bool result;
        Establish context = () =>
        {
            result = false;
            var adUser = dataSet.ADUser.NewADUserRow();
            adUser.GeneralStatusKey = 2;
            adUser.ADUserKey = 1;
            adUser.ADUserName = @"SAHL\ClintonS";
            dataSet.ADUser.AddADUserRow(adUser);
            workflowSecurityRepo.WhenToldTo(x => x.GetOfferRoleOrganisationStructure()).Return(dataSet);
        };

        Because of = () =>
        {
            result = workflowAssignmentRepo.IsUserActive(@"SAHL\ClintonS");
        };

        It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}