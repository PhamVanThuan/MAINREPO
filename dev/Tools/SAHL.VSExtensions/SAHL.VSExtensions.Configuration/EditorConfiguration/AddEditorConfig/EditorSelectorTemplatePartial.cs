using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.VSExtensions.Configuration.EditorConfiguration.AddEditorConfig
{
    public partial class EditorSelectorTemplate
    {
        public dynamic Model
        {
            get;
            protected set;
        }

        public EditorSelectorTemplate(dynamic model)
        {
            this.Model = model;
        }
    }
}
