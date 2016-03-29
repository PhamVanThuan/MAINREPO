using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Factories;
using SAHL.Web.Views.Common.Presenters.FutureDatedTransactions;
using SAHL.Common.Web.UI.Controls;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;

namespace SAHL.Web.Views.Common.Presenters.DebitOrderDetailsApplication
{
    /// <summary>
    /// 
    /// </summary>
    public class DebitOrderDetailsAppDelete : DebitOrderDetailsAppBase
    {


        /// <summary>
        /// 
        /// </summary>
        /// <param name="View"></param>
        /// <param name="Controller"></param>
        public DebitOrderDetailsAppDelete(IDebitOrderDetails View, SAHLCommonBaseController Controller)
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
            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnSubmitButtonClicked += new KeyChangedEventHandler(_view_OnSubmitButtonClicked);

            _view.gridPostBackType = GridPostBackType.SingleClick;

            _view.BindGridForApplication(base.Application);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            _view.ShowButtons = true;
            _view.ShowControls = false;
            _view.ShowLabels = false;
            _view.SubmitButtonText = "Delete";
        }

        void _view_OnSubmitButtonClicked(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            TransactionScope ts = new TransactionScope();
            try
            {
                int key = int.Parse(e.Key.ToString());
                for(int x=0;x<Application.ApplicationDebitOrders.Count;x++)
                {
                    if (base.Application.ApplicationDebitOrders[x].Key == key)
                    {
                        base.Application.ApplicationDebitOrders.RemoveAt(_view.Messages, x);
                        break;
                    }
                }
                IApplicationRepository AR = RepositoryFactory.GetRepository<IApplicationRepository>();
                AR.SaveApplication(base.Application);
                ts.VoteCommit();
                _view.Navigator.Navigate("DebitOrderDetailsDisplay");
            }
            catch (DomainValidationException)
            {
                ts.VoteRollBack();
            }
            catch (Exception)
            {
                ts.VoteRollBack();
            }
            finally
            {
                ts.Dispose();
            }

            
        }

        void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("DebitOrderDetailsAppDisplay");
        }
    }
}
