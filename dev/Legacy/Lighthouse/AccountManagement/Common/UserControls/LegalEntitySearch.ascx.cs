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

public partial class LegalEntitySearch : System.Web.UI.UserControl
{
	private int iLEType;//natural person by default
	public event EventHandler GridClick;
	public event EventHandler DropDownChanged;
	
	protected void Page_Load(object sender, EventArgs e)
    {
		if (ucLEList.gvLEList.Rows.Count == 0)
		{
			ucLEList.bindDataValue = false;
			ucLEList.acctSearch = false;
			selectedLEKey = "";

		}
		else
		{
			ucLEList.bindDataValue = true;
			ucLEList.acctSearch = false;
		}

		//event from uc grid click
		ucLEList.gvLEList.SelectedIndexChanged += new EventHandler(listSelect);
		bindData();

	}
	#region Public methods and properties
	public GridView gvLEList
	{
		get
		{
			return ucLEList.gvLEList;
		}
	}

	
	public void initializeSearchPanels()
	{
		pnlCompany.Visible = false;
		pnlNatural.Visible = true;
		clearSearchCriteria();
	}

	public void clearList()
	{
		initializeSearchPanels();
		ucLEList.gvLEList.DataSource = null;
		ucLEList.DataBind();  
	}


	#region User defined properties

	public bool naturalPerson
	{
		get
		{
			object o = ViewState["naturalPerson"];
			if (o == null)
				return true;   // return a default value
			else
				return (bool)o;
		}
		set
		{
			ViewState["naturalPerson"] = value;
		}
	}
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
	/// selected legal entity's unique identifier 
	/// </summary>
	/// <value></value>
	public string selectedLEKey
	{
		get
		{
			object o = ViewState["selectedLEKey"];
			if (o == null)
				return "";   // return a default value
			else
				return (string)o;
		}
		set
		{
			ViewState["selectedLEKey"] = value;
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
		idNumber = txtIDNumber.Text;
		passportNumber = txtPassportNum.Text;
		surname = txtSurname.Text;
		tradingName = txtTradingName.Text;
		registeredName = txtRegName.Text;
		registrationNumber = txtRegNumber.Text;
		
		if (ddType.SelectedItem != null)
			iLEType = Convert.ToInt32(ddType.SelectedItem.Value);
		else
			iLEType = 2;

		ucLEList.idNumber = idNumber;
		ucLEList.passportNumber = passportNumber;
		ucLEList.surname = surname;
		ucLEList.tradingName = tradingName;
		ucLEList.registeredName = registeredName;
		ucLEList.registrationNumber = registrationNumber;
		ucLEList.legalEntityType = Convert.ToString(iLEType);

		//update grid
		if (ucLEList.bindDataValue == true)
			ucLEList.refresh();
	}
	#endregion
	#region Control events
	protected void ddLEType_SelectedIndexChanged(object sender, EventArgs e)
	{
		//Show correct search fields depending on type
		if (ddType.SelectedItem.Value == "2")
		{
			clearSearchCriteria();
			naturalPerson = true;
			pnlNatural.Visible = true;
			pnlCompany.Visible = false;

		}
		else	//For all other legal entity types
		{
			clearSearchCriteria();
			naturalPerson = false;
			pnlNatural.Visible = false;
			pnlCompany.Visible = true;
		}
		//don't refresh grid if dropdown clicked to reduce trips to server
		ucLEList.bindDataValue = true;
		ucLEList.acctSearch = false;
		//list is unbound so as not to change when dropdown changes
		ucLEList.gvLEList.DataSource = null;
		ucLEList.DataBind();  

		if (DropDownChanged != null)
		{
			DropDownChanged(this, e);
		}

	}
	protected void cmdSearch_Click(object sender, EventArgs e)
	{
		ucLEList.bindDataValue = true;
		ucLEList.acctSearch = false;
		bindData();
	}
	/// <summary>
	/// User has selected a Legal entity in the grid so enable buttons
	/// </summary>
	protected void listSelect(object sender, EventArgs e)
	{
		selectedLEKey = ucLEList.selectedLegalEntityKey;
		ucLEList.bindDataValue = true;
		ucLEList.acctSearch = false;
		
		if (GridClick  != null)
		{
			GridClick (this, e);
		}
	}
	#endregion
	#region Private methods and properties
	private void clearSearchCriteria()
	{
		txtIDNumber.Text = "";
		txtPassportNum.Text = "";
		txtSurname.Text = "";
		txtTradingName.Text = "";
		txtRegName.Text = "";
		txtRegNumber.Text = "";
		iLEType = 2;
		 
	}
	#endregion
}
