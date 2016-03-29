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
using System.Security.Principal;
using SAHL.Common.Security;

using SAHL.Common.Globals;
using SAHL.Common.DomainMessages;
using SAHL.Common.CacheData;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Base;
namespace SAHL.Common.BusinessModel
{
    public partial class HOC : BusinessModelBase<SAHL.Common.BusinessModel.DAO.HOC_DAO>, IHOC
    {
        private IHOC _original;

		public void OnConstruction()
		{
			if (this.Key > 0)
			{
				SimpleHOC shoc = new SimpleHOC();
				shoc.SetHOCTotalSumInsured(this.HOCTotalSumInsured);
				_original = shoc;
			}

		}

        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);

            Rules.Add("HOCInsurerMandatory");
            Rules.Add("HOCRoofDescriptionConventional");
            Rules.Add("HOCRoofDescriptionThatch");
            Rules.Add("HOCRoofDescriptionCombined");
            Rules.Add("HOCTotalSumInsuredMinimum");
            Rules.Add("HOCLoanClosed");
            Rules.Add("HOCPolicyNumberNotNull");
            Rules.Add("HOCPaidUpNoHOC");
            Rules.Add("HOCSAHLCalculatePremium");
            Rules.Add("HOCCededStatus");
            Rules.Add("HOCTitleTypeSectionalTitle");
            Rules.Add("HOCValuationExpiredCheck");
            Rules.Add("HOCNoHOCDetailTypeToSAHLHOC");
            Rules.Add("HOCNonHOCCessionDetailTypeToSAHLHOC");
        }

        public override void ExtendedConstructor()
        {
            base.ExtendedConstructor();
        }

        #region Properties

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOC_DAO.AnniversaryDate
        /// </summary>
        public DateTime? AnniversaryDate
        {
            get { return _DAO.AnniversaryDate; }
            set
            {
                if (value.HasValue)
                {
                    _DAO.AnniversaryDate = DateTime.Parse(value.Value.ToShortDateString());
                }
                else
                    _DAO.AnniversaryDate = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CalculatePremium()
        {
            IHOCRepository hocRepository = RepositoryFactory.GetRepository<IHOCRepository>();
            hocRepository.CalculatePremium(this);
        }

        public void CalculatePremiumForUpdate()
        {
            IHOCRepository hocRepository = RepositoryFactory.GetRepository<IHOCRepository>();
            hocRepository.CalculatePremiumForUpdate(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public double PremiumThatch { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double PremiumShingle { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double PremiumConventional { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double HOCTotalSumInsured
        {
            get { return _DAO.HOCTotalSumInsured; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Sets the HOCSumInsured - throws an error if new SumInsured is less than Previous SumInsured
        /// </summary>
        /// <param name="HOCTotalSumInsured"></param>
        public void SetHOCTotalSumInsured(double HOCTotalSumInsured)
        {
            // The rule logic below has been turned into a warning rule : TRAC ticket #10885 (ITSD# 9485)
            // As per prod you should be able to decrease the HOC Sum Assured Amount
            //SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

            //bool isLightStoneAVM = false; // Ignore this rule for LightStone Valuations

            //if (this.Properties != null && this.Properties.Count > 0 && this.Properties[0].LatestCompleteValuation is IValuationDiscriminatedLightstoneAVM)
            //    isLightStoneAVM = true;

            //if (!isLightStoneAVM  && (HOCTotalSumInsured < this.HOCTotalSumInsured))
            //    spc.DomainMessages.Add(new Error("New HOC SumInsured can not be less than previous value of R " + this.HOCTotalSumInsured.ToString() + ".", ""));
            //else
            //    this._DAO.HOCTotalSumInsured = HOCTotalSumInsured;

            this._DAO.HOCTotalSumInsured = HOCTotalSumInsured;

            // warning validation : check if the New HOC SumInsured is less than the Previous HOC SumInsured
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            IRuleService svc = ServiceFactory.GetService<IRuleService>();
            svc.ExecuteRule(spc.DomainMessages, "HOCTotalSumAssuredLessThanPrevious", this);
        }



        #endregion

        #region Event Handlers

        public void OnHOCHistories_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        public void OnHOCHistories_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        public void OnProperties_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        public void OnProperties_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        public void OnHOCHistories_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        public void OnHOCHistories_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        public void OnProperties_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        public void OnProperties_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        #endregion


        #region IHOC Members


        #endregion

        /// <summary>
        /// Gets a shallow copy of the object when it was first loaded.  For new hoc, this will 
        /// be null.  Collections and methods are not implemented.
        /// </summary>
        public IHOC Original
        {
            get
            {
                return _original;
            }
        }

        private class SimpleHOC : IHOC
        {
            #region IHOC Members

            public int HOCKey
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

            public string HOCPolicyNumber
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

            public double HOCProrataPremium
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

            public double? HOCMonthlyPremium
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

            public double? HOCThatchAmount
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

            public double? HOCConventionalAmount
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

            public double? HOCShingleAmount
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

            public int? HOCStatusID
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

            public bool? HOCSBICFlag
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

            public double? CapitalizedMonthlyBalance
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

            public DateTime? CommencementDate
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

            public DateTime? AnniversaryDate
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

            public string UserID
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

            public DateTime? ChangeDate
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

            public bool Ceded
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

            public string SAHLPolicyNumber
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

            public DateTime? CancellationDate
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

            public IHOCHistory HOCHistory
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

            public IEventList<IHOCHistory> HOCHistories
            {
                get { throw new Exception("The method or operation is not implemented."); }
            }

            public IHOCInsurer HOCInsurer
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

            //public IEventList<IProperty> Properties
            //{
            //    get { throw new Exception("The method or operation is not implemented."); }
            //}

            public IHOCConstruction HOCConstruction
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

            public IHOCRoof HOCRoof
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

            public IHOCStatus HOCStatus
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

            public IHOCSubsidence HOCSubsidence
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

            public double HOCAdministrationFee
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
            public double HOCBasePremium
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
            public double SASRIAAmount
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

            public double PremiumThatch
            {
                get { throw new Exception("The method or operation is not implemented."); }
            }

            public double PremiumConventional
            {
                get { throw new Exception("The method or operation is not implemented."); }
            }

            public double PremiumShingle
            {
                get { throw new Exception("The method or operation is not implemented."); }
            }

            private double _hocTotalSumInsured;
            public double HOCTotalSumInsured
            {
                get { return _hocTotalSumInsured; }
            }

            public void SetHOCTotalSumInsured(double HOCTotalSumInsured)
            {
                _hocTotalSumInsured = HOCTotalSumInsured;
            }

            public void CalculateMonthlyPremium()
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public void CalculateProRataPremium()
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public IHOC Original
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

            #region IFinancialService Members

            public double Payment
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

            public ITrade Trade
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

            public DateTime? NextResetDate
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

            public IEventList<IFinancialServiceBankAccount> FinancialServiceBankAccounts
            {
                get { throw new Exception("The method or operation is not implemented."); }
            }

            public IEventList<IFinancialServiceCondition> FinancialServiceConditions
            {
                get { throw new Exception("The method or operation is not implemented."); }
            }

            //public IEventList<IRateOverride> RateOverrides
            //{
            //    get { throw new Exception("The method or operation is not implemented."); }
            //}

            public IAccount Account
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

            public IAccountStatus AccountStatus
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

            public ICategory Category
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

            public IEventList<IFinancialTransaction> LoanTransactions
            {
                get { throw new Exception("The method or operation is not implemented."); }
            }

            public System.Data.DataTable GetTransactions(IDomainMessageCollection Messages, List<int> TransactionTypeKeys)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public System.Data.DataTable GetTransactions(IDomainMessageCollection Messages, int GeneralStatusKey, List<int> TransactionTypeKeys)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public System.Data.DataTable GetTransactions(IDomainMessageCollection Messages, int GeneralStatusKey, List<int> TransactionTypeKeys, bool WithMortgageLoanInfo)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public IFinancialServiceType FinancialServiceType
            {
                get { throw new Exception("The method or operation is not implemented."); }
            }

            public IFinancialServiceBankAccount CurrentBankAccount
            {
                get { throw new Exception("The method or operation is not implemented."); }
            }

            #endregion


            public IEventList<IFinancialAdjustment> FinancialAdjustments
            {
                get { throw new NotImplementedException(); }
            }

            public IEventList<IFinancialServiceAttribute> FinancialServiceAttributes
            {
                get { throw new NotImplementedException(); }
            }

            public IEventList<IFinancialTransaction> FinancialTransactions
            {
                get { throw new NotImplementedException(); }
            }

            public IFinancialService FinancialServiceParent
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

            public DateTime? OpenDate
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

            public DateTime? CloseDate
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

            public IEventList<IFinancialService> FinancialServices
            {
                get { throw new NotImplementedException(); }
            }

            public IBalance Balance
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

            public ILoanBalance LoanBalance
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

            public IEventList<IArrearTransaction> ArrearTransactions
            {
                get { throw new NotImplementedException(); }
            }

            public IEventList<IFee> Fees
            {
                get { throw new NotImplementedException(); }
            }

            public IHOC HOC
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

            public ILifePolicy LifePolicy
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


            public void CalculatePremium()
            {
                throw new NotImplementedException();
            }

            public void CalculatePremiumForUpdate()
            {
                throw new Exception("The method or operation is not implemented.");
            }

            double IHOC.PremiumThatch
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

            double IHOC.PremiumShingle
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

            double IHOC.PremiumConventional
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


