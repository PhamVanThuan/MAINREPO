// VS2010 bug
// VS2010 calls OnInit at designtime
// If there is any code in OnInit that accesses Session variables or Page.Request etc...this will cause the page not to render
// http://blogs.msdn.com/b/webdevtools/archive/2010/05/06/another-error-creating-control-in-the-design-view-with-object-reference-not-set-in-visual-studio-2010.aspx
// Fix:
// If there is no active session, then dont call the base methods in OnPreInit & OnInit
// this is because there is code in uip that is causing this bug to happen.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using AjaxControlToolkit;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;
using SAHL.Common.Logging;
using SAHL.Common.Security;
using SAHL.Common.UI;
using SAHL.Common.Web.UI.Exceptions;
using SAHL.Communication;
using SAHL.Shared.Messages.Metrics;

namespace SAHL.Common.Web.UI
{
    public class SAHLCommonBaseView : WebFormView, IViewBase
    {
        #region Private Variables

        //private SAHLMenuCBO _CBOMenu;
        private SAHLPrincipal _currentPrincipal;

        private SAHLPrincipalCache _spc;
        private Dictionary<string, string> _viewAttributes = new Dictionary<string, string>();
        private ArrayList _webServices = new ArrayList();
        private HtmlInputHidden _hidUserToken;
        private CBOManager _cboManager;

        /// <summary>
        /// Key used to store whether a menu item was clicked to cause a post back.
        /// </summary>
        private const string IsMenuPostBackValue = "IsMenuPostBackValue";

        private bool _isMenuPostBack = false;

        private bool _shouldRunPage = true;

        private string _currentPresenter = String.Empty;

        /// <summary>
        /// This gets set if we need to navigate away - usually as a result of a tab click on the front-end.
        /// </summary>
        private string _navigateUrl;

        private DateTime _initTime;

        #endregion Private Variables

        #region Overrides

