<%@ Control Language="C#" CodeFile="title.ascx.cs" Inherits="title_ascx" %>
<link href="../style.css" type="text/css" rel="stylesheet" />
<table width="98%" border=0 cellpadding=0 cellspacing=0 style="border-bottom: black 1px solid;">
    <tr>
        <td style="width: 729px">
            <asp:Label ID="lblTitle" Runat="server" CssClass="Head" Width="718px" Height="24px"></asp:Label>
        </td>
        <td align="right"><A href="javascript:parent.location.href='<%=Session["Opener"]%>';" >X</A>
        </td>
    </tr>
    <%--<tr>
        <td colspan="2" style="height: 36px">
            <hr style="height: 1px" noshade="noshade" />
        </td>
    </tr>--%>
    
</table>
<script language=javascript>

</script>
