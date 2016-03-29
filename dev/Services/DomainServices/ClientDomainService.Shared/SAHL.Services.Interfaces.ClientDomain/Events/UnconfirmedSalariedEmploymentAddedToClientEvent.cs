using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using System;

namespace SAHL.Services.Interfaces.ClientDomain.Events
{
    public class UnconfirmedSalariedEmploymentAddedToClientEvent : Event
    {
        public UnconfirmedSalariedEmploymentAddedToClientEvent(DateTime date, int clientKey, double basicIncome, DateTime startDate,
            EmploymentStatus EmploymentStatus, int salaryPaymentDay, string employerName, string employerPhoneCode, string employerPhone, string employerContactPerson,
            string employerContactEmail, EmployerBusinessType employerBusinessType, EmploymentSector employmentSector, int employmentKey)
            : base(date)
        {
            this.ClientKey = clientKey;
            this.EmploymentType = EmploymentType.Salaried;
            this.BasicIncome = basicIncome;

            this.StartDate = startDate;
            this.EmploymentStatus = EmploymentStatus;
            this.SalaryPaymentDay = salaryPaymentDay;

            this.EmployerName = employerName;
            this.EmployerPhone = employerPhone;
            this.EmployerPhoneCode = employerPhoneCode;
            this.EmployerContactPerson = employerContactPerson;
            this.EmployerContactEmail = employerContactEmail;
            this.EmployerBusinessType = employerBusinessType;
            this.EmploymentSector = EmploymentSector;
            this.EmploymentKey = employmentKey;
        }

        public int ClientKey { get; protected set; }

        public SalaryDeductionRemunerationType RemunerationType { get; protected set; }

        public EmploymentType EmploymentType { get; protected set; }

        public double BasicIncome { get; protected set; }

        public DateTime StartDate { get; protected set; }

        public EmploymentStatus EmploymentStatus { get; protected set; }

        public int SalaryPaymentDay { get; protected set; }

        public string EmployerName { get; protected set; }

        public string EmployerPhone { get; protected set; }

        public string EmployerPhoneCode { get; protected set; }

        public string EmployerContactPerson { get; protected set; }

        public string EmployerContactEmail { get; protected set; }

        public EmployerBusinessType EmployerBusinessType { get; protected set; }

        public EmploymentSector EmploymentSector { get; protected set; }

        public int EmploymentKey { get; protected set; }
    }
}