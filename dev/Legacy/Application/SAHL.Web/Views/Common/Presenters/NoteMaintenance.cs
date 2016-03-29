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
    public class NoteMaintenance : SAHLCommonBasePresenter<INoteMaintenance>
    {
        protected CBOMenuNode _node;
        protected INoteRepository _noteRepository;
        protected IDebtCounsellingRepository _dcRepo;
        private int _genericKey;
        protected ILookupRepository _lookupRepository;
        protected IX2Repository _x2Repo;
        protected IADUser _adUser;
        private DateTime _diaryDate;
        protected InstanceNode inode;

        public NoteMaintenance(INoteMaintenance view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            inode = CBOManager.GetInstanceNode(_view.CurrentPrincipal);
            if (_node != null)
            {
                _genericKey = Convert.ToInt32(_node.GenericKey);

            }

            if (inode != null)
            {
                _view.WorkflowName = inode.WorkflowName;
            }

            _noteRepository = RepositoryFactory.GetRepository<INoteRepository>();
            _lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
            _x2Repo = RepositoryFactory.GetRepository<IX2Repository>();
            _dcRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);
            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);

            _view.gvNotesGridRowUpdating += new KeyChangedEventHandler(_view_gvNotesGridRowUpdating);
            _view.gvNotesGridRowInserting += new KeyChangedEventHandler(_view_gvNotesGridRowInserting);

            BindNoteDetails();

            // get the diary date
            switch (_node.GenericKeyTypeKey)
            {
                case (int)SAHL.Common.Globals.GenericKeyTypes.DebtCounselling2AM:
                    IDebtCounselling debtCounselling = _dcRepo.GetDebtCounsellingByKey(_genericKey);
                    if (debtCounselling != null && debtCounselling.DiaryDate.HasValue)
                        _diaryDate = debtCounselling.DiaryDate.Value;
                    break;
                default:
                    break;
            }

            // set screen values
            _view.GenericKey = _genericKey;
            _view.DiaryDate = _diaryDate;
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

        protected void BindNoteDetails()
        {
            //Get the Note Details
            List<INoteDetail> lstNoteDetails = _noteRepository.GetNoteDetailsByGenericKeyAndType(_genericKey, _node.GenericKeyTypeKey);
            _view.BindNotesGrid(lstNoteDetails);
        }

        protected void _view_gvNotesGridRowInserting(object sender, KeyChangedEventArgs e)
        {
            //this will need to happen inside of a transaction, so we commit/fail all
            using (TransactionScope txn = new TransactionScope())
            {
                try
                {
                    //Add a new Note Detail
                    Dictionary<string, string> dict = sender as Dictionary<string, string>;
                    string TagText = Convert.ToString(dict["TagText"]);
                    string NoteText = Convert.ToString(dict["NoteText"]);

                    INote note = _noteRepository.CreateEmptyNote();
                    note.GenericKey = _genericKey;
                    IGenericKeyType genericKeyType;
                    genericKeyType = _lookupRepository.GenericKeyType.ObjectDictionary[_node.GenericKeyTypeKey.ToString()];

                    note.GenericKeyType = genericKeyType;

                    INoteDetail noteDetail = _noteRepository.CreateEmptyNoteDetail();
                    noteDetail.NoteText = NoteText;
                    noteDetail.Tag = TagText;
                    //Get Workflow State
                    IInstance instance = _x2Repo.GetInstanceForGenericKey(_genericKey, SAHL.Common.Constants.WorkFlowName.DebtCounselling, SAHL.Common.Constants.WorkFlowProcessName.DebtCounselling);
                    string stateName = instance.State.Name;
                    noteDetail.WorkflowState = stateName;
                    noteDetail.InsertedDate = DateTime.Now;
                    noteDetail.LegalEntity = CurrentADUser.LegalEntity; // the legalentity of the aduser

                    //Link note detail to note object
                    note.NoteDetails.Add(null, noteDetail);
                    noteDetail.Note = note;
                    _noteRepository.SaveNote(note);
                    _noteRepository.SaveNoteDetail(noteDetail);

                }
                catch (Exception)
                {
                    if (_view.IsValid)
                        throw;
                    txn.VoteRollBack();
                }
                finally
                {
                    txn.VoteCommit();
                }
            }
          

            //Rebind the Data
            BindNoteDetails();
        }

        protected void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            TransactionScope txn = new TransactionScope();

            // save the diary date (if it has changed)
            try
            {
                switch (_node.GenericKeyTypeKey)
                {
                    case (int)SAHL.Common.Globals.GenericKeyTypes.DebtCounselling2AM:
                        IDebtCounselling debtCounselling = _dcRepo.GetDebtCounsellingByKey(_genericKey);
                        if (_view.DiaryDate != debtCounselling.DiaryDate)
                        {
                            // validate that the diary date is not before today
                            if (_view.DiaryDate.HasValue && _view.DiaryDate.Value.Date < DateTime.Now.Date)
                            {
                                string errorMessage = "Diary Date cannot be before today.";
                                _view.Messages.Add(new Error(errorMessage, errorMessage));
                                throw new Exception();
                            }

                            debtCounselling.DiaryDate = _view.DiaryDate;
                            _dcRepo.SaveDebtCounselling(debtCounselling);
                        }

                        break;
                    default:
                        break;
                }

                txn.VoteCommit();

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

        protected void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Cancel");
        }

        protected void _view_gvNotesGridRowUpdating(object sender, KeyChangedEventArgs e)
        {
            int NoteDetailKey = Convert.ToInt32(e.Key);
            //Update the tag for the row
            Dictionary<string, string> dict = sender as Dictionary<string, string>;
            string TagText = Convert.ToString(dict["TagText"]);
            INoteDetail noteDetail = _noteRepository.GetNoteDetailByKey(NoteDetailKey);
            noteDetail.Tag = TagText;
            _noteRepository.SaveNoteDetail(noteDetail);

            //Rebind the Data
            BindNoteDetails();
        }
    }
}