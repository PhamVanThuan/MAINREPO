using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.DataSets;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using SAHL.Common.Utils;

namespace SAHL.Web.Controls
{
    /// <summary>
    /// Proposal Graph
    /// </summary>
    public class ProposalGraph : SAHLChart
    {
        private IDebtCounsellingRepository _debtCounsellingRepository;
        private IMortgageLoanRepository _mortgageLoanRepository;
        private IControlRepository _ctrlRepo;

        private IControlRepository CtrlRepo
        {
            get
            {
                if (_ctrlRepo == null)
                    _ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();

                return _ctrlRepo;
            }
        }
        private IDebtCounsellingRepository DebtCounsellingRepository
        {
            get
            {
                if (_debtCounsellingRepository == null)
                {
                    _debtCounsellingRepository = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
                }
                return _debtCounsellingRepository;
            }
        }
        private IMortgageLoanRepository MortgageLoanRepository
        {
            get
            {
                if (_mortgageLoanRepository == null)
                {
                    _mortgageLoanRepository = RepositoryFactory.GetRepository<IMortgageLoanRepository>();
                }
                return _mortgageLoanRepository;
            }
        }

        public string MyProperty { get; set; }

        /// <summary>
        /// Initializes a new Proposal Graph
        /// </summary>
        public ProposalGraph()
        {
        }

        /// <summary>
        /// Render
        /// </summary>
        /// <param name="proposalKey"></param>
        public void Render(int proposalKey)
        {
            if (proposalKey < 1)
                return;

            //get the maximum no of periods to show
            int maxPeriods = Convert.ToInt16(CtrlRepo.GetControlByDescription("MaxPeriodForAmortisationGraph").ControlNumeric.Value);
            int maxPITerm = 0;


            //Get the proposal for the account
            IProposal proposal = DebtCounsellingRepository.GetProposalByKey(proposalKey);

            if (proposal == null)
                return;
            //Get the Mortgage Loan so that we may get the
            //Latest Valuation Amount
            IMortgageLoan mortgageLoan = MortgageLoanRepository.GetMortgageloanByAccountKey(proposal.DebtCounselling.Account.Key);

            //Get the Ammortization Schedule for the Account
            LoanCalculations.AmortisationScheduleDataTable ammortisationTable = DebtCounsellingRepository.GetAmortisationScheduleForAccountByProposalKey(proposalKey);

            //Get the Proposal LineItem Schedule
            LoanCalculations.AmortisationScheduleDataTable ammortisationTablePI = DebtCounsellingRepository.GetAmortisationScheduleForProposalByKey(proposalKey, maxPeriods);

			//If the proposal to render is not the current active accepted proposal, then show the current accepted active proposal too
			if (proposal.DebtCounselling.AcceptedActiveProposal != null && proposalKey != proposal.DebtCounselling.AcceptedActiveProposal.Key)
			{
				LoanCalculations.AmortisationScheduleDataTable acceptedActiveProposalTable = DebtCounsellingRepository.GetAmortisationScheduleForProposalByKey(proposal.DebtCounselling.AcceptedActiveProposal.Key, maxPeriods);
                this.RenderGraph("Active Proposal", acceptedActiveProposalTable, "Period", "Closing", Color.Green);
			}

            //Get the Oldest Legal Entity on the Account
            ILegalEntityNaturalPerson oldestNaturalPerson = ProposalGraph.FindOldestNaturalPerson(proposal.DebtCounselling.Clients);


            if (oldestNaturalPerson == null)
                throw new SAHL.Common.Exceptions.DomainValidationException("Note: There are no Natural Person's under debt counselling on this case.");

            //Render the Proposal detail on the graph
            if (ammortisationTablePI != null && ammortisationTablePI.Rows.Count > 0)
            {
				this.RenderGraph("Proposal", ammortisationTablePI, "Period", "Closing", Color.Firebrick);
                maxPITerm = ammortisationTablePI[ammortisationTablePI.Rows.Count - 1].Period;
            }

            if (ammortisationTable != null && ammortisationTable.Rows.Count > 0)
            {
                //Render the Ammorization Graph
				this.RenderGraph("Actual", ammortisationTable, "Period", "Closing", Color.MidnightBlue);

                //Render the Latest Valuation Amount
                //set the end term as the greater of the 2 tables terms
                int valEndTerm = maxPITerm > ammortisationTable[ammortisationTable.Rows.Count - 1].Period ? maxPITerm : ammortisationTable[ammortisationTable.Rows.Count - 1].Period;

                this.RenderValuationAmount(ammortisationTable[0].Period, mortgageLoan.GetActiveValuationAmount(), valEndTerm);

                //Render the time at which the oldest legal entity on the Debt Counselling Case would be 60, 
                //only if it fits on the graph
				int term60 = 0;
				DateTime? birthDay = oldestNaturalPerson.DateOfBirth;

				DateTime proposalStartDate = DebtCounsellingRepository.SortProposalItems(proposal)[0].StartDate;
				if (birthDay.HasValue)
				{
					int periodInMonths = birthDay.Value.AddYears(60).MonthDifference(proposalStartDate, birthDay.Value.Day);
					term60 = (ammortisationTable[0].Period + (birthDay.Value.AddYears(60) < proposalStartDate ? -periodInMonths :  +periodInMonths));
				}

				if (term60 < maxPeriods && (birthDay != null && birthDay.Value.AddYears(60) > proposalStartDate))
				{
					this.RenderLineAtTerm(term60, (int)mortgageLoan.GetActiveValuationAmount(), oldestNaturalPerson);
				}
            }
        }

