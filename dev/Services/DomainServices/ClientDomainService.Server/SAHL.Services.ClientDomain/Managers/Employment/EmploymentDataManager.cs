using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.ClientDomain.Managers.Employment.Statements;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Collections.Generic;
using SAHL.Core.BusinessModel.Enums;
using System.Linq;

namespace SAHL.Services.ClientDomain.Managers
{
    public class EmploymentDataManager : IEmploymentDataManager
    {
        private IDbFactory dbFactory;

        public EmploymentDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public int SaveSalaryDeductionEmployment(int clientKey, SalaryDeductionEmploymentModel model)
        {
            double? medicalAid = 0;
            double? PAYE = 0;
            double? pensionProvident = 0;
            double? UIF = 0;
            if (model.EmployeeDeductions != null && model.EmployeeDeductions.Any())
            {
                medicalAid = model.EmployeeDeductions.Where(x => x.Type == EmployeeDeductionTypeEnum.MedicalAid).Select(y => y.Value).FirstOrDefault();
                PAYE = model.EmployeeDeductions.Where(x => x.Type == EmployeeDeductionTypeEnum.PayAsYouEarnTax).Select(y => y.Value).FirstOrDefault();
                pensionProvident = model.EmployeeDeductions.Where(x => x.Type == EmployeeDeductionTypeEnum.PensionOrProvidendFund).Select(y => y.Value).FirstOrDefault();
                UIF = model.EmployeeDeductions.Where(x => x.Type == EmployeeDeductionTypeEnum.UnemploymentInsurance).Select(y => y.Value).FirstOrDefault();
            }
            return SaveEmploymentBase(clientKey, model, PAYE, UIF, pensionProvident, medicalAid);
        }

        public int SaveUnemployedEmployment(int clientKey, UnemployedEmploymentModel unemployedEmployement)
        {
            return SaveEmploymentBase(clientKey, unemployedEmployement, 0, 0, 0, 0);
        }

        public int SaveSalariedEmployment(int clientKey, SalariedEmploymentModel salariedEmployment)
        {
            double? medicalAid = 0;
            double? PAYE = 0;
            double? pensionProvident = 0;
            double? UIF = 0;
            if (salariedEmployment.EmployeeDeductions != null && salariedEmployment.EmployeeDeductions.Any())
            {
                medicalAid = salariedEmployment.EmployeeDeductions.Where(x => x.Type == EmployeeDeductionTypeEnum.MedicalAid).Select(y => y.Value).FirstOrDefault();
                PAYE = salariedEmployment.EmployeeDeductions.Where(x => x.Type == EmployeeDeductionTypeEnum.PayAsYouEarnTax).Select(y => y.Value).FirstOrDefault();
                pensionProvident = salariedEmployment.EmployeeDeductions.Where(x => x.Type == EmployeeDeductionTypeEnum.PensionOrProvidendFund).Select(y => y.Value).FirstOrDefault();
                UIF = salariedEmployment.EmployeeDeductions.Where(x => x.Type == EmployeeDeductionTypeEnum.UnemploymentInsurance).Select(y => y.Value).FirstOrDefault();
            }
            return SaveEmploymentBase(clientKey, salariedEmployment, PAYE, UIF, pensionProvident, medicalAid);
        }

        public IEnumerable<EmployerDataModel> FindExistingEmployer(EmployerModel employerModel)
        {
            IEnumerable<EmployerDataModel> employers;
            var employerQuery = new GetEmployerStatement(employerModel);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                employers = db.Select<EmployerDataModel>(employerQuery);
            }

            return employers;
        }

        public int SaveEmployer(EmployerModel employerModel)
        {
            int employerKey;
            //TODO: confirm accountant info and userId

            var employerDataModel = new EmployerDataModel(employerModel.EmployerName, employerModel.TelephoneNumber,
                employerModel.TelephoneCode, employerModel.ContactPerson, employerModel.ContactEmail,
                string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, (int)employerModel.EmployerBusinessType,
                string.Empty, DateTime.Now, (int)employerModel.EmploymentSector);

            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<EmployerDataModel>(employerDataModel);
                db.Complete();
                employerKey = employerDataModel.EmployerKey;
            }

            return employerKey;
        }

        private int SaveEmploymentBase(int clientKey, dynamic employment, double? PAYE, double? UIF, double? pensionProvident, double? medicalAid)
        {
            double salary = employment.EmploymentType == EmploymentType.SalariedwithDeduction ? employment.BasicIncome + employment.HousingAllowance : employment.BasicIncome;
            var newEmployment = new EmploymentDataModel(employment.Employer.EmployerKey, (int)employment.EmploymentType, (int)employment.RemunerationType, (int)employment.EmploymentStatus,
                            clientKey, employment.StartDate, null, null, null, null, null, null, null, null, null, salary, null, null, null, null, null, PAYE, UIF, pensionProvident,
                            medicalAid, null, null, null, null, null, null, null, null, null, null, null, salary, 0, false, false, null, employment.SalaryPaymentDay, null);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<EmploymentDataModel>(newEmployment);
                db.Complete();
                return newEmployment.EmploymentKey;
            }
        }

    }
}