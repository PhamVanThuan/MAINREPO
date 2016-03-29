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

public partial class MultiViewStart : System.Web.UI.Page
{
	private int iAcctKey = 0;
	protected void Page_Load(object sender, EventArgs e)
    {
		iAcctKey = Convert.ToInt32(Request.QueryString["AccountKey"]);

		if (!Page.IsPostBack)
		{
			MultiView1.SetActiveView(vwAcct);
		}


		ucLEAccount.accountKey = iAcctKey;	//Set Account Key property on UC

		ucLEList.accountKey = Convert.ToString(iAcctKey);
		ucLEList.legalEntityType = "";
		ucLEList.bindDataValue = true;
		ucLEList.acctSearch = true;

		ucLEList.gvLEList.SelectedIndexChanged += new EventHandler(accountGridSelect);

		ucLEListSearch.GridClick += new EventHandler(existingListSelect);
		ucLEListSearch.DropDownChanged += new EventHandler(roleTypeChange);

		ucLEtoDelete.btnRemove.Click += new EventHandler(removeLE);
		ucLEtoDelete.btnCancel.Click += new EventHandler(removeLECancel);

		ucLEtoAdd.btnAdd.Click += new EventHandler(addLE);
		ucLEtoAdd.btnCancel.Click += new EventHandler(addLECancel);

	}
	#region Public methods and properties
	#region user defined properties
	/// <summary>
	/// unique account identifier
	/// </summary>
	/// <value></value>
	public int accountKey
	{
		get { return iAcctKey; }
		set { iAcctKey = value; }
	}

	#endregion
	#endregion
	#region Control events

	/// <summary>
	/// Add a legal entity to chosen account
	/// show add view
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void cmdAdd_Click(object sender, EventArgs e)
	{
		MultiView1.SetActiveView(vwLESearchExisting);
		ucLEListSearch.initializeSearchPanels();
		if (ucLEListSearch.gvLEList.Rows.Count == 0)
		{
			ucLEtoAdd.Visible = false;
			ucRoleTypes.Visible = false;
		}

		ucLEtoAdd.controlType = 1;
		ucLEtoAdd.initializeControl();
	}
	/// <summary>
	/// Edit selected legal entity on account
	/// show edit view
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void cmdEdit_Click(object sender, EventArgs e)
	{
		MultiView1.SetActiveView(vwEdit);
	}
	/// <summary>
	/// remove selected legal entity from chosen account
	/// show remove view
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void cmdRemove_Click(object sender, EventArgs e)
	{
		MultiView1.SetActiveView(vwRemove);
		ucLEtoDelete.controlType = 2;
		ucLEtoDelete.initializeControl();
	}
	/// <summary>
	/// Back button of LE search view
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void cmdBack_edit_Click(object sender, EventArgs e)
	{
		MultiView1.SetActiveView(vwAcct);
	}
	/// <summary>
	/// Back button of remove view
	/// back to start view
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void removeLECancel(object sender, EventArgs e)
	{
		MultiView1.SetActiveView(vwAcct);
	}
	/// <summary>
	/// Back button of add existing view
	/// back to start view
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void addLECancel(object sender, EventArgs e)
	{
		MultiView1.SetActiveView(vwAcct);
	}

	/// <summary>
	/// User has selected a Legal entity in the account's legal entities 
	/// grid so enable buttons etc
	/// </summary>
	protected void accountGridSelect(object sender, EventArgs e)
	{
		cmdEdit.Enabled = true;
		cmdRemove.Enabled = true;

		//On grid click set properties on Remove UC
		lblAccountKey.Text = ucLEList.selectedLegalEntityKey.ToString();
		ucLEtoDelete.accountKey  = ucLEAccount.accountKey;
		ucLEtoDelete.leKey = Convert.ToInt32(ucLEList.selectedLegalEntityKey);
		ucLEtoDelete.roleTypeKey = Convert.ToInt32(ucLEList.selectedRoleTypeKey);

		//On grid click set properties on Edit UC
		ucLEtoEdit.leKey = Convert.ToInt32(ucLEList.selectedLegalEntityKey);
	}

	/// <summary>
	/// User has selected a Legal entity in the existing legal entities grid 
	/// so enable role types control
	/// </summary>
	protected void existingListSelect(object sender, EventArgs e)
	{
		ucRoleTypes.Visible = true;
		ucLEtoAdd.accountKey  = ucLEAccount.accountKey;
		ucLEtoAdd.leKey = Convert.ToInt32(ucLEListSearch.selectedLEKey);
		ucLEtoAdd.refresh();
		ucLEtoAdd.Visible = true;

	}
	/// <summary>
	/// User has changed the dropdown selection and refreshed the grid
	/// so hide controls
	/// </summary>
	protected void roleTypeChange(object sender, EventArgs e)
	{
		ucRoleTypes.Visible = false;
		ucLEtoAdd.Visible = false;
	}

	protected void MultiView1_ActiveViewChanged(object sender, EventArgs e)
	{
		if (MultiView1.GetActiveView() != vwLESearchExisting)
		{
			ucLEListSearch.clearList();
		}

	}

	#endregion
	#region Data functions

	/// <summary>
	/// Add selected legal entity to account
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void addLE(object sender, EventArgs e)
	{
		ucLEtoAdd.roleTypeKey = ucRoleTypes.SelectedKey;
		ucLEtoAdd.refresh();
		ucLEtoAdd.legalEntity.Insert();
		//refresh account's list of legal entities
		ucLEList.refresh();
		MultiView1.SetActiveView(vwAcct);
	}
	/// <summary>
	/// remove selected legal entity from having role in account
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void removeLE(object sender, EventArgs e)
	{
		ucLEtoDelete.legalEntity.Delete();
		//refresh account's list of legal entities
		ucLEList.refresh();
		MultiView1.SetActiveView(vwAcct);
	}
	#endregion
}
