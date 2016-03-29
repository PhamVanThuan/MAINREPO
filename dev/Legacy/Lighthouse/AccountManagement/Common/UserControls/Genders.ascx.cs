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

public partial class Genders : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

	}
	#region Public methods and properties

	#region User defined properties

	/// <summary>
	/// unique identifier of selected Gender in list
	/// by default first in list
	/// </summary>
	/// <value></value>
	public int selectedKey
	{
		get
		{
			return Convert.ToInt32(ddGenderTypes.SelectedItem.Value);
		}
	}
	#endregion

	#endregion
}
