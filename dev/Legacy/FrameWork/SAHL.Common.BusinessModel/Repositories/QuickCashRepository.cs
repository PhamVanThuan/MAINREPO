using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using System.Data;
using System;
using SAHL.Common;
using SAHL.Common.Exceptions;
using Castle.ActiveRecord;
//using SAHL.Common.UI;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Validation;
using System.Collections.Generic;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(IQuickCashRepository))]
    public class QuickCashRepository : AbstractRepositoryBase, IQuickCashRepository
    {
        /// <summary>
        /// 
        /// </summary>
        public IApplicationInformationQuickCashDetail CreateEmptyApplicationInformationQuickCashDetail()
        {
			return base.CreateEmpty<IApplicationInformationQuickCashDetail, ApplicationInformationQuickCashDetail_DAO>();
			//return new ApplicationInformationQuickCashDetail(new ApplicationInformationQuickCashDetail_DAO());
        }

        public void SaveApplicationInformationQuickCash(IApplicationInformationQuickCash applicationInformationQuickCash)
        {
			base.Save<IApplicationInformationQuickCash, ApplicationInformationQuickCash_DAO>(applicationInformationQuickCash);
			//ApplicationInformationQuickCash_DAO dao = (ApplicationInformationQuickCash_DAO)(applicationInformationQuickCash as IDAOObject).GetDAOObject();
			//dao.SaveAndFlush();
			//if (ValidationHelper.PrincipalHasValidationErrors())
			//    throw new DomainValidationException();
        }

        public void SaveApplicationInformationQuickCashDetail(IApplicationInformationQuickCashDetail applicationInformationQuickCashDetail)
        {
			base.Save<IApplicationInformationQuickCashDetail, ApplicationInformationQuickCashDetail_DAO>(applicationInformationQuickCashDetail);
			//ApplicationInformationQuickCashDetail_DAO dao = (ApplicationInformationQuickCashDetail_DAO)(applicationInformationQuickCashDetail as IDAOObject).GetDAOObject();
			//dao.SaveAndFlush();
			//if (ValidationHelper.PrincipalHasValidationErrors())
			//    throw new DomainValidationException();
        }

		public IApplicationInformationQuickCashDetail GetApplicationInformationQuickCashDetailByKey(int Key)
        {
			return base.GetByKey<IApplicationInformationQuickCashDetail, ApplicationInformationQuickCashDetail_DAO>(Key);
			//ApplicationInformationQuickCashDetail_DAO qcDetail = ApplicationInformationQuickCashDetail_DAO.Find(quickCashDetailKey);

			//if (qcDetail == null)
			//    return null;

			//return new ApplicationInformationQuickCashDetail(qcDetail);
        }

        public void DeleteApplicationExpense(IApplicationExpense appExpense)
        {
            ApplicationExpense_DAO dao = (ApplicationExpense_DAO)(appExpense as IDAOObject).GetDAOObject();
            dao.DeleteAndFlush();
        }

        public void DeleteApplicationDebtSettlement(IApplicationDebtSettlement appDebtSettlement)
        {
            ApplicationDebtSettlement_DAO dao = (ApplicationDebtSettlement_DAO)(appDebtSettlement as IDAOObject).GetDAOObject();
            dao.DeleteAndFlush();
        }

        public List<IApplicationInformationQuickCashDetail> GetApplicationInformationQuickCashDetailFromDisbursementObj(IDisbursement disbursement)
        {
            using (new SessionScope())
            {
                List<IApplicationInformationQuickCashDetail> aiqcdArray = new List<IApplicationInformationQuickCashDetail>();
                
                string hql = "select AIQCD from ApplicationInformationQuickCashDetail_DAO AIQCD join AIQCD.ApplicationExpenses OES join OES.ApplicationDebtSettlements ADS "
                            + "where ADS.Disbursement.Key = ?";
                SimpleQuery query = new SimpleQuery(typeof(ApplicationInformationQuickCashDetail_DAO), hql, disbursement.Key);
                ApplicationInformationQuickCashDetail_DAO[] aiqcd = ApplicationInformationQuickCashDetail_DAO.ExecuteQuery(query) as ApplicationInformationQuickCashDetail_DAO[];


                foreach (ApplicationInformationQuickCashDetail_DAO item in aiqcd)
                {
                    IApplicationInformationQuickCashDetail obj = new ApplicationInformationQuickCashDetail(ApplicationInformationQuickCashDetail_DAO.Find(item.Key));
                    aiqcdArray.Add(obj);
                }
                return aiqcdArray;
            }
        }

        public List<IApplicationInformationQuickCashDetail> GetApplicationInformationQuickCashDetailByAccountKey(int AccountKey)
        {
            using (new SessionScope())
            {
                List<IApplicationInformationQuickCashDetail> aiqcdArray = new List<IApplicationInformationQuickCashDetail>();

                string hql = "select AIQCD from ApplicationInformationQuickCashDetail_DAO AIQCD join AIQCD.OfferInformationQuickCash OIQ join OIQ.ApplicationInformation AI "
                            + "Join AI.Application A where A.Account.Key = ? and AIQCD.QuickCashPaymentType.Key in (1,2)";
                SimpleQuery query = new SimpleQuery(typeof(ApplicationInformationQuickCashDetail_DAO), hql, AccountKey);
                ApplicationInformationQuickCashDetail_DAO[] aiqcd = ApplicationInformationQuickCashDetail_DAO.ExecuteQuery(query) as ApplicationInformationQuickCashDetail_DAO[];


                foreach (ApplicationInformationQuickCashDetail_DAO item in aiqcd)
                {
                    IApplicationInformationQuickCashDetail obj = new ApplicationInformationQuickCashDetail(ApplicationInformationQuickCashDetail_DAO.Find(item.Key));
                    aiqcdArray.Add(obj);
                }
                return aiqcdArray;
            }
        }

    }
}
