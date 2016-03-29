using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;


namespace SAHL.Common.BusinessModel.Rules.Employer
{

    [RuleDBTag("EmployerUniqueName",
        "An employer must have a unique name",
        "SAHL.Rules.DLL",
      "SAHL.Common.BusinessModel.Rules.Employer.EmployerUniqueName")]
    [RuleInfo]
    public class EmployerUniqueName : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection messages, params object[] Parameters)
        {
            IEmployer employer = (IEmployer)Parameters[0];

            // if no employer name set, just exit out of here and let the mandatory rule do it's work
            if (String.IsNullOrEmpty(employer.Name))
                return 1;

            IEmploymentRepository repo = RepositoryFactory.GetRepository<IEmploymentRepository>();
            IReadOnlyEventList<IEmployer> employerlist = repo.GetEmployers(employer.Name);

            // loop through the list and if there is another record other than the current one, then exit
            foreach (IEmployer emp in employerlist)
            {
                if (emp.Key != employer.Key)
                {
                    AddMessage("Employer already exists", "", messages);
                    return 0;
                }

            }

            return 1;
        }
    }

    [RuleDBTag("EmployerContactEmailValidation",
        "Employer email address must be valid",
        "SAHL.Rules.DLL",
 "SAHL.Common.BusinessModel.Rules.Employer.EmployerContactEmailValidation")]
    [RuleInfo]
    public class EmployerContactEmailValidation : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection messages, params object[] Parameters)
        {
            IEmployer employer = (IEmployer)Parameters[0];

            string emailAddr = employer.ContactEmail;

            if (!String.IsNullOrEmpty(emailAddr))
            {
                if (!CommonValidation.IsEmail(emailAddr))
                    AddMessage("Employer contact email address is not in the correct format!", "", messages);
            }
            return 1;
        }
    }

    [RuleDBTag("EmployerAccountantEmailValidation",
    "Accountant email address must be valid",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Employer.EmployerAccountantEmailValidation")]
    [RuleInfo]
    public class EmployerAccountantEmailValidation : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection messages, params object[] Parameters)
        {
            IEmployer employer = (IEmployer)Parameters[0];

            string emailAddr = employer.AccountantEmail;

            if (!String.IsNullOrEmpty(emailAddr))
            {
                if (!CommonValidation.IsEmail(emailAddr))
                    AddMessage("Accountant email address is not in the correct format!", "", messages);
            }
            return 1;
        }
    }


    [RuleDBTag("EmployerAccountantPhoneNumberAndCodeValidation",
        "If an account phone number is entered, a number must be entered, and vice-versa",
        "SAHL.Rules.DLL",
  "SAHL.Common.BusinessModel.Rules.Employer.EmployerAccountantPhoneNumberAndCodeValidation")]
    [RuleInfo]
    public class EmployerAccountantPhoneNumberAndCodeValidation : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection messages, params object[] Parameters)
        {
            IEmployer employer = (IEmployer)Parameters[0];

            if (!String.IsNullOrEmpty(employer.AccountantTelephoneCode) && String.IsNullOrEmpty(employer.AccountantTelephoneNumber))
            {
                AddMessage("An accountant phone number is required", "", messages);                   
            }

            if (String.IsNullOrEmpty(employer.AccountantTelephoneCode) && (!String.IsNullOrEmpty(employer.AccountantTelephoneNumber)))
            {
                AddMessage("An Accountant phone code is required", "", messages);                   
            }

            return 1;
        }
    }


}
