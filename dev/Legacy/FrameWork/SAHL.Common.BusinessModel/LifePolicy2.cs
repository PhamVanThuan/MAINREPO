using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using System.Linq;
using SAHL.Common.BusinessModel.Base;

namespace SAHL.Common.BusinessModel
{
    public partial class LifePolicy : BusinessModelBase<SAHL.Common.BusinessModel.DAO.LifePolicy_DAO>, ILifePolicy
    {
        public double CurrentYearlyBalance
        {
            get
            {
                return this.FinancialService.Balance.Amount;
            }
        }

        public double CurrentArrearBalance
        {
            get
            {
                var arrearBalance = (from fs in this.FinancialService.FinancialServices
                                     where fs.FinancialServiceType.Key == (int)FinancialServiceTypes.ArrearBalance
                                     && fs.Balance.BalanceType.Key == (int)BalanceTypes.Arrear
                                     select fs.Balance.Amount).Sum();
                return arrearBalance;
            }
        }

        public double MonthlyPremium
        {
            get
            {
                ILifeRepository lifeRepo = RepositoryFactory.GetRepository<ILifeRepository>();
                return lifeRepo.GetMonthlyPremium(this.FinancialService.Account.Key);
            }
        }

        protected void OnLifeEventLogs_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnLifeEventLogs_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnLifePremiumForecasts_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnLifePremiumForecasts_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnLifeSumAssuredHistories_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnLifeSumAssuredHistories_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnLifeEventLogs_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnLifeEventLogs_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnLifePremiumForecasts_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnLifePremiumForecasts_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnLifeSumAssuredHistories_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnLifeSumAssuredHistories_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }
    }
}



