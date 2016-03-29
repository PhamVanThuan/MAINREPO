using SAHL.Core.UI.Context;
using SAHL.Website.Halo.ModelBinders;
using System.Web.Mvc;

namespace SAHL.Website.Halo
{
	public static class ModelBinderConfig
	{
		public static void Configure(ModelBinderDictionary binders)
		{
			binders.Add(typeof(TileBusinessContext), new TileBusinessContextModelBinder());
			binders.Add(typeof(EditorBusinessContext), new EditorBusinessContextModelBinder());
		}
	}
}