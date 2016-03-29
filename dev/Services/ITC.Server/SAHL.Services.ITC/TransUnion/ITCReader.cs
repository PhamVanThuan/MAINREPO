using SAHL.Services.Interfaces.ITC.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace SAHL.Services.ITC.TransUnion
{
    public class ItcReader
    {
        private XDocument xDocument;
        private XNamespace aw;
        private ItcProfile itcProfile;

        public ItcReader(XDocument itcResponse)
        {
            this.xDocument = itcResponse;
            aw = "https://secure.transunion.co.za/TUBureau";
            var empericaScore = GetEmpericaScore();
            var judgements = GetJudgements();
            var defaults = GetDefaults();
            var notices = GetNotices();
            var paymentProfiles = GetPaymentProfiles();
            var creditMatch = GetCreditMatch();
            var debtCounselling = GetDebtCounselling();
            itcProfile = new ItcProfile(empericaScore, judgements, defaults, paymentProfiles, notices, debtCounselling, creditMatch);
        }

        public ItcProfile GetITCProfile
        {
            get
            {
                return itcProfile;
            }
        }

        private int? GetEmpericaScore()
        {
            //empiricaEM07
            IEnumerable<XElement> empiricaEM07 =
                from el in xDocument.Descendants(aw + "EmpiricaEM07")
                select el;

            foreach (XElement el in empiricaEM07)
            {
                foreach (var item in el.Elements(aw + "EmpiricaScore"))
                {
                    var empiricaScore = Convert.ToInt32(item.Value);
                    return empiricaScore;
                }
            }
            return null;
        }

        private IEnumerable<ItcJudgement> GetJudgements()
        {
            //JudgementsNJ07
            List<ItcJudgement> judgments = new List<ItcJudgement>();
            IEnumerable<XElement> JudgementsNJ07 = xDocument.Descendants(aw + "JudgementsNJ07");

            foreach (XElement el in JudgementsNJ07.Descendants(aw + "JudgementsNJ07"))
            {
                var judgmentDate = DateTime.ParseExact(el.Element(aw + "JudgmentDate").Value, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                var amount = Convert.ToDecimal(el.Element(aw + "Amount").Value);
                judgments.Add(new ItcJudgement(judgmentDate, amount));
            }
            return judgments;
        }

        private IEnumerable<ItcDefault> GetDefaults()
        {
            //DefaultsD701Part1
            List<ItcDefault> defaults = new List<ItcDefault>();
            IEnumerable<XElement> DefaultsD701Part1 = xDocument.Descendants(aw + "DefaultsD701Part1");

            foreach (XElement el in DefaultsD701Part1.Descendants(aw + "DefaultD701Part1"))
            {
                var informationDate = DateTime.ParseExact(el.Element(aw + "InformationDate").Value, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                var defaultAmount = Convert.ToDecimal(el.Element(aw + "DefaultAmount").Value);
                var itcDefault = new ItcDefault(informationDate, defaultAmount);
                defaults.Add(itcDefault);
            }
            return defaults;
        }

        private IEnumerable<ItcNotice> GetNotices()
        {
            //NoticesNN08
            List<ItcNotice> notices = new List<ItcNotice>();
            IEnumerable<XElement> NoticesNN08 = xDocument.Descendants(aw + "NoticesNN08");

            foreach (XElement el in NoticesNN08.Descendants(aw + "NoticesNN08"))
            {
                var noticeDate = DateTime.ParseExact(el.Element(aw + "NoticeDate").Value, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                var noticeType = el.Element(aw + "NoticeType").Value;
                var noticeTypeCode = el.Element(aw + "NoticeTypeCode").Value;
                var notice = new ItcNotice(noticeDate, noticeType, noticeTypeCode);
                notices.Add(notice);
            }
            return notices;
        }

        private IEnumerable<ItcPaymentProfile> GetPaymentProfiles()
        {
            //PaymentProfilesP701
            List<ItcPaymentProfile> paymentProfiles = new List<ItcPaymentProfile>();
            IEnumerable<XElement> PaymentProfilesP701 = xDocument.Descendants(aw + "PaymentProfilesP701");
            foreach (XElement el in PaymentProfilesP701.Descendants(aw + "PaymentProfileP701"))
            {
                var supplierName = el.Element(aw + "SupplierName").Value;
                DateTime lastUpdateDate = DateTime.MinValue;
                if (el.Element(aw + "LastUpdateDate").Value != "00000000")
                {
                    lastUpdateDate = DateTime.ParseExact(el.Element(aw + "LastUpdateDate").Value, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                }
                var profile = GetPaymentProfile(el, supplierName, lastUpdateDate);
                paymentProfiles.Add(profile);
            }
            //NLRAccountsInformationM701
            IEnumerable<XElement> NLRAccountsInformationM701 = xDocument.Descendants(aw + "NLRAccountsInformationM701");
            foreach (XElement el in NLRAccountsInformationM701.Descendants(aw + "NLRAccountInformationM701"))
            {
                var supplierName = el.Element(aw + "SubscriberName").Value;
                DateTime lastUpdateDate = DateTime.MinValue;
                if (el.Element(aw + "LastUpdateDate").Value != "00000000")
                {
                    lastUpdateDate = DateTime.ParseExact(el.Element(aw + "LastUpdateDate").Value, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                }
                var profile = GetPaymentProfile(el, supplierName, lastUpdateDate);
                paymentProfiles.Add(profile);
            }
            return paymentProfiles;
        }

        private ItcPaymentProfile GetPaymentProfile(XElement element, string supplierName, DateTime lastUpdateDate)
        {
            var paymentProfile = new ItcPaymentProfile(supplierName, lastUpdateDate);

            foreach (var paymentHistory in element.Descendants(aw + "PaymentHistories"))
            {
                foreach (var paymentItem in paymentHistory.Descendants(aw + "PaymentHistory"))
                {
                    var date = DateTime.ParseExact(paymentItem.Element(aw + "Date").Value, "yyyyMM", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    var statusCode = paymentItem.Element(aw + "StatusCode").Value;
                    var paymentHistoryItem = new ItcPaymentHistory(date, statusCode);
                    paymentProfile.PaymentHistories.Add(paymentHistoryItem);
                }
            }
            return paymentProfile;
        }

        private bool GetCreditMatch()
        {
            bool creditMatch = false;

            var enquiryInfo = xDocument.Descendants(aw + "ConsEnqTransInfo0102");
            var definiteMatchCount = enquiryInfo.Descendants(aw + "DefiniteMatchCount").FirstOrDefault();
            if (definiteMatchCount != null)
            {
                creditMatch = Int32.Parse(definiteMatchCount.Value) == 1;
            }
            return creditMatch;
        }

        private ItcDebtCounselling GetDebtCounselling()
        {
            ItcDebtCounselling debtCounselling = null;

            var debtCounsellingDC01 = xDocument.Descendants(aw + "DebtCounsellingDC01").FirstOrDefault();
            if (debtCounsellingDC01 != null)
            {
                var dateString = debtCounsellingDC01.Descendants(aw + "DebtCounsellingDate").FirstOrDefault().Value;
                var date = DateTime.ParseExact(dateString, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                var code = debtCounsellingDC01.Descendants(aw + "DebtCounsellingCode").FirstOrDefault().Value;
                debtCounselling = new ItcDebtCounselling(date, code);
            }

            return debtCounselling;
        }
    }
}