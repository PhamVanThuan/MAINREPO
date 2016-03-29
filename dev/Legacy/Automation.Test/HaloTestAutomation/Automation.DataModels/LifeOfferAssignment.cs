using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automation.DataModels
{
    public class LifeOfferAssignment
    {
        public int LifeOfferAssignmentKey {get;set;}
        public int OfferKey{get;set;}
        public int LoanAccountKey{get;set;}
        public int LoanOfferKey{get;set;}
        public OfferTypeEnum LoanOfferTypeKey{get;set;}
        public DateTime DateAssigned{get;set;}
        public string ADUserName { get; set; }
    }
}
