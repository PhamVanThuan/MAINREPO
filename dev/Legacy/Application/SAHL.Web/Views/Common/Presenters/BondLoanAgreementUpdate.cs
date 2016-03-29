using System;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using Castle.ActiveRecord;
using System.Collections.Generic;

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class BondLoanAgreementUpdate : BondLoanAgreementBase
    {
        private IList<IAttorney> _listAttorney;
        private Dictionary<int, string> _dictAttorney;
        private ILookupRepository _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public BondLoanAgreementUpdate(IBondLoanAgreement view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            _view.BondGridPostBack = true; //Only enable postback when doing updates, needs to be set before the base initialises

            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;
            
            _view.OnSubmitButtonClicked += (_view_OnSubmitButtonClicked);
            _view.OnDeedsOfficeUpdate_SelectedIndexChanged += (_view_DeedsOfficeUpdate_SelectedIndexChanged);
            _view.OnBondGrid_SelectedIndexChanged += (_view_BondGrid_SelectedIndexChanged);

            BindDisplay();
            PrivateCacheData.Remove("BondKey");
            PrivateCacheData.Add("BondKey",0); 
        }

        /// <summary>
        /// Set the relevant properties for displaying controls within the view
        /// </summary>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.ShowLoanAgreeGrid = false;
            _view.AddLoanAgreement = false;
            _view.SubmitButtonText = "Update";

            //Only populate this here, so that the sleected value is not retained when reloading
            _view.BindAttorney(_dictAttorney);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            // Get the bond 
            IBond bond = SelectedBond(); 

            // Populate the object with changes
            foreach (IDeedsOffice dO in _lookupRepo.DeedsOffice)
            {
                if (dO.Key == _view.DeedsOfficeSelectedValue)
                    bond.DeedsOffice = dO;
            }

            foreach (IAttorney att in _listAttorney)
            {
                if (att.Key == _view.AttorneySelectedValue)
                    bond.Attorney = att;
            }

            bond.BondRegistrationAmount = _view.BondRegAmount;
            bond.BondRegistrationNumber = _view.BondRegNumber;

            // save
            TransactionScope txn = new TransactionScope();
            try
            {
                IBondRepository bRepo = RepositoryFactory.GetRepository<IBondRepository>();
                //bond.ValidateEntity();
                bRepo.SaveBond(bond);
            }
            catch (Exception)
            {
                txn.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }
            finally
            {
                txn.Dispose();
            }

            // And navigate
            if (_view.IsValid)
                Navigator.Navigate("BondLoanAgreement");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void _view_DeedsOfficeUpdate_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetAttorneysFromDeedsOfficeSelected();
            _view.BindAttorney(_dictAttorney);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void _view_BondGrid_SelectedIndexChanged(object sender, EventArgs e)
        {

            PrivateCacheData.Remove("BondKey");
            PrivateCacheData.Add("BondKey", _view.BondGridIndex); 
            BindDisplay();
        }

        /// <summary>
        /// 
        /// </summary>
        private void GetAttorneysFromDeedsOfficeSelected()
        {
            if (_dictAttorney == null)
                _dictAttorney = new Dictionary<int,string>();

            _dictAttorney.Clear();

            IRegistrationRepository regRep = RepositoryFactory.GetRepository<IRegistrationRepository>();
            _listAttorney = regRep.GetAttorneysByDeedsOfficeKey(Convert.ToInt32(_view.DeedsOfficeSelectedValue));

            foreach (IAttorney att in _listAttorney)
            {
                // reload the legal entity, otherwise we get lazy load exceptions
                if (!_dictAttorney.ContainsKey(att.Key))
                {
                    _dictAttorney.Add(att.Key, att.LegalEntity.DisplayName);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindDisplay()
        {
            _view.BindDeedsOffice(_lookupRepo.DeedsOffice);
            GetAttorneysFromDeedsOfficeSelected();
            int BondIndex =  Convert.ToInt32(PrivateCacheData["BondKey"]);

            _view.PopulateUpdateBond(BondIndex);
        }

    }
}
