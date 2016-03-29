using System;
using System.Web.UI;

namespace SAHL.Internet.Components
{
    /// <summary>
    /// Provides initialization of the SA Home Loans session object.
    /// </summary>
    public partial class SessionVariables : UserControl
    {
        /// <summary>
        /// Gets the SA Home Loans session object.
        /// </summary>
        protected SAHLWebSession SahlSession
        {
            get { return Session["SAHLWebSession"] as SAHLWebSession; }
            private set { Session["SAHLWebSession"] = value; }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data. </param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (SahlSession != null) return;
            if (Request == null) return;

            var session = new SAHLWebSession
                              {
                                  CurrentPage = Convert.ToString(Request.Url),
                                  AcceptTypes = Request.AcceptTypes,
                                  QueryStringAllKeys = Request.QueryString.AllKeys,
                                  UserLanguages = Request.UserLanguages,
                                  UserAgent = Request.UserAgent,
                                  URLReferrer = Convert.ToString(Request.UrlReferrer),
                                  HostAddress = Request.UserHostAddress,
                                  UserHostName = Request.UserHostName,
                                  QueryString = Convert.ToString(Request.QueryString)
                              };
            SahlSession = session;
        }
    }
}