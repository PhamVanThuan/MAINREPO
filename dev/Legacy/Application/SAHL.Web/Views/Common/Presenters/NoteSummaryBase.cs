using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;

namespace SAHL.Web.Views.Common.Presenters
{
    public class NoteSummaryBase : SAHLCommonBasePresenter<INoteSummary>
    {
        protected CBOMenuNode _node;
        private int _genericKey;
        private int _genericKeyTypeKey;
        private List<INoteDetail> _noteDetails;
        private INoteRepository _noteRepo;
        private IDebtCounsellingRepository _dcRepo;
        private DateTime _diaryDate;

        public NoteSummaryBase(INoteSummary view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            // Get the CBO Node   
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            _genericKey = _node.GenericKey;
            _genericKeyTypeKey = _node.GenericKeyTypeKey;
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            
            if (!_view.ShouldRunPage) 
                return;

            _noteRepo = RepositoryFactory.GetRepository<INoteRepository>();
            _dcRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();


            if (_genericKeyTypeKey == (int)SAHL.Common.Globals.GenericKeyTypes.DebtCounselling2AM)
            {
                IDebtCounselling debtCounselling = _dcRepo.GetDebtCounsellingByKey(_genericKey);

                // get the notes for all debtcounselling cases for the account
                _noteDetails = _noteRepo.GetAllDebtcounsellingNoteDetailsForAccount(debtCounselling.Account.Key);
                
                // get the diary date
                if (debtCounselling != null && debtCounselling.DiaryDate.HasValue)
                    _diaryDate = debtCounselling.DiaryDate.Value;
            }
            else
            {
                // get the notes
                _noteDetails = _noteRepo.GetNoteDetailsByGenericKeyAndType(_genericKey, _genericKeyTypeKey);
            }

            // set screen values
            _view.GenericKey = _genericKey;
            _view.GenericKeyTypeKey = _genericKeyTypeKey;
            _view.DiaryDate = _diaryDate;
            
            // bind the notes
            _view.BindNotesGrid(_noteDetails);          

            // get a unique list of legalentities from the notes and populate user filter control
            var legalEntities = (from n in _noteDetails orderby n.LegalEntity.DisplayName select n.LegalEntity).Distinct().ToList();
            _view.BindLegalEntityFilter(legalEntities);

            // get a unique list of dates from the notes and populate date filter controls
            Dictionary<string, string> uniqueDates = new Dictionary<string, string>();
            var dates = (from n in _noteDetails orderby n.InsertedDate ascending select n.InsertedDate.Date).Distinct().ToList();
            foreach (var date in dates)
            {
               uniqueDates.Add(date.ToString("yyyyMMdd"), date.ToString(SAHL.Common.Constants.DateFormat));
            }

            _view.BindDateFilters(uniqueDates);                      
        }
    }
}