        /// <summary>
        /// Render the Latest Valuation Amount
        /// </summary>
        /// <param name="startTerm"></param>
        /// <param name="valuationAmount"></param>
        /// <param name="endTerm"></param>
        public void RenderValuationAmount(int startTerm, double valuationAmount, int endTerm)
        {
            this.RenderLine(String.Format(@"Valuation: {0}", valuationAmount.ToString(SAHL.Common.Constants.CurrencyFormatNoCents)), startTerm, valuationAmount, endTerm, valuationAmount, false, Color.DarkGoldenrod);
        }

        /// <summary>
        /// Renders a Line at a certain Term
        /// </summary>
        /// <param name="term">The Term of which the oldest Legal Entity would be 60</param>
        /// <param name="height"></param>
        /// <param name="le"></param>
        public void RenderLineAtTerm(int term, int height, ILegalEntity le)
        {
            string leLegend = String.Format(@"{0} turns 60", le.GetLegalName(LegalNameFormat.InitialsOnly));

            this.RenderLine(leLegend, term, 0, term, height, false, Color.DarkSeaGreen);
        }

        /// <summary>
        /// Find the Oldest Legal Entity in the list that is of type Legal Entity Natural Person
        /// </summary>
        /// <param name="legalEntities"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Justification = "Neccesary casts")]
        private static ILegalEntityNaturalPerson FindOldestNaturalPerson(IList<ILegalEntity> legalEntities)
        {
            ILegalEntityNaturalPerson oldestNaturalPerson = null;
            foreach (ILegalEntity legalEntity in legalEntities)
            {
                if (legalEntity is ILegalEntityNaturalPerson)
                {
                    ILegalEntityNaturalPerson naturalPerson = (ILegalEntityNaturalPerson)legalEntity;
                    //Probably a first assignment
                    if (oldestNaturalPerson == null)
                    {
                        oldestNaturalPerson = naturalPerson;
                        continue;
                    }
                    else
                    {
                        if (((ILegalEntityNaturalPerson)legalEntity).AgeNextBirthday > oldestNaturalPerson.AgeNextBirthday)
                        {
                            oldestNaturalPerson = naturalPerson;
                        }
                    }
                }
            }
            return oldestNaturalPerson;
        }
    }
}