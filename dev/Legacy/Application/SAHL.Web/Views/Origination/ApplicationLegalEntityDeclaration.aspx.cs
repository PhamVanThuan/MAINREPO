using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Origination.Interfaces;
using SAHL.Common.Web.UI;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Controls;
using System.Collections.Specialized;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Origination
{
    public partial class ApplicationDeclaration : SAHLCommonBaseView, IApplicationLegalEntityDeclaration
    {
        IList<IApplicationDeclarationQuestionAnswerConfiguration> appDecQnAConfigurationList;
        bool _updateMode; // = false;

        #region IApplicationLegalEntityDeclaration Members

        public event EventHandler onCancelButtonClicked;
        public event EventHandler onBackButtonClicked;
        public event EventHandler onUpdateButtonClicked;

        public void ConfigurePanel(string legalEntityName)
        {
            pnlDeclarations.GroupingText = "Application declarations for " + legalEntityName;
        }

        public void BindDeclaration(IList<IApplicationDeclarationQuestionAnswerConfiguration> appDecQandAConfig, IEventList<IApplicationDeclaration> appDecs)
        {
            ILookupRepository lookups = RepositoryFactory.GetRepository<ILookupRepository>();
            appDecQnAConfigurationList = appDecQandAConfig;

            tblDeclarations.Rows.Clear();

            TableRow tableRow = new TableRow();
            tblDeclarations.Rows.Add(tableRow);

            TableCell tcDummyCell1 = new TableCell();
            tcDummyCell1.Height = new Unit(20, UnitType.Pixel);
            tcDummyCell1.Width = new Unit(80, UnitType.Percentage);

            TableCell tcDummyCell2 = new TableCell();
            tcDummyCell2.Height = new Unit(10, UnitType.Pixel);

            tableRow.Cells.Add(tcDummyCell1);
            tableRow.Cells.Add(tcDummyCell2);

            for (int i = 0; i < appDecQandAConfig.Count; i++)
            {
                TableRow tr = new TableRow();

                TableCell tcLabel = new TableCell();
                tcLabel.Text = appDecQandAConfig[i].ApplicationDeclarationQuestion.Description;
                tcLabel.Width = new Unit(80, UnitType.Percentage);

                TableCell tcInput = new TableCell();

                System.Web.UI.Control inputControl = null;

                SAHLDropDownList ddlDropDown;

                SAHLLabel lblAnswer = new SAHLLabel();

                if (appDecQandAConfig[i].ApplicationDeclarationQuestion.DisplayQuestionDate == false)
                {
                    ddlDropDown = new SAHLDropDownList();
                    ddlDropDown.Items.Add(new ListItem("-select-", "0"));
                    for (int j = 0; j < lookups.ApplicationDeclarationAnswers.Count; j++)
                    {
                        if (lookups.ApplicationDeclarationAnswers[j].Description != "Date")
                            ddlDropDown.Items.Add(new ListItem(lookups.ApplicationDeclarationAnswers[j].Description, lookups.ApplicationDeclarationAnswers[j].Key.ToString()));
                    }
                    inputControl = ddlDropDown;
                    ddlDropDown.PleaseSelectItem = true;

                    if (appDecs != null && appDecs.Count > 0)
                    {
                        for (int z = 0; z < appDecs.Count; z++)
                        {
                            if (appDecQandAConfig[i].ApplicationDeclarationQuestion.Key == appDecs[z].ApplicationDeclarationQuestion.Key)
                            {
                                ddlDropDown.SelectedValue = appDecs[z].ApplicationDeclarationAnswer.Key.ToString();

                                lblAnswer.Text = lookups.ApplicationDeclarationAnswers.ObjectDictionary[appDecs[z].ApplicationDeclarationAnswer.Key.ToString()].Description;
                            }
                        }
                    }

                    ddlDropDown.Visible = _updateMode;
                }
                else
                {
                    SAHLDateBox dt = new SAHLDateBox();
                    inputControl = dt;

                    if (appDecs != null && appDecs.Count > 0)
                    {
                        for (int z = 0; z < appDecs.Count; z++)
                        {
                            if (appDecQandAConfig[i].ApplicationDeclarationQuestion.Key == appDecs[z].ApplicationDeclarationQuestion.Key)
                            {
                                if (appDecs[z].ApplicationDeclarationDate != null)
                                {
                                    DateTime date = Convert.ToDateTime(appDecs[z].ApplicationDeclarationDate);
                                    dt.Date = date.Date;

                                    lblAnswer.Text = date.Date.ToString(SAHL.Common.Constants.DateFormat);
                                }
                                break;
                            }
                        }
                    }

                    dt.Visible = _updateMode;
                }

                lblAnswer.Visible = !_updateMode;

                if (inputControl != null)
                {
                    inputControl.ID = "Ctrl" + appDecQandAConfig[i].Key.ToString();
                    tcInput.Controls.Add(lblAnswer);
                    tcInput.Controls.Add(inputControl);
                }
                tr.Cells.Add(tcLabel);
                tr.Cells.Add(tcInput);
                tblDeclarations.Rows.Add(tr);
            }
        }

        public IEventList<IApplicationDeclaration> GetOfferDeclarations
        {
            get
            {
                ILookupRepository _lookups = RepositoryFactory.GetRepository<ILookupRepository>();

                IEventList<IApplicationDeclaration> appDecList = new EventList<IApplicationDeclaration>();
                IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                for (int i = 0; i < appDecQnAConfigurationList.Count; i++)
                {
                    string ControlName = "Ctrl" + appDecQnAConfigurationList[i].Key.ToString();
                    StringCollection SC = new StringCollection();
                    SC.AddRange(Request.Form.AllKeys);

                    if (SC.Contains(tblDeclarations.NamingContainer.UniqueID + '$' + ControlName))
                    {
                        string FormStrValue = Request.Form[tblDeclarations.NamingContainer.UniqueID + '$' + ControlName];
                        IApplicationDeclaration appDec = appRepo.GetEmptyApplicationDeclaration();
                        appDec.ApplicationDeclarationQuestion = appDecQnAConfigurationList[i].ApplicationDeclarationQuestion;

                        for (int x = 0; x < appDecQnAConfigurationList[i].ApplicationDeclarationQuestion.ApplicationDeclarationQuestionAnswers.Count; x++)
                        {
                            if (appDecQnAConfigurationList[i].ApplicationDeclarationQuestion.DisplayQuestionDate)
                            {
                                if (FormStrValue.Length > 0)
                                {
                                    System.Globalization.CultureInfo enGB = new System.Globalization.CultureInfo("en-GB");
                                    DateTime dt = Convert.ToDateTime(FormStrValue, enGB);
                                    appDec.ApplicationDeclarationDate = dt;
                                }
                                appDec.ApplicationDeclarationAnswer = _lookups.ApplicationDeclarationAnswers.ObjectDictionary[((int)OfferDeclarationAnswers.Date).ToString()];
                            }
                            else
                                if (appDecQnAConfigurationList[i].ApplicationDeclarationQuestion.ApplicationDeclarationQuestionAnswers[x].ApplicationDeclarationAnswer.Key.ToString() == FormStrValue)
                                {
                                    appDec.ApplicationDeclarationAnswer = appDecQnAConfigurationList[i].ApplicationDeclarationQuestion.ApplicationDeclarationQuestionAnswers[x].ApplicationDeclarationAnswer;
                                    break;
                                }
                        }
                        appDecList.Add(this.Messages, appDec);
                    }
                }
                return appDecList;
            }
        }

        public bool ShowCancelButton
        {
            set { btnCancel.Visible = value; }
        }

        public string UpdateButtonText
        {
            get { return btnUpdate.Text; }
            set { btnUpdate.Text = value; }
        }

        public bool ShowBackButton
        {
            set { btnBack.Visible = value; }
        }

        public bool ShowUpdateButton
        {
            set { btnUpdate.Visible = value; }
        }

        public bool UpdateMode
        {
            set { _updateMode = value; }
        }

        public void SetViewForNullDeclarations()
        {
            ButtonRow.Visible = false;

            tblDeclarations.Rows.Clear();

            TableRow tableRow1 = new TableRow();
            tblDeclarations.Rows.Add(tableRow1);

            TableCell tcDummyCell = new TableCell();
            tcDummyCell.Height = new Unit(20, UnitType.Pixel);
            tcDummyCell.Width = new Unit(100, UnitType.Percentage);

            tableRow1.Cells.Add(tcDummyCell);

            TableRow tableRow2 = new TableRow();
            tblDeclarations.Rows.Add(tableRow2);

            TableCell tcDisplayCell = new TableCell();
            tcDisplayCell.Height = new Unit(50, UnitType.Pixel);
            tcDisplayCell.Width = new Unit(100, UnitType.Percentage);
            tcDisplayCell.Text = "There are no Declarations required for this Legal Entity";
            tcDisplayCell.Font.Bold = false;

            tableRow2.Cells.Add(tcDisplayCell);
        }


        #endregion
  
        protected void Cancel_Click(object sender, EventArgs e)
        {
            if (onCancelButtonClicked != null)
                onCancelButtonClicked(sender, e);
        }

        protected void Update_Click(object sender, EventArgs e)
        {
            if (onUpdateButtonClicked != null)
                onUpdateButtonClicked(sender, e);
        }

        protected void Back_Click(object sender, EventArgs e)
        {
            onBackButtonClicked(sender, e);
        }


         
    }
}
