using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using System;

namespace SAHL.Services.Interfaces.ClientDomain.Events
{
    public class UnconfirmedUnemployedEmploymentAddedToClientEvent : Event
    {
        public UnconfirmedUnemployedEmploymentAddedToClientEvent(DateTime date, int clientKey, double basicIncome, DateTime startDate,
            EmploymentStatus employmentStatus, int salaryPaymentDay, string employerName, int employmentKey)
            : base(date)
        {
            this.ClientKey = clientKey;
            this.EmploymentType = EmploymentType.Salaried;
            this.BasicIncome = basicIncome;
            this.StartDate = startDate;
            this.EmploymentStatus = employmentStatus;
            this.SalaryPaymentDay = salaryPaymentDay;
            this.EmployerName = employerName;
            this.EmploymentKey = employmentKey;
        }

        public int ClientKey { get; protected set; }

        public UnemployedRemunerationType RemunerationType { get; protected set; }

        public EmploymentType EmploymentType { get; protected set; }

        public double BasicIncome { get; protected set; }

        public DateTime StartDate { get; protected set; }

        public EmploymentStatus EmploymentStatus { get; protected set; }

        public int SalaryPaymentDay { get; protected set; }

        public string EmployerName { get; protected set; }

        public int EmploymentKey { get; protected set; }
    }
}