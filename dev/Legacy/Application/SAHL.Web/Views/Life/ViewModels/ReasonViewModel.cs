using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAHL.Web.Views.Life.ViewModels
{
    public class ReasonViewModel
    {
        private string reason = "";
        private string comment = "";

        public string Reason
        {
            get { return reason; }
            set { reason = value; }
        }

        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }
    }
}