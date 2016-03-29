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
using SAHLLib;

public partial class LegalEntity : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
		/// First time page loaded so initialise toolbar
		if (!IsPostBack)
		{
			ucToolBar.initializeToolbar(SAHLLib.Security.etFunction.LoanBankAccount);
		}
		bindData();

		//setDataSourceConnectionStr();
		ucToolBar.btnSave.Click += new ImageClickEventHandler(save);

	}
	#region Public methods and properties

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
	#endregion

	#endregion
	#region Private methods and properties


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
	/// Updates the database with the legal entity changes specified by the user.
	/// </summary>
	private void save(object sender, ImageClickEventArgs e)
	{
		oLegalEntity.Update();
		ucToolBar.enableEdit(false);

	}

	/// <summary>
	/// Sets the form controls with their database values.
	/// </summary>
	private void bindData()
	{
		hfKey.Value = Convert.ToString(leKey);

		if (pnlEdit.Visible == false)
		{
			DataSourceSelectArguments oDSSA1 = new DataSourceSelectArguments();

			DataView oDV1 = (DataView)oLegalEntity.Select(oDSSA1);

			if (oDV1.Count > 0)
			{
				txtSalutation.Text = oDV1.Table.Rows[0]["Salutation"].ToString();
				txtFirstNames.Text = oDV1.Table.Rows[0]["FirstNames"].ToString();
				txtInitials.Text = oDV1.Table.Rows[0]["Initials"].ToString();
				txtSurname.Text = oDV1.Table.Rows[0]["Surname"].ToString();
				//hfLegalEntityKey.Value = oDV1.Table.Rows[0]["LegalEntityKey"].ToString();
			}
		}
	}

	#endregion
}
