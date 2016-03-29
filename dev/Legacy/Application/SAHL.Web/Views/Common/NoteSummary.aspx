<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    CodeBehind="NoteSummary.aspx.cs" Inherits="SAHL.Web.Views.Common.NoteSummary" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <script src="../../Scripts/jquery-1.5.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            // hide all the rows initially
            $(".noteRow").hide();
            // filter "Current" debtcounselling cases initially
            FilterChanged();
        });

        function PrintElem(elem) {
            Popup($(elem).html());
        }

        function Popup(data) {
            var mywindow = window.open('', 'Notes', 'height=400,width=600');
            mywindow.document.write('<html><head><title>Notes</title>');
            mywindow.document.write('<link rel="stylesheet" href="../../css/sahl.css" type="text/css" />');
            mywindow.document.write('</head><body >');
            mywindow.document.write(data);
            mywindow.document.write('</body></html>');
            mywindow.document.close();
            mywindow.print();
            return true;
        }

        function FilterChanged() {

            // get filter values
            var filterCases = $("#<%=ddlCases.ClientID%>").val();
            var filterLegalEntity = $("#<%=ddlLegalEntities.ClientID%>").val();
            var filterDateFrom = $("#<%=ddlDateFrom.ClientID%>").val();
            var filterDateTo = $("#<%=ddlDateTo.ClientID%>").val();

            if (filterCases == "All" && filterLegalEntity == "-select" && filterDateFrom == "-select" && filterDateTo == "-select") {
                // if no filters are selected then show everything
                $(".noteRow").show();
            }
            else {
                // hide all the rows initially
                $(".noteRow").hide();

                // loop thru each note row and check its attributes
                $(".noteRow").each(function () {

                    var classNames = $(this).attr("class") // gets the class names assocaited with the row eg: "noteRow le223 dte20110421"
                    var classArray = classNames.split(" "); // split into 4 variables so we can get each individual value
                    var generickeyVal = classArray[1].toString().replace("gk", ""); // get generickey value
                    var legalentityVal = classArray[2].toString().replace("le", ""); // get legalentity value
                    var dateVal = classArray[3].toString().replace("dte", ""); // get date value

                    // determine whether the row matches the generickey filter
                    var genericKey = $("#<%=hidGenericKey.ClientID%>").val();

                    var caseMatch = false;
                    if (filterCases == 'Current' && genericKey == generickeyVal) {
                        caseMatch = true;
                    }
                    else if (filterCases == 'All') {
                        caseMatch = true;
                    }

                    // determine whether the row matches the legalentity filter
                    var legalentityMatch = true;
                    if (filterLegalEntity != '-select-' && filterLegalEntity != legalentityVal) {
                        legalentityMatch = false;
                    }

                    // determine whether the row matches the dates filter
                    var dateMatch = true;
                    if (filterDateFrom == '-select-')
                        filterDateFrom = 11111111;
                    if (filterDateTo == '-select-')
                        filterDateTo = 999999999;
                    if (dateVal < filterDateFrom || dateVal > filterDateTo) {
                        dateMatch = false;
                    }

                    // if row matches filter then show row
                    if (caseMatch== true && legalentityMatch == true && dateMatch == true) {
                        $(this).show();
                    }
                });
            }
        }
    </script>
    <div style="text-align: left">
        <table id="filterTable" width="100%" class="tableStandard">
            <tr class="noteFilterRow">
                <td style="width: 50px">
                </td>
                <td style="vertical-align: middle">
                    <SAHL:SAHLLabel ID="SAHLLabel2" runat="server" Text = "Diary Date :" Font-Bold="true" />
                    <SAHL:SAHLLabel ID="lblDiaryDate" runat="server"></SAHL:SAHLLabel>
                </td>
                <td style="width: 150px;text-align:right;vertical-align:middle;padding-right:15px" >
                    <b>Filters :</b>
                </td>
                <td class="noteFilterCell" style="width: 110px">
                    
                    <br />
                    <SAHL:SAHLDropDownList ID="ddlCases" runat="server" onchange="FilterChanged()">
                        <asp:ListItem Value="Current">Current Case</asp:ListItem>
                        <asp:ListItem Value="All">All Cases</asp:ListItem>
                    </SAHL:SAHLDropDownList>
                </td>
                <td class="noteFilterCell" style="width: 250px">
                    User
                    <br />
                    <SAHL:SAHLDropDownList ID="ddlLegalEntities" runat="server" onchange="FilterChanged()">
                    </SAHL:SAHLDropDownList>
                </td>
                <td class="noteFilterCell" style="width: 150px">
                    From Date
                    <br />
                    <SAHL:SAHLDropDownList ID="ddlDateFrom" runat="server" onchange="FilterChanged()">
                    </SAHL:SAHLDropDownList>
                </td>
                <td class="noteFilterCell" style="width: 150px">
                    To Date
                    <br />
                    <SAHL:SAHLDropDownList ID="ddlDateTo" runat="server" onchange="FilterChanged()">
                    </SAHL:SAHLDropDownList>
                </td>
                <td style="width: 50px">
                
                </td>
            </tr>
        </table>
        <br />
        <table id="repeaterTable" width="100%">
            <tr>
                <td style="width: 50px">
                </td>
                <td>
                    <div id="divNotes" class="noteTable">
                        <asp:Repeater runat="server" ID="rptNotes">
                            <ItemTemplate>
                                <div class="noteRow gk<%# DataBinder.Eval(Container.DataItem, "Note.GenericKey")%> le<%# DataBinder.Eval(Container.DataItem, "LegalEntity.Key")%> dte<%# DataBinder.Eval(Container.DataItem, "InsertedDate","{0:yyyyMMdd}")%>">
                                    <table width="100%" class="tabledetails">
                                        <tr class="noteHeaderRow">
                                            <td style="width: 250px">
                                                <b>Date :</b>                                          
                                                <%# DataBinder.Eval(Container.DataItem, "InsertedDate","{0:dd/MM/yyy HH:mm}")%>
                                            </td>
                                            <td style="width: 250px">
                                                <b>User :</b>
                                                <%# DataBinder.Eval(Container.DataItem, "LegalEntity.DisplayName")%>
                                            </td>
                                            <td style="width: 250px">
                                                <b>Workflow State :</b>
                                                <%# DataBinder.Eval(Container.DataItem, "WorkflowState")%>
                                            </td>
                                            <td style="width: 250px">
                                                <b>Tag :</b>
                                                <%# DataBinder.Eval(Container.DataItem, "Tag")%>
                                            </td>
                                        </tr>
                                        <tr class="noteBlankRow">
                                            <td colspan="4" class="noteBlankCell">
                                                <br />
                                            </td>
                                        </tr>
                                        <tr class="noteDetailRow">
                                            <td colspan="4" class="noteDetailCell">
                                                <%# DataBinder.Eval(Container.DataItem, "NoteText")%>
                                            </td>
                                        </tr>
                                        <tr class="noteBlankRow">
                                            <td colspan="4">
                                                <hr>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </td>
            </tr>
        </table>
        <table id="tblButtons" width="100%">
            <tr align="center">
                <td colspan="2">
                    <br />
                    <SAHL:SAHLPanel ID="pnlPrint" runat="server" SecurityTag="NoteSummaryPrint">
                        <input type="button" value="Print" onclick="PrintElem('#divNotes')" />
                    </SAHL:SAHLPanel>
                </td>
            </tr>
            <tr align="center">
                <td colspan="2">
                    <asp:HiddenField ID="hidGenericKey" runat="server" />
                    <asp:HiddenField ID="hidGenericKeyTypeKey" runat="server" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
