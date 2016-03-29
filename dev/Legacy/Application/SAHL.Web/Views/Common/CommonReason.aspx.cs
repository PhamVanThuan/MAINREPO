using System;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Web.AJAX;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Common
{
    public partial class CommonReason : SAHLCommonBaseView,ICommonReason
    {
       
        #region Private Members

        private bool _memoPanelVisible;
        private bool _cancelButtonVisible;
        private bool _onlyOneReasonCanBeSelected;
        bool historyDisplay;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ShouldRunPage) return;
            RegisterWebService(ServiceConstants.Reason);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!ShouldRunPage) return;

            if (!historyDisplay)
                RegisterClientScripts();
            else
                RegisterAlternateConfirm(); 
            
            pnlMemo.Visible = _memoPanelVisible;
            btnCancel.Visible = _cancelButtonVisible;
        }

        private void RegisterAlternateConfirm()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("function btnConfirmClick()");
            sb.AppendLine("{");
            sb.AppendLine("     if(confirm('Are you sure you want to remove and recreate these decline reasons?'))");
            sb.AppendLine("         event.returnValue = true;");
            sb.AppendLine("     else");
            sb.AppendLine("         event.returnValue = false;");
            sb.AppendLine("}");

            if (!Page.ClientScript.IsClientScriptBlockRegistered("btnConfirmClick"))
                Page.ClientScript.RegisterStartupScript(GetType(), "btnConfirmClick", sb.ToString(), true);
        } 

        protected void gridHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            TableCellCollection Cells = e.Row.Cells;

            IReason reason = e.Row.DataItem as IReason;

            if (e.Row.DataItem != null)
            {
                if (reason != null)
                {
                    Cells[0].Text = reason.GenericKey.ToString();
                    Cells[1].Text = reason.ReasonDefinition.ReasonDescription.Description;
                    Cells[2].Text = reason.Comment;
                }
            }

        }

        private void RegisterClientScripts()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("function MyOnLoad()");
            sb.AppendLine("{");
            //sb.AppendLine(" alert('onload');");
            sb.AppendLine(" cbxReasonSelectedIndexChanged();");
            sb.AppendLine("}");

            //when the reason type changes, get the matching reason definition rows from the db if we don't have them yet, else just repopulate the listbox from the array
            sb.AppendLine("function cbxReasonSelectedIndexChanged()");
            sb.AppendLine("{");
            //sb.AppendLine(" alert(cbxReason.selectedIndex);");
            sb.AppendLine(" if(cbxReason.selectedIndex < 0) return;");
            sb.AppendLine(" var defArray = reasonArray[cbxReason.selectedIndex];");
            //sb.AppendLine(" alert(defArray.length);");
            sb.AppendLine(" if(defArray.length == 0)");//we need to get the data from the db
            sb.AppendLine(" {");
            sb.AppendLine("     var reasonTypeKey = cbxReason.value;");
            //sb.AppendLine("     if(isNumeric(reasonTypeKey))");
            sb.AppendLine("     SAHL.Web.AJAX.Reason.GetReasonDefinitionDescriptions(reasonTypeKey, " + SortByReasonDescription.ToString().ToLower()  + ", ServerSide_CallBack);");
            //sb.AppendLine("     SAHL.Web.AJAX.Reason.GetReasonDefinitionDescriptions(reasonTypeKey, true, ServerSide_CallBack);");
            sb.AppendLine(" }");
            sb.AppendLine(" else"); //we already have the data
            sb.AppendLine(" {");
            sb.AppendLine("     PopulateAvailableItems(cbxReason.selectedIndex);");
            sb.AppendLine(" }");
            sb.AppendLine("}");

            //after getting the rows back from the db, store them in our array
            sb.AppendLine("function ServerSide_CallBack(response)");
            sb.AppendLine("{");
            sb.AppendLine(" var items = response;");
            sb.AppendLine(" if (response.error != null)");
            sb.AppendLine(" {");
            sb.AppendLine("     alert(response.error);");
            sb.AppendLine("     return;");
            sb.AppendLine(" }");
            sb.AppendLine(" if (items != null)");
            sb.AppendLine(" {");
               //sb.AppendLine("alert(response);");
            sb.AppendLine("     for(var i in response)");
            sb.AppendLine("         {");
            sb.AppendLine("             var value = response[i];");
            sb.AppendLine("             reasonArray[cbxReason.selectedIndex][i] = value;");
            sb.AppendLine("         }");
            sb.AppendLine("         reasonArray.sort();");
            sb.AppendLine("     PopulateAvailableItems(cbxReason.selectedIndex);");
            sb.AppendLine(" }");
            sb.AppendLine("}");

            //first clear the listbox, then repopulate with reasons that are not in the selected list
            sb.AppendLine("function PopulateAvailableItems(reasonIndex)");
            sb.AppendLine("{");
            //sb.AppendLine(" var listAvailableItems = document.getElementById('" + lstCreditDeclineReasons.ClientID + "');");
            sb.AppendLine(" for(i=listAvailableItems.length;i>-1;i--)");
            sb.AppendLine(" {");
            sb.AppendLine("     listAvailableItems.remove(i);");
            sb.AppendLine(" }");
            //sb.AppendLine(" alert(reasonArray[reasonIndex].length);");
            //sb.AppendLine("listAvailableItems.sort();");
            sb.AppendLine(" for(i=0;i<reasonArray[reasonIndex].length;i++)");
            sb.AppendLine(" {");
            sb.AppendLine("     var exists = false;");
            sb.AppendLine("     for(k=0;k<listSelectedItems.options.length;k++)");
            sb.AppendLine("     {");
            //            sb.AppendLine("alert(listSelectedItems.options[k].value.split(','));");
            sb.AppendLine("         var arr = listSelectedItems.options[k].value.split(',');");
            sb.AppendLine("         if( (arr[0] == reasonIndex) && (arr[1] == i) )");
            sb.AppendLine("         {");
            sb.AppendLine("             exists = true;");
            sb.AppendLine("             break;");
            sb.AppendLine("         }");
            sb.AppendLine("     }");
            sb.AppendLine("     if(exists == false)");
            sb.AppendLine("     {");
            sb.AppendLine("         var keyArray = new Array();");
            sb.AppendLine("         keyArray[0] = reasonIndex;");
            sb.AppendLine("         keyArray[1] = i;");
            //sb.AppendLine("         alert(reasonArray[reasonIndex][i+1]);");
            sb.AppendLine("         var opt = new Option(reasonArray[reasonIndex][i].ReasonDescription, keyArray);");
            sb.AppendLine("         opt.title = reasonArray[reasonIndex][i].ReasonDescription;");  // this adds a tooltip to the listbox item
            //sb.AppendLine("         alert(opt.value);");
            sb.AppendLine("         listAvailableItems.options.add(opt);");
            sb.AppendLine("     }");
            sb.AppendLine(" }");
            //-------
            sb.AppendLine("if (listSelectedItems.options.length == 0)");
            sb.AppendLine("{");
            sb.AppendLine("if (listHiddenSelection.value.length > 0)");
            sb.AppendLine("{");
            sb.AppendLine("var arrSelItems = listHiddenSelection.value.split('|');");
            sb.AppendLine("for(j=0;j<arrSelItems.length;j++)");
            sb.AppendLine("{");
            sb.AppendLine("var arrSelItem = arrSelItems[j].split(':');");
            sb.AppendLine("var strValue = arrSelItem[0];");
            sb.AppendLine("var strText = arrSelItem[1];");
            sb.AppendLine("var strComment = '';");
            sb.AppendLine("if (arrSelItem.length == 3)");
            sb.AppendLine("{");
            sb.AppendLine("strComment = arrSelItem[2];");
            sb.AppendLine("}");

            sb.AppendLine("var strIndex = strValue.substring(strValue.indexOf(',')+1,strValue.length);");
            //sb.AppendLine("alert('Index is' + strIndex);");
            sb.AppendLine("addItemWithIndex(strIndex, strComment, strValue);");
            sb.AppendLine("}");
            sb.AppendLine("}");
            sb.AppendLine("}");

            sb.AppendLine("function addItemWithIndex(idx, strComment, strValue)");
            sb.AppendLine("{");
            sb.AppendLine(" if(idx < 0) return;");
            sb.AppendLine(" for(i=0;i<listAvailableItems.options.length;i++)");
            sb.AppendLine(" {");
            sb.AppendLine(" if (strValue == listAvailableItems.options[i].value)");
            sb.AppendLine("{");
            sb.AppendLine("idx = i;");
            sb.AppendLine("}");
            sb.AppendLine("}");

            sb.AppendLine(" var opt = new Option(listAvailableItems.options[idx].text, listAvailableItems.options[idx].value);");
            //sb.AppendLine("     var attr = document.createAttribute('rtKey_' + listAvailableItems.options[idx].value);");
            //sb.AppendLine("     attr.value = cbxReason.selectedIndex;");
            //sb.AppendLine("     opt.attributes.setNamedItem(attr);");
            sb.AppendLine(" listSelectedItems.options.add(opt);");
            sb.AppendLine(" listAvailableItems.options.remove(idx);");
            sb.AppendLine(" if(listAvailableItems.options.length > idx)");
            sb.AppendLine("     listAvailableItems.selectedIndex = idx;");
            sb.AppendLine(" else");
            sb.AppendLine("     listAvailableItems.selectedIndex = listAvailableItems.options.length-1;");
            sb.AppendLine(" CreateTextAreaWithComment(opt.value, strComment);");
            sb.AppendLine(" lstAvailableReasonIndexChanged()");
            sb.AppendLine(" ShowComment();");
            sb.AppendLine("}");

            //--------

            sb.AppendLine("}");

            //add the reason to the selected list, remove from available list and create a text area for the comment if necessary 
            sb.AppendLine("function addItem()");
            sb.AppendLine("{");
            //      sb.AppendLine(" var listAvailableItems = document.getElementById('" +lstCreditDeclineReasons.ClientID + "');");
            sb.AppendLine(" var idx = listAvailableItems.selectedIndex;");
            sb.AppendLine(" if(idx < 0) return;");
            sb.AppendLine(" var opt = new Option(listAvailableItems.options[idx].text, listAvailableItems.options[idx].value);");
            sb.AppendLine(" opt.title = listAvailableItems.options[idx].text;"); // this adds a tooltip to the listbox item
            //sb.AppendLine("     var attr = document.createAttribute('rtKey_' + listAvailableItems.options[idx].value);");
            //sb.AppendLine("     attr.value = cbxReason.selectedIndex;");
            //sb.AppendLine("     opt.attributes.setNamedItem(attr);");
            sb.AppendLine(" listSelectedItems.options.add(opt);");
            sb.AppendLine(" listAvailableItems.options.remove(idx);");
            sb.AppendLine(" if(listAvailableItems.options.length > idx)");
            sb.AppendLine("     listAvailableItems.selectedIndex = idx;");
            sb.AppendLine(" else");
            sb.AppendLine("     listAvailableItems.selectedIndex = listAvailableItems.options.length-1;");
            sb.AppendLine(" CreateTextArea(opt.value);");
            sb.AppendLine(" lstAvailableReasonIndexChanged()");
            sb.AppendLine(" ShowComment();");
            sb.AppendLine("}");

            //remove from selected list, add back to available list
            sb.AppendLine("function removeItem()");
            sb.AppendLine("{");
            sb.AppendLine(" var idx = listSelectedItems.selectedIndex;");
            sb.AppendLine(" if(idx < 0) return;");
            sb.AppendLine(" listSelectedItems.options.remove(idx);");
            sb.AppendLine(" if(listSelectedItems.options.length > idx)");
            sb.AppendLine("     listSelectedItems.selectedIndex = idx;");
            sb.AppendLine(" else");
            sb.AppendLine("     listSelectedItems.selectedIndex = listSelectedItems.options.length-1;");
            sb.AppendLine("if (listSelectedItems.options.length == 0) { listHiddenSelection.value = '';}");
            sb.AppendLine(" PopulateAvailableItems(cbxReason.selectedIndex);");
            sb.AppendLine(" lstSelectedReasonIndexChanged();");
            sb.AppendLine(" ShowComment();");
            //sb.AppendLine(" SaveSelection();");
            //sb.AppendLine(" if(listSelectedItems.options.length > 1) btnConfirm.disabled = false; else btnConfirm.disabled = true;");
            sb.AppendLine("}");

            //create a textbox to hold the comment for a particular reason. The name will contain the indexes into the reasonArray
            sb.AppendLine("function CreateTextArea(val)");
            sb.AppendLine("{");
            sb.AppendLine(" var arr = val.split(',');");
            sb.AppendLine(" var name = 'txtComment_' + arr[0] + '_' + arr[1];");
            sb.AppendLine(" var tc = document.getElementById(name);");
            sb.AppendLine(" if(tc == null)");
            sb.AppendLine(" {");
            sb.AppendLine("     var txtAreaElement = document.createElement('textarea');");
            sb.AppendLine("     txtAreaElement.id = name;");
            sb.AppendLine("     txtAreaElement.name = name;");
            sb.AppendLine("     txtAreaElement.style.height = txtComment.style.height;");
            sb.AppendLine("     txtAreaElement.style.width = txtComment.style.width;");
            sb.AppendLine("     txtAreaElement.style.display = 'none';");
            sb.AppendLine("     if(reasonArray[arr[0]][arr[1]].AllowComment == false)");
            sb.AppendLine("         txtAreaElement.value = 'No comments allowed for this reason';");
            sb.AppendLine("     divReason.appendChild(txtAreaElement);");
            //sb.AppendLine("alert('Created');");
            sb.AppendLine(" }");
            sb.AppendLine("}");


            //------------------------
            sb.AppendLine("function CreateTextAreaWithComment(val, strComment)");
            sb.AppendLine("{");
            sb.AppendLine(" var arr = val.split(',');");
            sb.AppendLine(" var name = 'txtComment_' + arr[0] + '_' + arr[1];");
            sb.AppendLine(" var tc = document.getElementById(name);");
            sb.AppendLine(" if(tc == null)");
            sb.AppendLine(" {");
            sb.AppendLine("     var txtAreaElement = document.createElement('textarea');");
            sb.AppendLine("     txtAreaElement.id = name;");
            sb.AppendLine("     txtAreaElement.name = name;");
            sb.AppendLine("     txtAreaElement.value = strComment;");
            sb.AppendLine("     txtAreaElement.style.height = txtComment.style.height;");
            sb.AppendLine("     txtAreaElement.style.width = txtComment.style.width;");
            sb.AppendLine("     txtAreaElement.style.display = 'none';");
            sb.AppendLine("     if(reasonArray[arr[0]][arr[1]].AllowComment == false)");
            sb.AppendLine("         txtAreaElement.value = 'No comments allowed for this reason';");
            sb.AppendLine("     divReason.appendChild(txtAreaElement);");
            //sb.AppendLine("alert('Created');");
            sb.AppendLine(" }");
            sb.AppendLine("}");


            //--------------------------


            //first hide all comments, then switch on the currently selected one (or txtComment if none are selected)
            sb.AppendLine("function ShowComment()");
            sb.AppendLine("{");
            //  sb.AppendLine(" var divReason = document.getElementById('" + divReason.ClientID + "');");
            sb.AppendLine(" for(x=0;x<divReason.all.length;x++)");
            sb.AppendLine(" {");
            sb.AppendLine("     var ele = divReason.all.item(x);");
            sb.AppendLine("     ele.style.display='none';");
            sb.AppendLine("     ele.readOnly = true;");
            sb.AppendLine(" }");
            sb.AppendLine(" lblComment.disabled = true;");
            sb.AppendLine(" var idx = listSelectedItems.selectedIndex;");
            //sb.AppendLine("alert(idx);");
            sb.AppendLine(" if(idx > -1)");
            sb.AppendLine(" {");
            sb.AppendLine("     var arr = listSelectedItems.options[idx].value.split(',');");
            sb.AppendLine("     var name = 'txtComment_' + arr[0] + '_' + arr[1];");
            sb.AppendLine("     var tc = document.getElementById(name);");
            sb.AppendLine("     tc.style.display = 'inline';");
            //sb.AppendLine("alert(reasonArray[arr[0]][arr[1]]);");
            sb.AppendLine("     if(reasonArray[arr[0]][arr[1]].AllowComment == true)");
            sb.AppendLine("     {");
            sb.AppendLine("         lblComment.disabled = false;");
            sb.AppendLine("         tc.readOnly = false;");
            sb.AppendLine("     }");
            sb.AppendLine(" }");
            sb.AppendLine(" else");
            sb.AppendLine("     txtComment.style.display = 'inline';");
            sb.AppendLine("}");

            //store the list of selected reasons for retrieval on submit
            sb.AppendLine("function SaveSelection()");
            sb.AppendLine("{");
            sb.AppendLine(" listHiddenItems.value = '';");
            //----------------
            sb.AppendLine(" listHiddenSelection.value = '';");
            //------------------

            sb.AppendLine(" for (i=0;i<listSelectedItems.options.length;i++)");
            sb.AppendLine(" {");
            sb.AppendLine("     var arr = listSelectedItems.options[i].value.split(',');");
            sb.AppendLine("     var arr2 = listSelectedItems.options[i].text.split(',');");
            sb.AppendLine("     var comment = '';");
            sb.AppendLine("     if(reasonArray[arr[0]][arr[1]].AllowComment == true)");
            sb.AppendLine("     {");
            sb.AppendLine("         var name = 'txtComment_' + arr[0] + '_' + arr[1];");
            sb.AppendLine("         var tc = document.getElementById(name);");
            sb.AppendLine("         comment = tc.value;");
            sb.AppendLine("     }");
            //---------------
            sb.AppendLine(" listHiddenSelection.value += '|' + listSelectedItems.options[i].value + ':' + listSelectedItems.options[i].text + ':' + comment;");

            //---------------
            sb.AppendLine("     listHiddenItems.value += '~/~' + reasonArray[arr[0]][arr[1]].Key + ' ' + listSelectedItems[i].text + ':' + comment;");
            sb.AppendLine(" }");
            //---------------------
            sb.AppendLine(" listHiddenSelection.value = listHiddenSelection.value.substring(1,listHiddenSelection.value.length);");
            //sb.AppendLine(" alert(listHiddenSelection.value);");
            //---------------------
            sb.AppendLine("}");


            sb.AppendLine("function lstAvailableReasonIndexChanged()");
            sb.AppendLine("{");
            sb.AppendLine(" if(listAvailableItems.selectedIndex > -1)");
            sb.AppendLine("     listSelectedItems.selectedIndex = -1;");
            sb.AppendLine(" ShowComment();");
            sb.AppendLine("}");

            //sb.AppendLine("function IsNumeric(expression)");
            //sb.AppendLine("{");
            //sb.AppendLine("var nums = '0123456789';");
            //sb.AppendLine("if (expression.length==0)return(false);");
            //sb.AppendLine("for (var n=0; n < expression.length; n++)");
            //sb.AppendLine("{");
            //sb.AppendLine("if(nums.indexOf(expression.charAt(n))==-1)return(false);");
            //sb.AppendLine("}");
            //sb.AppendLine("return(true);");

            sb.AppendLine("function lstSelectedReasonIndexChanged()");
            sb.AppendLine("{");
            sb.AppendLine(" var idx = listSelectedItems.selectedIndex;");
            sb.AppendLine(" if(idx == -1) return;");
            sb.AppendLine(" var arr = listSelectedItems.options[idx].value.split(',');");
            sb.AppendLine(" if(arr[0] != cbxReason.selectedIndex);");
            sb.AppendLine(" {");
            sb.AppendLine("     cbxReason.selectedIndex = arr[0];");
            sb.AppendLine("     cbxReasonSelectedIndexChanged();");
            sb.AppendLine(" }");
            sb.AppendLine(" listAvailableItems.selectedIndex = -1;");
            sb.AppendLine(" ShowComment();");
            //sb.AppendLine("alert('availableIdx: ' + listAvailableItems.selectedIndex);");
            sb.AppendLine("}");

            sb.AppendLine("function btnConfirmClick()");
            sb.AppendLine("{");
            sb.AppendLine(" if(listSelectedItems.options.length < 1 && HiddenInd.value != '1')");
            sb.AppendLine(" {");
            sb.AppendLine("     alert('Please select at least one Reason');");
            sb.AppendLine("     event.returnValue = false;");
            //sb.AppendLine("     event.cancel = true;");
            sb.AppendLine(" }");
            sb.AppendLine(" else");
            sb.AppendLine(" if(listSelectedItems.options.length > 0)");
            sb.AppendLine(" {");
            sb.AppendLine("     if(confirm('Are you sure you want to submit the selected reasons?'))");
            sb.AppendLine("     {");
            //sb.AppendLine("         btnConfirm.disabled = true;");
            sb.AppendLine("         SaveSelection();");
            //sb.AppendLine("alert('saved');");
            //sb.AppendLine("         Views_CommonReason.SubmitReasons(listHiddenItems.value);");
            sb.AppendLine("     }");
            sb.AppendLine("     else");
            sb.AppendLine("         event.returnValue = false;");
            sb.AppendLine(" }");
            sb.AppendLine("}");


            //sb.AppendLine("function SummaryReason_CallBack(response)");
            //sb.AppendLine("{");
            //sb.AppendLine(" if (response.error != null)");
            //sb.AppendLine(" {");
            //sb.AppendLine("     alert('Error ' + response.error);");
            //sb.AppendLine("     return;");
            //sb.AppendLine(" }");
            //sb.AppendLine(" txtComment.value = ''");
            //sb.AppendLine(" txtComment.value = response.value;");
            //sb.AppendLine("}");


            if (!Page.ClientScript.IsClientScriptBlockRegistered(GetType(), "MyOnLoad"))
            {
                StringBuilder load = new StringBuilder();
                //
                load.AppendLine("var cbxReason = document.getElementById('" + cbxReasonType.ClientID + "');");
                load.AppendLine("var listAvailableItems = document.getElementById('" + lstAvailableReasons.ClientID + "');");
                load.AppendLine("var listSelectedItems = document.getElementById('" + lstSelectedReasons.ClientID + "');");
                load.AppendLine("var txtComment = document.getElementById('" + txtComment.ClientID + "');");
                load.AppendLine("var lblComment = document.getElementById('" + lblComment.ClientID + "');");
                load.AppendLine("var btnConfirm = document.getElementById('" + btnConfirm.ClientID + "');");
                load.AppendLine("var HiddenInd = document.getElementById('" + HiddenInd.ClientID + "');");
                load.AppendLine("var listHiddenItems = document.getElementById('" + hiddenSelection.ClientID + "');");
                //------
                load.AppendLine("var listHiddenSelection = document.getElementById('" + HiddenSelection2.ClientID + "');");
                //------

                //load.AppendLine("alert(listHiddenItems.value);");
                //load.AppendLine("var output = ''");
                load.AppendLine("var reasonArray = new Array();");
                load.AppendLine("for (i=0;i<cbxReason.options.length;i++)");
                load.AppendLine("{");
                load.AppendLine("    reasonArray[i] = new Array();");
                load.AppendLine("}");
                load.AppendLine("MyOnLoad();");

                Page.ClientScript.RegisterStartupScript(GetType(), "MyOnLoad", load.ToString(), true);
            }

            if (!Page.ClientScript.IsClientScriptBlockRegistered(GetType(), "lstScripts"))
            {
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "lstScripts", sb.ToString(), true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            List<SelectedReason> lstReasons = new List<SelectedReason>();

            if (hiddenSelection.Value.Length > 0)
            {
                //split the hidden value on the delimiter the original rocket scientist that dev'd this concocted,
                //cause it is infinately simpler and less error prone than string manipulation in loops with if statements...
                string[] selections = System.Text.RegularExpressions.Regex.Split(hiddenSelection.Value, "~/~");

                for (int x = 0; x < selections.Length; x++)
                {
                    //split the item into the identifier (key description) and comment
                    string[] reason = System.Text.RegularExpressions.Regex.Split(selections[x], ":");

                    if (reason[0].Length > 0)
                    {
                        SelectedReason itm = new SelectedReason();
                        //some more rocket science: the first part is the reason key and after the first space the description for that key
                        //eg: "907 Some reason for something"
                        
                        //get the reason key
                        int firstSpace = reason[0].IndexOf(" ");
                        itm.ReasonDefinitionKey = int.Parse(reason[0].Substring(0, firstSpace));
                        //get the description, even though it is not used after this...
                        itm.Description = reason[0].Substring(firstSpace + 1, reason[0].Length - firstSpace - 1);
                        //get the comment if one was added
                        if (reason[1].Length > 0)
                            itm.Comment = reason[1];

                        lstReasons.Add(itm);
                    }
                }
                if (lstReasons.Count > 1 && _onlyOneReasonCanBeSelected)
                {
                    string errorMessage = "Only one reason can be selected";
                    this.Messages.Add(new Error(errorMessage, errorMessage));
                    return;
                }
            }

            if (OnSubmitButtonClicked != null)
            {
                KeyChangedEventArgs args = new KeyChangedEventArgs(lstReasons);
                OnSubmitButtonClicked(sender, args);
            }
        }

        #region ICommonReason Members

        /// <summary>
        /// 
        /// </summary>
        public event KeyChangedEventHandler OnSubmitButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnCancelButtonClicked;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="reasonTypes"></param>
        public void BindReasonTypes(List<IReasonType> reasonTypes)
        {           
            cbxReasonType.DataSource = reasonTypes;
            cbxReasonType.DataValueField = "Key";
            cbxReasonType.DataTextField = "Description";
            cbxReasonType.DataBind();           
        }

       /// <summary>
       /// 
       /// </summary>
        public bool MemoPanelVisible
        {
            set { _memoPanelVisible = value; }
        }

        /// <summary>
        /// Specify Whether the reasons be sorted by description
        /// </summary>
        public bool SortByReasonDescription { get; set; }

        public bool CancelButtonVisible
        {
            set { _cancelButtonVisible  = value; }
        }

        public bool SubmitButtonVisible
        {
            set { btnConfirm.Visible = value; }
        }

        /// <summary>
        /// This method is used to set the hiddenInd Text - if Text = 1, then the click
        /// event of the Confirm button would allow the user to proceed without selecting a reason
        /// </summary>
        public string SetHiddenIndText
        {
            set { HiddenInd.Value = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SubmitButtonText
        {
            set { btnConfirm.Text = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string CancelButtonText
        {
            set { btnCancel.Text = value; }
        }

        #endregion

        public bool ShowHistoryPanel
        {
            set
            {
                pnlHistory.Visible = value;
            }
        }

        public bool HistoryDisplay
        {
            set
            {
                historyDisplay = value;
            }
        }

        public bool ShowUpdatePanel
        {
            set
            {
                pnlReasons.Visible = value;
                pnlUpdateReasons.Visible = value;
                pnlMemo.Visible = value;
                pnlSubmit.Visible = value;
            }
        }

        public bool ShowSubmitButtons
        {
            set { pnlSubmit.Visible = value; }
        }

        public string SetHistoryPanelGroupingText
        {
            set
            {
                pnlHistory.GroupingText = value + " Reasons";
            }
        }

        public void BindReasonHistoryGrid(IReadOnlyEventList<IReason> reasons)
        {
            gridHistory.AddGridBoundColumn("", "Key", Unit.Percentage(0), HorizontalAlign.Left, false);
            gridHistory.AddGridBoundColumn("", "Reason", Unit.Percentage(20), HorizontalAlign.Left, true);
            gridHistory.AddGridBoundColumn("", "Comment", Unit.Percentage(80), HorizontalAlign.Left, true);
            gridHistory.DataSource = reasons;
            gridHistory.DataBind();

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (OnCancelButtonClicked != null)
            {
                OnCancelButtonClicked(sender, e);
            }
        }

        public bool OnlyOneReasonCanBeSelected
        {
            set { _onlyOneReasonCanBeSelected = value; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SelectedReason
    {
        private int _reasonDefinitionKey;
        private string _description;
        private string _comment;
        /// <summary>
        /// 
        /// </summary>
        public int ReasonDefinitionKey
        {
            get {return _reasonDefinitionKey;}
            set {_reasonDefinitionKey = value;}
        }
        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Comment
        {
            get {return _comment;}
            set {_comment = value;}
        }
    }
}