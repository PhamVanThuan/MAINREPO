using Automation.DataAccess;
using Automation.DataAccess.DataHelper;
using BuildingBlocks.Services.Contracts;
using Common.Enums;
using System.Collections.Generic;
using System.Linq;

namespace BuildingBlocks.Services
{
    public class DetailTypeService : _2AMDataHelper, IDetailTypeService
    {
        /// <summary>
        /// Gets a list of the detail types applying the filters required to match how the loan servicing screen displays detail types.
        /// </summary>
        /// <param name="detailClass">detailClass</param>
        /// <param name="status">GeneralStatus</param>
        /// <param name="allowScreen">true = on the screen, false = not on the screen</param>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.DetailType> GetDetailTypesForLoanServicingScreen(DetailClassEnum detailClass,
            GeneralStatusEnum status, bool allowScreen)
        {
            var detailTypes = base.GetLoanDetailTypeRecords();
            var filteredDetailTypes = (from dt in detailTypes
                                       where dt.DetailClassKey == (int)detailClass
                                           && dt.GeneralStatusKey == (int)status && dt.AllowScreen == allowScreen
                                       select dt).AsEnumerable();
            return filteredDetailTypes;
        }

        /// <summary>
        /// Get an empty LoanDetail Record.
        /// </summary>
        /// <returns></returns>
        public Automation.DataModels.LoanDetail GetEmptyDetailRecord()
        {
            var loanDetail = new Automation.DataModels.LoanDetail
                {
                    LoanDetailType = new Automation.DataModels.DetailType { LoanDetailClass = new Automation.DataModels.DetailClass() }
                };
            return loanDetail;
        }

        /// <summary>
        /// Get an existing loandetail record from the database
        /// </summary>
        /// <param name="expectedDetailType"></param>
        /// <param name="accountkey"></param>
        /// <param name="expectedloanDetailClass"></param>
        /// <returns></returns>
        public Automation.DataModels.LoanDetail GetLoanDetailRecord(DetailTypeEnum expectedDetailType, int accountkey,
            DetailClassEnum expectedloanDetailClass)
        {
            var detailTypeRecordCollection = base.GetLoanDetailTypeRecords((int)expectedDetailType);

            //get the detailtype model from the detailtypeclass, if not found return null
            var detailType = (from detailTypeRecord in detailTypeRecordCollection
                              where detailTypeRecord.DetailClassKey == (int)expectedloanDetailClass
                              select detailTypeRecord).FirstOrDefault();
            if (detailType == null)
                return null;

            //Get loandetail from the account
            return (from loandetail in base.GetLoanDetailRecords(accountkey)
                    where loandetail.DetailTypeKey == expectedDetailType
                    select loandetail).FirstOrDefault();
        }

        /// <summary>
        /// Returns an open account that has a specific detail type stored against the account
        /// </summary>
        /// <param name="detailTypeKey">Detail Type Description</param>
        /// <returns>AccountKey</returns>
        public int GetOpenAccountWithDetailType(DetailTypeEnum detailTypeKey)
        {
            QueryResults results = base.GetOpenAccountByDetailType(((int)detailTypeKey));
            return results.Rows(0).Column("AccountKey").GetValueAs<int>();
        }

        /// <summary>
        /// Removes all of the Detail Types on an Account
        /// </summary>
        /// <param name="accountKey">Account Number</param>
        /// <param name="detailTypes">list of detail types to be deleted. This is an optional parameter. Not passing a list of detail types will delete all</param>
        public void RemoveDetailTypes(int accountKey, List<DetailTypeEnum> detailTypes = null)
        {
            if (detailTypes != null)
            {
                foreach (var detailtype in detailTypes)
                {
                    base.RemoveDetailTypes(accountKey, detailtype);
                }
            }
            else
            {
                base.RemoveDetailTypes(accountKey, 0);
            }
        }
    }
}