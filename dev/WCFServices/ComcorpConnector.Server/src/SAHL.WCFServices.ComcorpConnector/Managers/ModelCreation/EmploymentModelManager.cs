using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation
{
    public class EmploymentModelManager : IEmploymentModelManager
    {
        private IValidationUtils validationUtils;

        public EmploymentModelManager(IValidationUtils validationUtils)
        {
            this.validationUtils = validationUtils;
        }

        public List<EmploymentModel> PopulateEmployment(Applicant comcorpApplicant)
        {
            List<EmploymentModel> employments = new List<EmploymentModel>();

            EmployerBusinessType employerBusinessType = !String.IsNullOrWhiteSpace(comcorpApplicant.EmployerBusinessType) 
                ? validationUtils.ParseEnum<EmployerBusinessType>(comcorpApplicant.EmployerBusinessType) 
                : EmployerBusinessType.Unknown;
            EmploymentSector employmentSector = !String.IsNullOrWhiteSpace(comcorpApplicant.EmploymentSector) 
                ? validationUtils.ParseEnum<EmploymentSector>(comcorpApplicant.EmploymentSector) 
                : EmploymentSector.Other;
            EmployerModel employerModel = new EmployerModel(null, comcorpApplicant.EmployerName, null, null, null, comcorpApplicant.EmployerEmail, employerBusinessType, employmentSector);

            EmploymentType employmentType = validationUtils.ParseEnum<EmploymentType>(comcorpApplicant.CurrentEmploymentType);

            double basicIncome = Convert.ToDouble(comcorpApplicant.EmployerGrossMonthlySalary);
            List<EmployeeDeductionModel> employeeDeductions = PopulateEmployeeDeductions(comcorpApplicant);
            switch (employmentType)
            {
                case EmploymentType.Salaried:
                    SalariedEmploymentModel salariedEmploymentModel = new SalariedEmploymentModel(basicIncome, comcorpApplicant.DateSalaryPaid, employerModel, comcorpApplicant.DateJoinedEmployer,
                        SalariedRemunerationType.Salaried, EmploymentStatus.Current, employeeDeductions);
                    employments.Add(salariedEmploymentModel);
                    break;
                case EmploymentType.SalariedwithDeduction:
                    SalaryDeductionEmploymentModel salaryDeductionEmploymentModel = new SalaryDeductionEmploymentModel(basicIncome, 0, comcorpApplicant.DateSalaryPaid, employerModel,
                        comcorpApplicant.DateJoinedEmployer, SalaryDeductionRemunerationType.Salaried, EmploymentStatus.Current, employeeDeductions);
                    employments.Add(salaryDeductionEmploymentModel);
                    break;
                case EmploymentType.Unemployed:
                    UnemployedRemunerationType remunerationType;
                    var validRemunerations = ((IList<UnemployedRemunerationType>)Enum.GetValues(typeof(UnemployedRemunerationType))).Select(x => x.ToString());
                    remunerationType = PopulateUnemployedRemunerationType(comcorpApplicant, validRemunerations);
                    UnemployedEmploymentModel unEmployedEmploymentModel = new UnemployedEmploymentModel(basicIncome, comcorpApplicant.DateSalaryPaid, employerModel, 
                        comcorpApplicant.DateJoinedEmployer, remunerationType, EmploymentStatus.Current);
                    employments.Add(unEmployedEmploymentModel);
                    break;
                default:
                    break;
            }

            return employments;
        }

        private UnemployedRemunerationType PopulateUnemployedRemunerationType(Applicant comcorpApplicant, IEnumerable<string> validRemunerations)
        {
            UnemployedRemunerationType remunerationType;
            if (validRemunerations.Contains(comcorpApplicant.EmployerRemunerationType))
            {
                remunerationType = validationUtils.ParseEnum<UnemployedRemunerationType>(comcorpApplicant.EmployerRemunerationType);
            }
            else
            {
                remunerationType = UnemployedRemunerationType.Unknown;
            }
            return remunerationType;
        }

        private List<EmployeeDeductionModel> PopulateEmployeeDeductions(Applicant comcorpApplicant)
        {
            return new List<EmployeeDeductionModel>() { 
                new EmployeeDeductionModel(EmployeeDeductionTypeEnum.MedicalAid, Convert.ToDouble(comcorpApplicant.EmployeeMedicalAid)),
                new EmployeeDeductionModel(EmployeeDeductionTypeEnum.PayAsYouEarnTax, Convert.ToDouble(comcorpApplicant.EmployeePAYE)),
                new EmployeeDeductionModel(EmployeeDeductionTypeEnum.PensionOrProvidendFund, Convert.ToDouble(comcorpApplicant.EmployeePension)),
                new EmployeeDeductionModel(EmployeeDeductionTypeEnum.UnemploymentInsurance, Convert.ToDouble(comcorpApplicant.EmployeeUIF))
            };
        }
    }
}