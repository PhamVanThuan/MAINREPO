using SAHL.Core.Caching;
using SAHL.Core.UI.Context;

namespace SAHL.Core.UI.Caching
{
    public class PrincipalEditorBusinessContextGeneratorFactor : ICacheKeyGeneratorFactory<PrincipalEditorBusinessContext>
    {
        public string GetKey<U>(PrincipalEditorBusinessContext context)
        {
            return string.Format("{0}_{1}_{2}_{3}_{4}_{5}", context.User.Identity.Name.ToLower(), context.BusinessContext.Context.ToLower(),
                context.BusinessContext.BusinessKey.KeyType, context.BusinessContext.BusinessKey.Key,
                context.BusinessContext.EditorType != null ? context.BusinessContext.EditorType.ToString() : "none",
                context.BusinessContext.EditorModelConfigurationType != null ? context.BusinessContext.EditorModelConfigurationType.ToString() : "none");
        }
    }
}