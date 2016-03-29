using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.ClientDomain.Specs.Managers.EmploymentDataManagerSpecs
{
    public class when_saving_salary_deduction_employment_with_employee_deductions : WithFakes
    {
        private static EmploymentDataManager employmentDataManager;
        private static FakeDbFactory fakeDbFactory;
        private static SalaryDeductionEmploymentModel salaryDeductionEmployment;
        private static EmployerModel employer;
        private static IEnumerable<EmployeeDeductionModel> deductions;
        private static int clientKey;
        private static double medicalAid;
        private static double paye;
        private static double pension;
        private static double uif;

        private Establish context = () =>
            {
                medicalAid = 1234;
                paye = 5678;
                pension = 91011;
                uif = 987;
                clientKey = 1212;
                employer = new EmployerModel(1, "Google", "031", "571500", "Bob", "test@google.co.za", 
                                            EmployerBusinessType.Company, EmploymentSector.ITandElectronics);
                deductions = new List<EmployeeDeductionModel>() {
                    new EmployeeDeductionModel(EmployeeDeductionTypeEnum.MedicalAid, medicalAid),
                    new EmployeeDeductionModel(EmployeeDeductionTypeEnum.PayAsYouEarnTax, paye),
                    new EmployeeDeductionModel(EmployeeDeductionTypeEnum.PensionOrProvidendFund, pension),
                    new EmployeeDeductionModel(EmployeeDeductionTypeEnum.UnemploymentInsurance, uif)
                };
                salaryDeductionEmployment = new SalaryDeductionEmploymentModel(15000, 5000, 1, employer, DateTime.Now.AddYears(-1),
                                                                               SalaryDeductionRemunerationType.Salaried, 
                                                                               EmploymentStatus.Current, deductions);
                fakeDbFactory = new FakeDbFactory();
                employmentDataManager = new EmploymentDataManager(fakeDbFactory);
            };

        private Because of = () =>
            {
                employmentDataManager.SaveSalaryDeductionEmployment(clientKey, salaryDeductionEmployment);
            };

        private It should_save_the_deductions_to_the_database = () =>
            {
                fakeDbFactory.FakedDb.InAppContext().Received()
                    .Insert(Arg.Is<EmploymentDataModel>(y => y.UIF == uif 
                        && y.PAYE == paye 
                        && y.MedicalAid == medicalAid 
                        && y.PensionProvident == pension
                        ));
            };
    }
}