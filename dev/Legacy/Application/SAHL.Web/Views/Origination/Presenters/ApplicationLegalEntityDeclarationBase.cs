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
using SAHL.Web.Views.Origination.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

using System.Collections.Generic;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using SAHL.Common.DomainMessages;


namespace SAHL.Web.Views.Origination.Presenters
{
    public class ApplicationLegalEntityDeclarationBase : SAHLCommonBasePresenter<IApplicationLegalEntityDeclaration>
    {
        private IApplicationRepository _applicationRepo;
        private ILegalEntityRepository _legalEntityRepo;
        private int _legalEntityKey;
        private int _applicationKey;
        private IApplicationRole _applicationRole;
        private IEventList<IApplicationDeclaration> _applicationDeclarations;

        public int LegalEntityKey
        {
            get { return _legalEntityKey; }
            set { _legalEntityKey = value; }
        }

        public int ApplicationKey
        {
            get { return _applicationKey; }
            set { _applicationKey = value; }
        }
	
        public ApplicationLegalEntityDeclarationBase(IApplicationLegalEntityDeclaration view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) 
                return;

            _legalEntityRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            _applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

            _view.onCancelButtonClicked += new EventHandler(_view_onCancelButtonClicked);
            _view.onUpdateButtonClicked += new EventHandler(_view_onUpdateButtonClicked);

            ILegalEntity legalEntity = _legalEntityRepo.GetLegalEntityByKey(_legalEntityKey); 
            IApplication application = _applicationRepo.GetApplicationByKey(_applicationKey); 

            _applicationRole = null;

            if (application != null && legalEntity != null)
            {
                for (int i = 0; i < application.ApplicationRoles.Count; i++)
                {
                    if (application.ApplicationRoles[i].LegalEntity.Key == legalEntity.Key 
                        && (application.ApplicationRoles[i].ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant 
                        || application.ApplicationRoles[i].ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant 
                        || application.ApplicationRoles[i].ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor 
                        || application.ApplicationRoles[i].ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor))
                    {
                        _applicationRole = application.ApplicationRoles[i];
                        break;
                    }
                }

                _view.ConfigurePanel(legalEntity.DisplayName);

                IApplicationInformation appInfo = application.GetLatestApplicationInformation();
                IOriginationSourceProduct osp = _applicationRepo.GetOriginationSourceProductBySourceAndProduct(application.OriginationSource.Key, appInfo.Product.Key);
                IList<IApplicationDeclarationQuestionAnswerConfiguration> appDecQandAConfig = new List<IApplicationDeclarationQuestionAnswerConfiguration>();

                if (_applicationRole != null)
                {
                    appDecQandAConfig = _applicationRepo.GetApplicationDeclarationsByLETypeAndApplicationRoleAndOSP(legalEntity.LegalEntityType.Key, _applicationRole.ApplicationRoleType.Key, (int)SAHL.Common.Globals.GenericKeyTypes.OfferRoleType, osp.Key);
                    _applicationDeclarations = _applicationRole.ApplicationDeclarations;
                }

                if (appDecQandAConfig == null || appDecQandAConfig.Count == 0)
                    _view.SetViewForNullDeclarations();
                else
                    _view.BindDeclaration(appDecQandAConfig, _applicationDeclarations);
            }
        }

