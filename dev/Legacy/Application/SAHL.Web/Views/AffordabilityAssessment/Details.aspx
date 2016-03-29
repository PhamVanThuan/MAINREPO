<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Inherits="SAHL.Web.Views.AffordabilityAssessment.Details" Title="Affordability Assessment Details" CodeBehind="Details.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">

    <script src="../../Scripts/Plugins/jquery-ui-1.11.4/external/jquery/jquery.js" type="text/javascript"></script>
    <script src="../../Scripts/Plugins/jquery-ui-1.11.4/jquery-ui.min.js" type="text/javascript"></script>
    <link rel="stylesheet" href="../../Scripts/Plugins/jquery-ui-1.11.4/jquery-ui.min.css">


    <script type="text/javascript">

        function showError(errors) {
            var errorListOL = $("#errorList");
            errorListOL.empty();
            for (var i = 0; i < errors.length; i++) {
                errorListOL.append("<li>" + errors[i] + "</li>");
            }

            $("#<%=btnSubmit.ClientID%>").attr('disabled', 'disabled');
            $("#valSummary").draggable();
            $("#valSummary").show();
        }


        $(function () {
            $("#valSummary").hide();

            var dialogCommentsDisplay, dialogCommentsUpdate, commentsHiddenField

            dialogCommentsDisplay = $("#dialog-comments-display").dialog({
                autoOpen: false,
                height: 250,
                width: 350,
                modal: true,
                show: 'blind',
                hide: 'blind',
                buttons: {
                    OK: function () {
                        dialogCommentsDisplay.dialog("close");
                    }
                }
            });

            dialogCommentsUpdate = $("#dialog-comments-update").dialog({
                autoOpen: false,
                height: 250,
                width: 350,
                modal: true,
                show: 'blind',
                hide: 'blind',
                buttons: {
                    OK: function () {
                        // set the comment hidden filed to the value captured on the screen
                        var comments = $('#<%=txtCommentsUpdate.ClientID%> ').val();
                        commentsHiddenField.value = comments;

                        SetCommentIcons();

                        dialogCommentsUpdate.dialog("close");
                    },
                    Cancel : function () {
                        dialogCommentsUpdate.dialog("close");
                    }
                }
            });

            // this will find all objects with a class of ".commentsIcon" and add the on click event
            $(".commentsIcon").on("click", function () {
                // get the name of this image clicked so we know what the comments relate to
                var clickedImageId = this.id;
                var result = clickedImageId.substring(clickedImageId.indexOf("img") + 3, clickedImageId.length);
                // build the name of the related hidden field
                var hiddenFieldID = clickedImageId.substring(0, clickedImageId.indexOf("img")) + 'hid' + result + '_Comments';
                // get the hidden field so we can get comments
                commentsHiddenField = document.getElementById(hiddenFieldID);

                // if we are in display mode then show the 'display' dialog otherwise shoe the 'update' dialog
                var screenMode = $('#<%=hidScreenMode.ClientID%> ').val();
                if (screenMode == "Display")
                {
                    // set the comments in the dialog to the value stored in the hidden field
                    $('#<%=txtCommentsDisplay.ClientID%> ').text(commentsHiddenField.value);

                    dialogCommentsDisplay.dialog("open");
                }
                else
                {
                    // set the comments in the dialog to the value stored in the hidden field
                    $('#<%=txtCommentsUpdate.ClientID%> ').text(commentsHiddenField.value);

                    dialogCommentsUpdate.dialog("open");
                }
            });
        });
    </script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            SetCommentIcons();

            HandleFieldChanges();

        });

        function StressFactorPercentage_SelectedIndexChanged(ddl) {
            // get the selected stress factor key
            var selectedValue = ddl.options[ddl.selectedIndex].value;
            // find the corresponding percentage increase
            var stressFactorPercentageIncrease =  $('#<%=ddlStressFactorPercentageLookup.ClientID%> option').eq(ddl.selectedIndex).text();
            // set the hidden field which gets used in the calculation
            $('#<%=hidStressFactorPercentageIncrease.ClientID%> ').val(stressFactorPercentageIncrease);
            // change the credit totals
            SumTotals(".credit");
            // change the to be totals
            SumTotals(".tobe");
        }

        function updateMinMonthlyFixedExpenses(grossIncome) {
            $.ajax({
                type: "POST",
                url: "../../Ajax/DecisionTree.asmx/DetermineNCRGuidelineMinMonthlyFixedExpenses",
                data: "{'grossMonthlyIncome': " + grossIncome + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (jsonData) {
                    var result = jsonData.d || { 'Result': 0 };
                    $("#<%=txtMinMonthlyFixedExpenses.ClientID%>").val(result.Result);
                    SumTotals(".credit");
                },
                error: function (e) {
                    showError(["An error occurred when determining the NCR Minimum Monthly Fixed Expenses."]);
                }
            });
        }

        function SetCommentIcons() {
            // get all the comment icon images
            var imageIDs = $("#divFields").find("img").map(function () {
                return $(this).attr("id");
            }).get();

            $('#<%=hidAllCommentsEntered.ClientID%> ').val('true');

            // loop thru each image and set the image src depending on whether hidden field has a value
            for (i = 0; i < imageIDs.length; i++) {

                // get the comment icon image
                var imageID = imageIDs[i];
                var img = document.getElementById(imageID);

                // get the comments from the hidden field
                var result = imageID.substring(imageID.indexOf("img") + 3, imageID.length);
                var hiddenFieldName = imageID.substring(0, imageID.indexOf("img")) + 'hid' + result + '_Comments';
                var hiddenField = document.getElementById(hiddenFieldName);
                var comment = hiddenField.value;

                // hide the icon initially
                img.style.visibility = "hidden";

                // now do the checks to decide whether to show the icon and what color it should be
                var screenMode = $('#<%=hidScreenMode.ClientID%> ').val();
                if (screenMode == "Display")
                {
                    // if we are in 'Display' mode and there are comments then show 'Green' icon
                    if (comment.length > 0) {
                        img.style.visibility = "visible";
                        img.setAttribute('src', "../../Images/comments_green.png");
                    }                    
                }
                else if (screenMode == "Update_Credit")
                {
                    // if we are in update mode and there are comments then show 'Green' icon
                    if (comment.length > 0) {
                        img.style.visibility = "visible";
                        img.setAttribute('src', "../../Images/comments_green.png");
                    }
                    else {
                        // if there are no comments but 'Credit' value is different to the 'Client' value then show 'Red' icon
                        var creditValue, clientValue, consolidateValue;

                        var clientFieldID = "#" + hiddenFieldName.replace("hid", "txt").replace("_Comments", "_Credit");
                        var creditFieldID = clientFieldID.replace("_Credit", "_Client");
                        var consolidateFieldID = clientFieldID.replace("_Credit", "_Consolidate");

                        clientValue = $(clientFieldID).val().replace(/[^0-9]/g, "");
                        creditValue = $(creditFieldID).val().replace(/[^0-9]/g, "");
                        consolidateValue = typeof $(consolidateFieldID).val() == "undefined" ? 0 : $(consolidateFieldID).val().replace(/[^0-9]/g, ""); // not all items have a consolidated value

                        if (creditValue != clientValue)
                        {
                            img.style.visibility = "visible";
                            img.setAttribute('src', "../../Images/comments_red.png");

                            // if we have any 'red' icons, it means user has to still enter some comments so set hidden field to false so was can use in screen validation
                            $('#<%=hidAllCommentsEntered.ClientID%> ').val('false');
                            
                        }
                        // if there are no comments but there is a 'Debt to Consolidate' value then show 'White' icon
                        else if (consolidateValue > 0) {
                            img.style.visibility = "visible";
                            img.setAttribute('src', "../../Images/comments.png");
                        }

                    }
                }
            }
        };

        function HandleFieldChanges() {
            // for each textbox with a CssClasss of 'client' hook into the 'change' event
            $(".client").each(function () {
                $(this).change(function (value) {
                    // set the value of the 'credit' field = value of the 'client' field
                    var creditFieldID = "#" + this.id.replace("_Client", "_Credit");                 
                    var clientFieldValue = $(this).val();
                    $(creditFieldID).val(clientFieldValue);

                    SetToBeAmount(creditFieldID, clientFieldValue);

                    // change the client totals
                    SumTotals(".client");
                    // change the credit totals
                    SumTotals(".credit");
                    // change the to be totals
                    SumTotals(".tobe");

                    // if the field that has chanegd is a 'client' income' field, call the decision tree
                    if ($(this).hasClass("income")) {
                        var grossIncome = SumValues(".client.income");
                        // call the DecisionTree
                        updateMinMonthlyFixedExpenses(grossIncome);
                    }
                });
            });

            // for each textbox with a CssClasss of 'credit' hook into the 'change' event
            $(".credit").each(function () {
                $(this).change(function (value) {

                    var creditFieldValue = $(this).val();
                    SetToBeAmount("#" + this.id, $(this).val());

                    // change the credit totals
                    SumTotals(".credit");
                    // change the consolidate totals
                    SumTotals(".consolidate");
                    // change the to be totals
                    SumTotals(".tobe");

                    SetCommentIcons();
                });
            });

            // for each textbox with a CssClasss of 'consolidate' hook into the 'change' event
            $(".consolidate").each(function () {
                $(this).change(function (value) {

                    var creditFieldID = "#" + this.id.replace("_Consolidate", "_Credit");
                    var creditFieldValue = $(creditFieldID).val().replace(/[^0-9]/g, "");

                    SetToBeAmount(creditFieldID, creditFieldValue);

                    // change the credit totals
                    SumTotals(".credit");
                    // change the consolidate totals
                    SumTotals(".consolidate");
                    // change the to be totals
                    SumTotals(".tobe");

                    SetCommentIcons();
                });
            });
        };

        function SetToBeAmount(creditFieldID, creditFieldValue) {
            // set the value of the 'to be' field = value of the 'credit' - 'consolidate' field
            var tobeFieldID = creditFieldID.replace("_Credit", "_ToBe");
            var consolidateFieldID = creditFieldID.replace("_Credit", "_Consolidate");
            var consolidateFieldValue = typeof $(consolidateFieldID).val() == "undefined" ? 0 : $(consolidateFieldID).val().replace(/[^0-9]/g, ""); // not all items have a consolidated value

            $(tobeFieldID).val(creditFieldValue.replace(/[^0-9]/g, "") - consolidateFieldValue);
        }

        function SumTotals(selector) {
            // sum up totals
            var grossIncome = SumValues(selector + ".income");
            var incomeDeductions = SumValues(selector + ".payrollDeductions");
            var netIncome = grossIncome - incomeDeductions;

            var necessaryExpensesTotal = SumValues(selector + ".necessaryExpenses");
            var allocableIncome = netIncome - necessaryExpensesTotal;

            if (selector == ".credit")
            {
                var minMonthlyFixedExpenses = $(".minMonthlyFixedExpenses").val().replace(/[^0-9]/g, "");
                var appliedNCROverride = 0;
                if (minMonthlyFixedExpenses > necessaryExpensesTotal)
                    appliedNCROverride = minMonthlyFixedExpenses - necessaryExpensesTotal;

                $(".appliedNCROverride").val(appliedNCROverride);


                allocableIncome = allocableIncome - appliedNCROverride;
            }

            var paymentObligationsTotal = SumValues(selector + ".paymentObligations");

            var discretionaryIncome = allocableIncome - paymentObligationsTotal;
            if (selector == ".tobe")
                discretionaryIncome = $(".creditAllocableIncome").val() - paymentObligationsTotal;


            var sahlObligationsTotal = SumValues(selector + ".sahlObligations");
            var otherExpensesTotal = SumValues(selector + ".otherExpenses");
            var surplusDeficit = discretionaryIncome - sahlObligationsTotal - otherExpensesTotal;

            // set the values of the total fields
            $(selector + "GrossIncome").val(grossIncome);
            $(selector + "NetIncome").val(netIncome);
            $(selector + "NecessaryExpensesTotal").val(necessaryExpensesTotal);
            $(selector + "AllocableIncome").val(allocableIncome);

            $(selector + "PaymentObligationsTotal").val(paymentObligationsTotal);
            $(selector + "DiscretionaryIncome").val(discretionaryIncome);
            $(selector + "OtherExpensesTotal").val(otherExpensesTotal);
            $(selector + "SurplusDeficit").val(surplusDeficit);

            // stress factor values
            if (selector == ".tobe") {
                var otherBond = $(".tobe.paymentObligations.bond").val().replace(/[^0-9]/g, "");
                var sahlBond = $(".tobe.sahlObligations.bond").val().replace(/[^0-9]/g, "");

                var stressFactorPercentIncrease = $('#<%=hidStressFactorPercentageIncrease.ClientID%>').val();
                var stressFactorValue = ((Number(otherBond) + Number(sahlBond)) * stressFactorPercentIncrease).toFixed();

                $(".stressFactorValue").val(stressFactorValue);
                $(".stressFactorValueApplied").val(surplusDeficit - stressFactorValue);
            }

            if (selector == ".credit" || selector == ".consolidate") {
                $(".summaryDebtToConsolidate").val($(".consolidatePaymentObligationsTotal").val());
            }

            // Summary Totals
            if (selector == ".credit")
            {
                $(".summaryDebtToConsolidate").val($(".consolidatePaymentObligationsTotal").val());             
                $(".summaryNetIncome").val(netIncome);
                $(".summaryTotalExpenses").val(necessaryExpensesTotal + appliedNCROverride + paymentObligationsTotal + sahlObligationsTotal + otherExpensesTotal);    
            }

            if (selector == ".credit" || selector == ".consolidate" || selector == '.tobe') {
                $(".summarySurplusDeficit").val(surplusDeficit);
                $(".summarySurplusPercent").val((($(".summarySurplusDeficit").val() / $(".summaryNetIncome").val()) * 100).toFixed() + "%");
            }
        };

        function SumValues(selector) {
            var sum = 0;
            $(selector).each(function () {
                var val = this.value.replace(/[^0-9]/g, ""); // remove any character that is not a number 
                sum += Number(val);
            });
            return sum;
        }

    </script>

    <div id="dialog-comments-display" title="Comments">
        <p>
            <span id="txtCommentsDisplay" runat="server"></span> 
        </p>
    </div>

    <div id="dialog-comments-update" title="Update Comments">
        <p>
            <textarea id="txtCommentsUpdate" runat="server" rows="10" cols="50"></textarea>
        </p>
    </div>

    <div id="divFields" style="text-align: center">
       
        <table id="tblFields" runat="server" style="width: 100%;" class="tableStandardMiddle">

            <tr>
                <td style="width: 5%" class="TitleText"></td>
                <td style="width: 15%"></td>
                <td style="width: 5%" class="TitleText">Per Client Monthly</td>
                <td style="width: 5%" class="TitleText">Credit Calculations</td>
                <td style="width: 5%" class="TitleText"></td>
                <td style="width: 15%"></td>
                <td style="width: 5%" class="TitleText">Monthly</td>
                <td style="width: 5%" class="TitleText">Debt to Consolidate</td>
                <td style="width: 5%" class="TitleText">As Is</td>
                <td style="width: 5%" class="TitleText">To Be</td>
                <td style="width: 5%"></td>
                <td style="width: 15%"></td>
                <td style="width: 5%"></td>
                <td style="width: 15%"></td>
            </tr>

            <tr>
                <td></td>
                <td style="background-color: #F2DCDB" class="TitleText" align="left">INCOME</td>
                <td>R</td>
                <td>R</td>
                <td></td>
                <td style="background-color: #F2DCDB" class="TitleText" align="left">PAYMENT OBLIGATIONS</td>
                <td>R</td>
                <td>R</td>
                <td>R</td>
                <td>R</td>
                <td colspan="4"><asp:HiddenField ID="hidScreenMode" runat="server" /><asp:HiddenField ID="hidAllCommentsEntered" runat="server" /></td>
            </tr>

            <tr>
                <td></td>
                <td style="background-color: #F2DCDB;" align="left">Basic Gross Salary/Drawings</td>
                <td style="background-color: #F2DCDB" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox ID="txtBasicGrossSalary_Drawings_Client" runat="server" DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" BackColor="#F2DCDB" TabIndex="1" CssClass="client income" /></td>
                <td style="background-color: #F2DCDB" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtBasicGrossSalary_Drawings_Credit" runat="server" BackColor="#F2DCDB" TabIndex="14"  CssClass="credit income"/></td>
                <td align="left">
                    <asp:Image ID="imgBasicGrossSalary_Drawings" CssClass="commentsIcon" runat="server" ImageUrl="~/Images/comments.png" ToolTip="Click for Comments" />
                    <asp:HiddenField ID="hidBasicGrossSalary_Drawings_Comments" runat="server" />
                </td>
                <td style="background-color: #F2DCDB" align="left">Other Bond/s</td>
                <td style="background-color: #F2DCDB" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtOtherBonds_Client" runat="server" BackColor="#F2DCDB" TabIndex="28" CssClass="client paymentObligations" /></td>
                <td style="background-color: #F2DCDB" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtOtherBonds_Consolidate" runat="server" BackColor="#F2DCDB" TabIndex="56" CssClass="consolidate paymentObligations" /></td>
                <td style="background-color: #F2DCDB" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtOtherBonds_Credit" runat="server" BackColor="#F2DCDB" TabIndex="42" CssClass="credit paymentObligations" /></td>
                <td style="background-color: #D9D9D9" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtOtherBonds_ToBe" runat="server" BackColor="#D9D9D9" CssClass="tobe paymentObligations bond" /></td>
                <td align="left">
                    <asp:Image ID="imgOtherBonds" runat="server" ImageUrl="~/Images/comments.png" ToolTip="Click for Comments" CssClass="commentsIcon" /><asp:HiddenField ID="hidOtherBonds_Comments" runat="server" />
                </td>
                <td colspan="3"></td>
            </tr>
            <tr>
                <td></td>
                <td style="background-color: #F2DCDB" align="left">Commission/Overtime</td>
                <td style="background-color: #F2DCDB" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtCommission_Overtime_Client" runat="server" BackColor="#F2DCDB" TabIndex="1"  CssClass="client income"/></td>
                <td style="background-color: #F2DCDB" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtCommission_Overtime_Credit" runat="server" BackColor="#F2DCDB" TabIndex="15"  CssClass="credit income"/></td>
                <td align="left">
                    <asp:Image ID="imgCommission_Overtime" runat="server" ImageUrl="~/Images/comments.png" ToolTip="Click for Comments" CssClass="commentsIcon" /><asp:HiddenField ID="hidCommission_Overtime_Comments" runat="server" />
                </td>
                <td style="background-color: #F2DCDB" align="left">Vehicle</td>
                <td style="background-color: #F2DCDB" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtVehicle_Client" runat="server" BackColor="#F2DCDB" TabIndex="29" CssClass="client paymentObligations" /></td>
                <td style="background-color: #F2DCDB" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtVehicle_Consolidate" runat="server" BackColor="#F2DCDB" TabIndex="57" CssClass="consolidate paymentObligations" /></td>
                <td style="background-color: #F2DCDB" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtVehicle_Credit" runat="server" BackColor="#F2DCDB" TabIndex="43" CssClass="credit paymentObligations" /></td>
                <td style="background-color: #D9D9D9" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtVehicle_ToBe" runat="server" BackColor="#D9D9D9" CssClass="tobe paymentObligations" /></td>
                <td align="left">
                    <asp:Image ID="imgVehicle" runat="server" ImageUrl="~/Images/comments.png" ToolTip="Click for Comments" CssClass="commentsIcon" /><asp:HiddenField ID="hidVehicle_Comments" runat="server" />
                </td>
                <td colspan="3"></td>
            </tr>
            <tr>
                <td></td>
                <td style="background-color: #F2DCDB" align="left">Net Rental</td>
                <td style="background-color: #F2DCDB" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtNet_Rental_Client" runat="server" BackColor="#F2DCDB" TabIndex="2" CssClass="client income" /></td>
                <td style="background-color: #F2DCDB" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtNet_Rental_Credit" runat="server" BackColor="#F2DCDB" TabIndex="16" CssClass="credit income" /></td>
                <td align="left">
                    <asp:Image ID="imgNet_Rental" runat="server" ImageUrl="~/Images/comments.png" ToolTip="Click for Comments" CssClass="commentsIcon" /><asp:HiddenField ID="hidNet_Rental_Comments" runat="server" />
                </td>
                <td style="background-color: #F2DCDB" align="left">Credit Card/s</td>
                <td style="background-color: #F2DCDB" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtCreditCards_Client" runat="server" BackColor="#F2DCDB" TabIndex="30" CssClass="client paymentObligations" /></td>
                <td style="background-color: #F2DCDB" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtCreditCards_Consolidate" runat="server" BackColor="#F2DCDB" TabIndex="58" CssClass="consolidate paymentObligations" /></td>
                <td style="background-color: #F2DCDB" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtCreditCards_Credit" runat="server" BackColor="#F2DCDB" TabIndex="44" CssClass="credit paymentObligations" /></td>
                <td style="background-color: #D9D9D9" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtCreditCards_ToBe" runat="server" BackColor="#D9D9D9" CssClass="tobe paymentObligations" /></td>
                <td align="left">
                    <asp:Image ID="imgCreditCards" runat="server" ImageUrl="~/Images/comments.png" ToolTip="Click for Comments" CssClass="commentsIcon" /><asp:HiddenField ID="hidCreditCards_Comments" runat="server" />
                </td>
                <td colspan="3"></td>
            </tr>

            <tr>
                <td></td>
                <td style="background-color: #F2DCDB" align="left">Investments</td>
                <td style="background-color: #F2DCDB" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtInvestments_Client" runat="server" BackColor="#F2DCDB" TabIndex="3" CssClass="client income" /></td>
                <td style="background-color: #F2DCDB" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtInvestments_Credit" runat="server" BackColor="#F2DCDB" TabIndex="17" CssClass="credit income" /></td>
                <td align="left">
                    <asp:Image ID="imgInvestments" runat="server" ImageUrl="~/Images/comments.png" ToolTip="Click for Comments" CssClass="commentsIcon" /><asp:HiddenField ID="hidInvestments_Comments" runat="server" />
                </td>
                <td style="background-color: #F2DCDB" align="left">Personal Loan/s</td>
                <td style="background-color: #F2DCDB" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtPersonalLoans_Client" runat="server" BackColor="#F2DCDB" TabIndex="31" CssClass="client paymentObligations" /></td>
                <td style="background-color: #F2DCDB" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtPersonalLoans_Consolidate" runat="server" BackColor="#F2DCDB" TabIndex="59" CssClass="consolidate paymentObligations" /></td>
                <td style="background-color: #F2DCDB" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtPersonalLoans_Credit" runat="server" BackColor="#F2DCDB" TabIndex="45" CssClass="credit paymentObligations" /></td>
                <td style="background-color: #D9D9D9" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtPersonalLoans_ToBe" runat="server" BackColor="#D9D9D9" CssClass="tobe paymentObligations" /></td>
                <td align="left">
                    <asp:Image ID="imgPersonalLoans" runat="server" ImageUrl="~/Images/comments.png" ToolTip="Click for Comments" CssClass="commentsIcon" /><asp:HiddenField ID="hidPersonalLoans_Comments" runat="server" />
                </td>
                <td colspan="3"></td>
            </tr>

            <tr>
                <td></td>
                <td style="background-color: #F2DCDB" align="left">Other Income 1</td>
                <td style="background-color: #F2DCDB" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtOtherIncome1_Client" runat="server" BackColor="#F2DCDB" TabIndex="4" CssClass="client income" /></td>
                <td style="background-color: #F2DCDB" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtOtherIncome1_Credit" runat="server" BackColor="#F2DCDB" TabIndex="18" CssClass="credit income" /></td>
                <td align="left">
                    <asp:Image ID="imgOtherIncome1" runat="server" ImageUrl="~/Images/comments.png" ToolTip="Click for Comments" CssClass="commentsIcon" /><asp:HiddenField ID="hidOtherIncome1_Comments" runat="server" />
                </td>
                <td style="background-color: #F2DCDB" align="left">Retail Accounts</td>
                <td style="background-color: #F2DCDB" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtRetailAccounts_Client" runat="server" BackColor="#F2DCDB" TabIndex="32" CssClass="client paymentObligations" /></td>
                <td style="background-color: #F2DCDB" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtRetailAccounts_Consolidate" runat="server" BackColor="#F2DCDB" TabIndex="60" CssClass="consolidate paymentObligations" /></td>
                <td style="background-color: #F2DCDB" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtRetailAccounts_Credit" runat="server" BackColor="#F2DCDB" TabIndex="46" CssClass="credit paymentObligations" /></td>
                <td style="background-color: #D9D9D9" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtRetailAccounts_ToBe" runat="server" BackColor="#D9D9D9" CssClass="tobe paymentObligations" /></td>
                <td align="left">
                    <asp:Image ID="imgRetailAccounts" runat="server" ImageUrl="~/Images/comments.png" ToolTip="Click for Comments" CssClass="commentsIcon" /><asp:HiddenField ID="hidRetailAccounts_Comments" runat="server" />
                </td>
                <td colspan="3"></td>
            </tr>

            <tr>
                <td></td>
                <td style="background-color: #F2DCDB" align="left">Other Income 2</td>
                <td style="background-color: #F2DCDB" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtOtherIncome2_Client" runat="server" BackColor="#F2DCDB" TabIndex="5" CssClass="client income" /></td>
                <td style="background-color: #F2DCDB" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtOtherIncome2_Credit" runat="server" BackColor="#F2DCDB" TabIndex="19" CssClass="credit income" /></td>
                <td align="left">
                    <asp:Image ID="imgOtherIncome2" runat="server" ImageUrl="~/Images/comments.png" ToolTip="Click for Comments" CssClass="commentsIcon" /><asp:HiddenField ID="hidOtherIncome2_Comments" runat="server" />
                </td>
                <td style="background-color: #F2DCDB" align="left">Other Debt Expenses</td>
                <td style="background-color: #F2DCDB" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtOtherDebtExpenses_Client" runat="server" BackColor="#F2DCDB" TabIndex="33" CssClass="client paymentObligations" /></td>
                <td style="background-color: #F2DCDB" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtOtherDebtExpenses_Consolidate" runat="server" BackColor="#F2DCDB" TabIndex="61" CssClass="consolidate paymentObligations" /></td>
                <td style="background-color: #F2DCDB" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtOtherDebtExpenses_Credit" runat="server" BackColor="#F2DCDB" TabIndex="47" CssClass="credit paymentObligations" /></td>
                <td style="background-color: #D9D9D9" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtOtherDebtExpenses_ToBe" runat="server" BackColor="#D9D9D9" CssClass="tobe paymentObligations" /></td>
                <td align="left">
                    <asp:Image ID="imgOtherDebtExpenses" runat="server" ImageUrl="~/Images/comments.png" ToolTip="Click for Comments" CssClass="commentsIcon" /><asp:HiddenField ID="hidOtherDebtExpenses_Comments" runat="server" />
                </td>
                <td colspan="3"></td>
            </tr>

            <tr>
                <td></td>
                <td style="background-color: #D9D9D9" class="TitleText" align="left">Gross Income</td>
                <td style="background-color: #D9D9D9" class="TitleText" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtGrossIncome_Client_Total" runat="server" Font-Bold="True" BackColor="#D9D9D9" CssClass="clientGrossIncome" /></td>
                <td style="background-color: #D9D9D9" class="TitleText" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtGrossIncome_Credit_Total" runat="server" Font-Bold="True" BackColor="#D9D9D9" CssClass="creditGrossIncome" /></td>
                <td></td>
                <td style="background-color: #D9D9D9" class="TitleText" align="left">Monthly Total</td>
                <td style="background-color: #D9D9D9" class="TitleText" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtPayment_Client_Total" runat="server" Font-Bold="True" BackColor="#D9D9D9" CssClass="clientPaymentObligationsTotal" /></td>
                <td style="background-color: #D9D9D9" class="TitleText" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtPayment_Consolidate_Total" runat="server" Font-Bold="True" BackColor="#D9D9D9" CssClass="consolidatePaymentObligationsTotal" /></td>
                <td style="background-color: #D9D9D9" class="TitleText" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtPayment_Credit_Total" runat="server" Font-Bold="True" BackColor="#D9D9D9" CssClass="creditPaymentObligationsTotal" /></td>
                <td style="background-color: #D9D9D9" class="TitleText" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtPayment_ToBe_Total" runat="server" Font-Bold="True" BackColor="#D9D9D9" CssClass="tobePaymentObligationsTotal" /></td>
                <td colspan="4"></td>
            </tr>
            <tr>
                <td></td>
                <td style="background-color: #F2DCDB" align="left">Payroll Deductions</td>
                <td style="background-color: #F2DCDB" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtPayrollDeductions_Client" runat="server" BackColor="#F2DCDB" TabIndex="6" CssClass="client payrollDeductions" /></td>
                <td style="background-color: #F2DCDB" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtPayrollDeductions_Credit" runat="server" BackColor="#F2DCDB" TabIndex="20" CssClass="credit payrollDeductions" /></td>
                <td align="left">
                    <asp:Image ID="imgPayrollDeductions" runat="server" ImageUrl="~/Images/comments.png" ToolTip="Click for Comments" CssClass="commentsIcon" /><asp:HiddenField ID="hidPayrollDeductions_Comments" runat="server" />
                </td>
                <td style="background-color: #CCC0DA" class="TitleText" align="left">DISCRETIONARY INCOME</td>
                <td style="background-color: #CCC0DA" class="TitleText" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtDiscretionaryIncome_Client_Total" runat="server" Font-Bold="True" BackColor="#CCC0DA" CssClass="clientDiscretionaryIncome" /></td>
                <td></td>
                <td style="background-color: #CCC0DA" class="TitleText" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtDiscretionaryIncome_Credit_Total" runat="server" Font-Bold="True" BackColor="#CCC0DA" CssClass="creditDiscretionaryIncome" /></td>
                <td style="background-color: #CCC0DA" class="TitleText" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtDiscretionaryIncome_ToBe_Total" runat="server" Font-Bold="True" BackColor="#CCC0DA" CssClass="tobeDiscretionaryIncome" /></td>
                <td colspan="6"></td>
            </tr>
            <tr>
                <td></td>
                <td style="background-color: #D9D9D9" class="TitleText" align="left">Net Income</td>
                <td style="background-color: #D9D9D9" class="TitleText" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtNetIncome_Client_Total" runat="server" Font-Bold="True" BackColor="#D9D9D9" CssClass="clientNetIncome" /></td>
                <td style="background-color: #D9D9D9" class="TitleText" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtNetIncome_Credit_Total" runat="server" Font-Bold="True" BackColor="#D9D9D9" CssClass="creditNetIncome" /></td>
                <td></td>
                <td style="background-color: #CCCCFF" align="left">SAHL Bond</td>
                <td style="background-color: #CCCCFF" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtSAHLBond_Client" runat="server" BackColor="#CCCCFF" TabIndex="34" CssClass="client sahlObligations" /></td>
                <td></td>
                <td style="background-color: #CCCCFF" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtSAHLBond_Credit" runat="server" BackColor="#CCCCFF" TabIndex="48" CssClass="credit sahlObligations" /></td>
                <td style="background-color: #CCCCFF" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtSAHLBond_ToBe" runat="server" BackColor="#CCCCFF" CssClass="tobe sahlObligations bond" /></td>
                <td align="left">
                    <asp:Image ID="imgSAHLBond" runat="server" ImageUrl="~/Images/comments.png" ToolTip="Click for Comments" CssClass="commentsIcon" /><asp:HiddenField ID="hidSAHLBond_Comments" runat="server" />
                </td>
                <td colspan="3"></td>
            </tr>
            <tr>
                <td></td>
                <td style="background-color: #FFFFCC" class="TitleText" align="left">NECESSARY EXPENSES</td>
                <td></td>
                <td></td>
                <td></td>
                <td style="background-color: #CCCCFF" align="left">HOC</td>
                <td style="background-color: #CCCCFF" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtHOC_Client" runat="server" BackColor="#CCCCFF" TabIndex="35" CssClass="client sahlObligations" /></td>
                <td></td>
                <td style="background-color: #CCCCFF" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtHOC_Credit" runat="server" BackColor="#CCCCFF" TabIndex="49" CssClass="credit sahlObligations" /></td>
                <td style="background-color: #CCCCFF" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtHOC_ToBe" runat="server" BackColor="#CCCCFF" CssClass="tobe sahlObligations hoc" /></td>
                <td align="left">
                    <asp:Image ID="imgHOC" runat="server" ImageUrl="~/Images/comments.png" ToolTip="Click for Comments" CssClass="commentsIcon" /><asp:HiddenField ID="hidHOC_Comments" runat="server" />
                </td>
                <td colspan="3"></td>
            </tr>

            <tr>
                <td></td>
                <td style="background-color: #FFFFCC" align="left">Accommodation/Rental</td>
                <td style="background-color: #FFFFCC" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtAccomodation_Client" runat="server" BackColor="#FFFFCC" TabIndex="7" CssClass="client necessaryExpenses" /></td>
                <td style="background-color: #FFFFCC" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtAccomodation_Credit" runat="server" BackColor="#FFFFCC" TabIndex="21" CssClass="credit necessaryExpenses" /></td>
                <td align="left">
                    <asp:Image ID="imgAccomodation" runat="server" ImageUrl="~/Images/comments.png" ToolTip="Click for Comments" CssClass="commentsIcon" /><asp:HiddenField ID="hidAccomodation_Comments" runat="server" />
                </td>
                                <td style="background-color: #FFFFCC" class="TitleText" align="left">OTHER EXPENSES</td>
                <td colspan="8"></td>
            </tr>
            <tr>
                <td></td>
                <td style="background-color: #FFFFCC" align="left">Transport</td>
                <td style="background-color: #FFFFCC" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtTransport_Client" runat="server" BackColor="#FFFFCC" TabIndex="8" CssClass="client necessaryExpenses" /></td>
                <td style="background-color: #FFFFCC" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtTransport_Credit" runat="server" BackColor="#FFFFCC" TabIndex="22" CssClass="credit necessaryExpenses" /></td>
                <td align="left">
                    <asp:Image ID="imgTransport" runat="server" ImageUrl="~/Images/comments.png" ToolTip="Click for Comments" CssClass="commentsIcon" /><asp:HiddenField ID="hidTransport_Comments" runat="server" />
                </td>
                <td style="background-color: #FFFFCC" align="left">Domestic Salary</td>
                <td style="background-color: #FFFFCC" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtDomesticSalary_Client" runat="server" BackColor="#FFFFCC" TabIndex="36" CssClass="client otherExpenses" /></td>
                <td></td>
                <td style="background-color: #FFFFCC" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtDomesticSalary_Credit" runat="server" BackColor="#FFFFCC" TabIndex="50" CssClass="credit otherExpenses" /></td>
                <td style="background-color: #D9D9D9" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtDomesticSalary_ToBe" runat="server" BackColor="#D9D9D9" CssClass="tobe otherExpenses" /></td>
                <td align="left">
                    <asp:Image ID="imgDomesticSalary" runat="server" ImageUrl="~/Images/comments.png" ToolTip="Click for Comments" CssClass="commentsIcon" /><asp:HiddenField ID="hidDomesticSalary_Comments" runat="server" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td style="background-color: #FFFFCC" align="left">Food</td>
                <td style="background-color: #FFFFCC" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtFood_Client" runat="server" BackColor="#FFFFCC" TabIndex="9" CssClass="client necessaryExpenses" /></td>
                <td style="background-color: #FFFFCC" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtFood_Credit" runat="server" BackColor="#FFFFCC" TabIndex="23" CssClass="credit necessaryExpenses" /></td>
                <td align="left">
                    <asp:Image ID="imgFood" runat="server" ImageUrl="~/Images/comments.png" ToolTip="Click for Comments" CssClass="commentsIcon" /><asp:HiddenField ID="hidFood_Comments" runat="server" />
                </td>
                <td style="background-color: #FFFFCC" align="left">Insurance Policies</td>
                <td style="background-color: #FFFFCC" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtInsurancePolicies_Client" runat="server" BackColor="#FFFFCC" TabIndex="37" CssClass="client otherExpenses" /></td>
                <td></td>
                <td style="background-color: #FFFFCC" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtInsurancePolicies_Credit" runat="server" BackColor="#FFFFCC" TabIndex="51" CssClass="credit otherExpenses" /></td>
                <td style="background-color: #D9D9D9" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtInsurancePolicies_ToBe" runat="server" BackColor="#D9D9D9" CssClass="tobe otherExpenses" /></td>
                <td align="left">
                    <asp:Image ID="imgInsurancePolicies" runat="server" ImageUrl="~/Images/comments.png" ToolTip="Click for Comments" CssClass="commentsIcon" /><asp:HiddenField ID="hidInsurancePolicies_Comments" runat="server" />
                </td>
                <td style="background-color: #FDE9D9" align="center" class="TitleText">SUMMARY</td>
                <td style="background-color: #FDE9D9"></td>
                <td></td>
            </tr>
            <tr>
                <td></td>
                <td style="background-color: #FFFFCC" align="left">Education</td>
                <td style="background-color: #FFFFCC" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtEducation_Client" runat="server" BackColor="#FFFFCC" TabIndex="10" CssClass="client necessaryExpenses" /></td>
                <td style="background-color: #FFFFCC" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtEducation_Credit" runat="server" BackColor="#FFFFCC" TabIndex="24" CssClass="credit necessaryExpenses" /></td>
                <td align="left">
                    <asp:Image ID="imgEducation" runat="server" ImageUrl="~/Images/comments.png" ToolTip="Click for Comments" CssClass="commentsIcon" /><asp:HiddenField ID="hidEducation_Comments" runat="server" />
                </td>
                                <td style="background-color: #FFFFCC" align="left">Committed Savings</td>
                <td style="background-color: #FFFFCC" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtCommittedSavings_Client" runat="server" BackColor="#FFFFCC" TabIndex="38" CssClass="client otherExpenses" /></td>
                <td></td>
                <td style="background-color: #FFFFCC" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtCommittedSavings_Credit" runat="server" BackColor="#FFFFCC" TabIndex="52" CssClass="credit otherExpenses" /></td>
                <td style="background-color: #D9D9D9" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtCommittedSavings_ToBe" runat="server" BackColor="#D9D9D9" CssClass="tobe otherExpenses" /></td>
                <td align="left">
                    <asp:Image ID="imgCommittedSavings" runat="server" ImageUrl="~/Images/comments.png" ToolTip="Click for Comments" CssClass="commentsIcon" /><asp:HiddenField ID="hidCommittedSavings_Comments" runat="server" />
                </td>
                <td style="background-color: #FDE9D9"></td>
                <td style="background-color: #FDE9D9" align="center" class="TitleText">Rands</td>
                <td></td>
            </tr>
            <tr>
                <td></td>
                <td style="background-color: #FFFFCC" align="left">Medical</td>
                <td style="background-color: #FFFFCC" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtMedical_Client" runat="server" BackColor="#FFFFCC" TabIndex="11" CssClass="client necessaryExpenses" /></td>
                <td style="background-color: #FFFFCC" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtMedical_Credit" runat="server" BackColor="#FFFFCC" TabIndex="25" CssClass="credit necessaryExpenses" /></td>
                <td align="left">
                    <asp:Image ID="imgMedical" runat="server" ImageUrl="~/Images/comments.png" ToolTip="Click for Comments" CssClass="commentsIcon" /><asp:HiddenField ID="hidMedical_Comments" runat="server" />
                </td>
                <td style="background-color: #FFFFCC" align="left">Security</td>
                <td style="background-color: #FFFFCC" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtSecurity_Client" runat="server" BackColor="#FFFFCC" TabIndex="39" CssClass="client otherExpenses" /></td>
                <td></td>
                <td style="background-color: #FFFFCC" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtSecurity_Credit" runat="server" BackColor="#FFFFCC" TabIndex="53" CssClass="credit otherExpenses" /></td>
                <td style="background-color: #D9D9D9" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtSecurity_ToBe" runat="server" BackColor="#D9D9D9" CssClass="tobe otherExpenses" /></td>
                <td align="left">
                    <asp:Image ID="imgSecurity" runat="server" ImageUrl="~/Images/comments.png" ToolTip="Click for Comments" CssClass="commentsIcon" /><asp:HiddenField ID="hidSecurity_Comments" runat="server" />
                </td>
                <td style="background-color: #FDE9D9" align="left" class="titleText">Debt to Consolidate</td>
                <td style="background-color: #FDE9D9">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtSummaryDebtToConsolidate" runat="server" BackColor="#D9D9D9" CssClass="summaryDebtToConsolidate" /></td>
                <td></td>
            </tr>
            <tr>
                <td></td>
                <td style="background-color: #FFFFCC" align="left">Utilities</td>
                <td style="background-color: #FFFFCC" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtUtilities_Client" runat="server" BackColor="#FFFFCC" TabIndex="12" CssClass="client necessaryExpenses" /></td>
                <td style="background-color: #FFFFCC" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtUtilities_Credit" runat="server" BackColor="#FFFFCC" TabIndex="26" CssClass="credit necessaryExpenses" /></td>
                <td align="left">
                    <asp:Image ID="imgUtilities" runat="server" ImageUrl="~/Images/comments.png" ToolTip="Click for Comments" CssClass="commentsIcon" /><asp:HiddenField ID="hidUtilities_Comments" runat="server" />
                </td>
                <td style="background-color: #FFFFCC" align="left">Telephone/T.V</td>
                <td style="background-color: #FFFFCC" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtTelephoneTV_Client" runat="server" BackColor="#FFFFCC" TabIndex="40" CssClass="client otherExpenses" /></td>
                <td></td>
                <td style="background-color: #FFFFCC" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtTelephoneTV_Credit" runat="server" BackColor="#FFFFCC" TabIndex="54" CssClass="credit otherExpenses" /></td>
                <td style="background-color: #D9D9D9" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtTelephoneTV_ToBe" runat="server" BackColor="#D9D9D9" CssClass="tobe otherExpenses" /></td>
                <td align="left">
                    <asp:Image ID="imgTelephoneTV" runat="server" ImageUrl="~/Images/comments.png" ToolTip="Click for Comments" CssClass="commentsIcon" /><asp:HiddenField ID="hidTelephoneTV_Comments" runat="server" />
                </td>
                <td style="background-color: #FDE9D9" colspan="2"></td>
                <td></td>
            </tr>
            <tr>
                <td></td>
                <td style="background-color: #FFFFCC" align="left">Child Support</td>
                <td style="background-color: #FFFFCC" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtChildSupport_Client" runat="server" BackColor="#FFFFCC" TabIndex="13" CssClass="client necessaryExpenses" /></td>
                <td style="background-color: #FFFFCC" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtChildSupport_Credit" runat="server" BackColor="#FFFFCC" TabIndex="27" CssClass="credit necessaryExpenses" /></td>
                <td align="left">
                    <asp:Image ID="imgChildSupport" runat="server" ImageUrl="~/Images/comments.png" ToolTip="Click for Comments" CssClass="commentsIcon" /><asp:HiddenField ID="hidChildSupport_Comments" runat="server" />
                </td>
                <td style="background-color: #FFFFCC" align="left">Other</td>
                <td style="background-color: #FFFFCC" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtOther_Client" runat="server" BackColor="#FFFFCC" TabIndex="41" CssClass="client otherExpenses" /></td>
                <td></td>
                <td style="background-color: #FFFFCC" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtOther_Credit" runat="server" BackColor="#FFFFCC" TabIndex="55" CssClass="credit otherExpenses" /></td>
                <td style="background-color: #D9D9D9" class="cellDisplay" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtOther_ToBe" runat="server" BackColor="#D9D9D9" CssClass="tobe otherExpenses" /></td>
                <td align="left">
                    <asp:Image ID="imgOther" runat="server" ImageUrl="~/Images/comments.png" ToolTip="Click for Comments" CssClass="commentsIcon" /><asp:HiddenField ID="hidOther_Comments" runat="server" />
                </td>
                <td style="background-color: #FDE9D9" align="left" class="titleText">Net Income</td>
                <td style="background-color: #FDE9D9">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtSummaryNetIncome" runat="server" BackColor="#D8E4BC" CssClass="summaryNetIncome" /></td>
                <td></td>
            </tr>
            <tr>
                <td></td>
                <td style="background-color: #D9D9D9" class="TitleText" align="left">Monthly Total</td>
                <td style="background-color: #D9D9D9" class="TitleText" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtMonthlyNecessaryExpenses_Client_Total" runat="server" Font-Bold="True" BackColor="#D9D9D9" CssClass="clientNecessaryExpensesTotal" /></td>
                <td style="background-color: #D9D9D9" class="TitleText" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtMonthlyNecessaryExpenses_Credit_Total" runat="server" Font-Bold="True" BackColor="#D9D9D9" CssClass="creditNecessaryExpensesTotal" /></td>
                <td></td>
                <td style="background-color: #D9D9D9" class="TitleText" align="left">Monthly Total</td>
                <td style="background-color: #D9D9D9" class="TitleText" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtOther_Client_Total" runat="server" Font-Bold="True" BackColor="#D9D9D9" CssClass="clientOtherExpensesTotal" /></td>
                <td></td>
                <td style="background-color: #D9D9D9" class="TitleText" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtOther_Credit_Total" runat="server" Font-Bold="True" BackColor="#D9D9D9" CssClass="creditOtherExpensesTotal" /></td>
                <td style="background-color: #D9D9D9" class="TitleText" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtOther_ToBe_Total" runat="server" Font-Bold="True" BackColor="#D9D9D9" CssClass="tobeOtherExpensesTotal" /></td>
                <td></td>
                <td style="background-color: #FDE9D9" align="left" class="titleText">Total Expenses</td>
                <td style="background-color: #FDE9D9">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtSummaryTotalExpenses" runat="server" BackColor="#B8CCE4" CssClass="summaryTotalExpenses" /></td>
                <td></td>
            </tr>
            <tr>
                <td></td>
                <td style="background-color: #F79646" class="TitleText" align="left">Applied NCR Override</td>
                <td style="background-color: #F79646"></td>
                <td style="background-color: #F79646" class="TitleText" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtAppliedNCROverride_Credit_Total" runat="server" BackColor="#F79646" CssClass="appliedNCROverride" /></td>
                <td></td>
                <td style="background-color: #FFFF00" class="TitleText" align="left">Surplus/Deficit</td>
                <td style="background-color: #FFFF00" class="TitleText" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtSurplusDeficit_Client_Total" runat="server" Font-Bold="True" BackColor="#FFFF00" CssClass="clientSurplusDeficit" /></td>
                <td></td>
                <td style="background-color: #FFFF00" class="TitleText" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtSurplusDeficit_Credit_Total" runat="server" Font-Bold="True" BackColor="#FFFF00" CssClass="creditSurplusDeficit" /></td>
                <td style="background-color: #FFFF00" class="TitleText" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtSurplusDeficit_ToBe_Total" runat="server" Font-Bold="True" BackColor="#FFFF00" CssClass="tobeSurplusDeficit" /></td>
                <td></td>
                <td style="background-color: #FDE9D9" align="left" class="titleText">Surplus / Deficit</td>
                <td style="background-color: #FDE9D9">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtSummarySurpusDeficit" runat="server" BackColor="#FFFF00" CssClass="summarySurplusDeficit" /></td>
                <td></td>
            </tr>
            <tr>
                <td></td>
                <td style="background-color: #D9D9D9" class="TitleText" align="left">ALLOCABLE INCOME</td>
                <td style="background-color: #D9D9D9" class="TitleText" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtAllocableIncome_Client_Total" runat="server" Font-Bold="True" BackColor="#D9D9D9" CssClass="clientAllocableIncome" /></td>
                <td style="background-color: #D9D9D9" class="TitleText" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtAllocableIncome_Credit_Total" runat="server" Font-Bold="True" BackColor="#D9D9D9" CssClass="creditAllocableIncome" /></td>
                <td></td>
                <td style="background-color: #E6B8B7" class="TitleText" align="left">After Stress Factor Applied</td>
                <td style="background-color: #E6B8B7">
                    <SAHL:SAHLLabel Width="35px" ID="lblStressFactorPercentage" runat="server" />
                    <SAHL:SAHLDropDownList ID="ddlStressFactorPercentage" runat="server" Width="75px" BackColor="#E5D5AC" onchange="StressFactorPercentage_SelectedIndexChanged(this)"/>
                    <SAHL:SAHLDropDownList ID="ddlStressFactorPercentageLookup" runat="server" CssClass="ui-helper-hidden"/>
                </td>
                <td style="background-color: #E6B8B7" class="TitleText" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtStressFactorValue" runat="server" Font-Bold="True" BackColor="#E6B8B7" CssClass="stressFactorValue" /></td>
                <td style="background-color: #E6B8B7">
                    <input type="hidden" id="hidStressFactorPercentageIncrease" runat="server" />
                    <input type="hidden" id="hidStressFactorKey" runat="server" />
                </td>
                <td style="background-color: #E6B8B7" class="TitleText" align="right">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtAfterStressFactorApplied" runat="server" Font-Bold="True" BackColor="#E6B8B7" CssClass="stressFactorValueApplied" /></td>
                <td></td>
                <td style="background-color: #FDE9D9" align="left" class="titleText">Surplus to Net Household Income %</td>
                <td style="background-color: #FDE9D9">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtSummarySurplusPercent" runat="server" BackColor="#D9D9D9" CssClass="summarySurplusPercent" /></td>
                <td></td>
            </tr>
            <tr>
                <td colspan="14">&nbsp;</td>
            </tr>

            <tr>
                <td></td>
                <td class="TitleText" align="left" style="background-color: #DCE6F1">NCR Assessment Guideline</td>
                <td style="background-color: #DCE6F1"></td>
                <td style="background-color: #DCE6F1"></td>
                <td></td>
                <td class="TitleText" align="left" style="background-color: #DCE6F1">Contributing Applicants :</td>
                <td align="left" style="background-color: #DCE6F1">
                    <SAHL:SAHLLabel Width="35px" ID="lblContributingApplicants" runat="server" />
                </td>
                <td colspan="7"></td>
            </tr>
            <tr>
                <td></td>
                <td class="LabelText" align="left" style="background-color: #DCE6F1">Min Monthly Fixed Expenses</td>
                <td style="background-color: #F79646">
                    <SAHL:SAHLTextBox DisplayInputType="Number" MaxLength="8" TextAlign="Right" Width="70px" ID="txtMinMonthlyFixedExpenses" runat="server" BackColor="#F79646" CssClass="minMonthlyFixedExpenses" /></td>
                <td style="background-color: #DCE6F1"></td>
                <td></td>
                <td class="TitleText" align="left" style="background-color: #DCE6F1">Household Dependants :</td>
                <td align="left" style="background-color: #DCE6F1">
                    <SAHL:SAHLLabel Width="35px" ID="lblHouseholdDependants" runat="server" />
                </td>
                <td colspan="3" align="right">
                    <SAHL:SAHLButton ID="btnSubmit" runat="server" Text="Save" OnClick="btnSubmit_Click" />
                    <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="False" />
                </td>
                <td colspan="4"></td>
            </tr>
        </table>

        <div style="left: 697px; position: absolute; top: 19px; border: 1px solid black; width:400px; background-color: white; cursor: move;" id="valSummary" class="SAHLValidationSummary">
	        <div id="valSummary_Header" class="SAHLValidationSummaryHeader backgroundDark" style="padding: 4px; text-align: left">
		        <img style="position: relative; left: 5px;" src="../../images/ValidatorInvalid.gif" alt=""/>
                <span style="margin-left: 10px; font-weight: bold;">Validation failed with errors</span>
	        </div>
	        <div id="valSummary_Body" class="SAHLValidationSummaryBody" style="background-color: white;text-align:left">
		        <div></div>
		        <div>
			        <ol id="errorList"></ol>
		        </div>
	        </div>
        </div>
    </div>
</asp:Content>
