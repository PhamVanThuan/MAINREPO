using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;

namespace SAHL.Web.Views.Administration.Presenters
{
    /// <summary>
    /// Serves as a base presenter class for all presenters to do with the EMployer admin screens.
    /// </summary>
    public class EmployerBase : SAHLCommonBasePresenter<SAHL.Web.Views.Administration.Interfaces.IEmployer>
    {

        private IEmploymentRepository _employmentRepository;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public EmployerBase(SAHL.Web.Views.Administration.Interfaces.IEmployer view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _view = view;
        }

        /// <summary>
        /// Gets an employment repository for use in the presenter - this will be created the first time this property is called.
        /// </summary>
        protected IEmploymentRepository EmploymentRepository
        {
            get
            {
                if (_employmentRepository == null)
                    _employmentRepository = RepositoryFactory.GetRepository<IEmploymentRepository>();
                return _employmentRepository;
            }
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!View.ShouldRunPage) return;

            _view.CancelButtonClicked += new EventHandler(_view_CancelButtonClicked);
            _view.EmployerSelected += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_EmployerSelected);
        }

        void _view_EmployerSelected(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            IEmployer employer = EmploymentRepository.GetEmployerByKey(Convert.ToInt32(e.Key));
            _view.SelectedEmployer = employer;
        }

        void _view_CancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("EmployerDetails");
        }

        protected bool SaveEmployer()
        {
            IEmployer employer = _view.SelectedEmployer;

            TransactionScope txn = new TransactionScope();
            try
            {
                EmploymentRepository.SaveEmployer(employer, View.CurrentPrincipal);
                txn.VoteCommit();
                return true;
            }
            catch (Exception)
            {
                txn.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }
            finally
            {
                txn.Dispose();
            }
            return false;
        }
    }
}
