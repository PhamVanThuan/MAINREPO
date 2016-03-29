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

public partial class AccountDetails : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
		//setDataSourceConnectionStr();
		bindData();

	}
	#region Public methods and properties
	#region User defined properties

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
	#endregion
	#endregion
	#region Data functions
	/// <summary>
	/// Sets the form controls with their database values.
	/// </summary>
	private void bindData()
	{
		DataSourceSelectArguments oDSSA = new DataSourceSelectArguments();
		DataView oDV = (DataView)oAccount.Select(oDSSA);

		hfAcctKey.Value = Convert.ToString(accountKey);
	}

	/*	/// <summary>
	/// Set all SQLDataSource objects connection string properties.
	/// </summary>
	private void setDataSourceConnectionStr()
	{
		//Need to create a function in SAHLLib to allow DB specification. For now replace SAHLDB with 2am.
		string sCon = DBConnection.ConnectionString().Replace("SAHLDB", "2am");

		oLoanSummary.ConnectionString = sCon;

	}
*/
	#endregion
}
