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

public partial class LegalEntityList : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
		//setDataSourceConnectionStr();
		if (bindDataValue  == true)
		{
			bindData();
		}
	}
	
	#region Public methods and properties

	/// <summary>
	/// grid as public property to allow parents to use grid events
	/// </summary>
	/// <value></value>
	public GridView gvLEList
	{
		get
		{
			return gvLegalEntities;
		}
	}

	/// <summary>
	/// public Function to refresh the control's data
	/// </summary>
	public void refresh()
	{
		bindData();
		gvLegalEntities.SelectedIndex = -1;

		selectedLegalEntityKey = "";
		selectedRoleTypeKey = "";
	}
	#region user defined properties

	/// <summary>
	/// boolean value to indicate refresh of grid data or not
	/// </summary>
	/// <value></value>
	public bool bindDataValue
	{
		get
		{
			object o = ViewState["bindDataValue"];
			if (o == null)
				return false;   // return a default value
			else
				return (bool)o;
		}
		set
		{
			ViewState["bindDataValue"] = value;
		}
	}

	/// <summary>
	/// boolean value to indicate whether to search list of legal entities 
	/// by account or not
	/// </summary>
	/// <value></value>
	public bool acctSearch
	{
		get
		{
			object o = ViewState["acctSearch"];
			if (o == null)
				return false;   // return a default value
			else
				return (bool)o;
		}
		set
		{
			ViewState["acctSearch"] = value;
		}
	}

	/// <summary>
	/// selected legal entity's unique identifier 
	/// </summary>
	/// <value></value>
	public string selectedLegalEntityKey
	{
		get
		{
			object o = ViewState["selectedLegalEntityKey"];
			if (o == null)
				return "";   // return a default value
			else
				return (string)o;
		}
		set
		{
			ViewState["selectedLegalEntityKey"] = value;
		}
	}

	/// <summary>
	/// selected legal entity's account identifier
	/// </summary>
	/// <value></value>
	/*	public string selectedAccountKey
		{
			get
			{
				object o = ViewState["selectedAccountKey"];
				if (o == null)
					return "";   // return a default value
				else
					return (string) o;
			}
			set
			{
				ViewState["selectedAccountKey"] = value;
			}
		}
	*/
	/// <summary>
	/// selected legal entity's role type identifier in chosen account
	/// </summary>
	/// <value></value>
	public string selectedRoleTypeKey
	{
		get
		{
			object o = ViewState["selectedRoleTypeKey"];
			if (o == null)
				return "";   // return a default value
			else
				return (string)o;
		}
		set
		{
			ViewState["selectedRoleTypeKey"] = value;
		}

	}
	/// <summary>
	/// chosen account used as search criteria if is list of legal entities for an account
	/// </summary>
	/// <value></value>
	public string accountKey
	{
		get
		{
			object o = ViewState["accountKey"];
			if (o == null)
				return "";   // return a default value
			else
				return (string)o;
		}
		set
		{
			ViewState["accountKey"] = value;
		}
	}
	/// <summary>
	/// legal entity list filter by this IDNumber
	/// </summary>
	/// <value></value>
	public string idNumber
	{
		get
		{
			object o = ViewState["idNumber"];
			if (o == null)
				return "";   // return a default value
			else
				return (string)o;
		}
		set
		{
			ViewState["idNumber"] = value;
		}
	}
	/// <summary>
	/// legal entity list filter by this Passport Number
	/// </summary>
	/// <value></value>
	public string passportNumber
	{
		get
		{
			object o = ViewState["passportNumber"];
			if (o == null)
				return "";   // return a default value
			else
				return (string)o;
		}
		set
		{
			ViewState["passportNumber"] = value;
		}
	}
	/// <summary>
	/// legal entity list filter by this Surname
	/// </summary>
	/// <value></value>
	public string surname
	{
		get
		{
			object o = ViewState["surname"];
			if (o == null)
				return "";   // return a default value
			else
				return (string)o;
		}
		set
		{
			ViewState["surname"] = value;
		}
	}
	/// <summary>
	/// legal entity list filter by this TradingName
	/// </summary>
	/// <value></value>
	public string tradingName
	{
		get
		{
			object o = ViewState["tradingName"];
			if (o == null)
				return "";   // return a default value
			else
				return (string)o;
		}
		set
		{
			ViewState["tradingName"] = value;
		}
	}
	/// <summary>
	/// legal entity list filter by this RegisteredName
	/// </summary>
	/// <value></value>
	public string registeredName
	{
		get
		{
			object o = ViewState["registeredName"];
			if (o == null)
				return "";   // return a default value
			else
				return (string)o;
		}
		set
		{
			ViewState["registeredName"] = value;
		}
	}
	/// <summary>
	/// legal entity list filter by this Registration Number
	/// </summary>
	/// <value></value>
	public string registrationNumber
	{
		get
		{
			object o = ViewState["registrationNumber"];
			if (o == null)
				return "";   // return a default value
			else
				return (string)o;
		}
		set
		{
			ViewState["registrationNumber"] = value;
		}
	}
	/// <summary>
	/// legal entity list filter by this type of legal entity
	/// eg. Natural Person, Company
	/// </summary>
	/// <value></value>
	public string legalEntityType
	{
		get
		{
			object o = ViewState["legalEntityType"];
			if (o == null)
				return "";   // return a default value
			else
				return (string)o;
		}
		set
		{
			ViewState["legalEntityType"] = value;
		}
	}

	#endregion

	#endregion
	#region Data functions
	/// <summary>
	/// Sets the form controls with their database values.
	/// </summary>
	private void bindData()
	{
		hfAcctKey.Value = Convert.ToString(accountKey) + "%";

		hfIDNum.Value = Convert.ToString(idNumber) + "%";
		hfPassportNum.Value = Convert.ToString(passportNumber) + "%";
		hfSurname.Value = Convert.ToString(surname) + "%";
		hfTradingName.Value = Convert.ToString(tradingName) + "%";
		hfRegisteredName.Value = Convert.ToString(registeredName) + "%";
		hfRegistrationNum.Value = Convert.ToString(registrationNumber) + "%";
		hfType.Value = Convert.ToString(legalEntityType) + "%";

		if (acctSearch == true)
			searchByAcctSetup ();
		else
			searchByFiltersSetup ();
	}
	/// <summary>
	/// Sets the data sources on grid to search by account key
	/// </summary>
	private void searchByAcctSetup()
	{
		gvLegalEntities.DataSource = obyAccount;
		gvLegalEntities.DataBind();
	}

	/// <summary>
	/// Sets the data sources on grid to search by filters
	/// </summary>
	private void searchByFiltersSetup()
	{
		gvLegalEntities.DataSource = obySearch;
		gvLegalEntities.DataBind();

	}

	#endregion
	#region Control events
	/// <summary>
	/// user has selected a row in the grid.  Fires 1st before parent's event
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void gvLegalEntities_SelectedIndexChanged(object sender, EventArgs e)
	{
		//Clicked Select Command Column
		int i = 0;
		i = gvLegalEntities.SelectedIndex;

		DataSourceSelectArguments oDSSA = new DataSourceSelectArguments();
		SqlDataSource osql = (SqlDataSource)gvLegalEntities.DataSource;
		DataView oDV = (DataView)osql.Select(oDSSA);

		selectedLegalEntityKey = Convert.ToString(oDV.Table.Rows[i]["LegalEntityKey"]);
		selectedRoleTypeKey = Convert.ToString(oDV.Table.Rows[i]["RoleTypeKey"]);

	}
#endregion

}
