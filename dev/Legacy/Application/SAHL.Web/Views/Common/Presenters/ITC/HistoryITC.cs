using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.ITC
{
    public class HistoryITC : SAHLCommonBasePresenter<SAHL.Web.Views.Common.Interfaces.IITC>
    {
        private Int32 _legalEntityKey;
        private IITCRepository _itcRepo;
        private ILegalEntityRepository _leRepo;

        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public HistoryITC(SAHL.Web.Views.Common.Interfaces.IITC view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
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

            //Account Key is hardcoded to 0 below so that this accounts current ITC
            //is not excluded
            DataTable listArchive = ITCRepo.GetArchivedITCByLegalEntityKey(_legalEntityKey, 0);

            List<BindableITC> listArchiveITC = new List<BindableITC>();
            foreach (DataRow archiveITC in listArchive.Rows)
            {
                listArchiveITC.Add(new BindableITC(archiveITC, lenp));
            }

            _view.BindITCGrid(listArchiveITC);
            _view.DoEnquiryColumnVisible = false;
            _view.ViewHistoryColumnVisible = false;
            _view.DoEnquiryButtonVisible = false;
            _view.StatusColumnVisible = false;
            _view.HeaderCaption = "All ITC's";
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