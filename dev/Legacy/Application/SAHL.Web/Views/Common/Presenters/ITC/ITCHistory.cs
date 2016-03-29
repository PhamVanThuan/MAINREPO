using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.ITC
{
    public class ITCHistory : SAHLCommonBasePresenter<SAHL.Web.Views.Common.Interfaces.IITC>
    {
        private CBOMenuNode _cboNode;
        private Int32 _accountKey;
        private Int32 _legalEntityKey;
        private IITCRepository _itcRepo;
        private ILegalEntityRepository _leRepo;

        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ITCHistory(SAHL.Web.Views.Common.Interfaces.IITC view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            // Get the CBO Node
            _cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_cboNode == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            // Get the NaturalPerson LegalEntity
            if (!GlobalCacheData.ContainsKey("LegalEntityKey"))
                throw new NullReferenceException("No Legal Entity found");

            _legalEntityKey = (int)GlobalCacheData["LegalEntityKey"];

            ILegalEntity le = LERepo.GetLegalEntityByKey(_legalEntityKey);
            ILegalEntityNaturalPerson lenp = le as ILegalEntityNaturalPerson;
            if (lenp == null)
                throw new NullReferenceException("No Legal Entity Natural Person found");

            switch (_cboNode.GenericKeyTypeKey)
            {
                case (int)SAHL.Common.Globals.GenericKeyTypes.Account:
                    _accountKey = Convert.ToInt32(_cboNode.GenericKey);
                    break;
                case (int)SAHL.Common.Globals.GenericKeyTypes.Offer:
                    IApplicationRepository appRep = RepositoryFactory.GetRepository<IApplicationRepository>();
                    IApplication app = appRep.GetApplicationByKey(_cboNode.GenericKey);
                    _accountKey = app.ReservedAccount.Key;
                    break;
                default:
                    break;
            }

            DataTable listArchive = ITCRepo.GetArchivedITCByLegalEntityKey(_legalEntityKey, _accountKey);
            List<BindableITC> listArchiveITC = new List<BindableITC>();

            foreach (DataRow archiveITC in listArchive.Rows)
            {
                listArchiveITC.Add(new BindableITC(archiveITC, lenp));
            }

            //IList<SAHL.Common.BusinessModel.Interfaces.IITC> listOther = ITCRepo.GetITCForHistoryByLeExcludingAccount(_legalEntityKey, _accountKey);
            //List<BindableITC> listOtherITC = new List<BindableITC>();
            //foreach (SAHL.Common.BusinessModel.Interfaces.IITC otherITC in listOther)
            //{
            //    listOtherITC.Add(new BindableITC(otherITC, lenp, 0, false, false));
            //}

            _view.BindITCGrid(listArchiveITC);
            //_view.BindOtherAccountITCGrid(listOtherITC);
            //_view.AccountColumnVisible = true;
            _view.DoEnquiryColumnVisible = false;
            _view.ViewHistoryColumnVisible = false;
            _view.DoEnquiryButtonVisible = false;
            _view.StatusColumnVisible = false;
            _view.HeaderCaption = "Archived ITC's";
        }

        /// <summary>
        ///
        /// </summary>
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
        public ILegalEntityRepository LERepo
        {
            get
            {
                if (_leRepo == null)
                    _leRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();

                return _leRepo;
            }
        }
    }
}