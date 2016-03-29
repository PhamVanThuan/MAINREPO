using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SAHL.Web.Public.Models;
using SAHL.Web.Public.AttorneyService;
using DevExpress.Web.Mvc;

namespace SAHL.Web.Public.Controllers
{
    [Authorize]
    public class NoteController : BaseController
    {
        private IAttorney attorneyService;

        /// <summary>
        /// Initializes a new note controller
        /// </summary>
        /// <param name="attorneyService"></param>
        public NoteController(IAttorney attorneyService)
            : base(attorneyService)
        {
            this.attorneyService = attorneyService;
        }

        /// <summary>
        /// Note/Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (Session[SessionConstants.DebtCounsellingKey] == null)
            {
                ShowMessage("The Debt Counselling Case has not been loaded, please go to the Search Screen", MessageType.Error);
                return View();
            }

            DebtCounselling debtCounselling = attorneyService.GetDebtCounsellingByKey((int)Session[SessionConstants.DebtCounsellingKey], Username, Password);

            NoteDetail[] noteDetails = attorneyService.GetNotesByDebtCounselling((int)Session[SessionConstants.DebtCounsellingKey], Username, Password);

            Dictionary<int, string> legalEntities = new Dictionary<int, string>();
            foreach (var noteDetail in noteDetails)
            {
                if (!legalEntities.ContainsKey(noteDetail.LegalEntityKey))
                {
                    legalEntities.Add(noteDetail.LegalEntityKey, noteDetail.LegalEntityDisplayName);
                }
            }

            //Map the View Model
            IndexNoteViewModel indexNoteViewModel = new IndexNoteViewModel
            {
                AccountNumber = Session[SessionConstants.AccountKey].ToString(),
                DiaryDate = debtCounselling.DiaryDate,
                NoteDetails = AutoMapper.Mapper.Map<NoteDetail[], List<NoteDetailViewModel>>(noteDetails),
                Users = new SelectList(legalEntities, "Key", "Value"),
                Dates = new SelectList((from n in noteDetails orderby n.InsertedDate ascending select n.InsertedDate.Date.ToShortDateString()).Distinct().ToList())
            };

            return View(indexNoteViewModel);
        }


        #region Add

        /// <summary>
        /// Note/Add
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            ViewBag.PostedHtml = string.Empty;
            return View();
        }

        /// <summary>
        /// Note/Add
        /// </summary>
        /// <param name="addNoteDetailViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(AddNoteDetailViewModel addNoteDetailViewModel)
        {
            addNoteDetailViewModel.NoteText = HtmlEditorExtension.GetHtml("txtNoteText");

            // Custom validation due to using the Dev Express HTML Editor Control
            if (string.IsNullOrEmpty(addNoteDetailViewModel.NoteText))
            {
                ShowMessage("Please capture notes before clicking the Add button.", MessageType.Error);
                return View();
            }

            ViewBag.PostedHtml = addNoteDetailViewModel.NoteText;
            if (ModelState.IsValid)
            {
                AttorneyService.NoteDetail noteDetail = new AttorneyService.NoteDetail();
                int legalEntityKey = Convert.ToInt32(Session[SessionConstants.LegalEntityKey]);
                int debtCounsellingKey = Convert.ToInt32(Session[SessionConstants.DebtCounsellingKey]);
                noteDetail.LegalEntityKey = legalEntityKey;
                noteDetail.InsertedDate = DateTime.Now;
                noteDetail.Tag = String.Empty;
                noteDetail.NoteText = addNoteDetailViewModel.NoteText;
                attorneyService.SaveNoteDetail(noteDetail, legalEntityKey, debtCounsellingKey, Username, Password);

                if (IsValid)
                {
                    ShowMessage("The note has been added successfully.", MessageType.Info);
                    return Redirect("~/Note/Index");
                }
            }
            return View();
        }

        #endregion
    }
}
