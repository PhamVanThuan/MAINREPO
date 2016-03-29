using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using System.Diagnostics.CodeAnalysis;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Web.Controls
{
    /// <summary>
    /// Common employment display grid.
    /// </summary>
    public class EmploymentGrid : SAHLGridView
    {

        #region Private Attributes

        private IEmployment _employment;
        private int _selectedEmploymentKey = -1;
        private bool _columnLegalEntityVisible;
        private bool _columnStartDateVisible = true;

        #endregion

        #region Private Attributes and Enumerations

        /// <summary>
        /// Defines all columns used in the <see cref="EmploymentGrid"/>.
        /// </summary>
        private enum GridColumns
        {
            Key = 0,
            LegalEntityName,
            EmployerName,
            EmploymentType,
            RemunerationType,
            StartDate,
            Income,
            Status
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public EmploymentGrid()
        {
            AutoGenerateColumns = false;
            FixedHeader = false;
            EnableViewState = false;
            HeaderCaption = "Employment Details";
            EmptyDataSetMessage = "No employment details found";
            NullDataSetMessage = EmptyDataSetMessage;
            EmptyDataText = EmptyDataSetMessage;
            PostBackType = GridPostBackType.SingleClick;
            RowStyle.CssClass = "TableRowA";
            GridWidth = Unit.Percentage(100);
            Width = Unit.Percentage(100);
            GridHeight = Unit.Pixel(150);
            ShowFooter = true;

            if (!DesignMode)
            {
                // add the columns to the grid
                this.AddGridBoundColumn("Key", "Employment Key", Unit.Empty, HorizontalAlign.Left, false);
                this.AddGridBoundColumn("LegalEntityName", "Legal Entity", Unit.Pixel(150), HorizontalAlign.Left, true);
                this.AddGridBoundColumn("EmployerName", "Employer Name", Unit.Empty, HorizontalAlign.Left, true);
                this.AddGridBoundColumn("EmploymentType", "Employment Type", Unit.Pixel(120), HorizontalAlign.Left, true);
                this.AddGridBoundColumn("RemunerationType", "Remuneration Type", Unit.Pixel(130), HorizontalAlign.Left, true);
                this.AddGridBoundColumn("StartDate", "Start Date", Unit.Pixel(100), HorizontalAlign.Left, true);
                this.AddGridBoundColumn("Income", "Income", Unit.Pixel(150), HorizontalAlign.Right, true);
                this.AddGridBoundColumn("Status", "Status", Unit.Pixel(70), HorizontalAlign.Left, true);
            }

            this.SelectedIndexChanged += new EventHandler(EmploymentGrid_SelectedIndexChanged);
        }

        void EmploymentGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            _employment = null;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Binds a collection of <see cref="IEmployment"/> entities to the grid.
        /// </summary>
        /// <param name="employmentDetails"></param>
        /// <param name="showPrevious">Whether to show previous employment details.</param>
        public void BindEmploymentList(IEventList<IEmployment> employmentDetails, bool showPrevious)
        {
            List<GridEntity> gridEntities = new List<GridEntity>();
            double totalConfirmed = 0.0;
            int rows = 0;

            List<IEmployment> sortedlist = new List<IEmployment>(employmentDetails);

            // sort the employment values - first by status (Current then Previous), and then by start date
            sortedlist.Sort(
              delegate(IEmployment e1, IEmployment e2)
              {
                  if (e1.EmploymentStatus != e2.EmploymentStatus)
                      return e1.EmploymentStatus.Description.CompareTo(e2.EmploymentStatus.Description);

                  if (e1.EmploymentStartDate.HasValue && e2.EmploymentStartDate.HasValue)
                      return e1.EmploymentStartDate.Value.CompareTo(e2.EmploymentStartDate.Value);

                  return 0;
              });


            foreach (IEmployment e in sortedlist)
            {
                // check to see if we need to show previous employment records
                if (!showPrevious && e.EmploymentStatus != null && e.EmploymentStatus.Key == (int)EmploymentStatuses.Previous)
                    continue;

                gridEntities.Add(
                    new GridEntity(e.Key,
                        (e.LegalEntity == null ? "-" : e.LegalEntity.DisplayName),
                        (e.Employer == null ? "" : e.Employer.Name),
                        (e.EmploymentType == null ? "" : e.EmploymentType.Description),
                        (e.RemunerationType == null ? "" : e.RemunerationType.Description),
                        ((e.EmploymentStartDate.HasValue) ? e.EmploymentStartDate.Value.ToString(SAHL.Common.Constants.DateFormat) : "-"),
                        ((e.ConfirmedIncome > 0) ? e.ConfirmedIncome.ToString(SAHL.Common.Constants.CurrencyFormat) : "-"),
                        (e.EmploymentStatus == null ? "" : e.EmploymentStatus.Description)
                    )
                );
                if (e.EmploymentStatus != null && e.EmploymentStatus.Key == (int)EmploymentStatuses.Current)
                    totalConfirmed += e.ConfirmedIncome;

                if (e.Key == _selectedEmploymentKey)
                    this.SelectedIndex = rows;

                rows++;
            }
            DataSource = gridEntities;
            DataBind();

            if (rows > 0)
            {
                // add the total to the grid
                this.FooterRow.Cells[(int)GridColumns.Income].Text = totalConfirmed.ToString(SAHL.Common.Constants.CurrencyFormat);
                this.FooterRow.Cells[(int)GridColumns.Income].HorizontalAlign = HorizontalAlign.Right;
            }
        }

        protected override void  OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            Columns[(int)GridColumns.LegalEntityName].Visible = _columnLegalEntityVisible;
            Columns[(int)GridColumns.StartDate].Visible = _columnStartDateVisible;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets/sets whether the legal entity column is visible.  This defaults to false.
        /// </summary>
        public bool ColumnLegalEntityVisible
        {
            get
            {
                return _columnLegalEntityVisible;
            }
            set
            {
                _columnLegalEntityVisible = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the legal entity column is visible.  This defaults to true.
        /// </summary>
        public bool ColumnStartDateVisible
        {
            get
            {
                return _columnStartDateVisible;
            }
            set
            {
                _columnStartDateVisible = value;
            }
        }

        /// <summary>
        /// Gets a reference to the currently selected <see cref="IEmployment"/>.  If no row is selected, a null is returned.
        /// </summary>
        public IEmployment SelectedEmployment
        {
            get
            {
                if (Rows.Count == 0)
                    return null;

                if (_employment == null && SelectedIndex > -1)
                {
                    int employmentKey = Int32.Parse(Rows[SelectedIndex].Cells[(int)GridColumns.Key].Text);
                    IEmploymentRepository rep = RepositoryFactory.GetRepository<IEmploymentRepository>();
                    _employment = rep.GetEmploymentByKey(employmentKey);
                }
                return _employment;
            }
            set
            {
                _selectedEmploymentKey = value.Key;
            }
        }

        #endregion


        #region Private Classes

        /// <summary>
        /// Internal class to make binding to the grid a little easier as we have to support different data source entities.
        /// </summary>
        private class GridEntity
        {
            private int _key;
            private string _legalEntityName;
            private string _employerName;
            private string _employmentType;
            private string _remunerationType;
            private string _startDate;
            private string _income;
            private string _status;

            public GridEntity(int key, string legalEntityName, string employerName, string employmentType, string remunerationType, string startDate, string income, string status)
            {
                _key = key;
                _legalEntityName = legalEntityName;
                _employerName = employerName;
                _employmentType = employmentType;
                _remunerationType = remunerationType;
                _startDate = startDate;
                _income = income;
                _status = status;
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string LegalEntityName
            {
                get { return _legalEntityName; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string EmployerName
            {
                get { return _employerName; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public int Key
            {
                get { return _key; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string EmploymentType
            {
                get { return _employmentType; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string RemunerationType
            {
                get { return _remunerationType; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string StartDate
            {
                get { return _startDate; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string Income
            {
                get { return _income; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string Status
            {
                get { return _status; }
            }

        }
        #endregion

    }
}
