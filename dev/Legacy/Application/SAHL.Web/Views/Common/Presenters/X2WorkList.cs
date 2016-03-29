using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Diagnostics.CodeAnalysis;

using SAHL.Common;


namespace SAHL.Web.Views.Common.Presenters
{
    public class X2WorkList : SAHLCommonBasePresenter<IX2WorkList>
    {
        IX2Repository _repo;
        DataTable gridData;
        public X2WorkList(IX2WorkList view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals")]
        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.OnSelectButtonClicked += new KeyChangedEventHandler(_view_OnSelectButtonClicked);
            StateNode node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2) as StateNode;

            if (node == null)
                throw new Exception("Cannot navigate to the WorkList view when no node is selected.");

            _repo = RepositoryFactory.GetRepository<IX2Repository>();

            IState state = _repo.GetStateByKey((int)node.GenericKey);

            string gridHeading = "Worklist - " + state.WorkFlow.Name + " (" + state.Name + ")";

            IEventList<IDataGridConfiguration> configList = _repo.GetDataGridConfigurationByWorkFlowName(state.WorkFlow.Name, state.WorkFlow.Process.Name);

            DataTable gridConfig = new DataTable();
            gridConfig.Columns.Add("ColumnName", typeof(string));
            gridConfig.Columns.Add("ColumnDescription", typeof(string));
            gridConfig.Columns.Add("Width", typeof(int));
            gridConfig.Columns.Add("Visible", typeof(bool));
            gridConfig.Columns.Add("FormatString", typeof(string));
            gridConfig.Columns.Add("FormatType", typeof(GridFormatType));

            gridData = PrivateCacheData["WORKLIST"] as DataTable;

            if (configList.Count > 0)
            {
                for (int i = 0; i < configList.Count; i++)
                {
                    DataRow row = gridConfig.NewRow();
                    row[0] = configList[i].ColumnName;
                    row[1] = configList[i].ColumnDescription;
                    row[2] = configList[i].Width.EndsWith("%") ? int.Parse(configList[i].Width.TrimEnd('%')) : int.Parse(configList[i].Width.TrimEnd('p', 'x'));
                    row[3] = configList[i].Visible;

                    if (configList[i].FormatType != null)
                    {
                        row[4] = configList[i].FormatType.Format;

                        switch (configList[i].FormatType.Key)
                        {
                            case (int)SAHL.Common.Globals.FormatTypes.CurrencyFormat:
                                row[5] = GridFormatType.GridCurrency;
                                break;
                            case (int)SAHL.Common.Globals.FormatTypes.NumberFormat:
                                row[5] = GridFormatType.GridNumber;
                                break;
                            case (int)SAHL.Common.Globals.FormatTypes.RateFormat:
                                row[5] = GridFormatType.GridRate;
                                break;
                            case (int)SAHL.Common.Globals.FormatTypes.DateFormat:
                                row[5] = GridFormatType.GridDate;
                                break;
                            case (int)SAHL.Common.Globals.FormatTypes.DateTimeFormat:
                                row[5] = GridFormatType.GridDateTime;
                                break;
                            default:
                                row[5] = GridFormatType.GridString;
                                break;
                        }
                    }
                    else
                    {
                        row[4] = "";
                        row[5] = GridFormatType.GridString;
                    }

                    gridConfig.Rows.Add(row);
                }

                // if it's a postback, we don't need to reload the data from the database
                if (gridData == null || !_view.IsPostBack)
                {
                    gridData = new DataTable();
                    X2Service.GetCustomInstanceDetails(gridData, configList[0].StatementName, (int)node.GenericKey, _view.CurrentPrincipal);
                    PrivateCacheData.Add("WORKLIST", gridData);
                }

            }
            else
            {
                DataRow row = gridConfig.NewRow();
                row.ItemArray = new object[] { "InstanceID", "ID", 8, true, "", null };
                gridConfig.Rows.Add(row);
                row = gridConfig.NewRow();
                row.ItemArray = new object[] { "Subject", "Subject", 37, true, "", null };
                gridConfig.Rows.Add(row);
                row = gridConfig.NewRow();
                row.ItemArray = new object[] { "StateChange", "State Changed", 12, true, "dd/MM/yyyy", GridFormatType.GridDate };
                gridConfig.Rows.Add(row);
                row = gridConfig.NewRow();
                row.ItemArray = new object[] { "Priority", "Priority", 8, true, "", null };
                gridConfig.Rows.Add(row);
                row = gridConfig.NewRow();
                row.ItemArray = new object[] { "Deadline", "Deadline", 10, true, "dd/MM/yyyy", GridFormatType.GridDate };
                gridConfig.Rows.Add(row);
                row = gridConfig.NewRow();
                row.ItemArray = new object[] { "Message", "Message", 25, true, "", null };
                gridConfig.Rows.Add(row);

                gridData.Columns.Add("ID", typeof(long));
                gridData.Columns.Add("Subject", typeof(string));
                gridData.Columns.Add("StateChanged", typeof(DateTime));
                gridData.Columns.Add("Priority", typeof(int?));
                gridData.Columns.Add("Deadline", typeof(DateTime));
                gridData.Columns.Add("Message", typeof(string));

                if (gridData == null || !_view.IsPostBack)
                {
                    gridData = new DataTable();
                    IEventList<IWorkList> workList = _repo.GetWorkListByState(_view.CurrentPrincipal, node.GenericKey);

                    for (int i = 0; i < workList.Count; i++)
                    {
                        row = gridData.NewRow();
                        row.ItemArray = new object[] {
                        workList[i].Instance.ID, 
                        workList[i].Instance.Subject, 
                        workList[i].Instance.StateChangeDate,
                        workList[i].Instance.Priority, 
                        workList[i].Instance.DeadlineDate,
                        workList[i].Message};

                        // gridData.Rows.Add(row);
                    }
                    PrivateCacheData.Add("WORKLIST", gridData);
                }
            }

