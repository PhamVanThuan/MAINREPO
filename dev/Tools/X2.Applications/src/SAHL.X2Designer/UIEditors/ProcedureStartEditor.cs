using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;

namespace SAHL.ReWorkDesigner.UIEditors
{
    class ProcedureStartEditor : UITypeEditor
    {
        		// [RefreshProperties(RefreshProperties.All)]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            return base.EditValue(context, provider, value);
        }
    }
}
