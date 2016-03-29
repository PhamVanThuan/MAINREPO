using Automation.DataAccess;
using Automation.DataModels;
using Common.Enums;
using System.Collections.Generic;

namespace BuildingBlocks.Services.Contracts
{
    public interface IEmploymentService
    {
        void DeleteEmployer(int employerkey);

        Automation.DataModels.Employer InsertEmployer(Automation.DataModels.Employer employer);

        void DeleteLegalEntityEmployment(int legalEntityKey, EmploymentTypeEnum employmentType);

        Automation.DataModels.Employment GetSubsidisedEmployment();

        void InsertUnconfirmedSalariedEmployment(int legalentitykey);

        void ConfirmAllEmployment(int offerKey);

        void ConfirmSuretorEmployment(int offerKey);

        Automation.DataModels.Employer GetEmployer(string employerName);

        Automation.DataModels.Employer GetFullyPopulatedEmployer();

        Automation.DataModels.SubsidyProvider GetFullyPopulatedSubsidyProvider();

        QueryResults GetEmploymentByCriteria(int legalEntityKey, EmploymentTypeEnum employmentType, RemunerationTypeEnum remunerationType, EmploymentStatusEnum employmentStatus);

        IEnumerable<Employment> GetEmployments(int legalEntityKey, EmploymentStatusEnum employmentStatus);

        QueryResults GetEmploymentByGenericKey(int generickey, bool legalEntityKey, bool employmentKey);

        void DeleteLegalEntityEmployment(int legalEntityKey);

        void UpdateAllEmploymentStatus(int legalEntityKey, EmploymentStatusEnum employmentStatus);

        Automation.DataModels.Employment InsertEmployment(Automation.DataModels.Employment employment);

        void UpdateSalaryPaymentDay(int legalEntityKey, int? salaryPaymentDay);

        void UpdateEmploymentIncome(int offerKey, double income);

    }
}