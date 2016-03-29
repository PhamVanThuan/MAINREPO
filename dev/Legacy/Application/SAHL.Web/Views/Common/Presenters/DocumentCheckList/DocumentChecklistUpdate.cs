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
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel;
using SAHL.Common.UI;

using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using System.Threading;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;

namespace SAHL.Web.Views.Common.Presenters.DocumentCheckList
{
    /// <summary>
    /// 
    /// </summary>
    public class DocumentChecklistUpdate : SAHLCommonBasePresenter<IDocumentChecklist>
    {
        protected CBOMenuNode _node;
        int _genericKey;
        int _genericKeyTypeKey;
        IList<IApplicationDocument> _doclist;
        IDocumentCheckListService _dcs;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public DocumentChecklistUpdate(IDocumentChecklist view, SAHLCommonBaseController controller)
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
            if (!_view.ShouldRunPage) return;

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            _genericKey = Convert.ToInt32(_node.GenericKey);
            _genericKeyTypeKey = _node.GenericKeyTypeKey;

            if (_genericKeyTypeKey == (int)GenericKeyTypes.Offer)
            {
                _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelClicked);
                _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitClicked);
                _dcs = ServiceFactory.GetService<IDocumentCheckListService>();
                _doclist = _dcs.GetList(_genericKey);
                _view.SetViewOnly = false;
                if (_doclist.Count > 0)
                {
                    _view.BindDocumentList(_doclist);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnCancelClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("DocumentChecklistSummary");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnSubmitClicked(object sender, EventArgs e)
        {
            //ListItemCollection applicationDocumentList = _view.clbDocumentItems;
            IList<IApplicationDocument> appDocSavedList = new List<IApplicationDocument>();
            Dictionary<int, bool> applicationDocumentList = _view.GetCheckedItems;
            bool passed = true;

            // Dictionary of Application Document
            Dictionary<int, IApplicationDocument> appDocDict = new Dictionary<int, IApplicationDocument>();
            foreach (IApplicationDocument appDoc in _doclist)
            {
                appDocDict.Add(appDoc.Key, appDoc);
            }

            // State of Selected Items
            Dictionary<int, bool> selectedStateDict = new Dictionary<int, bool>();
            foreach (IApplicationDocument appDoc in _doclist)
            {
                selectedStateDict.Add(appDoc.Key, appDoc.DocumentReceivedDate.HasValue);
            }

            foreach (KeyValuePair<int, bool> kv in applicationDocumentList)
            {
                IApplicationDocument appDocument = appDocDict[Convert.ToInt32(kv.Key)];
                bool selectedState = selectedStateDict[Convert.ToInt32(kv.Key)];
                if (kv.Value != selectedState)
                {
                    if (kv.Value)
                        appDocument.DocumentReceivedDate = DateTime.Now;
                    else
                        appDocument.DocumentReceivedDate = null;

                    appDocument.DocumentReceivedBy = Thread.CurrentPrincipal.Identity.Name;
                    appDocSavedList.Add(appDocument);
                }
            }

            /*
            foreach (ListItem AppDocItem in applicationDocumentList)
            {
                IApplicationDocument appDocument = appDocDict[Convert.ToInt32(AppDocItem.Value)];
                bool selectedState = selectedStateDict[Convert.ToInt32(AppDocItem.Value)];
                if (AppDocItem.Selected != selectedState)
                {
                    if (AppDocItem.Selected)
                        appDocument.DocumentReceivedDate = DateTime.Now;
                    else
                        appDocument.DocumentReceivedDate = null;

                    appDocument.DocumentReceivedBy = Thread.CurrentPrincipal.Identity.Name;
                    appDocSavedList.Add(appDocument);
                }
            }
            */

            TransactionScope trans = new TransactionScope();

            try
            {
                _dcs.SaveList(appDocSavedList);
                trans.VoteCommit();
            }

            catch (Exception)
            {
                trans.VoteRollBack();
                passed = false;
                if (_view.IsValid)
                    throw;
            }
            finally
            {
                trans.Dispose();
            }

            if (passed)
                Navigator.Navigate("DocumentChecklistSummary");
        }
    }
}
