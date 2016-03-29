using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.X2.BusinessModel.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Common.Presenters
{
	public class CommonNoteMaintenance : SAHLCommonBasePresenter<INoteMaintenance>
	{
		protected CBOMenuNode node;
		protected INoteRepository noteRepository;
		private int genericKey;
		private int genericKeyTypeKey;
		protected ILookupRepository lookupRepository;
		protected IX2Repository x2Repository;
		protected IADUser adUser;
		protected INote note;
        protected InstanceNode instanceNode;
        private string workflowname;

		protected IADUser CurrentADUser
		{
			get
			{
				if (adUser == null)
				{
					ISecurityRepository secRepo = RepositoryFactory.GetRepository<ISecurityRepository>();
					adUser = secRepo.GetADUserByPrincipal(_view.CurrentPrincipal);
				}
				return adUser;
			}
		}

		public CommonNoteMaintenance(INoteMaintenance view, SAHLCommonBaseController controller)
			: base(view, controller)
		{

			node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            instanceNode = CBOManager.GetInstanceNode(_view.CurrentPrincipal);

			if (node == null)
				throw new NullReferenceException(StaticMessages.NullCurrentCBONode);
		}

		protected override void OnViewInitialised(object sender, EventArgs e)
		{
			base.OnViewInitialised(sender, e);
			if (!_view.ShouldRunPage) return;

			node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            instanceNode = CBOManager.GetInstanceNode(_view.CurrentPrincipal);

			if (node != null)
			{
				genericKey = Convert.ToInt32(node.GenericKey);
				genericKeyTypeKey = Convert.ToInt32(node.GenericKeyTypeKey);

			}

            if (instanceNode != null)
            {
                workflowname = instanceNode.WorkflowName;   
            }

			noteRepository = RepositoryFactory.GetRepository<INoteRepository>();
			lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
            x2Repository = RepositoryFactory.GetRepository<IX2Repository>();

			_view.OnSubmitButtonClicked += new EventHandler(OnSubmitButtonClicked);
			_view.OnCancelButtonClicked += new EventHandler(OnCancelButtonClicked);

			_view.gvNotesGridRowUpdating += new KeyChangedEventHandler(OnNoteRowUpdating);
			_view.gvNotesGridRowInserting += new KeyChangedEventHandler(OnNoteRowInserting);

			//get the note for the generic key
			note = noteRepository.GetNoteByGenericKeyAndType(genericKey, genericKeyTypeKey);
			if (note == null)
			{
				note = noteRepository.CreateEmptyNote();
				note.GenericKey = genericKey;
				note.GenericKeyType = lookupRepository.GenericKeyType.ObjectDictionary[((int)genericKeyTypeKey).ToString()]; ;
			}

			//get the diary date from the note
			_view.BindNotesGrid(note.NoteDetails.ToList());

			// set screen values
			_view.GenericKey = genericKey;
            _view.ADUserKey = CurrentADUser.Key;
            _view.WorkflowName = workflowname;
			_view.DiaryDate = note.DiaryDate;
		}

		protected void OnSubmitButtonClicked(object sender, EventArgs e)
		{
			using (var transaction = new TransactionScope())
			{
				// save the diary date (if it has changed)
				try
				{
					if (_view.DiaryDate != note.DiaryDate)
					{
						// validate that the diary date is not before today
						if (_view.DiaryDate.HasValue && _view.DiaryDate.Value.Date < DateTime.Now.Date)
						{
							string errorMessage = "Diary Date cannot be before today.";
							_view.Messages.Add(new Error(errorMessage, errorMessage));
							throw new Exception();
						}

						note.DiaryDate = _view.DiaryDate;
						noteRepository.SaveNote(note);
					}

					transaction.VoteCommit();
				}
				catch (Exception)
				{
                    transaction.VoteRollBack();

					if (_view.IsValid)
						throw;
				}
			}
		}

		protected void OnCancelButtonClicked(object sender, EventArgs e)
		{
			_view.Navigator.Navigate("CommonNoteSummary");
		}

		protected void OnNoteRowUpdating(object sender, KeyChangedEventArgs e)
		{
			int NoteDetailKey = Convert.ToInt32(e.Key);
			//Update the tag for the row
			Dictionary<string, string> dict = sender as Dictionary<string, string>;
			string TagText = Convert.ToString(dict["TagText"]);
			INoteDetail noteDetail = noteRepository.GetNoteDetailByKey(NoteDetailKey);
			noteDetail.Tag = TagText;
			noteRepository.SaveNoteDetail(noteDetail);
		}

		protected void OnNoteRowInserting(object sender, KeyChangedEventArgs e)
		{
			//this will need to happen inside of a transaction, so we commit/fail all
			using (TransactionScope transaction = new TransactionScope())
			{
				try
				{
					//Add a new Note Detail
					Dictionary<string, string> dict = sender as Dictionary<string, string>;
					string TagText = Convert.ToString(dict["TagText"]);
					string NoteText = Convert.ToString(dict["NoteText"]);

					INoteDetail noteDetail = noteRepository.CreateEmptyNoteDetail();
					noteDetail.NoteText = NoteText;
					noteDetail.Tag = TagText;
					//Get Workflow State
					IInstance instance = x2Repository.GetInstanceForGenericKey(genericKey, SAHL.Common.Constants.WorkFlowName.PersonalLoans, SAHL.Common.Constants.WorkFlowProcessName.PersonalLoan);
					string stateName = instance.State.Name;
					noteDetail.WorkflowState = stateName;
					noteDetail.InsertedDate = DateTime.Now;
					noteDetail.LegalEntity = CurrentADUser.LegalEntity; // the legalentity of the aduser

					//Link note detail to note object
					note.NoteDetails.Add(null, noteDetail);
					noteDetail.Note = note;
					noteRepository.SaveNote(note);
					noteRepository.SaveNoteDetail(noteDetail);

					transaction.VoteCommit();
				}
				catch (Exception)
				{
                    transaction.VoteRollBack();

					if (_view.IsValid)
						throw;
				}
			}

			_view.BindNotesGrid(note.NoteDetails.ToList());
		}
	}
}