            _view.SetupGrid(gridConfig, gridHeading);

        }

        protected override void OnViewLoaded(object sender, EventArgs e)
        {
            base.OnViewLoaded(sender, e);
            if (!_view.ShouldRunPage) return;

            // trac # 13941  - databinding must happen here otherwsie it screws up the grouping/paging
            _view.BindGrid(gridData);

        }

        void _view_OnSelectButtonClicked(object sender, KeyChangedEventArgs e)
        {
            if (e == null)
                return;

            long instanceID = Convert.ToInt64(e.Key);

            if (instanceID > 0)
            {
                //add the instance to the tasks node and navigate to the appropriate view
                //Instance.State.StateForms[0].Form.Name is the navigate value

                _repo = RepositoryFactory.GetRepository<IX2Repository>();
                IInstance instance = _repo.GetInstanceByKey(instanceID);
                List<CBONode> nodes = CBOManager.GetMenuNodes(_view.CurrentPrincipal, CBONodeSetType.X2);

                TaskListNode taskListNode = null;

                for (int i = 0; i < nodes.Count; i++)
                {
                    if (nodes[i] is TaskListNode)
                    {
                        taskListNode = nodes[i] as TaskListNode;
                        break;
                    }
                }

                if (taskListNode == null)
                {
                    taskListNode = new TaskListNode(null);
                    CBOManager.AddCBOMenuNode(_view.CurrentPrincipal, null, taskListNode, CBONodeSetType.X2);
                }

                //the name of the relevant x2data table will be the same as the storagetable field from the workflow table
                //the storagekey field in the workflow table is the name of the column in the x2data table that has the actual key
                //forms for a stage are ordered by the formorder column in the stateform table
                IWorkFlow wf = _repo.GetWorkFlowByKey(instance.WorkFlow.ID);
                IDictionary<string, object> dict = X2Service.GetX2DataRow(instanceID);
                int businessKey = Convert.ToInt32(dict[wf.StorageKey]);

                // setup the instance node description
                string nodeDesc = "", longDesc = "";
                _repo.SetInstanceNodeDescription(wf, instance, businessKey, out nodeDesc, out longDesc);
                
                if (instance.State.Forms.Count == 0)
                    throw new Exception("No form selected for user state. Go select a form in the designer.");

                InstanceNode iNode = new InstanceNode(businessKey, taskListNode, nodeDesc, longDesc, instance.ID, instance.State.Forms[0].Name); 

                CBOManager.AddCBOMenuNode(_view.CurrentPrincipal, taskListNode, iNode, CBONodeSetType.X2);
                CBOManager.SetCurrentCBONode(_view.CurrentPrincipal, iNode, CBONodeSetType.X2);
                _view.Navigator.Navigate(iNode.URL);
            }
        }
    }
}