<%@ Control Language="C#" CodeFile="toolbar.ascx.cs" Inherits="toolbar_ascx" %>
<asp:ImageButton ID="ibCancel"  ToolTip="Cancel" ImageUrl="~/common/images/cancel.gif"
    Runat="server" OnClick="ibCancel_Click" />&nbsp;
<asp:ImageButton ID="ibSave"  ToolTip="Save"
        ImageUrl="~/common/images/save.PNG" Runat="server"  /><asp:ImageButton ID="ibEdit" 
            ToolTip="Edit" ImageUrl="~/common/images/edit.gif" Runat="server" OnClick="ibEdit_Click" />
