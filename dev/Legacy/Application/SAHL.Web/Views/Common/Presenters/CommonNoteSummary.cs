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
    public class CommonNoteSummary : SAHLCommonBasePresenter<INoteSummary>
    {
        protected CBOMenuNode node;
		private INote note;
        private int genericKey;
        private int genericKeyTypeKey;
        private List<INoteDetail> noteDetails;
        private INoteRepository noteRepository;

		public CommonNoteSummary(INoteSummary view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            // Get the CBO Node   
            node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            genericKey = node.GenericKey;
            genericKeyTypeKey = node.GenericKeyTypeKey;
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            
            if (!_view.ShouldRunPage) 
                return;

            noteRepository = RepositoryFactory.GetRepository<INoteRepository>();

			note = noteRepository.GetNoteByGenericKeyAndType(genericKey, genericKeyTypeKey);
			if (note == null)
			{
				note = noteRepository.CreateEmptyNote();
			}

			noteDetails = note.NoteDetails.ToList();

            // set screen values
            _view.GenericKey = genericKey;
            _view.GenericKeyTypeKey = genericKeyTypeKey;

			if(note.DiaryDate.HasValue)
			{
				_view.DiaryDate = note.DiaryDate.Value;
			}
            
            // bind the notes
            _view.BindNotesGrid(noteDetails);          

            // get a unique list of legalentities from the notes and populate user filter control
            var legalEntities = (from n in noteDetails orderby n.LegalEntity.DisplayName select n.LegalEntity).Distinct().ToList();
            _view.BindLegalEntityFilter(legalEntities);

            // get a unique list of dates from the notes and populate date filter controls
            Dictionary<string, string> uniqueDates = new Dictionary<string, string>();
            var dates = (from n in noteDetails orderby n.InsertedDate ascending select n.InsertedDate.Date).Distinct().ToList();
            foreach (var date in dates)
            {
               uniqueDates.Add(date.ToString("yyyyMMdd"), date.ToString(SAHL.Common.Constants.DateFormat));
            }

            _view.BindDateFilters(uniqueDates);                      
        }
    }
}