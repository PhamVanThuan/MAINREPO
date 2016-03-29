using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAHL.Website.Halo.ModelBinders
{
    public class EditorBusinessContextModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var context = bindingContext.ValueProvider.GetValue("context").AttemptedValue;
            var businessKey = long.Parse(bindingContext.ValueProvider.GetValue("businessKey").AttemptedValue);
            var businessKeyType = (BusinessKeyType)Enum.Parse(typeof(BusinessKeyType), bindingContext.ValueProvider.GetValue("businessKeyType").AttemptedValue);
            string editorModelTypeName = bindingContext.ValueProvider.GetValue("editorModelTypeName") == null ? String.Empty : bindingContext.ValueProvider.GetValue("editorModelTypeName").AttemptedValue;
            string editorModelConfigTypeName = bindingContext.ValueProvider.GetValue("editorModelConfigurationTypeName") == null ? String.Empty : bindingContext.ValueProvider.GetValue("editorModelConfigurationTypeName").AttemptedValue; 

            Type editorModelType = null;
            if (!String.IsNullOrEmpty(editorModelTypeName))
            {
                editorModelType = Type.GetType(editorModelTypeName);
            }

            Type editorModelConfigType = null;
            if (!String.IsNullOrEmpty(editorModelConfigTypeName))
            {
                editorModelConfigType = Type.GetType(editorModelConfigTypeName);
            }

            return new EditorBusinessContext(context, businessKeyType, businessKey, editorModelType, editorModelConfigType);
        }
    }
}