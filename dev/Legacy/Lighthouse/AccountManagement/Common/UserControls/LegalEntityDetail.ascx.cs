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

public partial class LegalEntityDetail : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
		if (!IsPostBack)
		{

		}
		bindData();

		//setDataSourceConnectionStr();

	}

	#region Public methods and properties

	public enum editType { none = 0, add = 1, remove = 2};
	/// <summary>
	/// Add button exposed as public
	/// </summary>
	/// <value></value>
	public Button btnAdd
	{
		get
		{
			return cmdAdd;
		}
	}
	/// <summary>
	/// Remove button exposed as public
	/// </summary>
	/// <value></value>
	public Button btnRemove
	{
		get
		{
			return cmdRemove;
		}
	}
	/// <summary>
	/// Cancel button exposed as public
	/// </summary>
	/// <value></value>
	public Button btnCancel
	{
		get
		{
			return cmdCancel;
		}
	}
	/// <summary>
	/// Data Source exposed as public
	/// </summary>
	/// <value></value>
	public SqlDataSource legalEntity
	{
		get
		{
			return oLegalEntity;
		}
	}
	/// <summary>
	/// legal entity key Hidden field exposed as public
	/// </summary>
	/// <value></value>
	public HiddenField leKeyhf
	{
		get
		{
			return hfLEKey;
		}
	}
	/// <summary>
	/// account key Hidden field exposed as public
	/// </summary>
	/// <value></value>
	public HiddenField accountKeyhf
	{
		get
		{
			return hfAccountKey;
		}
	}
	/// <summary>
	/// role type key Hidden field exposed as public
	/// </summary>
	/// <value></value>
	public HiddenField RoleTypehf
	{
		get
		{
			return hfRoleTypeKey;
		}
	}

	/// <summary>
	/// public method to show correct command buttons depending on edit type
	/// </summary>
	public void initializeControl()
	{
		switch ((editType)controlType)
		{
			case editType.add:
				showAdd();
				break;
			case editType.remove:
				showRemove();
				break;
			default:
				showCancel();
				break;
		}
	}

	#region User defined properties

	/// <summary>
	/// legal entity unique identifier saved in view state
	/// </summary>
	/// <value></value>
	public int leKey
	{
		get
		{
			object o = ViewState["leKey"];
			if (o == null)
				return 0;   // return a default value
			else
				return (int)o;
		}
		set
		{
			ViewState["leKey"] = value;
		}
	}
	/// <summary>
	/// account unique identifier saved in view state
	/// </summary>
	/// <value></value>
	public int accountKey
	{
		get
		{
			object o = ViewState["accountKey"];
			if (o == null)
				return 0;   // return a default value
			else
				return (int)o;
		}
		set
		{
			ViewState["accountKey"] = value;
		}
	}
	/// <summary>
	/// role type unique identifier saved in view state
	/// </summary>
	/// <value></value>
	public int roleTypeKey
	{
		get
		{
			object o = ViewState["roleTypeKey"];
			if (o == null)
				return 0;   // return a default value
			else
				return (int)o;
		}
		set
		{
			ViewState["roleTypeKey"] = value;
		}
	}
	/// <summary>
	/// Type of user control to display
	/// </summary>
	/// <value></value>
	public int controlType
	{
		get
		{
			object o = ViewState["controlType"];
			if (o == null)
				return 0;   // return a default value
			else
				return (int)o;
		}
		set
		{
			ViewState["controlType"] = value;
		}
	}
	#endregion
	#endregion
	#region Data functions
	/*	/// <summary>
	/// Set all SQLDataSource objects connection string properties.
	/// </summary>
	private void setDataSourceConnectionStr()
	{
		//Need to create a function in SAHLLib to allow DB specification. For now replace SAHLDB with 2am.
		string sCon = DBConnection.ConnectionString().Replace("SAHLDB", "2am");

		oLegalEntity.ConnectionString = sCon;
	}*/
	/// <summary>
	/// Sets the form controls with their database values.
	/// </summary>
	private void bindData()
	{
		hfLEKey.Value = Convert.ToString(leKey);
		hfAccountKey.Value = Convert.ToString(accountKey);
		hfRoleTypeKey.Value = Convert.ToString(roleTypeKey);
		DataSourceSelectArguments oDSSA = new DataSourceSelectArguments();
		DataView oDV = (DataView)oLegalEntity.Select(oDSSA);

	}
	/// <summary>
	/// Function to refresh the control's data
	/// </summary>
	public void refresh()
	{
		bindData();
	}

	#endregion
	#region Private methods and properties
	/// <summary>
	/// private method to display Add buttons
	/// </summary>
	private void showAdd()
	{
		cmdAdd.Visible = true;
		cmdRemove.Visible = false;
		cmdCancel.Visible = true;
	}
	/// <summary>
	/// private method to display Remove buttons
	/// </summary>
	private void showRemove()
	{
		cmdAdd.Visible = false;
		cmdRemove.Visible = true;
		cmdCancel.Visible = true;
	}
	/// <summary>
	/// private method to display Cancel buttons
	/// </summary>
	private void showCancel()
	{
		cmdAdd.Visible = false;
		cmdRemove.Visible = false;
		cmdCancel.Visible = true;
	}
	#endregion

}
