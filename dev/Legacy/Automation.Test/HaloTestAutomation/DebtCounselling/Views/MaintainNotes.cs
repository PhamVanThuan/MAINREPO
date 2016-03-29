using System;
using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.DebtCounselling;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using Navigation = BuildingBlocks.Navigation;

namespace DebtCounsellingTests.Views
{
    [TestFixture, RequiresSTA]
    public sealed class MaintainNotes : DebtCounsellingTests.TestBase<NoteMaintenance>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.DebtCounsellingConsultant);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendProposal, TestUsers.DebtCounsellingConsultant);
            base.Browser.Navigate<Navigation.NotesNode>().MaintainNotes();
        }

        /// <summary>
        /// Tests adding a note to a debt counselling case
        /// </summary>
        [Test]
        public void TestNoteDetails()
        {
            base.View.ClickAddNoteDetails();
            base.View.ClickUpdateAddNoteDetails();
            BuildingBlocks.Timers.GeneralTimer.Wait(3000);
            base.View.AssertNoteRequiredMessage();
            string noteText = "Automation Tests";
            string noteTag = "1234";
            base.View.PopulateNoteDetails(noteText, noteTag);
            base.View.ClickUpdateAddNoteDetails();
            //We need to wait for notes to save as it is sloooooww!! :(
            BuildingBlocks.Timers.GeneralTimer.Wait(3000);
            NoteAssertions.AssertNoteText(base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey, noteText);
            NoteAssertions.AssertNoteTag(base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey, noteTag);
        }

        /// <summary>
        /// Checks that the diary date can be added to the debt counselling case.
        /// </summary>
        [Test]
        public void TestNoteDiaryDate()
        {
            //Test that a Diary Date has to be provided to be able to check diary date
            base.View.ClickCheckDiaryDateButton();
            base.View.AssertNoDiaryDateToCheckMessage();
            base.View.PopulateDiaryDate(DateTime.Now);
            base.View.ClickSaveDiaryDateButton();
            BuildingBlocks.Timers.GeneralTimer.Wait(3000);
            DebtCounsellingAssertions.AssertDiaryDate(base.TestCase.DebtCounsellingKey, expecteddiarydate: DateTime.Now);
        }
    }
}