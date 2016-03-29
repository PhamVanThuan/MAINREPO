using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHLLib;


public partial class toolbar_ascx : System.Web.UI.UserControl
{

    public ImageButton btnSave
    {
        get
        {
            return ibSave;
        }
    }


    void Page_Load(object sender, EventArgs e)
    {
        

        if (!IsPostBack)
        {
            
        }
    }

    public void initializeToolbar(SAHLLib.Security.etFunction a_etFunction) {

        if (panelsExist())
        {
            enableEdit(false);
            if (!Security.editAllowed(a_etFunction))
            {
                hideAllImages();
            }
        }
        else
        {
            hideAllImages();
        }
    }

    

    public void enableEdit(bool a_enable)
    {

        Control oCntrlEdit = null;
        Control oCntrlDisplay = null;

        oCntrlEdit = Parent.FindControl("pnlEdit");
        oCntrlDisplay = Parent.FindControl("pnlDisplay");

        if (a_enable == true)
        {
            oCntrlEdit.Visible = true;
            oCntrlDisplay.Visible = false;
            showSaveCancelImages();
        }
        else
        {
            oCntrlDisplay.Visible = true;
            oCntrlEdit.Visible = false;
            showEditImage();
        }
    }

    private bool panelsExist()
    {
        Control oCntrlEdit = null;
        Control oCntrlDisplay = null;

        oCntrlEdit = Parent.FindControl("pnlEdit");
        oCntrlDisplay = Parent.FindControl("pnlDisplay");
        
        if (oCntrlEdit != null && oCntrlDisplay != null)
            return true;
        else
            return false;

    }

    private void showSaveCancelImages()
    {
        ibCancel.Visible = true;
        ibEdit.Visible = false;
        ibSave.Visible = true;
    }

    private void showEditImage()
    {
        ibCancel.Visible = false;
        ibEdit.Visible = true;
        ibSave.Visible = false;
    }

    protected void ibEdit_Click(object sender, ImageClickEventArgs e)
    {
        enableEdit(true);
    }

    protected void ibCancel_Click(object sender, ImageClickEventArgs e)
    {
        enableEdit(false);
    }
    
    private void hideAllImages()
    {
        ibSave.Visible = false;
        ibCancel.Visible = false;
        ibEdit.Visible = false;
    }




}
