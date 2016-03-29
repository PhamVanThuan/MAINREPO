using System.Data;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.Common.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface INTUReasons : IViewBase
    {
        /// <summary>
        /// Bind the Properties Grid
        /// </summary>
        /// <param name="DT"></param>
        void BindgrdHistory(DataTable DT);
        /// <summary>
        /// 
        /// </summary>
        string PanelHeader { get; set; }

    }
}
