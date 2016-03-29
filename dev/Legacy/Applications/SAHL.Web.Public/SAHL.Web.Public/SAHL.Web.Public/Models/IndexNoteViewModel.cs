using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAHL.Web.Public.Models
{
    /// <summary>
    /// View Model associated with the Index Method on the Note Controller
    /// </summary>
    public class IndexNoteViewModel
    {
        /// <summary>
        /// Today's Date and Time
        /// </summary>
        public DateTime TodaysDate
        {
            get
            {
               return  DateTime.Now;
            }
        }

        /// <summary>
        /// Account Number
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Diary Date
        /// </summary>
        public DateTime? DiaryDate { get; set; }

        /// <summary>
        /// List of Users contained in the Note Details
        /// </summary>
        public System.Web.Mvc.SelectList Users { get; set; }

        /// <summary>
        /// From Dates
        /// </summary>
        public System.Web.Mvc.SelectList Dates { get; set; }

        /// <summary>
        /// A List of Note Details
        /// </summary>
        public List<NoteDetailViewModel> NoteDetails { get; set; }
    }
}