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
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.ManualDebitOrder_DAO
	/// </summary>
	public partial class ManualDebitOrder : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ManualDebitOrder_DAO>, IManualDebitOrder
	{
        public bool Active
        {
            get { return (_DAO.GeneralStatus.Key == (int)GeneralStatusKey.Active); }
        }


        private IManualDebitOrder _original;

        public override void ExtendedConstructor()
        {
            base.ExtendedConstructor();

            _original = new SimpleFinancialServiceRecurringTransaction();
            _original.ActionDate = this.ActionDate;
        }

        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);
            Rules.Add("ManualDebitOrderStartDayMinimum");
			Rules.Add("ManualDebitOrderStartDayMaximum");
			Rules.Add("ManualDebitOrderUpdateExpired");
			Rules.Add("ManualDebitOrderBankAccountMandatory");
			Rules.Add("ManualDebitOrderAmountMinimum");
        }

        /// <summary>
        /// Gets a shallow copy of the object when it was first loaded. For new objects, this will 
        /// be null.  Collections and methods are not implemented.
        /// </summary>
        public IManualDebitOrder Original
        {
            get
            {
                return _original;
            }
        }


        private class SimpleFinancialServiceRecurringTransaction : IManualDebitOrder
        {
            private DateTime _actionDate;

            public DateTime ActionDate
            {
                get
                {
                    return this._actionDate;
                }
                set
                {
                    this._actionDate = value;
                }
            }

            #region IManualDebitOrder members
            
            public int Key
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public DateTime InsertDate
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            

            public string Reference
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public double Amount
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public string UserID
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public IBankAccount BankAccount
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public IFinancialService FinancialService
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public IGeneralStatus GeneralStatus
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public IMemo Memo
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public ITransactionType TransactionType
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public bool Active
            {
                get { throw new NotImplementedException(); }
            }

            public bool ValidateEntity()
            {
                throw new NotImplementedException();
            }

            public object Clone()
            {
                throw new NotImplementedException();
            }

            public void Refresh()
            {
                throw new NotImplementedException();
            }


            public IManualDebitOrder Original
            {
                get { throw new NotImplementedException(); }
            }

           

            #endregion



            public IBatchTotal BatchTotal
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }
        }



    }
}


