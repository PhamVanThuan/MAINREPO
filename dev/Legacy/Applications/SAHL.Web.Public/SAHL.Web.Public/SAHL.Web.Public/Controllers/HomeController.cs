using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SAHL.Web.Public.Models;

namespace SAHL.Web.Public.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        /// <summary>
        /// Initializes a new home Controller
        /// </summary>
        public HomeController(SAHL.Web.Public.AttorneyService.IAttorney attorney) :base(attorney)
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
