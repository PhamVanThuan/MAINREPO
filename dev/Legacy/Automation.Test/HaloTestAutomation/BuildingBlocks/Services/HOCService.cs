using Automation.DataAccess;
using Automation.DataAccess.DataHelper;
using BuildingBlocks.Services.Contracts;
using Common.Enums;
using NUnit.Framework;
using System.Linq;

namespace BuildingBlocks.Services
{
    public class HOCService : _2AMDataHelper, IHOCService
    {
        public Automation.DataModels.HOCAccount GetHOCAccountDetails(int accountKey, int relatedMortgageLoanAccount = 0)
        {
            //we have passed in the related mortgage loan account and we need to find the HOC Account Number
            if (relatedMortgageLoanAccount != 0)
            {
                var HOC = base.GetOpenRelatedAccountsByProductKey(relatedMortgageLoanAccount, ProductEnum.HomeOwnersCover);
                accountKey = (from h in HOC select h.Column("RelatedAccountKey").GetValueAs<int>()).FirstOrDefault();
            }
            return base.GetHOCAccount(accountKey);
        }

        /// <summary>
        /// Returns a true/false depending on whether or not a Mortgage Loan account has an open HOC
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public bool MortgageLoanAccountHasOpenHOC(int accountKey)
        {
            Automation.DataModels.HOCAccount HOCAccount;
            QueryResults accRelationship;
            bool SAHLHOCExists = false;
            accRelationship = GetOpenRelatedAccountsByProductKey(accountKey, ProductEnum.HomeOwnersCover);
            if (accRelationship.HasResults)
            {
                HOCAccount = GetHOCAccountDetails(accRelationship.Rows(0).Column("RelatedAccountKey").GetValueAs<int>());
                SAHLHOCExists = HOCAccount.HOCInsurerKey == (int)HOCInsurerEnum.SAHLHOC ? true : false;
                return SAHLHOCExists;
            }
            else
            {
                return SAHLHOCExists;
            }
        }

        public Automation.DataModels.HOCAccount GetHOCAccountByPropertyKey(int propertyKey)
        {
            var hocAccountKey = 0;
            var acc = base.GetAccountByPropertyKey(propertyKey, AccountStatusEnum.Open, OriginationSourceEnum.SAHomeLoans);
            if (acc == null)
            {
                var offerKey = base.GetOfferByPropertyKey(propertyKey);
                hocAccountKey = base.GetHOCForOffer(offerKey);
                if (hocAccountKey == 0)
                    return null;
            }
            else
            {
                hocAccountKey = (from r in base.GetOpenRelatedAccountsByProductKey(acc.AccountKey, ProductEnum.HomeOwnersCover)
                                 select r.Column("accountkey").GetValueAs<int>()).FirstOrDefault();
            }
            return base.GetHOCAccount(hocAccountKey);
        }
    }
}