using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SAHL.Web.ClientSecure.Models;

namespace SAHL.Web.ClientSecure.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        /// <summary>
        /// Initializes a new home Controller
        /// </summary>
        public HomeController(SAHL.Web.ClientSecure.ClientSecureService.IClientSecure clientSecureService)
            : base(clientSecureService)
        {

        }

        /// <summary>
        /// Home/Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            try
            {

            }
            catch(Exception ex)
            {

            }
            return View();
        }
    }
}
