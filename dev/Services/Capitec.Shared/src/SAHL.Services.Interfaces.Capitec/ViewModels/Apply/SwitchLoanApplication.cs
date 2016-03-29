using SAHL.Core.Data.Models.Capitec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.Capitec.ViewModels.Apply
{
    public class SwitchLoanApplication
    {
		public SwitchLoanApplication(SwitchLoanDetails switchLoanDetails, IEnumerable<Applicant> applicants, Guid userId, string timestamp)
        {
            this.SwitchLoanDetails = switchLoanDetails;
            this.Applicants = applicants;
            this.UserId = userId;
            this.ApplicationDate = DateTime.Now;
            var timestampMilliSeconds = double.Parse(timestamp.Split('-').First());
            this.CaptureStartDate = new DateTime(1970, 1, 1).AddMilliseconds(timestampMilliSeconds);
        }
        
        public Guid UserId { get; protected set; }

        public SwitchLoanDetails SwitchLoanDetails { get; protected set; }

        public IEnumerable<Applicant> Applicants { get; protected set; }

        public DateTime ApplicationDate { get; protected set; }

        public DateTime CaptureStartDate { get; protected set; }
    }
}
