using SAHL.Common.BusinessModel.Helpers;
using SAHL.Core.SystemMessages;
using SAHL.DecisionTree.Shared;
using SAHL.Services.Interfaces.DecisionTree.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.V3.Framework.Model
{
    public class QualifyApplicationFor30YearLoanTermResult
    {
        public QualifyApplicationFor30YearLoanTermResult(ApplicationLoanDetail loanDetailForCurrentTerm, ApplicationLoanDetail loanDetailFor30YearTerm, bool qualifiesForThirtyYearTerm, double pricingAdjustmentThirtyYear, ISystemMessageCollection messages)
        {
            this.LoanDetailForCurrentTerm = loanDetailForCurrentTerm;
            this.LoanDetailFor30YearTerm = loanDetailFor30YearTerm;
            this.QualifiesForThirtyYearTerm = qualifiesForThirtyYearTerm;
            this.PricingAdjustmentThirtyYear = pricingAdjustmentThirtyYear;
            this.Messages = messages;
        }

        public ApplicationLoanDetail LoanDetailForCurrentTerm { get; private set; }

        public ApplicationLoanDetail LoanDetailFor30YearTerm { get; private set; }

        public bool QualifiesForThirtyYearTerm { get; private set; }

        public double PricingAdjustmentThirtyYear { get; private set; }
        
        public ISystemMessageCollection Messages { get; private set; }
    }
}
