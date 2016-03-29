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
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.ITC
{
    public class ApplicationITC : BaseITC
    {
        private int _applicationKey;
        private IApplication _application;
        private IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

        /// <summary>
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ApplicationITC(SAHL.Web.Views.Common.Interfaces.IITC view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            if (!_view.ShouldRunPage)
                return;

            _view.OnDoEnquiryButtonClicked += new EventHandler(_view_OnDoEnquiryButtonClicked);
            _view.OnViewHistoryButtonClicked += new EventHandler(_view_OnViewHistoryButtonClicked);

            base.OnViewInitialised(sender, e);

            // get the application object
            _applicationKey = base.GenericKey;
            _application = appRepo.GetApplicationByKey(base.GenericKey);

            base.AccountSequence = _application.ReservedAccount;

            if (_application is IApplicationMortgageLoan)
            {
                //offer roles exist and should be used

                // this if block is untested!!!

                // get the legal entity roles off the application
                int[] roles =
                    {
                        (int)SAHL.Common.Globals.OfferRoleTypes.MainApplicant,
                        (int)SAHL.Common.Globals.OfferRoleTypes.Suretor,
                        (int)SAHL.Common.Globals.OfferRoleTypes.LeadMainApplicant,
                        (int)SAHL.Common.Globals.OfferRoleTypes.LeadSuretor
                    };

                base.ListLE = _application.GetNaturalPersonLegalEntitiesByRoleType(_view.Messages, roles);
            }
            else
            {
                base.ListLE = _application.GetNaturalPersonsByExternalRoleType(ExternalRoleTypes.Client, GeneralStatuses.Active);
            }

            PopulateITCList();
        }

        /// <summary>
        ///
        /// </summary>
        private void PopulateITCList()
        {
            // get the list of existing itc's for all the legalentities
            int[] legalEntityKeys = new int[base.ListLE.Count];
            for (int i = 0; i < base.ListLE.Count; i++)
            {
                legalEntityKeys[i] = base.ListLE[i].Key;
            }
            IList<SAHL.Common.BusinessModel.Interfaces.IITC> listITC = ITCRepo.GetITCByLegalEntityKeys(legalEntityKeys);

            List<BindableITC> listBindableITC = new List<BindableITC>();
            bool redoITC;

            if (base.ListLE != null)
            {
                foreach (ILegalEntityNaturalPerson le in base.ListLE)
                {
                    SAHL.Common.BusinessModel.Interfaces.IITC itc = null;

                    foreach (SAHL.Common.BusinessModel.Interfaces.IITC itcItem in listITC)
                    {
                        if (itcItem.LegalEntity.Key == le.Key)
                        {
                            itc = itcItem;
                            break;
                        }
                    }

                    if (itc == null)
                    {
                        itc = ITCRepo.GetEmptyITC();
                        itc.ReservedAccount = base.AccountSequence;
                    }

                    DataTable listArchiveITC = ITCRepo.GetArchivedITCByLegalEntityKey(le.Key, base.AccountSequence.Key);

                    redoITC = false;
                    //quick and dirty rule, should really convert the data to xml and xpath query....
                    //check for Dispute or SAFPS, otherwise must be at least 5 days between ITC queries
                    if (itc.ChangeDate != null && itc.ResponseXML != null)
                    {
                        if (itc.ChangeDate < DateTime.Now.AddDays(-5) || itc.ResponseXML.Contains("DisputeIndicator") || itc.ResponseXML.Contains("SAFPSNF01") || itc.ResponseXML.Contains("ErrorMessage"))
                            redoITC = true;
                    }
                    else
                        redoITC = true;

                    //Determine if the Client allowed ITC on the application
                    //Declaration check is currently only enforced on MortgageLoanApplications
                    bool performITC = true;
                    if (_application is IApplicationMortgageLoan || _application is IApplicationUnsecuredLending)
                    {
                        if ((int)OfferDeclarationAnswers.Yes != appRepo.GetApplicationDeclarationAnswerToQuestion(le.Key, _applicationKey, (int)OfferDeclarationQuestions.PerformITC))
                        {
                            performITC = false;
                        }
                    }

                    listBindableITC.Add(new BindableITC(itc, le, listArchiveITC.Rows.Count, redoITC, performITC));
                }
            }

            _view.BackButtonVisible = false;
            _view.BindITCGrid(listBindableITC);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_OnDoEnquiryButtonClicked(object sender, EventArgs e)
        {
            List<Int32> listITCDoEnquiry = _view.ListITCDoEnquiry;
            IITCService itcService = ServiceFactory.GetService<IITCService>();
            ILegalEntityNaturalPerson lenp = null;

            if (listITCDoEnquiry != null)
            {
                foreach (Int32 leKey in listITCDoEnquiry)
                {
                    foreach (ILegalEntity le in base.ListLE)
                    {
                        if (leKey == le.Key)
                        {
                            lenp = le as ILegalEntityNaturalPerson;

                            if (lenp != null)
                            {
                                SAHL.Common.BusinessModel.Interfaces.IITC saveITC = ITCRepo.GetEmptyITC();

                                saveITC.ReservedAccount = base.AccountSequence;
                                saveITC.LegalEntity = lenp;
                                saveITC.UserID = _view.CurrentPrincipal.Identity.Name;

                                IList<IAddressStreet> listStreetAddress = ITCRepo.GetITCAddressListByLegalEntityKey(lenp.Key);

                                // call validate here so that all rules regarding the creation of ITC's can be run
                                // this should be done before the request is made, avoiding false calls to TU ITC
                                saveITC.ValidateEntity();

                                // only do the enquiry if no messages have been added
                                if (_view.IsValid)
                                    itcService.DoEnquiry(lenp, listStreetAddress, saveITC);
                            }
                        }
                    }
                }
            }

            if (_view.IsValid)
                Navigator.Navigate(_view.ViewName);
            else
                PopulateITCList();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_OnViewHistoryButtonClicked(object sender, EventArgs e)
        {
            IList<ICacheObjectLifeTime> lifeTimes = new List<ICacheObjectLifeTime>();
            if (GlobalCacheData.ContainsKey("LegalEntityKey")) GlobalCacheData.Remove("LegalEntityKey");
            GlobalCacheData.Add("LegalEntityKey", _view.LegalEntityKeyForHistory, lifeTimes);

            //PrivateCacheData.Add("KEY", _view.LegalEntityKeyForHistory);
            Navigator.Navigate("History");
        }
    }
}