using System;
using System.Collections.Generic;

namespace SAHL.Services.Interfaces.ITC.Models
{
    public class ItcProfile
    {
        private IEnumerable<ItcJudgement> judgments;
        private IEnumerable<ItcDefault> defaults;
        private IEnumerable<ItcPaymentProfile> paymentProfiles;
        private IEnumerable<ItcNotice> notices;
        private int? empericaScore;

        public ItcProfile(int? empericaScore, IEnumerable<ItcJudgement> judgments, IEnumerable<ItcDefault> defaults, 
                IEnumerable<ItcPaymentProfile> paymentProfiles, IEnumerable<ItcNotice> notices, ItcDebtCounselling debtCounselling, bool creditBureauMatch)
        {
            this.empericaScore = empericaScore;
            this.judgments = judgments;
            this.defaults = defaults;
            this.paymentProfiles = paymentProfiles;
            this.notices = notices;
            this.CreditBureauMatch = creditBureauMatch;
            this.DebtCounselling = debtCounselling;
        }

        public int? EmpericaScore { get { return empericaScore; } }

        public bool CreditBureauMatch { get; private set; }

        public IEnumerable<ItcJudgement> Judgments { get { return judgments; } }

        public IEnumerable<ItcDefault> Defaults { get { return defaults; } }

        public IEnumerable<ItcPaymentProfile> PaymentProfiles { get { return paymentProfiles; } }

        public IEnumerable<ItcNotice> Notices { get { return notices; } }

        public ItcDebtCounselling DebtCounselling { get; private set; }
    }

    public class ItcJudgement
    {
        public ItcJudgement(DateTime judgmentDate, decimal amount)
        {
            this.JudgmentDate = judgmentDate;
            this.Amount = amount;
        }

        public DateTime JudgmentDate { get; protected set; }

        public decimal Amount { get; protected set; }
    }

    public class ItcDefault
    {
        public ItcDefault(DateTime informationDate, decimal defaultAmount)
        {
            this.DefaultAmount = defaultAmount;
            this.InformationDate = informationDate;
        }

        public decimal DefaultAmount { get; protected set; }

        public DateTime InformationDate { get; protected set; }
    }

    public class ItcNotice
    {
        public ItcNotice(DateTime noticeDate, string noticeType, string noticeTypeCode)
        {
            this.NoticeDate = noticeDate;
            this.NoticeType = noticeType;
            this.NoticeTypeCode = noticeTypeCode;
        }

        public DateTime NoticeDate { get; protected set; }

        public string NoticeType { get; protected set; }

        public string NoticeTypeCode { get; protected set; }
    }

    public class ItcDebtCounselling
    {
        public DateTime Date { get; set; }

        public string Code { get; set; }

        public ItcDebtCounselling(DateTime date, string code)
        {
            this.Date = date;
            this.Code = code;
        }
    }

    public class ItcPaymentHistory
    {
        public ItcPaymentHistory(DateTime date, string statusCode)
        {
            this.Date = date;
            this.StatusCode = statusCode;
        }

        public DateTime Date { get; protected set; }

        public string StatusCode { get; protected set; }
    }

    public class ItcPaymentProfile
    {
        public ItcPaymentProfile(string supplierName, DateTime lastUpdateDate)
        {
            this.SupplierName = supplierName;
            this.LastUpdateDate = lastUpdateDate;
            PaymentHistories = new List<ItcPaymentHistory>();
        }

        public string SupplierName { get; protected set; }

        public DateTime LastUpdateDate { get; protected set; }

        public List<ItcPaymentHistory> PaymentHistories { get; protected set; }
    }
}