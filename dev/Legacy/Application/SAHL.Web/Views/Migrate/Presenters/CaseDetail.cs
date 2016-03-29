using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Web.Views.Migrate.Presenters;
using SAHL.Web.Views.Migrate.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.DataAccess;
using System.Data;
using System.Data.SqlClient;

namespace SAHL.Web.Views.Migrate.Presenters
{
	public class CaseDetail : CreateBase
	{
		private IMigrationDebtCounsellingRepository migrationDebtCounsellingRepository;
		public IMigrationDebtCounsellingRepository MigrationDebtCounsellingRepository
		{
			get
			{
				if (migrationDebtCounsellingRepository == null)
				{
					migrationDebtCounsellingRepository = RepositoryFactory.GetRepository<IMigrationDebtCounsellingRepository>();
				}
				return migrationDebtCounsellingRepository;
			}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
		public CaseDetail(ICreateCase view, SAHLCommonBaseController controller)
			: base(view, controller)
		{
		}

		/// <summary>
		/// OnView Initialised event - retrieve data for use by presenters
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void OnViewInitialised(object sender, EventArgs e)
		{
			base.OnViewInitialised(sender, e);
			if (!_view.ShouldRunPage)
				return;

			_view.OnSubmitButtonClicked += new EventHandler(OnSaveCaseDetailClicked);
			_view.WizardPage = 3;

			if (GlobalCacheData.ContainsKey(ViewConstants.CancelView))
				GlobalCacheData.Remove(ViewConstants.CancelView);

			if (GlobalCacheData.ContainsKey(ViewConstants.SelectView))
				GlobalCacheData.Remove(ViewConstants.SelectView);

			GlobalCacheData.Add(ViewConstants.CancelView, "CaseDetail", LifeTimes);
			GlobalCacheData.Add(ViewConstants.SelectView, "CaseLegalEntities", LifeTimes);

            _view.ConsultantUsers = MigrationDebtCounsellingRepository.GetConsultantUsers();

            //_view.Get171DateFromEworks = Get171DateFromEworks();
            //_view.Get60DaysDate();


            //CRepo.GetnWorkingDaysFromDate(60, _view.SelectedCase.DateOf171.HasValue ? _view.SelectedCase.DateOf171.Value : DateTime.Now);
            Get171DateFromEworks();
			_view.PopulateCaseDetails();
		}

        /// <summary>
        /// 
        /// </summary>
        private void Get171DateFromEworks()
        {
            string query = SAHL.Common.DataAccess.UIStatementRepository.GetStatement("Migrate", "Get171DateFromEWork");

            using (IDbConnection conn = Helper.GetSQLDBConnection())
            {
                conn.Open();
                ParameterCollection pc = new ParameterCollection();
                pc.Add(new SqlParameter("@AccountKey", _view.AccountKey));

                object o = Helper.ExecuteScalar(conn, query, pc);

                if (o != null && !String.IsNullOrEmpty(o.ToString()))
                {
                    _view.Get171DateFromEworks = Convert.ToDateTime(o);
                    _view.Get60DaysDate = CRepo.GetnWorkingDaysFromDate(60, Convert.ToDateTime(o));
                }
                else
                {
                    _view.Get171DateFromEworks = null;
                }
            }
        }

		/// <summary>
		/// On Save Case Detail Clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void OnSaveCaseDetailClicked(object sender, EventArgs e)
		{
			//Check if the view has any error messages
			if (!_view.IsValid)
			{
				return;
			}

            _view.SelectedCase.SixtyDaysDate = CRepo.GetnWorkingDaysFromDate(60, _view.SelectedCase.DateOf171.HasValue ? _view.SelectedCase.DateOf171.Value : DateTime.Now);

			MigrationDebtCounsellingRepository.SaveDebtCounselling(_view.SelectedCase);

			if (_view.IsValid)
				Navigate();
		}
	}
}