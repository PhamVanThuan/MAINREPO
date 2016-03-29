using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.DebtCounselling;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using Common.Extensions;
using NUnit.Framework;
using System;
using System.Threading;

namespace PersonalLoansTests.Views
{
    [TestFixture, RequiresSTA]
    public sealed class Notes : PersonalLoansWorkflowTestBase<NoteMaintenance>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.FindCaseAtState(WorkflowStates.PersonalLoansWF.ManageApplication, WorkflowRoleTypeEnum.PLConsultantD);

            var aduserFirstNames = base.CaseOwner.RemoveDomainPrefix();
            var legalEntityKey = base.Service<ILegalEntityService>().GetLegalEntityKeyByFirstNames(aduserFirstNames);

            base.Service<INotesService>().DeleteNotes(base.GenericKey);
            base.Service<INotesService>().InsertNote(base.GenericKey, GenericKeyTypeEnum.Offer_OfferKey
                , WorkflowStates.PersonalLoansWF.ManageApplication, "AutomationTests", legalEntityKey, DateTime.Now.Date, "TagTest");
            base.Browser.Navigate<BuildingBlocks.Navigation.NotesNode>().MaintainNotes();
        }

        [Test]
        public void when_maintain_note_users_can_save_the_diary_date()
        {
            var expectedDate = DateTime.Now.AddDays(5);
            base.Browser.Page<NoteMaintenance>().PopulateDiaryDate(expectedDate);
            base.Browser.Page<NoteMaintenance>().ClickSaveDiaryDateButton();
            NoteAssertions.AssertDiaryDate(base.GenericKey, GenericKeyTypeEnum.Offer_OfferKey, expectedDate);
        }

        [Test]
        public void when_add_diary_date_they_cant_use_date_in_past()
        {
            var date = DateTime.Now.Subtract(TimeSpan.FromDays(31));
            base.Browser.Page<NoteMaintenance>().PopulateDiaryDate(date);
            base.Browser.Page<NoteMaintenance>().ClickSaveDiaryDateButton();
            base.Browser.Page<NoteMaintenance>().AssertDiaryDateNotInPastMessage();
        }

        [Test]
        public void when_add_note_the_note_text_is_mandatory()
        {
            base.Browser.Page<NoteMaintenance>().ClickAddNoteDetails();
            base.Browser.Page<NoteMaintenance>().ClickUpdateAddNoteDetails();
            //Assert uses ajax control so need to wait a little while
            Thread.Sleep(2000);
            base.Browser.Page<NoteMaintenance>().AssertNoteRequiredMessage();
            base.Browser.Page<NoteMaintenance>().ClickCancelAddNoteDetails();
        }

        [Test]
        public void when_check_diary_button_clicked_should_notify_user_number_cases_diarised_for_same_date()
        {
            base.Browser.Page<NoteMaintenance>().PopulateDiaryDate(DateTime.Now);
            base.Browser.Page<NoteMaintenance>().ClickCheckDiaryDateButton();

            var count =
                base.Service<INotesService>().CountActiveWorkflowRolesByNotesDiaryDate
                                    (WorkflowRoleTypeEnum.PLConsultantD, GenericKeyTypeEnum.Offer_OfferKey, base.CaseOwner, DateTime.Now.Date);
            Thread.Sleep(25000);
            base.Browser.Page<NoteMaintenance>().AssertCheckDiaryMessage(count, base.CaseOwner, DateTime.Now.Date);
        }

        [Test]
        public void when_users_add_note_they_can_provide_a_tag_number()
        {
            /*
            base.Service<INotesService>().DeleteNotes(base.GenericKey);
            base.Browser.Navigate<BuildingBlocks.Navigation.NotesNode>().MaintainNotes();
            base.Browser.Page<NoteMaintenance>().ClickAddNoteDetails();
            base.Browser.Page<NoteMaintenance>().PopulateNoteDetails("Test adding note", "Tag test");
            base.Browser.Page<NoteMaintenance>().ClickUpdateAddNoteDetails();
            //Assert uses ajax control so need to wait a little while
            Thread.Sleep(2000);
            NoteAssertions.AssertNoteTag(base.GenericKey, GenericKeyTypeEnum.Offer_OfferKey, "Tag test");
            */
            Assert.Ignore("Until we find a solution to setting these tags the tests will need to be ignored.");
        }

        [Test]
        public void when_users_add_note_should_save_workflowstate()
        {
            /*
            base.Service<INotesService>().DeleteNotes(base.GenericKey);
            base.Browser.Navigate<BuildingBlocks.Navigation.NotesNode>().MaintainNotes();
            base.Browser.Page<NoteMaintenance>().ClickAddNoteDetails();
            base.Browser.Page<NoteMaintenance>().PopulateNoteDetails("Test adding note", "Tagtest");
            base.Browser.Page<NoteMaintenance>().ClickUpdateAddNoteDetails();
            //Assert uses ajax control so need to wait a little while
            Thread.Sleep(2000);
            NoteAssertions.AssertWorkflowState(base.GenericKey, GenericKeyTypeEnum.Offer_OfferKey, WorkflowStates.PersonalLoansWF.ManageApplication);
           */
            Assert.Ignore("Until we find a solution to setting these tags the tests will need to be ignored.");
        }

        [Test]
        public void when_users_edit_note_should_not_be_able_to_change_note_text()
        {
            base.Browser.Page<NoteMaintenance>().ClickEditNoteDetails();
            Thread.Sleep(2000);
            base.Browser.Page<NoteMaintenance>().AssertNoteTextDisabled();
            base.Browser.Page<NoteMaintenance>().ClickCancelAddNoteDetails();
        }

        [Test]
        public void when_users_add_note_should_save_aduser_that_captured_note()
        {
            /*
            base.Service<INotesService>().DeleteNotes(base.GenericKey);
            base.Browser.Navigate<BuildingBlocks.Navigation.NotesNode>().MaintainNotes();
            base.Browser.Page<NoteMaintenance>().ClickAddNoteDetails();
            base.Browser.Page<NoteMaintenance>().PopulateNoteDetails("Test adding note", "Tagtest");
            base.Browser.Page<NoteMaintenance>().ClickUpdateAddNoteDetails();
            //Assert uses ajax control so need to wait a little while
            Thread.Sleep(2000);
            NoteAssertions.AssertAdUserThatCaptureNote(base.GenericKey, GenericKeyTypeEnum.Offer_OfferKey, base.CaseOwner);
            */
            Assert.Ignore("Until we find a solution to setting these tags the tests will need to be ignored.");
        }

        [Test]
        public void when_users_check_diary_date_without_date_should_get_message()
        {
            base.Browser.Page<NoteMaintenance>().ClearDiaryDate();
            base.Browser.Page<NoteMaintenance>().ClickCheckDiaryDateButton();
            Thread.Sleep(2000);
            base.Browser.Page<NoteMaintenance>().AssertNoDiaryDateToCheckMessage();
        }
    }
}