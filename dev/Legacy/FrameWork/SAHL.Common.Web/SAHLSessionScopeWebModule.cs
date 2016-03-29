namespace SAHL.Common.Web
{
    using System;
    using System.Web;
    using Castle.ActiveRecord;
    using Castle.ActiveRecord.Framework;
    using Castle.ActiveRecord.Framework.Scopes;

    /// <summary>
    /// HttpModule to set up a session for the request lifetime.
    /// <seealso cref="SessionScope"/>
    /// </summary>
    /// <remarks>
    /// To install the module, you must:
    /// <para>
    ///    <list type="number">
    ///      <item>
    ///        <description>
    ///        Add the module to the <c>httpModules</c> configuration section within <c>system.web</c>
    ///        </description>
    ///      </item>
    ///    </list>
    /// </para>
    /// </remarks>
    public class SAHLSessionScopeWebModule : IHttpModule
    {
        /// <summary>
        /// The key used to store the session in the context items
        /// </summary>
        protected static readonly String SessionKey = "SAHLSessionScopeWebModule.session";

        /// <summary>
        /// Initialize the module.
        /// </summary>
        /// <param name="app">The app.</param>
        public void Init(HttpApplication app)
        {
            app.BeginRequest += OnBeginRequest;
            app.EndRequest += OnEndRequest;
        }

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule"></see>.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Called when request is started, create a session for the request
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private static void OnBeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Items.Add(SessionKey, new SessionScope(FlushAction.Never));
        }

        /// <summary>
        /// Called when the request ends, dipose of the scope
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private static void OnEndRequest(object sender, EventArgs e)
        {
            SessionScope session = (SessionScope)HttpContext.Current.Items[SessionKey];

            if (session != null)
            {
                session.Dispose();
            }
        }
    }
}