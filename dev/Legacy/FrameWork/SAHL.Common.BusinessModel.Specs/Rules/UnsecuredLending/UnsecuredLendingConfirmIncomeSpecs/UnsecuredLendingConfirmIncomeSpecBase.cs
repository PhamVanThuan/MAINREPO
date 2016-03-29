using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Rules.UnsecuredLending.PersonalLoan;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Specs.Rules.UnsecuredLending.UnsecuredLendingConfirmIncomeSpecs
{
    public class UnsecuredLendingConfirmIncomeSpecBase : RulesBaseWithFakes<UnsecuredLendingConfirmIncome>
    {
        protected static IApplicationUnsecuredLending CreateMockedApplicationUnsecuredLending(bool confirmedIncome)
        {
            IEmploymentStatus employmentStatus = An<IEmploymentStatus>();
            employmentStatus.WhenToldTo(x => x.Key).Return((int)EmploymentStatuses.Current);

            IEmployment employment = An<IEmployment>();
            employment.WhenToldTo(x => x.EmploymentStatus).Return(employmentStatus);
            employment.WhenToldTo(x => x.IsConfirmed).Return(confirmedIncome);
            employment.WhenToldTo(x => x.ConfirmedIncomeFlag).Return(confirmedIncome);

            IEventList<IEmployment> employmentList = new EventList<IEmployment>();
            employmentList.Add(null, employment);

            ILegalEntity legalEntity = An<ILegalEntity>();
            legalEntity.WhenToldTo(x => x.Employment).Return(employmentList);

            IExternalRole externalRole = An<IExternalRole>();
            externalRole.WhenToldTo(x => x.ExternalRoleType.Key).Return((int)ExternalRoleTypes.Client);
            externalRole.WhenToldTo(x => x.LegalEntity).Return(legalEntity);

            var externalRoles = new IExternalRole[] { externalRole };

            IReadOnlyEventList<IExternalRole> readOnlyExternalRoles = new ReadOnlyEventList<IExternalRole>(externalRoles);

            var applicationUnsecuredLending = An<IApplicationUnsecuredLending>();
            applicationUnsecuredLending.WhenToldTo(x => x.ActiveClientRoles).Return(readOnlyExternalRoles);

            return applicationUnsecuredLending;
        }
    }
}