using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDFDocumentWriter
{
    public class ErrorEventArgs : EventArgs
    {
        public Exception ex = null;
        public ErrorEventArgs(Exception ex)
        {
            this.ex = ex;
        }
    }
}
