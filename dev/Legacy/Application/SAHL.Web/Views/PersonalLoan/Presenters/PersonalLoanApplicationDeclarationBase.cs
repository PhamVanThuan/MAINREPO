using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.PersonalLoan.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Web.Views.PersonalLoan.Presenters
{
    public class PersonalLoanApplicationDeclarationBase : SAHLCommonBasePresenter<IPersonalLoanApplicationDeclaration>
    {
        private IApplicationRepository _applicationRepo;
        private ILegalEntityRepository _legalEntityRepo;
        private int _legalEntityKey;
        private int _applicationKey;

        private IExternalRole _externalRole;
        private IEventList<IExternalRoleDeclaration> _externalRoleDeclarations;

        public PersonalLoanApplicationDeclarationBase(IPersonalLoanApplicationDeclaration view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

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

            _externalRole = null;

            if (application != null && legalEntity != null)
            {
                var externalRoles = _legalEntityRepo.GetExternalRoles(GenericKeyTypes.Offer, ExternalRoleTypes.Client, legalEntity.Key);

                foreach (var externalRole in externalRoles)
                {
                    if (externalRole.GenericKey == application.Key && externalRole.GeneralStatus.Key == (int)GeneralStatuses.Active)
                    {
                        _externalRole = externalRole;
                        break;
                    }
                }

                _view.ConfigurePanel(legalEntity.DisplayName);

                IApplicationInformation appInfo = application.GetLatestApplicationInformation();
                IOriginationSourceProduct osp = _applicationRepo.GetOriginationSourceProductBySourceAndProduct(application.OriginationSource.Key, (int)Products.PersonalLoan);
                IList<IApplicationDeclarationQuestionAnswerConfiguration> appDecQandAConfig = new List<IApplicationDeclarationQuestionAnswerConfiguration>();

                if (_externalRole != null)
                {
                    appDecQandAConfig = _applicationRepo.GetApplicationDeclarationsByLETypeAndApplicationRoleAndOSP(legalEntity.LegalEntityType.Key, _externalRole.ExternalRoleType.Key, (int)SAHL.Common.Globals.GenericKeyTypes.ExternalRoleType, osp.Key);
                    _externalRoleDeclarations = _externalRole.ExternalRoleDeclarations;
                }

                if (appDecQandAConfig == null || appDecQandAConfig.Count == 0)
                    _view.SetViewForNullDeclarations();
                else
                    _view.BindDeclaration(appDecQandAConfig, _externalRoleDeclarations);
            }
        }

        protected void _view_onCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Cancel");
        }

        protected void _view_onUpdateButtonClicked(object sender, EventArgs e)
        {
            if (_externalRoleDeclarations != null && _externalRoleDeclarations.Count > 0)
                UpdateApplicationDeclarations();
            else
                AddApplicationDeclarations();

            if (_view.IsValid)
                _view.Navigator.Navigate(_view.UpdateButtonText);
        }

        private void UpdateApplicationDeclarations()
        {
            IEventList<IExternalRoleDeclaration> appDecLst = _view.GetExternalRoleDeclarations;

            // Copy Values from New List to List from DB
            foreach (IExternalRoleDeclaration externalRoleDeclaration in _externalRoleDeclarations)
            {
                for (int i = 0; i < appDecLst.Count; i++)
                {
                    if (externalRoleDeclaration.ApplicationDeclarationQuestion.Key == appDecLst[i].ApplicationDeclarationQuestion.Key)
                    {
                        externalRoleDeclaration.ApplicationDeclarationAnswer = appDecLst[i].ApplicationDeclarationAnswer;
                        externalRoleDeclaration.ExternalRoleDeclarationDate = appDecLst[i].ExternalRoleDeclarationDate;
                        break;
                    }
                }
            }

            ValidateApplicationDeclarations(_externalRoleDeclarations);

            if (_view.Messages.ErrorMessages.Count == 0)
            {
                TransactionScope txn = new TransactionScope();
                try
                {
                    for (int i = 0; i < _externalRoleDeclarations.Count; i++)
                    {
                        _applicationRepo.SaveExternalRoleDeclaration(_externalRoleDeclarations[i]);
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
            IEventList<IExternalRoleDeclaration> externalRoleDecLst = _view.GetExternalRoleDeclarations;

            ValidateApplicationDeclarations(externalRoleDecLst);

            if (_view.Messages.ErrorMessages.Count == 0)
            {
                TransactionScope txn = new TransactionScope();
                try
                {
                    for (int i = 0; i < externalRoleDecLst.Count; i++)
                    {
                        externalRoleDecLst[i].ExternalRole = _externalRole;

                        _applicationRepo.SaveExternalRoleDeclaration(externalRoleDecLst[i]);
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
        /// <param name="externalRoleDecLst"></param>
        private void ValidateApplicationDeclarations(IEventList<IExternalRoleDeclaration> externalRoleDecLst)
        {
            bool declaredInsolvent = false;
            bool underAdmin = false;
            for (int i = 0; i < externalRoleDecLst.Count; i++)
            {
                int answerKey = externalRoleDecLst[i].ApplicationDeclarationAnswer != null ? externalRoleDecLst[i].ApplicationDeclarationAnswer.Key : -1;

                DateTime? answerDate = null;
                if (externalRoleDecLst[i].ExternalRoleDeclarationDate.HasValue)
                    answerDate = externalRoleDecLst[i].ExternalRoleDeclarationDate.Value;

                switch (externalRoleDecLst[i].ApplicationDeclarationQuestion.Key)
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