using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.UI;

namespace SAHL.Common.Web.UI
{
    /// <summary>
    /// Interface for all SAHL presenters.
    /// </summary>
    public interface IPresenterBase
    {
        /// <summary>
        /// Gets the presenter name.
        /// </summary>
        string Name { get; }
        CBOManager CBOManager { get; }
    }
}
