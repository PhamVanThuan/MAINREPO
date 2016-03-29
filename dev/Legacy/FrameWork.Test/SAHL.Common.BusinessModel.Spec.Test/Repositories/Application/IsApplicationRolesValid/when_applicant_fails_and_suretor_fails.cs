using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Common.BusinessModel.Spec.Test.Repositories.Application.IsApplicationRolesValid
{
    [Subject("when_applicant_fails_and_suretor_fails")]
    public class when_applicant_fails_and_suretor_fails : WithFakes
    {
        static IApplication _application;
        static ApplicationRepository _applicationRepo;
        static bool result;

        Establish context = () =>
        {
            result = true;
            _application = An<IApplication>();

            ILegalEntity applicant = An<ILegalEntity>();
            ILegalEntity suretor = An<ILegalEntity>();

            applicant.WhenToldTo(x => x.ValidateEntity()).Return(false);
            suretor.WhenToldTo(x => x.ValidateEntity()).Return(false);

            IApplicationRole applicantRole = An<IApplicationRole>();
            IApplicationRole suretorRole = An<IApplicationRole>();

            IApplicationRoleType mainRoleType = An<IApplicationRoleType>();
            IApplicationRoleType suretorRoleType = An<IApplicationRoleType>();

            mainRoleType.WhenToldTo(x => x.Key).Return((int)SAHL.Common.Globals.OfferRoleTypes.MainApplicant);
            suretorRoleType.WhenToldTo(x => x.Key).Return((int)SAHL.Common.Globals.OfferRoleTypes.Suretor);

            applicantRole.WhenToldTo(x => x.ApplicationRoleType).Return(mainRoleType);
            suretorRole.WhenToldTo(x => x.ApplicationRoleType).Return(suretorRoleType);

            applicantRole.WhenToldTo(x => x.LegalEntity).Return(applicant);
            suretorRole.WhenToldTo(x => x.LegalEntity).Return(suretor);

            IEnumerable<IApplicationRole> enumIApplicationRole = new[] { applicantRole, suretorRole };
            IReadOnlyEventList<IApplicationRole> readOnlyList = new ReadOnlyEventList<IApplicationRole>(enumIApplicationRole);

            _application.WhenToldTo(x => x.ApplicationRoles).Return(readOnlyList);

            _applicationRepo = new ApplicationRepository();
        };

        Because of = () =>
        {
            result = _applicationRepo.IsApplicationRolesValid(_application);
        };

        It should = () =>
        {
            result.ShouldBeFalse();
        };
    }
}
