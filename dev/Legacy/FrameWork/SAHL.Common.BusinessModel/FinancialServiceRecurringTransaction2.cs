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
	/*public partial class FinancialServiceRecurringTransaction : BusinessModelBase<SAHL.Common.BusinessModel.DAO.FinancialServiceRecurringTransaction_DAO>, IFinancialServiceRecurringTransaction
	{
        private IFinancialServiceRecurringTransaction _original;

        public override void ExtendedConstructor()
        {
            base.ExtendedConstructor();

            _original = new SimpleFinancialServiceRecurringTransaction();
            _original.StartDate = this.StartDate;
        }

        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);
            Rules.Add("FinancialServiceRecurringTransactionStartDayMinimum");
            Rules.Add("FinancialServiceRecurringTransactionStartDayMaximum");
            Rules.Add("FinancialServiceRecurringTransactionUpdateExpired");
            Rules.Add("FinancialServiceRecurringTransactionBankAccountMandatory");
            Rules.Add("FinancialServiceRecurringTransactionAmountMinimum");
        }

        /// <summary>
        /// Gets a shallow copy of the object when it was first loaded.  For new objects, this will 
        /// be null.  Collections and methods are not implemented.
        /// </summary>
        public IFinancialServiceRecurringTransaction Original
        {
            get
            {
                return _original;
            }
        }

        private class SimpleFinancialServiceRecurringTransaction : IFinancialServiceRecurringTransaction
        {
            private DateTime? _startDate;

            #region IFinancialServiceRecurringTransaction Members

            public DateTime? InsertDate
            {
                get
                {
                    throw new Exception("The method or operation is not implemented.");
                }
                set
                {
                    throw new Exception("The method or operation is not implemented.");
                }
            }

            public int? Frequency
            {
                get
                {
                    throw new Exception("The method or operation is not implemented.");
                }
                set
                {
                    throw new Exception("The method or operation is not implemented.");
                }
            }

            public ITransactionType TransactionType
            {
                get
                {
                    throw new Exception("The method or operation is not implemented.");
                }
                set
                {
                    throw new Exception("The method or operation is not implemented.");
                }
            }

            public char FrequencyType
            {
                get
                {
                    throw new Exception("The method or operation is not implemented.");
                }
                set
                {
                    throw new Exception("The method or operation is not implemented.");
                }
            }

            public int? NumUntilNextRun
            {
                get
                {
                    throw new Exception("The method or operation is not implemented.");
                }
                set
                {
                    throw new Exception("The method or operation is not implemented.");
                }
            }

            public string Reference
            {
                get
                {
                    throw new Exception("The method or operation is not implemented.");
                }
                set
                {
                    throw new Exception("The method or operation is not implemented.");
                }
            }

            public bool Active
            {
                get
                {
                    throw new Exception("The method or operation is not implemented.");
                }
                set
                {
                    throw new Exception("The method or operation is not implemented.");
                }
            }

            public DateTime? StartDate
            {
                get
                {
                    return _startDate;
                }
                set
                {
                    _startDate = value;
                }
            }

            public int? Term
            {
                get
                {
                    throw new Exception("The method or operation is not implemented.");
                }
                set
                {
                    throw new Exception("The method or operation is not implemented.");
                }
            }

            public int? RemainingTerm
            {
                get
                {
                    throw new Exception("The method or operation is not implemented.");
                }
                set
                {
                    throw new Exception("The method or operation is not implemented.");
                }
            }

            public int? TransactionDay
            {
                get
                {
                    throw new Exception("The method or operation is not implemented.");
                }
                set
                {
                    throw new Exception("The method or operation is not implemented.");
                }
            }

            public int? HourOfRun
            {
                get
                {
                    throw new Exception("The method or operation is not implemented.");
                }
                set
                {
                    throw new Exception("The method or operation is not implemented.");
                }
            }

            public double? Amount
            {
                get
                {
                    throw new Exception("The method or operation is not implemented.");
                }
                set
                {
                    throw new Exception("The method or operation is not implemented.");
                }
            }

            public string StatementName
            {
                get
                {
                    throw new Exception("The method or operation is not implemented.");
                }
                set
                {
                    throw new Exception("The method or operation is not implemented.");
                }
            }

            public DateTime? PreviousRunDate
            {
                get
                {
                    throw new Exception("The method or operation is not implemented.");
                }
                set
                {
                    throw new Exception("The method or operation is not implemented.");
                }
            }

            public string UserName
            {
                get
                {
                    throw new Exception("The method or operation is not implemented.");
                }
                set
                {
                    throw new Exception("The method or operation is not implemented.");
                }
            }

            public string Notes
            {
                get
                {
                    throw new Exception("The method or operation is not implemented.");
                }
                set
                {
                    throw new Exception("The method or operation is not implemented.");
                }
            }

            public int Key
            {
                get
                {
                    throw new Exception("The method or operation is not implemented.");
                }
                set
                {
                    throw new Exception("The method or operation is not implemented.");
                }
            }

            public IBankAccount BankAccount
            {
                get
                {
                    throw new Exception("The method or operation is not implemented.");
                }
                set
                {
                    throw new Exception("The method or operation is not implemented.");
                }
            }

            public IFinancialService FinancialService
            {
                get
                {
                    throw new Exception("The method or operation is not implemented.");
                }
                set
                {
                    throw new Exception("The method or operation is not implemented.");
                }
            }

            public IRecurringTransactionType RecurringTransactionType
            {
                get
                {
                    throw new Exception("The method or operation is not implemented.");
                }
                set
                {
                    throw new Exception("The method or operation is not implemented.");
                }
            }

            public IFinancialServiceRecurringTransaction Original
            {
                get { throw new Exception("The method or operation is not implemented."); }
            }

            #endregion

            #region IEntityValidation Members

            public bool ValidateEntity()
            {
                throw new Exception("The method or operation is not implemented.");
            }

            #endregion

            #region IBusinessModelObject Members

            public object Clone()
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public void Refresh()
            {
                throw new Exception("The method or operation is not implemented.");
            }

            #endregion
        }
	}*/
}


