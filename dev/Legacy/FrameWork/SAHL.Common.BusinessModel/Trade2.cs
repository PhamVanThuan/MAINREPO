using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Security;
using SAHL.Common.DomainMessages;
using SAHL.Common.CacheData;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// 
	/// </summary>
	public partial class Trade : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Trade_DAO>, ITrade
	{

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Rules"></param>
        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);
            Rules.Add("TradeCompanyMandatory");
            Rules.Add("TradeCAPMandatory");
            Rules.Add("TradeStrikeRateMax");
            Rules.Add("TradeStrikeRateMin");
            Rules.Add("TradeTermMin");
            Rules.Add("TradeCAPTermMax");
            Rules.Add("TradeTradeBalanceMin");
            Rules.Add("TradeTradePremiumMin");
            Rules.Add("TradeTradeDate");
            Rules.Add("TradeCAPCommencement");
            Rules.Add("TradePremiumMandatory");
            Rules.Add("TradeValidateEffectiveDate");
        }

        /// <summary>
        /// 
        /// </summary>
        public ICapType CapType
        {
            get
            {
                if (null == _DAO.CapType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ICapType, CapType_DAO>(_DAO.CapType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.CapType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                {
                    //can not change the cap type if the cap balance is greater than zero
                    if (_DAO.CapType != null && _DAO.CapBalance > 0)
                    {
                        CapType_DAO capType = (CapType_DAO)obj.GetDAOObject();
                        if (capType.Key != _DAO.CapType.Key)
                        {
                            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
                            IDomainMessageCollection dmc = spc.DomainMessages;
                            dmc.Add(new Error("The Cap Type can not be updated when the Cap Balance is greater than R 0.00", ""));
                        }
                    }
                    else
                    {
                        _DAO.CapType = (CapType_DAO)obj.GetDAOObject();
                    }
                }
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}


