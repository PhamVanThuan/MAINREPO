using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SAHL.Web.ClientSecure.Models;
using SAHL.Web.ClientSecure.Security;
using SAHL.Web.ClientSecure.ClientSecureService;


namespace SAHL.Web.ClientSecure.Controllers
{
    public class AccountController : BaseController
    {
        private IClientSecure clientSecureService;

        /// <summary>
        /// Initializes a new Attorney Controller
        /// </summary>
        /// <param name="attorneyService"></param>
        public AccountController(IClientSecure clientSecureService)
            : base(clientSecureService)
        {
            this.clientSecureService = clientSecureService;
        }

        /// <summary>
        /// Account/Login
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Account/Login
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginViewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                string leKey = String.Empty;
                //For now, the legal entity key is the email address
                //We have a transport mechanism to get messages to and from the service.
                //The SAHL Principal Cache is that mechanism and we need a unique key to identify a user to get those messages before we actually login and have a proper
                //Legal Entity Key
                Session.Add(SessionConstants.LegalEntityKey, loginViewModel.UserName);
                if (clientSecureService.Login(out leKey, loginViewModel.UserName, SAHL.Web.ClientSecure.Security.Password.Encrypt(loginViewModel.Password)))
                {
                    //save credentials that we will pass to external authenticated services
                    SaveCredentials(loginViewModel.UserName, SAHL.Web.ClientSecure.Security.Password.Encrypt(loginViewModel.Password));
                    FormsAuthentication.SetAuthCookie(loginViewModel.UserName, loginViewModel.RememberMe);
                    Session.Add(SessionConstants.LegalEntityKey, leKey);

                    int? subsidyAccountKey = clientSecureService.GetSubsidyAccountKey(Convert.ToInt32(leKey));
                    if(subsidyAccountKey.HasValue)
                    {
                        Session.Add(SessionConstants.SubsidyAccountKey, subsidyAccountKey);
                    }

                    if (clientSecureService.MortgageLoanAccountKeys(loginViewModel.UserName, SAHL.Web.ClientSecure.Security.Password.Encrypt(loginViewModel.Password)).Count() == 0)
                    {
                        return LogOffBlank();
                    }

                    return RedirectToAction("LoanStatement", "Report");
                }
                else if (!IsValid)
                {
                    return View(loginViewModel);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(loginViewModel);
        }

        /// <summary>
        /// Account/Logoff
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            FormsAuthentication.RedirectToLoginPage();
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// Account/Logoff
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult LogOffBlank()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            FormsAuthentication.RedirectToLoginPage();
            return RedirectToAction("Blank", "Blank");
        }


        /// <summary>
        /// Account/ResetPassword
        /// </summary>
        /// <returns></returns>
        public ActionResult ResetPassword()
        {
            return View();
        }

        /// <summary>
        /// Account/ResetPassword
        /// </summary>
        /// <param name="passwordViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResetPassword(PasswordViewModel passwordViewModel)
        {
            if (ModelState.IsValid)
            {
                Session.Add(SessionConstants.LegalEntityKey, passwordViewModel.UserName);
                if (clientSecureService.ResetPassword(passwordViewModel.UserName))
                {
                    ShowMessage("You should receive details to assist you shortly. If you have not received an e-mail in 20 minutes, please contact the helpdesk (details listed at the bottom of this page) to confirm your e-mail address", MessageType.Info);
                    return RedirectToAction("Login", "Account");
                }
                else if (!IsValid)
                {
                    return View(passwordViewModel);
                }
            }
            return View(passwordViewModel);
        }

        /// <summary>
        /// Account/Register
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangePassword()
        {
            return View();
        }

        /// <summary>
        /// Account/ChangePassword
        /// </summary>
        /// <param name="changePasswordViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                string newPassword = SAHL.Web.ClientSecure.Security.Password.Encrypt(changePasswordViewModel.NewPassword);
                if (clientSecureService.LegalEntityChangePassword(Username, Password, newPassword))
                {
                    ////save credentials that we will pass to external authenticated services
                    SaveCredentials(Username, SAHL.Web.ClientSecure.Security.Password.Encrypt(changePasswordViewModel.NewPassword));

                    ShowMessage("Password change was successful", MessageType.Info);
                }
                //Let's get some details as to what went wrong?
                else if (!IsValid)
                {
                    return View(changePasswordViewModel);
                }
            }
            return View(changePasswordViewModel);
        }
    }
}
