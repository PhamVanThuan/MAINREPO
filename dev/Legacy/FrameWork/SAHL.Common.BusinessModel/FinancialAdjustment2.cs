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
using Castle.ActiveRecord.Queries;
using SAHL.Common.Utils;

namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.FinancialAdjustment_DAO
	/// </summary>
	public partial class FinancialAdjustment : BusinessModelBase<SAHL.Common.BusinessModel.DAO.FinancialAdjustment_DAO>, IFinancialAdjustment
	{
		public IFinancialAdjustmentTypeSource FinancialAdjustmentTypeSource
		{
			get
			{
				var fas = this.FinancialAdjustmentSource.Key;
				var fat = this.FinancialAdjustmentType.Key;
				string hql = @"from FinancialAdjustmentTypeSource_DAO fats where
							   fats.FinancialAdjustmentType.Key = ? AND fats.FinancialAdjustmentSource.Key = ?";

				SimpleQuery<FinancialAdjustmentTypeSource_DAO> q = new SimpleQuery<FinancialAdjustmentTypeSource_DAO>(hql, this.FinancialAdjustmentType.Key, (int)this.FinancialAdjustmentSource.Key);
				FinancialAdjustmentTypeSource_DAO[] list = q.Execute();

				if (list.Length > 0)
				{
					IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return bmtm.GetMappedType<IFinancialAdjustmentTypeSource>(list[0]);
				}

				return null;
			}
		}

        /// <summary>
        /// 
        /// </summary>
        public int Term
        {
            get
            {
                if (!EndDate.HasValue || !FromDate.HasValue)
                    return 0;
                else
                    return FromDate.Value.MonthDifference(EndDate.Value, 1);
            }
        }


        public string Value
        {
            get 
            {
                string adjustment = string.Empty;

                if (this.DifferentialProvisionAdjustment != null)
                {
                    adjustment = DifferentialProvisionAdjustment.DifferentialAdjustment.ToString(SAHL.Common.Constants.RateFormat);
                }
                else if (this.FixedRateAdjustment != null)
                {
                    adjustment = this.FixedRateAdjustment.Rate.ToString(SAHL.Common.Constants.RateFormat);
                }
                else if (this.InterestRateAdjustment != null)
                {
                    adjustment = (this.InterestRateAdjustment.Adjustment).ToString(SAHL.Common.Constants.RateFormat);
                }
                else if (this.PaymentAdjustment != null)
                {
                    adjustment = this.PaymentAdjustment.Amount.ToString(SAHL.Common.Constants.CurrencyFormat);
                }
                else if (this.ReversalProvisionAdjustment != null)
                {
                    adjustment = this.ReversalProvisionAdjustment.ReversalPercentage.ToString(SAHL.Common.Constants.CurrencyFormat);
                }	
				else if (this.StaticRateAdjustment != null)
				{
					adjustment = this.StaticRateAdjustment.Rate.ToString(SAHL.Common.Constants.RateFormat);
				}
                return adjustment;
            }
        }

        /// <summary>
        /// ToString() overload
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Value;
        }
	}
}


