using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.ITC
{
    /// <summary>
    ///
    /// </summary>
    public class BaseITC : SAHLCommonBasePresenter<SAHL.Web.Views.Common.Interfaces.IITC>
    {
        private IReadOnlyEventList<ILegalEntityNaturalPerson> _listLE;
        private IList<SAHL.Common.BusinessModel.Interfaces.IITC> listITC;
        private IITCRepository _itcRepo;
        private CBOMenuNode _node;
        private IAccountSequence _accountSequence;

        private int _genericKey;

        public int GenericKey
        {
            get { return _genericKey; }
            set { _genericKey = value; }
        }

        public CBOMenuNode Node
        {
            get { return _node; }
            set { _node = value; }
        }

        public IAccountSequence AccountSequence
        {
            get { return _accountSequence; }
            set { _accountSequence = value; }
        }

        public IReadOnlyEventList<ILegalEntityNaturalPerson> ListLE
        {
            get { return _listLE; }
            set { _listLE = value; }
        }

        public IList<SAHL.Common.BusinessModel.Interfaces.IITC> ListITC
        {
            get { return listITC; }
            set { listITC = value; }
        }

        public IITCRepository ITCRepo
        {
            get
            {
                if (_itcRepo == null)
                    _itcRepo = RepositoryFactory.GetRepository<IITCRepository>();

                return _itcRepo;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public BaseITC(SAHL.Web.Views.Common.Interfaces.IITC view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            // Get the CBO Node
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            _genericKey = _node.GenericKey;

            //_view.OnDoEnquiryButtonClicked += new EventHandler(_view_OnDoEnquiryButtonClicked);
            //_view.OnViewHistoryButtonClicked += new EventHandler(_view_OnViewHistoryButtonClicked);
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            //PopulateITCList();
        }

        ///// <summary>
        /////
        ///// </summary>
        //private void PopulateITCList()
        //{
        //    // get the list of previous itc's
        //    IList<SAHL.Common.BusinessModel.Interfaces.IITC> listITC = ITCRepo.GetITCByAccountKey(_accountSequence.Key);

        //    List<BindableITC> listBindableITC = new List<BindableITC>();
        //    bool redoITC;

        //    if (_listLE != null)
        //    {
        //        foreach (ILegalEntityNaturalPerson le in _listLE)
        //        {
        //            SAHL.Common.BusinessModel.Interfaces.IITC itc = null;

        //            foreach (SAHL.Common.BusinessModel.Interfaces.IITC itcItem in listITC)
        //            {
        //                if (itcItem.LegalEntity.Key == le.Key)
        //                    itc = itcItem;
        //            }

        //            if (itc == null)
        //            {
        //                itc = ITCRepo.GetEmptyITC();
        //                //itc.ResponseStatus = "No ITC Done";
        //                itc.ReservedAccount = _accountSequence;
        //            }

        //            DataTable listArchiveITC = ITCRepo.GetArchivedITCByLegalEntityKey(le.Key, _accountSequence.Key);

        //            redoITC = false;
        //            //quick and dirty rule, should really convert the data to xml and xpath query....
        //            //check for Dispute or SAFPS, otherwise must be at least 5 days between ITC queries
        //            if (itc.ChangeDate != null && itc.ResponseXML != null)
        //            {
        //                if (itc.ChangeDate < DateTime.Now.AddDays(-5) || itc.ResponseXML.Contains("DisputeIndicator") || itc.ResponseXML.Contains("SAFPSNF01") || itc.ResponseXML.Contains("ErrorMessage"))
        //                    redoITC = true;
        //            }
        //            else
        //                redoITC = true;

        //            //Check if the selected node is an Application, or an Account
        //            bool performITC = false;
        //            IApplication application = null;
        //            if (_node.GenericKeyTypeKey == (int)SAHL.Common.Globals.GenericKeyTypes.Offer)
        //            {
        //                //Determine if the Client allowed ITC on the application
        //                IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
        //                application = appRepo.GetApplicationByKey(_applicationKey);
        //                int offerDeclarationAnswerKey = appRepo.GetApplicationDeclarationAnswerToQuestion(le.Key, _applicationKey, (int)OfferDeclarationQuestions.PerformITC);

        //                if (offerDeclarationAnswerKey == (int)OfferDeclarationAnswers.Yes)
        //                {
        //                    performITC = true;
        //                }
        //            }
        //            else
        //                performITC = true; //There is currently no restriction on doing Enquiries on Accounts, so always set this to true

        //            // TODO: Clinton's hack until we do the OfferDeclaration story for Personal Loans
        //            if (application != null && application is IApplicationUnsecuredLending)
        //                performITC = true;

        //            listBindableITC.Add(new BindableITC(itc, le, listArchiveITC.Rows.Count, redoITC, performITC));
        //        }
        //    }

        //    _view.BackButtonVisible = false;
        //    _view.BindITCGrid(listBindableITC);
        //}

        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void _view_OnDoEnquiryButtonClicked(object sender, EventArgs e)
        //{
        //    List<Int32> listITCDoEnquiry = _view.ListITCDoEnquiry;
        //    IITCService itcService = ServiceFactory.GetService<IITCService>();
        //    ILegalEntityNaturalPerson lenp = null;

        //    if (listITCDoEnquiry != null)
        //    {
        //        foreach (Int32 leKey in listITCDoEnquiry)
        //        {
        //            foreach (ILegalEntity le in _listLE)
        //            {
        //                if (leKey == le.Key)
        //                {
        //                    lenp = le as ILegalEntityNaturalPerson;

        //                    if (lenp != null)
        //                    {
        //                        SAHL.Common.BusinessModel.Interfaces.IITC saveITC = ITCRepo.GetEmptyITC();

        //                        saveITC.ReservedAccount = _accountSequence;
        //                        saveITC.LegalEntity = lenp;
        //                        saveITC.UserID = _view.CurrentPrincipal.Identity.Name;

        //                        IList<IAddressStreet> listStreetAddress = ITCRepo.GetITCAddressListByLegalEntityKey(lenp.Key);

        //                        // call validate here so that all rules regarding the creation of ITC's can be run
        //                        // this should be done before the request is made, avoiding false calls to TU ITC
        //                        saveITC.ValidateEntity();

        //                        // only do the enquiry if no messages have been added
        //                        if (_view.IsValid)
        //                            itcService.DoEnquiry(lenp, listStreetAddress, saveITC);
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    if (_view.IsValid)
        //        Navigator.Navigate(_view.ViewName);
        //    else
        //        PopulateITCList();
        //}

        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void _view_OnViewHistoryButtonClicked(object sender, EventArgs e)
        //{
        //    IList<ICacheObjectLifeTime> lifeTimes = new List<ICacheObjectLifeTime>();
        //    if (GlobalCacheData.ContainsKey("LegalEntityKey")) GlobalCacheData.Remove("LegalEntityKey");
        //    GlobalCacheData.Add("LegalEntityKey", _view.LegalEntityKeyForHistory, lifeTimes);

        //    //PrivateCacheData.Add("KEY", _view.LegalEntityKeyForHistory);
        //    Navigator.Navigate("History");
        //}
    }
}