using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.VSExtensions.Configuration.EditorConfiguration.AddEditorConfig
{
    public partial class EditorConfigurationTemplate
    {
        public dynamic Model
        {
            get;
            protected set;
        }

        public EditorConfigurationTemplate(dynamic model)
        {
            this.Model = model;
        }
    }
}
