using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Life.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;
using System.Collections;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Life.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class Test : SAHLCommonBasePresenter<ITest>
    {
        private SAHL.Common.BusinessModel.Interfaces.IAccount _lifeAccount;
        private IReadOnlyEventList<ILegalEntityNaturalPerson> _lstPersons;
        private IReadOnlyEventList<ILegalEntity> _lstLegalEntities;
        private IList<SAHL.Web.Controls.LegalEntityGridItem> _lstLegalEntityGridItems;
        private SAHL.Web.Controls.LegalEntityGridItem _legalEntityGridItem;
        private int _genericKey;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="View"></param>
        /// <param name="Controller"></param>
        public Test(ITest View, SAHLCommonBaseController Controller)
            : base(View, Controller)
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

            _view.OnLegalEntityGridSelectedIndexChanged += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(OnLegalEntityGridSelectedIndexChanged);
            _view.OnLegalEntityGridNewSelectedIndexChanged += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(OnLegalEntityGridNewSelectedIndexChanged);

            _genericKey = 10056322; // fin service of policy 1498616

            IAccountRepository AccountRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            _lifeAccount = AccountRepo.GetAccountByFinancialServiceKey(_view.Messages, Convert.ToInt32(_genericKey));

            // Get Assured lives
            _lstPersons = _lifeAccount.GetNaturalPersonLegalEntitiesByRoleType(_view.Messages, new int[] { 1 });
            _lstLegalEntities = _lifeAccount.GetLegalEntitiesByRoleType(_view.Messages, new int[] { 1 });

            // Bind Old Grid
            _view.BindLegalEntityGrid(_lstPersons);

            // Bind New Grid
            //_lstLegalEntityGridItems.Add(new SAHL.Web.Controls.LegalEntityGridItem(_lstLegalEntities,null));
            //_view.BindLegalEntityGridNew(_lstLegalEntityGridItems);
            _view.BindLegalEntityGridNew(_lstLegalEntities);
        }

        void OnLegalEntityGridSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
        }

        void OnLegalEntityGridNewSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            int ikey = (int)e.Key;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewLoaded(object sender, EventArgs e)
        {
            base.OnViewLoaded(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
        }

    }
}
