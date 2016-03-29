using Automation.DataAccess.DataHelper;
using BuildingBlocks.Services.Contracts;
using Common.Enums;
using System;
using System.Linq;

namespace BuildingBlocks.Services
{
    public sealed class NotesService : _2AMDataHelper, INotesService
    {
        private IAssignmentService assignment;

        public NotesService()
        {
            this.assignment = ServiceLocator.Instance.GetService<IAssignmentService>();
        }

        public int CountActiveWorkflowRolesByNotesDiaryDate(WorkflowRoleTypeEnum workflowRoleTypeKey, GenericKeyTypeEnum genericKeyType, string aduserName, DateTime diaryDate)
        {
            var genericKeys = (from r in this.assignment.GetActiveWorkflowRolesByADUser(aduserName)
                               where r.Column("workflowroletypekey").GetValueAs<int>() == (int)workflowRoleTypeKey
                               select r.Column("generickey").GetValueAs<int>()).ToArray();

            var dates = (from n in base.GetNotes(genericKeyType, genericKeys)
                         where !String.IsNullOrEmpty(n.Column("diarydate").Value)
                         select DateTime.Parse(n.Column("diarydate").Value)).ToArray();
            var count = (from date in dates where date == diaryDate select date).Count();
            return count - 1;
        }
    }
}