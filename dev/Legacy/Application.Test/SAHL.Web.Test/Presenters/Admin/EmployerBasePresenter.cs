using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using Rhino.Mocks.Interfaces;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Administration.Presenters;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Test.Presenters.Admin
{
    public class EmployerBasePresenter : TestViewBase
    {

        #region Attributes and Constants

        private SAHL.Web.Views.Administration.Interfaces.IEmployer _view;
        private IEmploymentRepository _employmentRepository;

        protected const string CancelButtonClicked = "CancelButtonClicked";
        protected const string EmployerSelected = "EmployerSelected";

        #endregion

        #region Setup/TearDown

        [SetUp]
        public void Setup()
        {
            _view = _mockery.CreateMock<SAHL.Web.Views.Administration.Interfaces.IEmployer>();
            SetupMockedView(_view);
            SetupPrincipalCache(base.TestPrincipal);
            SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);
            ClearMockCache();

            // create repositories
            _employmentRepository = _mockery.CreateMock<IEmploymentRepository>();
            MockCache.Add(typeof(IEmploymentRepository).ToString(), _employmentRepository);
        }

        [TearDown]
        public void TearDown()
        {
            _view = null;
        }

        #endregion

        #region Properties

        protected IEmploymentRepository EmploymentRepository
        {
            get
            {
                return _employmentRepository;
            }
        }

        protected SAHL.Web.Views.Administration.Interfaces.IEmployer View
        {
            get
            {
                return _view;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Common initialisation procedure.
        /// </summary>
        /// <returns></returns>
        protected virtual IDictionary<string, IEventRaiser> OnInitialiseCommon()
        {
            Dictionary<string, IEventRaiser> dict = new Dictionary<string, IEventRaiser>();

            // cancel button click event
            View.CancelButtonClicked += null;
            LastCall.Constraints(Is.NotNull());
            IEventRaiser erCancelButton = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add(CancelButtonClicked, erCancelButton);

            // employer selected event
            View.EmployerSelected += null;
            LastCall.Constraints(Is.NotNull());
            IEventRaiser erEmployerSelected = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add(EmployerSelected, erEmployerSelected);

            return dict;
        }

        /// <summary>
        /// Tests the cancel click event.  This only tests what happens when cancel is clicked, the building up of the mocks
        /// and the actual presenter test needs to be handled in the derived class.
        /// </summary>
        protected void OnCancelClicked()
        {
            ISimpleNavigator navigator = _mockery.CreateMock<ISimpleNavigator>();
            SetupResult.For(View.Navigator).Return(navigator);
            navigator.Navigate("Cancel");
        }

        /// <summary>
        /// Tests the selection of an employer. This only tests what happens when cancel is clicked, the building up of the mocks
        /// and the actual presenter test needs to be handled in the derived class.
        public void OnEmployerSelected()
        {
            IEmployer employer = _mockery.CreateMock<IEmployer>();
            SetupResult.For(EmploymentRepository.GetEmployerByKey(0)).IgnoreArguments().Return(employer);
            View.SelectedEmployer = employer;

        }

        #endregion

    }
}
