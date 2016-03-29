using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    ///
    /// </summary>
    public partial class ApplicationUnknown : Application, IApplicationUnknown
    {
        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);
        }
    }
}