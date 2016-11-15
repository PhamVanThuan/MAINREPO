﻿using System;
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
using DevExpress.XtraCharts;
using System.Data;

namespace SAHL.Web.Controls
{
    /// <summary>
    /// Proposal Graph
    /// </summary>
    public class ProposalGraphWFC : ChartControl
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

        /// <summary>
        /// Initializes a new Proposal Graph
        /// </summary>
        public ProposalGraphWFC()
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
            ILegalEntityNaturalPerson oldestNaturalPerson = ProposalGraphWFC.FindOldestNaturalPerson(proposal.DebtCounselling.Clients);

            //Render the Proposal detail on the graph
            if (ammortisationTablePI != null && ammortisationTablePI.Rows.Count > 0)
            {
				RenderGraph("Proposal", ammortisationTablePI, "Period", "Closing", Color.Firebrick);
                maxPITerm = ammortisationTablePI[ammortisationTablePI.Rows.Count - 1].Period;
            }

            if (ammortisationTable != null && ammortisationTable.Rows.Count > 0)
            {
                //Render the Ammorization Graph
				RenderGraph("Actual", ammortisationTable, "Period", "Closing", Color.MidnightBlue);

                //Render the Latest Valuation Amount
                //set the end term as the greater of the 2 tables terms
                int valEndTerm = maxPITerm > ammortisationTable[ammortisationTable.Rows.Count - 1].Period ? maxPITerm : ammortisationTable[ammortisationTable.Rows.Count - 1].Period;

                RenderValuationAmount(ammortisationTable[0].Period, mortgageLoan.GetActiveValuationAmount(), valEndTerm);

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
					RenderLineAtTerm(term60, (int)mortgageLoan.GetActiveValuationAmount(), oldestNaturalPerson);
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
            RenderLine(String.Format(@"Valuation: {0}", valuationAmount.ToString(SAHL.Common.Constants.CurrencyFormatNoCents)), startTerm, valuationAmount, endTerm, valuationAmount, false, Color.DarkGoldenrod);
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

            RenderLine(leLegend, term, 0, term, height, false, Color.DarkSeaGreen);
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

        public void RenderGraph(string graphName, DataTable graphDataTable, string xAxisName, string yAxisName, Color color)
		{
			Series series = new Series(graphName, ViewType.Line);
            ((LineSeriesView)series.View).LineMarkerOptions.Visible = true;
            ((LineSeriesView)series.View).LineStyle.Thickness = 4;
            ((LineSeriesView)series.View).LineMarkerOptions.Size = 3;
            ((LineSeriesView)series.View).Color = color;

            series.DataSource = graphDataTable;
			series.ValueDataMembers.AddRange(new string[] { yAxisName });
			series.ValueScaleType = ScaleType.Numerical;
			series.ArgumentScaleType = ScaleType.Numerical;
			series.ArgumentDataMember = xAxisName;

			series.Label.Visible = false;
			this.Series.Add(series);
		}

		/// <summary>
		/// Render a Line
		/// </summary>
		/// <param name="graphName"></param>
		/// <param name="x1"></param>
		/// <param name="y1"></param>
		/// <param name="x2"></param>
		/// <param name="y2"></param>
		/// <param name="showLabel"></param>
        /// <param name="color"></param>
		public void RenderLine(string graphName, double x1, double y1, double x2, double y2, bool showLabel, Color color)
		{
			Series series = new DevExpress.XtraCharts.Series(graphName, ViewType.Line);

            ((LineSeriesView)series.View).LineMarkerOptions.Visible = true;
            ((LineSeriesView)series.View).LineStyle.Thickness = 4;
            ((LineSeriesView)series.View).LineMarkerOptions.Size = 3;
            ((LineSeriesView)series.View).Color = color;

            series.Points.Add(new SeriesPoint(x1, y1));
			series.Points.Add(new SeriesPoint(x2, y2));
			series.ValueScaleType = ScaleType.Numerical;
			series.ArgumentScaleType = ScaleType.Numerical;
			series.Label.Visible = showLabel;
			this.Series.Add(series);
		}
    }
}