using Automation.DataAccess;
using Common.Enums;
using System;

namespace BuildingBlocks.Services.Contracts
{
    public interface INotesService
    {
        QueryResults GetNotes(GenericKeyTypeEnum genericKeyType, params int[] genericKey);

        void InsertNote(int genericKey, GenericKeyTypeEnum genericKeyType, string workflowState, string noteText, int legalEntityKey, DateTime diaryDate, string tag);

        void DeleteNotes(int genericKey);

        int CountActiveWorkflowRolesByNotesDiaryDate(WorkflowRoleTypeEnum workflowRoleTypeKey, GenericKeyTypeEnum genericKeyType, string aduserName, DateTime diaryDate);
    }
}