        protected override void OnPreInit(EventArgs e)
        {
            _initTime = DateTime.Now;

            if (Context == null || Context.Session == null)
                return;

            EnableViewState = false;
            Response.BufferOutput = true;

            // get the application title and key, use the key to setup application style sheets and
            // application master pages.
            if (Session["Master"] == null)
            {
                // TODO: MAKE A CONFIG SECTION
                Session.Add("CSS", "SAHL.css");
                Session.Add("Master", "SAHL.master");
            }

            // set the master page from the session variables
            if (Session["Master"] != null)
                this.MasterPageFile = "~/MasterPages/" + Session["Master"];

            base.OnPreInit(e);

            if (ViewPreInitialised != null)
                ViewPreInitialised(this, new EventArgs());

            // _CBOMenu = ControlFinder.FindControl<SAHLMenuCBO>(Master, "mnuCBO");
            _currentPrincipal = Context.User as SAHLPrincipal;
            _spc = SAHLPrincipalCache.GetPrincipalCache(_currentPrincipal);
            _spc.DomainMessages = new DomainMessageCollection();

            // look for a new node set value sent from the client - if it's been set then we update it
            string nodeSet = Request.Form["hidNodeSet"];
            if (!String.IsNullOrEmpty(nodeSet))
            {
                CBONodeSetType nodeSetType = (nodeSet == "1" ? CBONodeSetType.X2 : CBONodeSetType.CBO);
                CBOManager.SetCurrentNodeSet(_currentPrincipal, nodeSetType);
                ShouldRunPage = false;

                // if a CBO node is selected, transfer user to that page
                CBONode node = CBOManager.GetCurrentCBONode(_currentPrincipal, nodeSetType);
                if (node != null)
                    _navigateUrl = node.URL;
                else
                {
                    // no default node selected - navigate to the first node in the user's CBO node list
                    List<CBONode> nodes = CBOManager.GetMenuNodes(_currentPrincipal, nodeSetType);
                    if (nodes.Count > 0)
                        _navigateUrl = nodes[0].URL;
                    else
                        _navigateUrl = "Blank";
                }
                return;
            }

            // security first - make sure the user belongs to a user organisation structure
            if (!_spc.IsAuthenticated)
                throw new UserOrganisationSecurityException(String.Format("User '{0}' does not exist or has not been added to the organisation structure.", _currentPrincipal.Identity.Name));

            // clear out the presenter data if it's not a postback
            if (!IsPostBack)
                _spc.GetPresenterData().Clear();

            // check for item in the cache telling us a menu item was clicked
            _shouldRunPage = String.IsNullOrEmpty(Request.Form["navigate"]);
            _isMenuPostBack = !_shouldRunPage;

            GlobalData globalData = _spc.GetGlobalData();
            if (globalData.ContainsKey(IsMenuPostBackValue))
            {
                _isMenuPostBack = Convert.ToBoolean(globalData[IsMenuPostBackValue]);
                globalData.Remove(IsMenuPostBackValue);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            ////Check if the given page supports session or not (this tested as reliable indicator
            //// if EnableSessionState is true), should not care about a page that does
            //// not need session
            //if (Context.Session != null)
            //{
            //    //Tested and the IsNewSession is more advanced then simply checking if
            //    // a cookie is present, it does take into account a session timeout, because
            //    // I tested a timeout and it did show as a new session
            //    if (Session.IsNewSession)
            //    {
            //        // If it says it is a new session, but an existing cookie exists, then it must
            //        // have timed out (can't use the cookie collection because even on first
            //        // request it already contains the cookie (request and response
            //        // seem to share the collection)
            //        string szCookieHeader = Request.Headers["Cookie"];
            //        if ((null != szCookieHeader) && (szCookieHeader.IndexOf("ASP.NET_SessionId") >= 0))
            //        {
            //            //Response.Redirect("~/TimeoutError.aspx");
            //            Response.Redirect("~/TimeoutError.aspx", true);
            //        }
            //    }
            //}
            if (Context == null || Context.Session == null)
                return;

            base.OnInit(e);

            if (!String.IsNullOrEmpty(_navigateUrl))
            {
                Navigator.Navigate(_navigateUrl);
                return;
            }

            ViewSettings viewSettings = UIPConfiguration.Config.GetViewSettingsFromName(base.ViewName);
            for (int i = 0; i < viewSettings.CustomAttributes.Count; i++)
            {
                _viewAttributes.Add(viewSettings.CustomAttributes[i].Name.ToLower(), viewSettings.CustomAttributes[i].Value);
            }

            // create the user token field that is used to ensure that only a single browser window can be used per
            // login
            _hidUserToken = new HtmlInputHidden();
            _hidUserToken.ID = "hidUserToken";
            this.Page.Form.Controls.Add(_hidUserToken);

            // ensure that the user tokens correspond, otherwise a new browser window has been opened and we need to
            // alert the user
            string userToken = Request.Form[_hidUserToken.UniqueID];
            if (String.IsNullOrEmpty(userToken) || _spc.UserToken == null)
            {
                // this is the first page hit so the value hasn't been set
                UserTokenDetails utd = new UserTokenDetails(DateTime.Now.ToString("u"),
                    Request.ServerVariables["REMOTE_ADDR"], Request.ServerVariables["REMOTE_HOST"],
                    Request.ServerVariables["HTTP_X_FORWARDED_FOR"]);
                _hidUserToken.Value = utd.Token;
                _spc.UserToken = utd;
            }
            else
            {
                UserTokenDetails utd = (UserTokenDetails)_spc.UserToken;
                if (utd.Token != userToken)
                {
                    StringBuilder sb = new StringBuilder();
                    string crlf = Environment.NewLine;
                    sb.Append("You cannot use the application in more than one browser. ");
                    sb.Append("Details about the last login by '" + CurrentPrincipal.Identity.Name + "' include:" + crlf);
                    sb.Append("Remote Address: " + utd.RemoteAddress + crlf);
                    if (!String.IsNullOrEmpty(utd.RemoteHost) && utd.RemoteAddress != utd.RemoteHost)
                        sb.Append("Remote Host: " + utd.RemoteHost + crlf);
                    if (!String.IsNullOrEmpty(utd.ForwardedFor) && utd.RemoteAddress != utd.ForwardedFor)
                        sb.Append("Client IP (if behind proxy): " + utd.ForwardedFor + crlf);
                    throw new MultipleLoginException(sb.ToString());
                }
            }

            if (ViewInitialised != null)
                ViewInitialised(this, new EventArgs());
        }

        protected override void OnLoad(EventArgs e)
        {
            // Create a css link object and add it to the header of the page
            if (Session["CSS"] != null)
            {
                HtmlLink cssHtmlLink = new HtmlLink();
                cssHtmlLink.Href = "../CSS/" + Session["CSS"];
                cssHtmlLink.Attributes.Add("rel", "stylesheet");
                cssHtmlLink.Attributes.Add("type", "text/css");

                // Add the HtmlLink to the Head section of the page.
                this.Header.Controls.Add(cssHtmlLink);
            }

            base.OnLoad(e);

            if (ViewLoaded != null)
                ViewLoaded(this, new EventArgs());

            ClientScript.RegisterHiddenField("VIEWNAME", base.ViewName);

            try
            {
                string url = base.Request.Url.ToString();

                // we need a correlation id and generickey for this page if available
                string correlationId = string.Format("{0}_{1}_{2}", base.ViewName, DateTime.Now.ToShortTimeString(), Guid.NewGuid());

                Metrics.ThreadContext.Add(Metrics.CORRELATIONID, correlationId);
                Logger.ThreadContext.Add(Logger.CORRELATIONID, correlationId);

                Metrics.ThreadContext.Add(Metrics.VIEWNAME, base.ViewName);
                Logger.ThreadContext.Add(Logger.VIEWNAME, base.ViewName);
                Metrics.ThreadContext.Add(Metrics.PRESENTERNAME, this.CurrentPresenter);
                Logger.ThreadContext.Add(Logger.PRESENTERNAME, this.CurrentPresenter);
                Metrics.ThreadContext.Add(Logger.URL, url);
                Logger.ThreadContext.Add(Logger.URL, url);

                CBONode _cboNode = CBOManager.GetCurrentCBONode(this.CurrentPrincipal);
                if (_cboNode != null)
                {
                    Metrics.ThreadContext.Add(Metrics.GENERICKEY, string.Format("{0}", _cboNode.GenericKey));
                    Logger.ThreadContext.Add(Logger.GENERICKEY, string.Format("{0}", _cboNode.GenericKey));
                }

                MetricsPlugin.Metrics.PublishLatencyMetric(_initTime, DateTime.Now - _initTime, url);
                MetricsPlugin.Metrics.PublishThroughputMetric(url, new List<TimeUnit>() { TimeUnit.Seconds });
            }
            catch
            {
#if DEBUG
                throw;
#endif
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (ViewPreRender != null)
                ViewPreRender(this, new EventArgs());
        }

        protected override void OnUnload(EventArgs e)
        {
            // clean the current domain messages collection.
            _spc.DomainMessages = null;

            base.OnUnload(e);
        }

        protected override void OnError(EventArgs e)
        {
            Response.Clear();

            // store some info about the error here, to use on the error page and to store the exception
            // Get the last exception thrown on the server
            Exception ex = Server.GetLastError();

            // try and cast to certain exception types - we use these to determine what to do with the exception
            INotificationException notificationException = ex as INotificationException;

            //Additional info
            SAHLUIException sahlException = new SAHLUIException("", ex);
            if (HttpContext.Current != null)
            {
                sahlException.Server = HttpContext.Current.Server.MachineName;
                sahlException.UserIdentity = HttpContext.Current.User.Identity.Name;
                sahlException.ThreadIdentity = Thread.CurrentPrincipal.Identity.Name;
                sahlException.RequestURL = HttpContext.Current.Request.Url.AbsoluteUri;
                sahlException.TimeStamp = HttpContext.Current.Timestamp;
                sahlException.UserHostAddress = HttpContext.Current.Request.UserHostAddress;
                sahlException.UserHostName = HttpContext.Current.Request.UserHostName;
            }

            // for certain exception types we want to skip the exception handling framework - they're exceptions
            // but not ones we need to log or need to be alerted on
            if ((notificationException == null) && !(ex is MultipleLoginException) && !(ex is UserOrganisationSecurityException))
            {
                if (ex is UIPException)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Session State Contents : ");
                    HttpSessionState Session = HttpContext.Current.Session;
                    for (int i = 0; i < Session.Count; i++)
                    {
                        sb.AppendLine("Key: " + Session.Keys[i] + " Value: " + Session[i].ToString());
                    }
                    ex.Data.Add("HttpSessionStateValues", sb.ToString());

                    //ex = new UIPException(ex.Message + sb.ToString(), ex);
                    //UIPManager.StartNavigationTask("SAHL");
                    //Navigator.Navigate("StartUp");
                }

                SAHLPrincipal principal = SAHLPrincipal.GetCurrent();

                //ICBOService CBOService = ServiceFactory.GetService<ICBOService>();

                CBONode thisNode = CBOManager.GetCurrentCBONode(principal) as CBONode;
                CBONode _currentNode = thisNode;

                while ((thisNode != null) && (thisNode as InstanceNode == null))
                {
                    if (thisNode.ParentNode == null)
                        break;

                    thisNode = thisNode.ParentNode;
                }

                InstanceNode _instanceNode = thisNode as InstanceNode;

                if (_instanceNode != null)
                {
                    string applicationKeyString = "";

                    if (_instanceNode.X2Data.ContainsKey("ApplicationKey"))
                        applicationKeyString = "ApplicationKey";
                    else if (_instanceNode.X2Data.ContainsKey("OfferKey"))
                        applicationKeyString = "OfferKey";
                    else if (_instanceNode.X2Data.ContainsKey("CapOfferKey"))
                        applicationKeyString = "CapOfferKey";

                    if (!String.IsNullOrEmpty(applicationKeyString))
                    {
                        int applicationKey = (int)_instanceNode.X2Data[applicationKeyString];
                        if (applicationKey != 0)
                            ex.Data.Add(applicationKeyString, applicationKey);
                    }
                }

                sahlException.SAHLPrincipalName = ((principal != null && principal.Identity != null && principal.Identity.Name != null) ? principal.Identity.Name : "");
                sahlException.NodeDescription = ((_currentNode != null && _currentNode.LongDescription != null) ? _currentNode.LongDescription : "");
                sahlException.NodePath = ((_currentNode != null && _currentNode.NodePath != null) ? _currentNode.NodePath : "");
                sahlException.GenericKey = (_currentNode != null ? _currentNode.GenericKey.ToString() : "");
                sahlException.GenericKeyTypeKey = (_currentNode != null ? _currentNode.GenericKeyTypeKey.ToString() : "");

                var methodBase = MethodBase.GetCurrentMethod();
                LogPlugin.Logger.LogErrorMessageWithException(methodBase.Name, sahlException.Message, sahlException);
            }

            // if it was a notification exception, then we can just redirect away to a notification screen
            if ((notificationException != null) && (_spc != null))
            {
                List<string> LifeTimeViews = new List<string>();
                LifeTimeViews.Add(base.ViewName);
                LifeTimeViews.Add("Notification");
                ICacheObjectLifeTime NotificationLifeTime = new SimplePageCacheObjectLifeTime(LifeTimeViews);
                if (_spc.GetGlobalData().ContainsKey("NotificationMessage"))
                    _spc.GetGlobalData()["NotificationMessage"] = notificationException.NotificationMessage;
                else
                    _spc.GetGlobalData().Add("NotificationMessage", notificationException.NotificationMessage, NotificationLifeTime);
                Navigator.Navigate("Notification");
            }
            else
            {
                //If controller is null we must view the error on the application error page
                //  that doesn't inherit from the controller base
                Session["APP_EX"] = ex;
                Response.Redirect("~/ApplicationError.aspx");
            }

            // clear the error so that asp.net doesn't try handle the exception after us
            Server.ClearError();
        }

        public override void Dispose()
        {
            base.Dispose();

            if (Metrics.ThreadContext.ContainsKey(Metrics.CORRELATIONID))
            {
                Metrics.ThreadContext.Remove(Metrics.CORRELATIONID);
            }
        }
        #endregion Overrides

        #region IViewBase Members

        public event ViewHandler ViewLoaded;

        public event ViewHandler ViewInitialised;

        public event ViewHandler ViewPreInitialised;

        public event ViewHandler ViewPreRender;

        /// <summary>
        /// Gets the domain messages collection to be handled by the view.  Any messages to be displayed by the view
        /// should be added to this collection.  If <see cref="IValidationSummary"/> is not null, the messages
        /// collection on that will be used.
        /// </summary>
        public IDomainMessageCollection Messages
        {
            get
            {
                return _spc.DomainMessages;
            }
        }

    

        /// <summary>
        /// Gets/sets the fully qualified type of the presenter currently being used behind the view.
        /// </summary>
        public string CurrentPresenter
        {
            get
            {
                return _currentPresenter;
            }
            set
            {
                _currentPresenter = value;
            }
        }

        public SAHLPrincipal CurrentPrincipal
        {
            get
            {
                return _currentPrincipal;
            }
        }

        /// <summary>
        /// Implements <see cref="IViewBase.IsValid"/>.
        /// </summary>
        public new bool IsValid
        {
            get
            {
                if (Master.ValidationSummary == null)
                    return base.IsValid;

                return Master.ValidationSummary.IsValid;
            }
        }

        #endregion IViewBase Members

        #region Properties

        public CBOManager CBOManager
        {
            get
            {
                if (_cboManager == null)
                    _cboManager = new CBOManager();

                return _cboManager;
            }
        }

        /// <summary>
        /// Determines whether the loading of the view is as a result of a menu click.  This is useful as sometimes
        /// a user will click the SAME menu item as the currently displayed view and the view may need to know this
        /// in order to be able to clear up cached data, especially if the view moves between different display states.
        /// </summary>
        public virtual bool IsMenuPostBack
        {
            get
            {
                return _isMenuPostBack;
            }
            set
            {
                // add this to the global cache so it's available after the navigation occurs - this is cleared
                // out on preint and a loca variable set
                GlobalData globalData = _spc.GetGlobalData();
                globalData.Add(IsMenuPostBackValue, value, new List<ICacheObjectLifeTime>());
                _isMenuPostBack = value;
            }
        }

        /// <summary>
        /// Gets the master page pertaining to the view.
        /// </summary>
        public new SAHLMasterBase Master
        {
            get
            {
                return base.Master as SAHLMasterBase;
            }
        }

        /// <summary>
        /// Determines whether a page should be run or not.  This relies on a "navigate" value being passed in the
        /// form collection - if this is found it will return false, otherwise it will always return true.
        /// </summary>
        /// <returns></returns>
        public bool ShouldRunPage
        {
            get
            {
                return _shouldRunPage;
            }
            set
            {
                _shouldRunPage = value;
            }
        }

        /// <summary>
        /// The name of the view.
        /// </summary>
        public new string ViewName
        {
            get
            {
                return base.ViewName;
            }
        }

        public IDictionary<string, string> ViewAttributes
        {
            get
            {
                return _viewAttributes;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Writes trace information out, with the fully qualified name of <c>source</c> as the category.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        public void AddTrace(object source, string message)
        {
            Trace.Write(source.GetType().FullName, message);
        }

        /// <summary>
        /// Registers a web service for use by a view.
        /// </summary>
        /// <param name="serviceUrl">The URL of the web service e.g. <c>~/AJAX/Address.asmx</c>.</param>
        public void RegisterWebService(string serviceUrl)
        {
            if (Page.Master != null && !_webServices.Contains(serviceUrl))
            {
                ToolkitScriptManager scriptManager = ControlFinder.FindControl<ToolkitScriptManager>(Page.Master, "scriptManager");
                scriptManager.Services.Add(new ServiceReference(serviceUrl));
                _webServices.Add(serviceUrl);
            }
        }

        #endregion Methods

        /// <summary>
        /// Class used internally
        /// </summary>
        [Serializable]
        private class UserTokenDetails
        {
            public UserTokenDetails(string token, string remoteAddress, string remoteHost, string forwardedFor)
            {
                Token = token;
                RemoteAddress = remoteAddress;
                RemoteHost = remoteHost;
                ForwardedFor = forwardedFor;
            }

            public string Token;
            public string RemoteAddress;
            public string RemoteHost;
            public string ForwardedFor;
        }

        [Serializable]
        public class SAHLUIException : Exception
        {
            public SAHLUIException(string message, Exception exception)
                : base(message, exception)
            {
            }

            public string Server { get; set; }

            public DateTime TimeStamp { get; set; }

            public string PrincipalIdentity { get; set; }

            public string UserIdentity { get; set; }

            public string ThreadIdentity { get; set; }

            public string RequestURL { get; set; }

            public string UserHostAddress { get; set; }

            public string UserHostName { get; set; }

            public string NodeDescription { get; set; }

            public string NodePath { get; set; }

            public string GenericKey { get; set; }

            public string GenericKeyTypeKey { get; set; }

            public string SAHLPrincipalName { get; set; }

            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("Server", Server);
                info.AddValue("TimeStamp", TimeStamp);
                info.AddValue("PrincipalIdentity", PrincipalIdentity);
                info.AddValue("UserIdentity", UserIdentity);
                info.AddValue("ThreadIdentity", ThreadIdentity);
                info.AddValue("RequestURL", RequestURL);
                info.AddValue("UserHostAddress", UserHostAddress);
                info.AddValue("UserHostName", UserHostName);
                info.AddValue("NodeDescription", NodeDescription);
                info.AddValue("NodePath", NodePath);
                info.AddValue("GenericKey", GenericKey);
                info.AddValue("GenericKeyTypeKey", GenericKeyTypeKey);
                info.AddValue("SAHLPrincipalName", SAHLPrincipalName);
                base.GetObjectData(info, context);
            }
        }
    }
}