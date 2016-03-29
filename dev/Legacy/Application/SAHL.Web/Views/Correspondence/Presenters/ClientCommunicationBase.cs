using System;
using System.Collections.Generic;
using SAHL.Web.Views.Correspondence.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.CacheData;
using System.Text;
using SAHL.Common.Service.Interfaces;
using System.IO;
using SAHL.Common.DomainMessages;
using SAHL.Common.Collections;
using System.Security.Principal;
using System.Text.RegularExpressions;
using Castle.ActiveRecord;

namespace SAHL.Web.Views.Correspondence.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class ClientCommunicationBase : SAHLCommonBasePresenter<IClientCommunication>
    {
        private CBOMenuNode _node;
        private IADUser _adUser;
        private int _genericKey, _genericKeyTypeKey;
        protected List<ICacheObjectLifeTime> _lifeTimes;
        protected virtual List<ICacheObjectLifeTime> LifeTimes
        {
            get
            {
                if (_lifeTimes == null)
                {
                    List<string> views = new List<string>();

                    _lifeTimes = new List<ICacheObjectLifeTime>();
                    _lifeTimes.Add(new SimplePageCacheObjectLifeTime(views));
                }
                return _lifeTimes;
            }
        }

        public CBOMenuNode Node
        {
            get { return _node; }
        }

        public int GenericKey
        {
            get { return _genericKey; }
            protected set { _genericKey = value; }

        }

        public int GenericKeyTypeKey
        {
            get { return _genericKeyTypeKey; }
        }

        public IList<BindableRecipient> RecipientsList { get; set; }

        public IList<SAHL.Common.Globals.SMSTypes> SMSTypes { get; set; }

        private ICorrespondenceRepository _correspondenceRepo;
        public ICorrespondenceRepository CorrespondenceRepo
        {
            get
            {
                if (_correspondenceRepo == null)
                    _correspondenceRepo = RepositoryFactory.GetRepository<ICorrespondenceRepository>();

                return _correspondenceRepo;
            }
        }

        private IReportRepository _reportRepo;
        public IReportRepository ReportRepo
        {
            get
            {
                if (_reportRepo == null)
                    _reportRepo = RepositoryFactory.GetRepository<IReportRepository>();

                return _reportRepo;
            }
        }

        private ILookupRepository _lookupRepo;
        public ILookupRepository LookupRepo
        {
            get
            {
                if (_lookupRepo == null)
                    _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

                return _lookupRepo;
            }
        }

        private ILegalEntityRepository _legalEntityRepo;
        public ILegalEntityRepository LegalEntityRepo
        {
            get
            {
                if (_legalEntityRepo == null)
                    _legalEntityRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();

                return _legalEntityRepo;
            }
        }

        private IOrganisationStructureRepository _osRepo;
        public IOrganisationStructureRepository OSRepo
        {
            get
            {
                if (_osRepo == null)
                    _osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();

                return _osRepo;
            }
        }
        private IApplicationRepository applicationRepository;
        public IApplicationRepository ApplicationRepository
        {
            get
            {
                if (this.applicationRepository == null)
                {
                    this.applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();
                }
                return this.applicationRepository;
            }
        }

        //private IControlRepository _controlRepo;
        //public IControlRepository ControlRepo
        //{
        //    get
        //    {
        //        if (_controlRepo == null)
        //            _controlRepo = RepositoryFactory.GetRepository<IControlRepository>();

        //        return _controlRepo;
        //    }
        //}

        private ICommonRepository _cRepo;

        public ICommonRepository CRepo
        {
            get
            {
                if (_cRepo == null)
                    _cRepo = RepositoryFactory.GetRepository<ICommonRepository>();

                return _cRepo;
            }
        }

        /// <summary>
        /// Gets the <see cref="IADUser"/> for the current principal.
        /// </summary>
        protected IADUser CurrentADUser
        {
            get
            {
                if (_adUser == null)
                {
                    ISecurityRepository secRepo = RepositoryFactory.GetRepository<ISecurityRepository>();
                    _adUser = secRepo.GetADUserByPrincipal(_view.CurrentPrincipal);
                }
                return _adUser;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ClientCommunicationBase(IClientCommunication view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            // Get the CBO Node   
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            _genericKey = _node.GenericKey;
            _genericKeyTypeKey = _node.GenericKeyTypeKey;

            _view.GenericKey = _genericKey;
            _view.GenericKeyTypeKey = _genericKeyTypeKey;

            // default mode is Email
            _view.CorrespondenceMedium = SAHL.Common.Globals.CorrespondenceMediums.Email;

            RecipientsList = new List<BindableRecipient>();

            SMSTypes = new List<SMSTypes>();
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            // initialise events
            _view.OnSendButtonClicked += new KeyChangedEventHandler(OnSendButtonClicked);
            _view.OnCancelButtonClicked += new EventHandler(OnCancelButtonClicked);

            // Bind Recipients Grid 
            _view.BindRecipients(RecipientsList);

            // Bind the SMSTypes
            _view.BindSMSTypes(SMSTypes);

        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;

            if (!_view.IsPostBack)
            {
                this.GlobalCacheData.Remove("ClientCommunication");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Cancel");
        }

        protected virtual void OnSendButtonClicked(object sender, KeyChangedEventArgs e)
        {
            TransactionScope txn = new TransactionScope();
            try
            {
                // get selected info
                SelectedClientCommuncation selectedClientCommunication = e.Key as SelectedClientCommuncation;

                //Store the selected client communication info in the cache
                if (this.GlobalCacheData.ContainsKey("ClientCommunication"))
                {
                    this.GlobalCacheData.Remove("ClientCommunication");
                }
                this.GlobalCacheData.Add("ClientCommunication", selectedClientCommunication, LifeTimes);

                // perform screen validation
                if (ValidateScreenInput(selectedClientCommunication) == false)
                    return;

                // get the report statement entry
                IReportStatement reportStatement = null;
                switch (selectedClientCommunication.CorrespondenceMedium)
                {
                    case CorrespondenceMediums.Email:
                        reportStatement = ReportRepo.GetReportStatementByName("Adhoc - Email")[0];
                        break;
                    case CorrespondenceMediums.SMS:
                        reportStatement = ReportRepo.GetReportStatementByName("Adhoc - SMS")[0];
                        break;
                    default:
                        break;
                }

                if (reportStatement == null)
                    throw new Exception("Client Communication failed - cannot find reportstatement entry");

                // loop thru each selected recipient and save their correspondence and client email records
                foreach (var recipient in selectedClientCommunication.SelectedRecipients)
                {
                    #region save correspondence and correspondencedetail
                    ICorrespondence correspondence = CorrespondenceRepo.CreateEmptyCorrespondence();

                    correspondence.GenericKey = _genericKey;
                    correspondence.GenericKeyType = LookupRepo.GenericKeyType.ObjectDictionary[Convert.ToString(_genericKeyTypeKey)];
                    correspondence.ReportStatement = reportStatement;
                    correspondence.CorrespondenceMedium = LookupRepo.CorrespondenceMediums.ObjectDictionary[Convert.ToString((int)selectedClientCommunication.CorrespondenceMedium)];

                    switch (selectedClientCommunication.CorrespondenceMedium)
                    {
                        case CorrespondenceMediums.Email:
                            correspondence.DestinationValue = recipient.EmailAddress;
                            break;
                        case CorrespondenceMediums.SMS:
                            correspondence.DestinationValue = recipient.CellPhoneNumber;
                            break;
                        default:
                            break;
                    }

                    DateTime now = DateTime.Now;
                    correspondence.DueDate = now;
                    correspondence.ChangeDate = now;
                    correspondence.CompletedDate = now;
                    correspondence.UserID = _view.CurrentPrincipal.Identity.Name;
                    correspondence.LegalEntity = LegalEntityRepo.GetLegalEntityByKey(recipient.Key);

                    ICorrespondenceDetail correspondenceDetail = CorrespondenceRepo.CreateEmptyCorrespondenceDetail();
                    correspondenceDetail.Correspondence = correspondence;
                    correspondenceDetail.CorrespondenceText = selectedClientCommunication.Body;

                    correspondence.CorrespondenceDetail = correspondenceDetail;

                    CorrespondenceRepo.SaveCorrespondence(correspondence);

                    #endregion

                    #region save client email
                    IMessageService messageService = ServiceFactory.GetService<IMessageService>();
                    switch (selectedClientCommunication.CorrespondenceMedium)
                    {
                        case CorrespondenceMediums.Email:
                            string userEmailAddress = CurrentADUser.LegalEntity.EmailAddress;

                            bool emailSuccess = messageService.SendEmailExternal(_genericKey, userEmailAddress, recipient.EmailAddress, "", "", selectedClientCommunication.Subject, selectedClientCommunication.Body, "", "", "", ContentTypes.HTML);
                            if (emailSuccess == false)
                                throw new Exception("Client Communication failed - Email Failed");
                            break;
                        case CorrespondenceMediums.SMS:
                            bool smsSuccess = messageService.SendSMS(_genericKey, selectedClientCommunication.Body, recipient.CellPhoneNumber);
                            if (smsSuccess == false)
                                throw new Exception("Client Communication failed - SMS Failed");
                            break;
                        default:
                            break;
                    }
                    #endregion
                }

                // all done - lets navigate
                _view.Navigator.Navigate("Submit");
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

        private bool ValidateScreenInput(SelectedClientCommuncation selectedClientCommunication)
        {
            bool valid = true;
            string errorMessage = "";

            // validate that at least one recipient has been selected
            if (selectedClientCommunication.SelectedRecipients.Count == 0)
            {
                errorMessage = "Must select at least one recipient.";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
                valid = false;
            }

            switch (_view.CorrespondenceMedium)
            {
                case CorrespondenceMediums.Email:
                    if (String.IsNullOrEmpty(selectedClientCommunication.Subject))
                    {
                        errorMessage = "Email Subject must be entered.";
                        _view.Messages.Add(new Error(errorMessage, errorMessage));
                        valid = false;
                    }
                    else if (selectedClientCommunication.Subject.Length > 80)
                    {
                        errorMessage = "Email Subject must be less than 80 chars. (current length=" + selectedClientCommunication.Subject.Length.ToString() + "chars).";
                        _view.Messages.Add(new Error(errorMessage, errorMessage));
                        valid = false;
                    }

                    if (String.IsNullOrEmpty(selectedClientCommunication.Body))
                    {
                        errorMessage = "Email Body must be entered.";
                        _view.Messages.Add(new Error(errorMessage, errorMessage));
                        valid = false;
                    }
                    break;
                case CorrespondenceMediums.SMS:
                    if (String.Compare(selectedClientCommunication.SMSType, SAHL.Common.Constants.DefaultDropDownItem, true) == 0)
                    {
                        errorMessage = "SMS Type must be selected.";
                        _view.Messages.Add(new Error(errorMessage, errorMessage));
                        valid = false;
                        break;
                    }

                    if (String.IsNullOrEmpty(selectedClientCommunication.Body))
                    {
                        errorMessage = "SMS Text must be entered.";
                        _view.Messages.Add(new Error(errorMessage, errorMessage));
                        valid = false;
                    }
                    else if (selectedClientCommunication.Body.Length > 160)
                    {
                        errorMessage = "SMS Text must be less than 160 chars. (current length=" + selectedClientCommunication.Body.Length.ToString() + "chars).";
                        _view.Messages.Add(new Error(errorMessage, errorMessage));
                        valid = false;
                    }
                    break;
                default:
                    break;
            }

            return valid;

        }
    }
}
