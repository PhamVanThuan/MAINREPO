<%@ Page Language="C#" AutoEventWireup="true" Inherits="Views_PopYesNoCancel" Title="Confirm" Codebehind="PopYesNoCancel.aspx.cs" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Confirm</title>
    <script type="text/javascript">
    function doButtonClick(val)
    {
        window.returnValue = val;
        window.close();
    }
    function body_onload()
    {
        if (window.dialogArguments[1] == false) document.getElementById("YesButton").style.visibility = "hidden";
        if (window.dialogArguments[2] == false) document.getElementById("NoButton").style.visibility = "hidden";
        if (window.dialogArguments[3] == false) document.getElementById("CancelButton").style.visibility = "hidden";
        
    }
    </script>
</head>
<body onload="body_onload()">
    
    <table border="0" style="width:100%;">
        <tr>
            <td align="center" style="height:90px; padding:6px; background-color:#FFFFFF;">
                <b>
                <script type="text/javascript">
                    document.write( window.dialogArguments[0] );
                    
                </script>
                </b>
            </td>
        </tr>
        <tr>
            <td align="center" style="height:50px;">
                <input type="button" class="BtnNormal3" id="YesButton" value="Yes" accesskey="Y" onclick="doButtonClick('YES')" />
                <input type="button" class="BtnNormal3" id="NoButton" value="No" accesskey="N" onclick="doButtonClick('NO')" />
                <input type="button" class="BtnNormal3" id="CancelButton" value="Cancel" accesskey="C" onclick="doButtonClick('CANCEL')" />
            </td>
        </tr>
    </table>

</body>
</html>
