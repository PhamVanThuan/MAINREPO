using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SAHL.Web.ClientSecure.ClientSecureService;
using System.Reflection;
using System.Web.Security;
using SAHL.Common.Logging;

namespace SAHL.Web.ClientSecure.Controllers
{
    public class BaseController : Controller
    {
        protected const string _validationFailureMessage = @"An error has occurred, if this continues please contact us on Tel:  0861 888 777 or E-mail: admin@sahomeloans.com.";

        private IClientSecure clientSecureService;
        /// <summary>
        /// Initializes a new Base Controller
        /// </summary>
        public BaseController(IClientSecure clientSecureService)
        {
            this.clientSecureService = clientSecureService;
            //Firstly, ensure that the Legal Entity Key is in the session, otherwise, redirect to login page, we are probably still logged in though
            if (Session != null && Session[SessionConstants.LegalEntityKey] == null)
            {
                FormsAuthentication.SignOut();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public bool IsValid
        {
            get
            {
                //get the messages
                ServiceMessage serviceMessage = clientSecureService.GetServiceMessage(Session[SessionConstants.LegalEntityKey].ToString());
                //show any relevant messages
                HandleServiceMessage(serviceMessage);
                
                return serviceMessage.Success;
            }
        }

        /// <summary>
        /// Username
        /// </summary>
        public string Username
        {
            get
            {
                return Session[SessionConstants.Username].ToString();
            }
        }

        /// <summary>
        /// Password
        /// </summary>
        public string Password
        {
            get
            {
                return Session[SessionConstants.Password].ToString();
            }
        }

        /// <summary>
        /// Show Message
        /// </summary>
        /// <param name="messageToShow"></param>
        protected void ShowMessage(string message, MessageType messageType)
        {
            if (Session[SessionConstants.Messages] == null)
            {
                Session[SessionConstants.Messages] = new List<Message>();
            }
            ((List<Message>)Session[SessionConstants.Messages]).Add(new Message
            {
                Details = message,
                MessageType = messageType
            });
        }

        /// <summary>
        /// Save Credentials
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        protected void SaveCredentials(string username, string password)
        {
            Session[SessionConstants.Username] = username;
            Session[SessionConstants.Password] = password;
        }

        /// <summary>
        /// Handle Service Response
        /// </summary>
        /// <param name="response"></param>
        protected void HandleServiceMessage(ServiceMessage serviceMessage)
        {
            if (!serviceMessage.Success)
            {
                foreach (var message in serviceMessage.ServiceMessages)
                {
                    // if there are any real errors, show a default message
                    if (message.MessageType == ServiceMessageType.Error)
                        ShowMessage(_validationFailureMessage, MessageType.Error);
                    else
                        ShowMessage(message.Description, (MessageType)Enum.Parse(typeof(MessageType), message.MessageType.ToString()));
                }
            }
        }

        /// <summary>
        /// On Exception
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            var routeParameters = new Dictionary<string, object>();
            string action = String.Empty;
            string controller = String.Empty;
            if (filterContext.RouteData.Values.ContainsKey("action"))
            {
                action = filterContext.RouteData.Values["action"].ToString();
                routeParameters.Add("Action", action);
            }
            if (filterContext.RouteData.Values.ContainsKey("controller"))
            {
                controller = filterContext.RouteData.Values["controller"].ToString();
                routeParameters.Add("Controller", controller);
            }
            if (filterContext.RouteData.Values.ContainsKey("id"))
            {
                routeParameters.Add("ID", filterContext.RouteData.Values["id"]);
            }
            LogPlugin.Logger.LogErrorMessageWithException(String.Format("{0}/{1}", controller, action), filterContext.Exception.Message, filterContext.Exception, routeParameters);
            ShowMessage(_validationFailureMessage, MessageType.Error);  //ShowMessage(filterContext.Exception.Message, MessageType.Error);
            base.OnException(filterContext);
        }
    }

    /// <summary>
    /// Message
    /// </summary>
    public class Message
    {
        public MessageType MessageType { get; set; }
        public string Details { get; set; }
    }

    /// <summary>
    /// Message Type
    /// </summary>
    public enum MessageType
    {
        Info,
        Success,
        Warning,
        Error
    }

    public static class SessionConstants
    {
        public const string Username = "Username";
        public const string Password = "Password";
        public const string Messages = "Messages";
        public const string LegalEntityKey = "LegalEntityKey";
        public const string DebtCounsellingKey = "DebtCounsellingKey";
        public const string AccountKey = "AccountKey";
        public const string SubsidyAccountKey = "SubsidyAccountKey";
    }
}
