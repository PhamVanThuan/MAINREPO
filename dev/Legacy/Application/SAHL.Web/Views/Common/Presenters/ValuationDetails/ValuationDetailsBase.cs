using System;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Globals;


namespace SAHL.Web.Views.Common.Presenters.ValuationDetails
{

    /// <summary>
    /// Valuation details Base
    /// </summary>
    public class ValuationDetailsBase : SAHLCommonBasePresenter<IValuationManualDetailsView>
    {
        CBOMenuNode _node;
        protected IEventList<IProperty> _properties;
        protected IAccountHOC _hocAccount;
        protected IValuationDiscriminatedSAHLManual valSAHLManual;
        protected int _genericKey;
        protected bool _hocRetrievedByAccount;
        protected IHOCRepository _hocRepo;

        public CBOMenuNode Node
        {
            get
            {
                return _node;
            }
        }

        public IEventList<IProperty> Properties
        {
            get
            {
                return _properties;
            }
        }
        /// <summary>
        /// Constructor for ValuationDetails Base
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ValuationDetailsBase(IValuationManualDetailsView view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            // set up all data stuff	
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            _hocRepo = RepositoryFactory.GetRepository<IHOCRepository>();
            _genericKey = Convert.ToInt32(_node.GenericKey);

            if (_view.IsMenuPostBack)
                GlobalCacheData.Remove(ViewConstants.ValuationManual);

            IAccount acc;
            IProperty property;
            switch (_node.GenericKeyTypeKey)
            {
                case (int)SAHL.Common.Globals.GenericKeyTypes.Account:
                    acc = RepositoryFactory.GetRepository<IAccountRepository>().GetAccountByKey(_genericKey);
                    IMortgageLoanAccount mla = acc as IMortgageLoanAccount;
                    if (_properties == null)
                        _properties = new EventList<IProperty>();
                    if (mla != null)
                        _properties.Add(_view.Messages, mla.SecuredMortgageLoan.Property);
                    _hocAccount = _hocRepo.RetrieveHOCByAccountKey(_genericKey, ref _hocRetrievedByAccount);
                    break;

                case (int)SAHL.Common.Globals.GenericKeyTypes.Property:
                    property = RepositoryFactory.GetRepository<IPropertyRepository>().GetPropertyByKey(_genericKey);
                    _properties = new EventList<IProperty>();
                    _properties.Add(_view.Messages, property);

                    if (_node.ParentNode.GenericKeyTypeKey == (int)SAHL.Common.Globals.GenericKeyTypes.Account)
                        _hocAccount = _hocRepo.RetrieveHOCByAccountKey(_node.ParentNode.GenericKey, ref _hocRetrievedByAccount);
                    else if (_node.ParentNode.GenericKeyTypeKey == (int)SAHL.Common.Globals.GenericKeyTypes.Offer)
                        _hocAccount = _hocRepo.RetrieveHOCByOfferKey(_node.ParentNode.GenericKey, ref _hocRetrievedByAccount);
                    else
                        throw new Exception("GenericKeyTypeKey not supported in HOC Account retrieve");
                    break;

                case (int)SAHL.Common.Globals.GenericKeyTypes.Offer:
                    IApplicationMortgageLoan app = RepositoryFactory.GetRepository<IApplicationRepository>().GetApplicationByKey(_genericKey) as IApplicationMortgageLoan;

                    if (app != null)
                    {
                        if (_properties == null)
                            _properties = new EventList<IProperty>();
                        _properties.Add(_view.Messages, app.Property);

                        _hocAccount = _hocRepo.RetrieveHOCByOfferKey(_genericKey, ref _hocRetrievedByAccount);
                    }
                    break;

                default:
                    break;
            }

            if (_properties != null)
            {
                // Get the latest complete SAHLManual Valuation
                for (int i = 0; i < _properties.Count; i++)
                {
                    if (_properties[i].LatestCompleteValuation != null)
                        valSAHLManual = _properties[i].LatestCompleteValuation as IValuationDiscriminatedSAHLManual;
                    if (valSAHLManual != null)
                        break;
                }
            }

            // Setup the View Interface
            _view.Properties = _properties;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;


        }

        #region HOC Update Helper Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        protected void ManualValuationHOCUpdate(IValuationDiscriminatedSAHLManual val)
        {
            if (_hocAccount != null)
            {
                IHOC hoc = _hocAccount.HOC;
                if (hoc.HOCInsurer.Key == (int)HOCInsurers.SAHLHOC
                    && (hoc.HOCStatus.Key == (int)HocStatuses.Open || hoc.HOCStatus.Key == (int)HocStatuses.PaidUpwithHOC))
                {

                    double total = (val.HOCConventionalAmount.HasValue ? val.HOCConventionalAmount.Value : 0)
                                + (val.HOCShingleAmount.HasValue ? val.HOCShingleAmount.Value : 0)
                                + (val.HOCThatchAmount.HasValue ? val.HOCThatchAmount.Value : 0);

                    hoc.HOCConventionalAmount = val.HOCConventionalAmount.HasValue ? val.HOCConventionalAmount.Value : 0;
                    hoc.HOCShingleAmount = val.HOCShingleAmount.HasValue ? val.HOCShingleAmount.Value : 0;
                    hoc.HOCThatchAmount = val.HOCThatchAmount.HasValue ? val.HOCThatchAmount.Value : 0;
                    hoc.SetHOCTotalSumInsured(total);
                    hoc.HOCRoof = _view.GetHOCRoof;
                    hoc.UserID = _view.CurrentPrincipal.Identity.Name;
                    hoc.ChangeDate = DateTime.Now;
                    _hocRepo.SaveHOC(hoc);
                    _hocRepo.UpdateHOCPremium(hoc.Key);
                    hoc.Refresh();
                    _hocRepo.UpdateHOCWithHistory(_view.Messages, hoc.HOCInsurer.Key, hoc, 'V');
                }
            }
        }

        #endregion
    }
}
