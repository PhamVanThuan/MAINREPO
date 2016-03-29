using Automation.DataAccess;
using BuildingBlocks.Services.Contracts;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Linq;

namespace BuildingBlocks.Assertions
{
    public class NoteAssertions
    {
        private static INotesService notesService;

        static NoteAssertions()
        {
            notesService = ServiceLocator.Instance.GetService<INotesService>();
        }

        public static void AssertDiaryDate(int genericKey, GenericKeyTypeEnum genericKeyType, DateTime expectedDiaryDate)
        {
            QueryResults notes = notesService.GetNotes(genericKeyType, genericKey);
            Assert.True(notes.HasResults, "There are no notes for generickey:{0}", genericKey);

            var date = (from note in notes
                        select note.Column("DiaryDate").GetValueAs<DateTime>()).FirstOrDefault();

            Assert.AreEqual(expectedDiaryDate.Date, date.Date, "Diary date does not match expected diary date.");
        }

        public static void AssertNoteTag(int genericKey, GenericKeyTypeEnum genericKeyType, string expectedtag)
        {
            QueryResults notes = notesService.GetNotes(genericKeyType, genericKey);
            Assert.True(notes.HasResults, "There are no notes for generickey:{0}", genericKey);

            string savedtag = notes.Rows(0).Column("Tag").Value;
            StringAssert.AreEqualIgnoringCase(expectedtag, savedtag, String.Format("Expected tag:{0}, but was {1}", expectedtag, savedtag));
        }

        public static void AssertNoteText(int genericKey, GenericKeyTypeEnum genericKeyType, string expectednoteText)
        {
            QueryResults notes = notesService.GetNotes(genericKeyType, genericKey);
            Assert.True(notes.HasResults, "There are no notes for generickey:{0}", genericKey);

            string savedNoteText = notes.Rows(0).Column("NoteText").Value;
            StringAssert.AreEqualIgnoringCase(expectednoteText, savedNoteText, String.Format("expected note text:{0}, but was {1}", expectednoteText, savedNoteText));
        }

        public static void AssertWorkflowState(int genericKey, GenericKeyTypeEnum genericKeyType, string expectedworkflowstate)
        {
            QueryResults notes = notesService.GetNotes(genericKeyType, genericKey);
            Assert.True(notes.HasResults, "There are no notes for generickey:{0}", genericKey);

            string savedworkflowstate = notes.Rows(0).Column("WorkflowState").Value;
            StringAssert.AreEqualIgnoringCase(expectedworkflowstate, savedworkflowstate,
                String.Format("Expected tag:{0}, but was {1}", expectedworkflowstate, savedworkflowstate));
        }

        public static void AssertAdUserThatCaptureNote(int genericKey, GenericKeyTypeEnum genericKeyType, string expectedAduserName)
        {
            QueryResults notes = notesService.GetNotes(genericKeyType, genericKey);
            Assert.True(notes.HasResults, "There are no notes for generickey:{0}", genericKey);

            string aduserName = notes.Rows(0).Column("adusername").Value;
            StringAssert.AreEqualIgnoringCase(expectedAduserName, aduserName);
        }
    }
}