        protected void _view_onCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Cancel");
        }

        protected void _view_onUpdateButtonClicked(object sender, EventArgs e)
        {
            if (_applicationDeclarations != null && _applicationDeclarations.Count > 0)
                UpdateApplicationDeclarations();
            else
                AddApplicationDeclarations();

            if (_view.IsValid)
                _view.Navigator.Navigate(_view.UpdateButtonText);
        }

        private void UpdateApplicationDeclarations()
        {
            IEventList<IApplicationDeclaration> appDecLst = _view.GetOfferDeclarations;
            // Copy Values from New List to List from DB
            foreach (IApplicationDeclaration appDeclaration in _applicationDeclarations)
            {
                for (int i = 0; i < appDecLst.Count; i++)
                {
                    if (appDeclaration.ApplicationDeclarationQuestion.Key == appDecLst[i].ApplicationDeclarationQuestion.Key)
                    {
                        appDeclaration.ApplicationDeclarationAnswer = appDecLst[i].ApplicationDeclarationAnswer;
                        appDeclaration.ApplicationDeclarationDate = appDecLst[i].ApplicationDeclarationDate;
                        break;
                    }
                }
            }

            ValidateApplicationDeclarations(_applicationDeclarations);

            if (_view.Messages.ErrorMessages.Count == 0)
            {
                TransactionScope txn = new TransactionScope();
                try
                {
                    for (int i = 0; i < _applicationDeclarations.Count; i++)
                    {
                        _applicationRepo.SaveApplicationDeclaration(_applicationDeclarations[i]);
                    }
                    txn.VoteCommit();
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
            }
        }

        private void AddApplicationDeclarations()
        {
            IEventList<IApplicationDeclaration> appDecLst = _view.GetOfferDeclarations;

            ValidateApplicationDeclarations(appDecLst);

            if (_view.Messages.ErrorMessages.Count == 0)
            {
                TransactionScope txn = new TransactionScope();
                try
                {
                    for (int i = 0; i < appDecLst.Count; i++)
                    {
                        appDecLst[i].ApplicationRole = _applicationRole;
                        _applicationRepo.SaveApplicationDeclaration(appDecLst[i]);
                    }
                    txn.VoteCommit();
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
            }
        }
        
        /// <summary>
        ///  These rules are being done here because they do not affect the integrity of the domain and
        /// are taking very long to fire when fired from the rules project
        /// </summary>
        /// <param name="appDecLst"></param>
        private void ValidateApplicationDeclarations(IEventList<IApplicationDeclaration> appDecLst)
        {
            bool declaredInsolvent = false;
            bool underAdmin = false;
            for (int i = 0; i < appDecLst.Count; i++)
            {
                int answerKey = appDecLst[i].ApplicationDeclarationAnswer != null ? appDecLst[i].ApplicationDeclarationAnswer.Key : -1;
                
                DateTime? answerDate = null;
                if (appDecLst[i].ApplicationDeclarationDate.HasValue)
                    answerDate = appDecLst[i].ApplicationDeclarationDate.Value;

                switch (appDecLst[i].ApplicationDeclarationQuestion.Key)
                {
                    case (int)SAHL.Common.Globals.OfferDeclarationQuestions.DeclaredInsolvent:
                        if (answerKey == (int)OfferDeclarationAnswers.Yes)
                            declaredInsolvent = true;
                        break;
                    case (int)SAHL.Common.Globals.OfferDeclarationQuestions.UnderAministrationOrder:
                        if (answerKey == (int)OfferDeclarationAnswers.Yes)
                            underAdmin = true;
                        break;
                    case (int)SAHL.Common.Globals.OfferDeclarationQuestions.DateRehabilitatedFromInsolvency:
                        if (!declaredInsolvent && answerDate != null)
                            _view.Messages.Add(new Error("'Date rehabilitated from insolvency' must not be entered.", "'Date rehabilitated from insolvency' must not be entered."));
                        if (declaredInsolvent && answerDate == null)
                            _view.Messages.Add(new Error("'Date rehabilitated from insolvency' must be entered.", "'Date rehabilitated from insolvency' must be entered."));
                        break;
                    case (int)SAHL.Common.Globals.OfferDeclarationQuestions.DateAdministrationOrderRescinded:
                        if (!underAdmin && answerDate != null)
                            _view.Messages.Add(new Error("'Date administration order rescinded' must not be entered.", "'Date administration order rescinded' must not be entered."));
                        if (underAdmin && answerDate == null)
                            _view.Messages.Add(new Error("'Date administration order rescinded' must be entered.", "'Date administration order rescinded' must be entered."));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
