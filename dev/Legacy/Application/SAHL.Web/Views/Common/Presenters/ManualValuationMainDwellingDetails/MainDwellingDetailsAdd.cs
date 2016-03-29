using System;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using System.Collections.Generic;
using SAHL.Common.CacheData;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;
using SAHL.Common.Factories;
using SAHL.Common.Collections;


namespace SAHL.Web.Views.Common.Presenters.ManualValuationMainDwellingDetails
{
	public class MainDwellingDetailsAdd : SAHLCommonBasePresenter<IValuationManualPropertyDetailsView>
	{
		private CBOMenuNode _node;
		private IProperty _property;
		private ILookupRepository _lookupRepo;
		private IPropertyRepository _propertyRepo;
		private IList<ICacheObjectLifeTime> _lifeTimes;
		private IValuationDiscriminatedSAHLManual _valManual;
		private IValuationMainBuilding _valMainBuilding;
		private IValuationCottage _valCottage;

		public MainDwellingDetailsAdd(IValuationManualPropertyDetailsView view, SAHLCommonBaseController controller)
			: base(view, controller)
		{
		}

		protected override void OnViewInitialised(object sender, EventArgs e)
		{
			base.OnViewInitialised(sender, e);

			if (!_view.ShouldRunPage)
				return;

			if (_view.IsMenuPostBack)
				GlobalCacheData.Remove(ViewConstants.ValuationManual);

			_lifeTimes = new List<ICacheObjectLifeTime>();

			_node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

			_propertyRepo = RepositoryFactory.GetRepository<IPropertyRepository>();
			_lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

			switch (_node.GenericKeyTypeKey)
			{
				case (int)GenericKeyTypes.Account:
					IAccount acc = RepositoryFactory.GetRepository<IAccountRepository>().GetAccountByKey(_node.GenericKey);
					IMortgageLoanAccount mla = acc as IMortgageLoanAccount;
					_property = mla.SecuredMortgageLoan.Property;
					break;
				case (int)GenericKeyTypes.Property:
					IProperty property = RepositoryFactory.GetRepository<IPropertyRepository>().GetPropertyByKey(_node.GenericKey);
					_property = property;
					break;
				case (int)GenericKeyTypes.Offer:
					IApplicationMortgageLoan app = RepositoryFactory.GetRepository<IApplicationRepository>().GetApplicationByKey(_node.GenericKey) as IApplicationMortgageLoan;
					_property = app.Property;
					break;
				default:
					break;
			}

			_valManual = null;
			// if we have come back to this screen then get changed values from cache
			if (GlobalCacheData.ContainsKey(ViewConstants.ValuationManual))
			{
				_valManual = GlobalCacheData[ViewConstants.ValuationManual] as IValuationDiscriminatedSAHLManual;
				_valMainBuilding = _valManual.ValuationMainBuilding;
				_valCottage = _valManual.ValuationCottage;
			}

			_view.Property = _property;
			_view.HideAllPanels();
			_view.ShowPanelsForAdd();

			_view.BindBuildingClassification(_lookupRepo.ValuationClassification);
			_view.BindCottageRoofTypes(_lookupRepo.ValuationRoofTypes);
			_view.BindMainBuildingRoofType(_lookupRepo.ValuationRoofTypes);

			if (_valManual != null)
				_view.BindUpdateableFields(_valManual, _valMainBuilding, _valCottage);

			_view.OnNextButtonClicked += _view_OnNextButtonClicked;
			_view.OnCancelButtonClicked += _view_OnCancelButtonClicked;
		}

		protected void _view_OnNextButtonClicked(object sender, EventArgs e)
		{
			ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

			IValuationDiscriminatedSAHLManual valManual = null;

			if (_valManual == null)
				valManual = _propertyRepo.CreateEmptyValuation(ValuationDataProviderDataServices.SAHLManualValuation) as IValuationDiscriminatedSAHLManual;
			else
				valManual = _valManual;

			valManual = _view.GetValuationManual(valManual);

			// set these values here so that the mandatory fields validation passes
			valManual.Property = _property;
			valManual.ValuationStatus = lookupRepo.ValuationStatus.ObjectDictionary[Convert.ToString((int)ValuationStatuses.Complete)];

			// Main Building
			IValuationMainBuilding valMainBuilding = _propertyRepo.CreateEmptyValuationMainBuilding();
			valMainBuilding = _view.GetValuationMainBuilding(valMainBuilding);
			valMainBuilding.Valuation = valManual;
			valManual.ValuationMainBuilding = valMainBuilding;

			// validate manual validation
			ExclusionSets.Add(RuleExclusionSets.ValuationMainBuildingView);
			valManual.ValidateEntity();
			ExclusionSets.Remove(RuleExclusionSets.ValuationMainBuildingView);

			// validate main building
			valMainBuilding.ValidateEntity();

			// Cottage 
			IValuationCottage valCottage = _propertyRepo.CreateEmptyValuationCottage();
			valCottage = _view.GetValuationCottage(valCottage);
			if (valCottage != null)
			{
				valCottage.Valuation = valManual;

				if (valCottage.ValidateEntity()) // validate cottage
					valManual.ValuationCottage = valCottage;
			}

			//only navigate if the view is valid
			if (_view.IsValid)
			{
				GlobalCacheData.Remove(ViewConstants.ValuationManual);
				GlobalCacheData.Add(ViewConstants.ValuationManual, valManual, _lifeTimes);
				GlobalCacheData.Remove(ViewConstants.Properties);
				GlobalCacheData.Add(ViewConstants.Properties, new EventList<IProperty>(new IProperty[] { _property }), _lifeTimes);
				Navigator.Navigate("Next");
			}
		}

		protected virtual void _view_OnCancelButtonClicked(object sender, EventArgs e)
		{
			Navigator.Navigate("Cancel");
		}

	}
}
