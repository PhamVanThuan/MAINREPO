using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Origination.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.CacheData;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Origination.Presenters
{
    public class GenericCalculator : SAHLCommonBasePresenter<IGenericCalculator>
    {
        private IOrganisationStructureRepository _orgRepo;
        private ILookupRepository _lookupRepo;
        private IControlRepository _controlRepo;
        private ICommonRepository _commonRepo;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public GenericCalculator(IGenericCalculator view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;

            _view.OnAmortisationScheduleButtonClicked += new EventHandler(_view_OnAmortisationScheduleButtonClicked);
            _view.OnCompanySelectedIndexChanged+=new KeyChangedEventHandler(_view_OnCompanySelectedIndexChanged);

            _orgRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            _controlRepo = RepositoryFactory.GetRepository<IControlRepository>();
            _commonRepo = RepositoryFactory.GetRepository<ICommonRepository>();

            // get the origination sources that the use has access to
            List<IOriginationSource> osList = new List<IOriginationSource>();
            IList<IOrganisationStructureOriginationSource> ososList = _orgRepo.GetOrgStructureOriginationSourcesPerADUser(_view.CurrentPrincipal.Identity.Name);
            foreach (IOrganisationStructureOriginationSource osos in ososList)
            {
                if (osos.OriginationSource.Key == (int)SAHL.Common.Globals.OriginationSources.SALife)
                    continue;

                osList.Add(osos.OriginationSource);
            }
            if (osList.Count > 0)
            {
                // sort the originationsource list ascending so that SAHL appears first (if the user has access to it)
                osList.Sort(delegate(IOriginationSource os1, IOriginationSource os2) { return os1.Key.CompareTo(os2.Key); });
                _view.CompanyList = osList;

                // get the list of link rates for the first company in the list
                GetLinkRates(osList[0].Key);
            }

            // get the market rate
            IMarketRate mr = _lookupRepo.MarketRates.ObjectDictionary[((int)SAHL.Common.Globals.MarketRates.ThreeMonthJIBARRounded).ToString()];
            _view.MarketRate = mr.Value;

            if (GlobalCacheData.ContainsKey(ViewConstants.GenericCalc))
            {
                //this will be populated if we come back from the Ammortization Schedule page
                Dictionary<string, string> cacheDict;
                cacheDict = (Dictionary<string, string>)GlobalCacheData[ViewConstants.GenericCalc];
                if (cacheDict.ContainsKey("Term"))
                    _view.Term = Convert.ToInt16(cacheDict["Term"]);
                if (cacheDict.ContainsKey("CurrentBalance"))
                    _view.CurrentBalance = Convert.ToDouble(cacheDict["CurrentBalance"]);
                if (cacheDict.ContainsKey("Company"))
                    _view.CompanySelectedValue = cacheDict["Company"];
                if (cacheDict.ContainsKey("LinkRate"))
                    _view.LinkRateSelectedValue = cacheDict["LinkRate"];
                if (cacheDict.ContainsKey("CalcType"))
                    _view.CalcType = cacheDict["CalcType"];
                if (cacheDict.ContainsKey("InstalmentTotal"))
                    _view.InstalmentTotal = Convert.ToDouble(cacheDict["InstalmentTotal"]);

                _view.ReloadView = true;

                GlobalCacheData.Remove(ViewConstants.GenericCalc);
            }

            //set control table values
            IControl ctrl = _controlRepo.GetControlByDescription("BondHigh");
            _view.MaxBondAmount = (ctrl.ControlNumeric.HasValue ? Convert.ToDouble(ctrl.ControlNumeric) : 0);
            ctrl = _controlRepo.GetControlByDescription("Calc - maxTerm");
            _view.MaxTerm = (ctrl.ControlNumeric.HasValue ? Convert.ToInt32(ctrl.ControlNumeric) : 240);
        }
        private void GetLinkRates(int originationSourceKey)
        {
            // get the list of link rates for selected company
            _view.LinkRateList.Clear();
            Dictionary<int, string> tmp = _commonRepo.GetLinkRatesByOriginationSource(originationSourceKey);
            foreach (KeyValuePair<int, string> dic in tmp)
            {
                _view.LinkRateList.Add(dic.Key, dic.Value);
            }
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);

            if (!_view.ShouldRunPage)
                return;
        }

        void _view_OnCompanySelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            int selectedCompany = Convert.ToInt32(e.Key);
            GetLinkRates(selectedCompany);
        }

        void _view_OnAmortisationScheduleButtonClicked(object sender, EventArgs e)
        {
            //Setup return values for the calc
            Dictionary<string, string> cacheDict = new Dictionary<string, string>();
            cacheDict.Add("Term", _view.Term.ToString());
            cacheDict.Add("CurrentBalance", _view.CurrentBalance.ToString());
            cacheDict.Add("Company", _view.CompanySelectedValue);
            cacheDict.Add("LinkRate", _view.LinkRateSelectedValue);
            cacheDict.Add("CalcType", _view.CalcType);
            cacheDict.Add("InstalmentTotal", _view.InstalmentTotal.ToString());

            // Setup values to use on AmortisationSchedule screen
            Dictionary<string, double> calcDict = new Dictionary<string, double>();
            calcDict.Add("CurrentBalanceV", _view.CurrentBalance);
            calcDict.Add("InterestRateV", _view.InterestRate);
            calcDict.Add("RemainingTermV", _view.Term);
            calcDict.Add("InstalmentTotalV", _view.InstalmentTotal);
            calcDict.Add("AmorInstallmentV", _view.InstalmentTotal);
            calcDict.Add("MarketRateV", _view.MarketRate);

            // populate the cache variables
            IList<ICacheObjectLifeTime> lifeTimes = new List<ICacheObjectLifeTime>();

            GlobalCacheData.Remove(ViewConstants.NavigateTo);
            GlobalCacheData.Remove(ViewConstants.GenericCalc);
            GlobalCacheData.Remove(ViewConstants.AmortisationSchedule);

            GlobalCacheData.Add(ViewConstants.NavigateTo, _view.ViewName, lifeTimes);
            GlobalCacheData.Add(ViewConstants.GenericCalc, cacheDict, lifeTimes);
            GlobalCacheData.Add(ViewConstants.AmortisationSchedule, calcDict, lifeTimes);

            Navigator.Navigate("AmortisationSchedule");
        }
    }
}
