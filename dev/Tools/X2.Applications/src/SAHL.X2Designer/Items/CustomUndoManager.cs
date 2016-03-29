using System;
using System.Collections.Generic;
using System.Text;
using Northwoods.Go;
using System.Windows.Forms;

namespace SAHL.X2Designer.Items
{
    class CustomUndoManager : GoUndoManager
    {

        public override void DocumentChanged(object sender, GoChangedEventArgs e)
        {
            if (this.IsUndoing || this.IsRedoing) return;

            if (!SkipEvent(e))
            {

                GoUndoManagerCompoundEdit cedit = this.CurrentEdit;
                if (cedit == null || cedit.IsComplete)
                {
                    cedit = new GoUndoManagerCompoundEdit();
                    this.CurrentEdit = cedit;
                }

                // make a copy of the event to save as an edit in the list
                GoChangedEventArgs edit = new GoChangedEventArgs(e);
                cedit.AddEdit(edit);
                if (this.ChecksTransactionLevel && this.TransactionLevel <= 0)
                    System.Diagnostics.Trace.WriteLine("Change not within a transaction: " + edit.ToString());
            }
            base.DocumentChanged(sender, e);
        }

        public override bool SkipEvent(GoChangedEventArgs evt)
        {
            return base.SkipEvent(evt);
        }
    }
}
