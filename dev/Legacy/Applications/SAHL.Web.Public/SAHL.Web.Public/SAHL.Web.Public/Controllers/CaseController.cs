using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SAHL.Web.Public.Models;
using SAHL.Web.Public.AttorneyService;

namespace SAHL.Web.Public.Controllers
{
    [Authorize]
    public class CaseController : BaseController
    {
        private IAttorney attorneyService;

        /// <summary>
        /// Initializes a new note controller
        /// </summary>
        /// <param name="attorneyService"></param>
        public CaseController(IAttorney attorneyService) : base(attorneyService)
        {
            this.attorneyService = attorneyService;
        }

        /// <summary>
        /// Case/Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Case/Search
        /// </summary>
        /// <returns></returns>
        public ActionResult Search()
        {
            return View();
        }

        /// <summary>
        /// Case/Search
        /// </summary>
        /// <param name="searchViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Search(SearchViewModel searchViewModel)
        {
            DebtCounselling[] cases = attorneyService.SearchForCases(int.Parse(Session[SessionConstants.LegalEntityKey].ToString()), 
                                                                     searchViewModel.CaseNumber ?? 0, 
                                                                     !String.IsNullOrEmpty(searchViewModel.IDNumber) ? searchViewModel.IDNumber.Trim() : null,
                                                                     !String.IsNullOrEmpty(searchViewModel.LegalEntityName) ? searchViewModel.LegalEntityName.Trim() : null, 
                                                                     Username, 
                                                                     Password);

            //Find some cases and then pass the model back to the view
            List<SearchResultViewModel> resultsViewModel = AutoMapper.Mapper.Map<DebtCounselling[], List<SearchResultViewModel>>(cases);
            searchViewModel.SearchResults = resultsViewModel;
            return View(searchViewModel);
        }

        /// <summary>
        /// /Case/Detail
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="debtCounsellingKey"></param>
        /// <returns></returns>
        public ActionResult Detail(int debtCounsellingKey, int accountKey)
        {
            //Load the Case
            CaseDetailViewModel caseDetailViewModel = new CaseDetailViewModel
            {
                CaseNumber = debtCounsellingKey,
                AccountKey = accountKey
            };
            Session[SessionConstants.DebtCounsellingKey] = debtCounsellingKey;
            Session[SessionConstants.AccountKey] = accountKey;
            return View(caseDetailViewModel);
        }
    }
}
