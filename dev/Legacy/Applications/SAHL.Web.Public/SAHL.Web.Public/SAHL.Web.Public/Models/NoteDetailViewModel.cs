using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAHL.Web.Public.Models
{
    /// <summary>
    /// Note Detail View Model
    /// </summary>
    public class NoteDetailViewModel
    {
        public int LegalEntityKey { get; set; }
        public string LegalEntityDisplayName { get; set; }
        public DateTime InsertedDate { get; set; }
        public string NoteText { get; set; }
        public string WorkflowState { get; set; }
    }
}