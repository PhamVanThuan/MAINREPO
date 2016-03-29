using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAHL.Website.Halo.ModelBinders
{
	public class TileBusinessContextModelBinder : DefaultModelBinder
	{
		public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var context = bindingContext.ValueProvider.GetValue("context").AttemptedValue;
			var businessKey = long.Parse(bindingContext.ValueProvider.GetValue("businessKey").AttemptedValue);
			var businessKeyType = (BusinessKeyType)Enum.Parse(typeof(BusinessKeyType), bindingContext.ValueProvider.GetValue("businessKeyType").AttemptedValue);
			string tileConfigTypeName = bindingContext.ValueProvider.GetValue("tileConfigTypeName") == null ? String.Empty : bindingContext.ValueProvider.GetValue("tileConfigTypeName").AttemptedValue;
			string tileModelTypeName = bindingContext.ValueProvider.GetValue("tileModelTypeName") == null ? String.Empty : bindingContext.ValueProvider.GetValue("tileModelTypeName").AttemptedValue; 

			Type tileConfigType = null;
			if (!String.IsNullOrEmpty(tileConfigTypeName))
			{
				tileConfigType = Type.GetType(tileConfigTypeName);
			}

			Type tileModelType = null;
			if (!String.IsNullOrEmpty(tileModelTypeName))
			{
				tileModelType = Type.GetType(tileModelTypeName);
			}

			return new TileBusinessContext(context, businessKeyType, businessKey, tileModelType, tileConfigType);
		}
	}
}