using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Web.Public.Models
{
    public class AddNoteDetailViewModel
    {
		[Display(Name = "Notes")]
        public string NoteText
        {
            get;
            set;
        }
    }
}