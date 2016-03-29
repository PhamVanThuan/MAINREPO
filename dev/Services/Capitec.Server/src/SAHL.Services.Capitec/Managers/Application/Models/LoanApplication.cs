using SAHL.Services.Interfaces.Capitec.ViewModels.Apply;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Capitec.Managers.Application.Models
{
    public class LoanApplication
    {
        public LoanApplication(DateTime applicationDate, ApplicationLoanDetails loanDetails, IEnumerable<SAHL.Services.Interfaces.Capitec.ViewModels.Apply.Applicant> applicants, Guid userId, DateTime captureStartTime, Guid branchId)
        {
            this.ApplicationDate = applicationDate;
            this.LoanDetails = loanDetails;
            this.Applicants = applicants;
            this.UserId = userId;
            this.CaptureStartTime = captureStartTime;
            this.BranchId = branchId;
        }

        public DateTime ApplicationDate { get; protected set; }

        public Guid UserId { get; protected set; }

        public ApplicationLoanDetails LoanDetails { get; protected set; }

        public IEnumerable<SAHL.Services.Interfaces.Capitec.ViewModels.Apply.Applicant> Applicants { get; protected set; }
        public DateTime CaptureStartTime { get; protected set; }

        public Guid BranchId { get; set; }
    }
}
