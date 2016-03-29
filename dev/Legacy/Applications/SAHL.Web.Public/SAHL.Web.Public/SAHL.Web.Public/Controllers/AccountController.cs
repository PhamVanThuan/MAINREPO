using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SAHL.Web.Public.Models;
using SAHL.Web.Public.Security;
using SAHL.Web.Public.AttorneyService;


namespace SAHL.Web.Public.Controllers
{
    public class AccountController : BaseController
    {
        private IAttorney attorneyService;

        /// <summary>
        /// Initializes a new Attorney Controller
        /// </summary>
        /// <param name="attorneyService"></param>
        public AccountController(IAttorney attorneyService)
            : base(attorneyService)
        {
            this.attorneyService = attorneyService;
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
                if (attorneyService.Login(out leKey, loginViewModel.UserName, SAHL.Web.Public.Security.Password.Encrypt(loginViewModel.Password)))
                {

                    //save credentials that we will pass to external authenticated services
                    SaveCredentials(loginViewModel.UserName, SAHL.Web.Public.Security.Password.Encrypt(loginViewModel.Password));
                    FormsAuthentication.SetAuthCookie(loginViewModel.UserName, loginViewModel.RememberMe);
                    Session.Add(SessionConstants.LegalEntityKey, leKey);

                    return RedirectToAction("Search", "Case");
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
            return RedirectToAction("Index", "Home");
        }


        /// <summary>
        /// Account/ForgottenPassword
        /// </summary>
        /// <returns></returns>
        public ActionResult ForgottenPassword()
        {
            return View();
        }

        /// <summary>
        /// Account/ForgottenPassword
        /// </summary>
        /// <param name="passwordViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ForgottenPassword(PasswordViewModel passwordViewModel)
        {
            if (ModelState.IsValid)
            {
                Session.Add(SessionConstants.LegalEntityKey, passwordViewModel.UserName);
                if (attorneyService.ForgottenPassword(passwordViewModel.UserName))
                {
                    ShowMessage("Password reset was succesfull, an email containing your login details have been sent", MessageType.Info);
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
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Account/Register
        /// </summary>
        /// <param name="registerViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                Session.Add(SessionConstants.LegalEntityKey, registerViewModel.UserName);
                if (attorneyService.RegisterUser(registerViewModel.UserName))
                {
                    ShowMessage("Registration was succesful, an email containing your registration details have been sent", MessageType.Info);
                    return RedirectToAction("Login", "Account");
                }
                //Let's get some details as to what went wrong?
                else if (!IsValid)
                {
                    return View(registerViewModel);
                }
            }
            return View(registerViewModel);
        }
    }
}
