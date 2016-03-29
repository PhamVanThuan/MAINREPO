<%@ Control Language="C#" codefile="title.ascx.cs" Inherits="title_ascx" %>
<link href="../style.css" type="text/css" rel="stylesheet" />
<table width="100%">
    <tr>
        <td>
            <asp:Label ID="lblTitle" Runat="server" CssClass="Head"></asp:Label>
        </td>
        <td align="right"><A href="javascript:parent.location.href='<%=Session["Opener"]%>';" >x</A>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <hr style="height: 1px" noshade="noshade" />
        </td>
    </tr>
    
</table>
<script language=javascript>

</script>
