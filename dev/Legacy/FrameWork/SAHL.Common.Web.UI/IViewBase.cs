using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Web.UI.Events;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.Security;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.UI;
using SAHL.Communication;

namespace SAHL.Common.Web.UI
{
    public delegate void ViewHandler(object sender, EventArgs e);

    /// <summary>
    /// Interface for a basic SAHL view.
    /// </summary>
    public interface IViewBase : IView
    {
        /// <summary>
        /// Page Load event.
        /// </summary>
        event ViewHandler ViewLoaded;

        /// <summary>
        /// Page Init event.
        /// </summary>
        event ViewHandler ViewInitialised;

        /// <summary>
        /// Page PreInt event.
        /// </summary>
        event ViewHandler ViewPreInitialised;

        /// <summary>
        /// Page PreRender event.
        /// </summary>
        event ViewHandler ViewPreRender;

        /// <summary>
        /// Gets a list of domain messages that need to be displayed on the view.
        /// </summary>
        IDomainMessageCollection Messages { get; }

       
        /// <summary>
        /// Gets the current user.
        /// </summary>
        SAHLPrincipal CurrentPrincipal { get; }

        CBOManager CBOManager { get; }

        /// <summary>
        /// The view name.
        /// </summary>
        new string ViewName { get; }

        /// <summary>
        /// Gets whether the current request is as a result of a postback.
        /// </summary>
        bool IsPostBack { get; }

        /// <summary>
        /// Determines whether the page has validated successfully.
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Determines whether the page should run, or an external postback has been initiated that means the 
        /// page does not need to run as navigation will occur somewhere later in the page life cycle. This can be 
        /// set if your page wants to mark itself as not having to run any more.
        /// </summary>
        /// <returns></returns>
        bool ShouldRunPage { get; set; }

        /// <summary>
        /// Gets a list of any defined view sttributes.
        /// </summary>
        IDictionary<string, string> ViewAttributes { get; }

        /// <summary>
        /// Determines whether the view is being loaded as a result of a menu click.
        /// </summary>
        bool IsMenuPostBack { get; set; }

        /// <summary>
        /// Gets/sets the fully qualified type of the presenter currently being used behind the view.
        /// </summary>
        string CurrentPresenter { get; set; }

        /// <summary>
        /// Gets the SAHL Master page associated with the view.
        /// </summary>
        SAHLMasterBase Master { get; }


        /// <summary>
        ///  Writes trace information out, with the fully qualified name of <c>source</c> as the category.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        void AddTrace(object source, string message);


    }
}
