using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Cap.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Cap.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class CAPReAllocateOfferCapBroker : SAHLCommonBasePresenter<ICAPReAllocateOffer>
    {
        #region Private Variables

        DataTable _capOfferList;
        ICapRepository _capRepository;

        #endregion


        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CAPReAllocateOfferCapBroker(ICAPReAllocateOffer view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            _view.OnUpdateButtonClicked += new EventHandler(_view_OnUpdateButtonClicked);

            _capRepository = RepositoryFactory.GetRepository<ICapRepository>();

            IList<IBroker> aduserlist = _capRepository.GetCapBrokers();
            _view.BindBrokerLists(aduserlist);

            if (_view.IsPostBack)
            {
                if (_view.AllocatedToListSelectedValue != -1)
                {
                    _capOfferList = _capRepository.GetCapOffersByBrokerKey(_view.AllocatedToListSelectedValue);
                    _view.BindGrid(_capOfferList);
                }
            }
        }

        #region Event Handlers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnUpdateButtonClicked(object sender, EventArgs e)
        {
            IList<int> selectedOffers = _view.SelectedOffers;
            if (selectedOffers.Count > 0 && _view.ReAllocateToListSelectedValue != -1)
            {
                IBroker newBroker = _capRepository.GetBrokerByBrokerKey(_view.ReAllocateToListSelectedValue);
                IList<ICapApplication> capOfferList = new List<ICapApplication>();
                for (int iC = 0; iC < selectedOffers.Count; iC++)
                {
                    ICapApplication capOffer = _capRepository.GetCapOfferByKey(selectedOffers[iC]);
                    if (capOffer != null)
                    {
                        capOffer.Broker = newBroker;
                        capOfferList.Add(capOffer);
                    }
                }

              
                TransactionScope trans = new TransactionScope();
                try
                {
                    for (int i = 0; i < capOfferList.Count; i++)
                    {
                        _capRepository.SaveCapApplication(capOfferList[i]);
                        _capRepository.UpdateX2CapCaseData(capOfferList[i].Key, capOfferList[i].Broker.Key);
                    }
                    trans.VoteCommit();
                }
                catch (Exception)
                {
                    trans.VoteRollBack();
                    if (_view.IsValid)
                        throw;
                }
                finally
                {
                    trans.Dispose();
                }
            }

            //Navigator.Navigate("Update");
            if(_view.Messages.Count==0)
                Navigator.Navigate("CapReAllocateOffer");
        }

        #endregion

    }
}
