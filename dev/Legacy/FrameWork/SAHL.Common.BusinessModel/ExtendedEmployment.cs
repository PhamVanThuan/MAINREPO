using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    // lurk remunerationin DAO
    // add it back in the discriminated (2) file
    // make an extended employe obj
    // in the set if salaries / basic + comm make a new extended emp obj else it will be null
    public class ExtendedEmployment : IExtendedEmployment
    {

        private Employment_DAO _dao;
        private IEmployment _employment;

        public ExtendedEmployment(IEmployment employment)
        {
            _employment = employment;
            _dao = (Employment_DAO)(employment as IDAOObject).GetDAOObject();
        }

        /// <summary>
        /// Implements <see cref="IExtendedEmployment.ConfirmedVariableIncome"/>.
        /// </summary>
        public double ConfirmedVariableIncome
        {
            get
            {
                return (ConfirmedCommission ?? 0D) +
                    (ConfirmedOvertime ?? 0D) +
                    (ConfirmedShift ?? 0D) +
                    (ConfirmedPerformance ?? 0D) +
                    (ConfirmedAllowances ?? 0D);
            }
        }

        /// <summary>
        /// Implements <see cref="IExtendedEmployment.VariableIncome"/>.
        /// </summary>
        public double VariableIncome
        {
            get
            {
                return (Commission ?? 0D) +
                    (Overtime ?? 0D) +
                    (Shift ?? 0D) +
                    (Performance ?? 0D) +
                    (Allowances ?? 0D);
            }
        }

        /// <summary>
        /// Implements <see cref="IExtendedEmployment.Deductions"/>.
        /// </summary>
        public double Deductions
        {
            get
            {
                return (PAYE ?? 0D) +
                    (UIF ?? 0D) +
                    (PensionProvident ?? 0D) +
                    (MedicalAid ?? 0D);
            }
        }

        /// <summary>
        /// Implements <see cref="IExtendedEmployment.ConfirmedDeductions"/>.
        /// </summary>
        public double ConfirmedDeductions
        {
            get
            {
                return (ConfirmedPAYE ?? 0D) +
                    (ConfirmedUIF ?? 0D) +
                    (ConfirmedPensionProvident ?? 0D) +
                    (ConfirmedMedicalAid ?? 0D);
            }
        }

        public int? SalaryPayDay
        {
            get
            {
                return _dao.SalaryPaymentDay;
            }
        }

        /// <summary>
        /// Implements <see cref="IExtendedEmployment.NetIncome"/>.
        /// </summary>
        public double NetIncome
        {
            get
            {
                return _employment.MonthlyIncome - Deductions;
            }
        }

        /// <summary>
        /// Implements <see cref="IExtendedEmployment.ConfirmedNetIncome"/>.
        /// </summary>
        public double ConfirmedNetIncome
        {
            get
            {
                return _employment.ConfirmedIncome - ConfirmedDeductions;
            }
        }

        public double? Commission
        {
            get { return _dao.Commission; }
            set { _dao.Commission = value; }
        }

        public double? Overtime
        {
            get { return _dao.Overtime; }
            set { _dao.Overtime = value; }
        }

        public double? Shift
        {
            get { return _dao.Shift; }
            set { _dao.Shift = value; }
        }

        public double? Performance
        {
            get
            {
                return _dao.Performance;
            }
            set { _dao.Performance = value; }
        }

        public double? Allowances
        {
            get { return _dao.Allowances; }
            set { _dao.Allowances = value; }
        }

        public double? PAYE
        {
            get { return _dao.PAYE; }
            set { _dao.PAYE = value; }
        }

        public double? UIF
        {
            get { return _dao.UIF; }
            set { _dao.UIF = value; }
        }

        public double? PensionProvident
        {
            get { return _dao.PensionProvident; }
            set { _dao.PensionProvident = value; }
        }

        public double? MedicalAid
        {
            get { return _dao.MedicalAid; }
            set { _dao.MedicalAid = value; }
        }

        public double? ConfirmedCommission
        {
            get { return _dao.ConfirmedCommission; }
            set { _dao.ConfirmedCommission = value; }
        }
        public double? ConfirmedOvertime
        {
            get { return _dao.ConfirmedOvertime; }
            set { _dao.ConfirmedOvertime = value; }
        }
        public double? ConfirmedShift
        {
            get { return _dao.ConfirmedShift; }
            set { _dao.ConfirmedShift = value; }
        }
        public double? ConfirmedPerformance
        {
            get { return _dao.ConfirmedPerformance; }
            set { _dao.ConfirmedPerformance = value; }
        }
        public double? ConfirmedAllowances
        {
            get { return _dao.ConfirmedAllowances; }
            set { _dao.ConfirmedAllowances = value; }
        }
        public double? ConfirmedPAYE
        {
            get { return _dao.ConfirmedPAYE; }
            set { _dao.ConfirmedPAYE = value; }
        }
        public double? ConfirmedUIF
        {
            get { return _dao.ConfirmedUIF; }
            set { _dao.ConfirmedUIF = value; }
        }
        public double? ConfirmedPensionProvident
        {
            get { return _dao.ConfirmedPensionProvident; }
            set { _dao.ConfirmedPensionProvident = value; }
        }
        public double? ConfirmedMedicalAid
        {
            get { return _dao.ConfirmedMedicalAid; }
            set { _dao.ConfirmedMedicalAid = value; }
        }
    }
}
