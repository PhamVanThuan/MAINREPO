using SAHL.Web.ClientSecure.ClientSecureService;
using SAHL.Web.ClientSecure.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAHL.Web.ClientSecure.Controllers
{

    [Authorize]
    public class ServiceController : BaseController
    {
        private IClientSecure clientSecureService;
        /// <summary>
        /// Initializes a new home Controller
        /// </summary>
        public ServiceController(IClientSecure clientSecureService)
            : base(clientSecureService)
        {
            this.clientSecureService = clientSecureService;
        }

        //
        // GET: /Service/
        public ActionResult RequestFunds()
        {
            //setup accountkey list
            IList<Int32> accKeys = clientSecureService.MortgageLoanAccountKeys(Username, Password);
            ViewBag.AccountKey = new SelectList(accKeys, 1);

            return View();
        }

        /// <summary>
        /// Account/ChangePassword
        /// </summary>
        /// <param name="requestFundsViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RequestFunds(RequestFundsViewModel requestFundsViewModel)
        {
            if (ModelState.IsValid)
            {
                if (clientSecureService.RequestFunds(Username, Password, requestFundsViewModel.AccountKey, requestFundsViewModel.Amount))
                {
                    ShowMessage("Fund request sent successfully. A consultant will contact you shortly.", MessageType.Info);
                    return RedirectToAction("RequestFunds", "Service");
                }
                else
                {
                    ShowMessage("An error has occurred. Please contact the SA Home Loans Client Services department on 0861 888 777 to request additional funds.", MessageType.Error);
                    return RedirectToAction("RequestFunds", "Service");
                }
            }
            
            IList<Int32> accKeys = clientSecureService.MortgageLoanAccountKeys(Username, Password);
            ViewBag.AccountKey = new SelectList(accKeys, 1);

            return View(requestFundsViewModel);
        }
    }
}
