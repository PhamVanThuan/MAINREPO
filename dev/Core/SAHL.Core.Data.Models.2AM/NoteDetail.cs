using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class NoteDetailDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public NoteDetailDataModel(int noteKey, string tag, string workflowState, DateTime insertedDate, string noteText, int legalEntityKey)
        {
            this.NoteKey = noteKey;
            this.Tag = tag;
            this.WorkflowState = workflowState;
            this.InsertedDate = insertedDate;
            this.NoteText = noteText;
            this.LegalEntityKey = legalEntityKey;
		
        }
		[JsonConstructor]
        public NoteDetailDataModel(int noteDetailKey, int noteKey, string tag, string workflowState, DateTime insertedDate, string noteText, int legalEntityKey)
        {
            this.NoteDetailKey = noteDetailKey;
            this.NoteKey = noteKey;
            this.Tag = tag;
            this.WorkflowState = workflowState;
            this.InsertedDate = insertedDate;
            this.NoteText = noteText;
            this.LegalEntityKey = legalEntityKey;
		
        }		

        public int NoteDetailKey { get; set; }

        public int NoteKey { get; set; }

        public string Tag { get; set; }

        public string WorkflowState { get; set; }

        public DateTime InsertedDate { get; set; }

        public string NoteText { get; set; }

        public int LegalEntityKey { get; set; }

        public void SetKey(int key)
        {
            this.NoteDetailKey =  key;
        }
    }
}