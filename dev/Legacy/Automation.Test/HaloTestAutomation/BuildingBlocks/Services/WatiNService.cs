using BuildingBlocks.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using WatiN.Core;
using WatiN.Core.DialogHandlers;
using WatiN.Core.UtilityClasses;

namespace BuildingBlocks.Services
{
    public class WatiNService : IWatiNService
    {
        private ICommonService commonService;

        public WatiNService(ICommonService commonService)
        {
            this.commonService = commonService;
        }

        /// <summary>
        ///This will get all the grids/tables on the provided view.
        /// </summary>
        /// <param name="objectMapsPresenterClass">This will be the instance of the class in the ObjectMaps Library. i.e. ObjectMaps.AssignAdminControls class </param>
        public List<Table> GetTables(object objectMapsPresenterClass)
        {
            List<Table> tables = new List<Table>();
            foreach (PropertyInfo property in objectMapsPresenterClass.GetType().GetProperties())
            {
                if (property.GetGetMethod().ReturnType == typeof(Table))
                {
                    MethodInfo propertyGetMethod = property.GetGetMethod();
                    Table table = propertyGetMethod.Invoke(objectMapsPresenterClass, null) as Table;
                    if (table != null)
                        if (table.Exists)
                            tables.Add(table);
                }
            }
            return tables;
        }

        /// <summary>
        /// This will check all the checkboxes on the specified view, except the exclusions.
        /// </summary>
        /// <param name="b">Instance of the TestBrowser</param>
        /// <param name="checkBoxExclusions">Array of checkboxes that should be excluded</param>
        public void GenericCheckAllCheckBoxes(TestBrowser b, params CheckBox[] checkBoxExclusions)
        {
            Frame f = b.Frame(Find.ByIndex(0));
            if (checkBoxExclusions.Length == 0)
            {
                foreach (var checkbox in f.CheckBoxes)
                {
                    if (checkbox.Enabled && checkbox.Exists)
                    {
                        checkbox.Checked = true;
                    }
                }
            }
            else
            {
                foreach (var checkbox in f.CheckBoxes)
                {
                    foreach (var checkbox2 in checkBoxExclusions)
                    {
                        if (checkbox.Id != checkbox2.Id && checkbox.Enabled && checkbox.Exists)
                        {
                            checkbox.Checked = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        ///This will uncheck all the checkboxes on the specified view.
        /// </summary>
        /// <param name="b">Instance of the TestBrowser</param>
        public void GenericUncheckAllCheckBoxes(TestBrowser b)
        {
            Frame f = b.Frame(Find.ByIndex(0));
            if (f.CheckBoxes.Count > 0)
            {
                foreach (var checkbox in f.CheckBoxes)
                {
                    if (checkbox.Enabled && checkbox.Exists)
                    {
                        checkbox.Checked = false;
                    }
                }
            }
        }

        /// <summary>
        /// Closes all open IE TestBrowser windows
        /// </summary>
        public void CloseAllOpenIEBrowsers()
        {
            IECollection browserCollection = IE.InternetExplorers();
            foreach (IE ie in browserCollection)
            {
                ie.Dispose();
            }
        }

        /// <summary>
        /// Set WatiN timeout settings with one common value
        /// <param name="waitTimeOut">WatiN.Core.settings.AttachToIETimeOut, WatiN.Core.settings.WaitForCompleteTimeOutt & WatiN.Core.settings.WaitUntilExistsTimeOut</param>
        /// </summary>
        public void SetWatiNTimeouts(int waitTimeOut)
        {
            if (waitTimeOut != -1)
            {
                Settings.AttachToBrowserTimeOut = waitTimeOut;
                Settings.WaitForCompleteTimeOut = waitTimeOut;
                Settings.WaitUntilExistsTimeOut = waitTimeOut;
            }
        }

        /// <summary>
        /// Executes a shell command synchronously.
        /// </summary>
        /// <param name="command">string command</param>
        /// <returns>string, as output of the command.</returns>
        private void ExecuteCommandSync(object command)
        {
            // create the ProcessStartInfo using "cmd" as the program to be run,
            // and "/c " as the parameters.
            // Incidentally, /c tells cmd that we want it to execute the command that follows,
            // and then exit.
            var procStartInfo =
                new ProcessStartInfo("cmd", "/c " + command)
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

            // The following commands are needed to redirect the standard output.
            // This means that it will be redirected to the Process.StandardOutput StreamReader.
            // Do not create the black window.
            // Now we create a process, assign its ProcessStartInfo and start it
            var proc = new Process { StartInfo = procStartInfo };
            proc.Start();
            // Get the output into a string
            string result = proc.StandardOutput.ReadToEnd();
        }

        ///<summary>
        /// Stops all running IE Processes
        ///</summary>
        public void KillAllIEProcesses()
        {
            ExecuteCommandSync("taskkill /F /IM iexplore.exe");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public string HandleFileDownload(TestBrowser browser, Element element)
        {
            string dialogMessage = "No Message";
            //sets up the dialogue handler
            browser.DialogWatcher.CloseUnhandledDialogs = false;
            FileDownloadHandler confirm = new FileDownloadHandler(FileDownloadOptionEnum.Open);
            browser.DialogWatcher.Add(confirm);
            //clicks the button
            element.ClickNoWait();
            //handles the dialogue box and clicks the OK button
            SimpleTimer timer = new SimpleTimer(TimeSpan.FromSeconds(30));
            while (!timer.Elapsed)
            {
                confirm.WaitUntilDownloadCompleted(5);
            }
            return dialogMessage;
        }

        public void HandleConfirmationPopup(Element element)
        {
            var confirmDialogue = new ConfirmDialogHandler();
            using (new UseDialogOnce(element.DomContainer.DialogWatcher, confirmDialogue))
            {
                element.ClickNoWait();
                var timer = new SimpleTimer(TimeSpan.FromSeconds(30));
                while (!timer.Elapsed)
                {
                    if (confirmDialogue.Exists())
                    {
                        confirmDialogue.OKButton.Click();
                        break;
                    }
                }
            }
            element.DomContainer.WaitForComplete();
        }

        public void CancelConfirmationPopup(Element button)
        {
            var confirmDialogue = new ConfirmDialogHandler();
            using (new UseDialogOnce(button.DomContainer.DialogWatcher, confirmDialogue))
            {
                button.ClickNoWait();
                confirmDialogue.WaitUntilExists(10);
                confirmDialogue.CancelButton.Click();
            }
            button.DomContainer.WaitForComplete();
        }

        public string HandleConfirmationPopupAndReturnConfirmationMessage(Element button)
        {
            string message = string.Empty;
            var confirmDialogue = new ConfirmDialogHandler();
            using (new UseDialogOnce(button.DomContainer.DialogWatcher, confirmDialogue))
            {
                button.ClickNoWait();
                confirmDialogue.WaitUntilExists(10);
                message = confirmDialogue.Message;
                confirmDialogue.OKButton.Click();
            }
            button.DomContainer.WaitForComplete();
            return message;
        }



        /// <summary>
        /// Find a list of rows in the provided table
        /// </summary>
        /// <param name="cellTextExpression"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<TableRow> FindRows(string cellTextExpression, Table table)
        {
            var rows = new List<TableRow>();
            foreach (var row in table.TableRows)
            {
                foreach (var cell in row.TableCells)
                    if (cell.Text != null && cell.Text.Equals(cellTextExpression) && !rows.Contains(row))
                        rows.Add(row);
            }
            return rows;
        }

        /// <summary>
        /// This will check/uncheck all checkboxes in a table
        /// </summary>
        /// <param name="isChecked"></param>
        /// <param name="table"></param>
        public void CheckTableCheckboxes(bool isChecked, Table table)
        {
            foreach (var checkbox in table.CheckBoxes)
                checkbox.Checked = isChecked;
        }

        /// <summary>
        /// This will check all the checkboxes on the specified view, except the exclusions.
        /// </summary>
        /// <param name="domContainer">Instance of the TestBrowser</param>
        /// <param name="checkBoxExclusions">Array of checkboxes that should be excluded</param>
        public void GenericCheckAllCheckBoxes(DomContainer domContainer, params CheckBox[] checkBoxExclusions)
        {
            Frame f = domContainer.Frame(Find.ByIndex(0));
            if (checkBoxExclusions.Length == 0)
            {
                foreach (var checkbox in f.CheckBoxes)
                {
                    if (checkbox.Enabled && checkbox.Exists)
                    {
                        checkbox.Checked = true;
                    }
                }
            }
            else
            {
                foreach (var checkbox in f.CheckBoxes)
                {
                    foreach (var checkbox2 in checkBoxExclusions)
                    {
                        if (checkbox.Id != checkbox2.Id && checkbox.Enabled && checkbox.Exists)
                        {
                            checkbox.Checked = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Splits a single amount into rands and cents and populates the given rand and cent fields
        /// </summary>
        /// <param name="randsField">TextField</param>
        /// <param name="centsField">TextField</param>
        /// <param name="value">RRRR.CC</param>
        public void PopulateRandCentsFields(TextField randsField, TextField centsField, decimal value)
        {
            string sAmount = Math.Round(value, 2).ToString();
            if (sAmount.IndexOf('.') > 0)
            {
                string randValue;
                string centsValue;

                commonService.SplitRandsCents(out randValue, out centsValue, sAmount);
                if (centsValue.Length > 2) centsValue = centsValue.Substring(0, 1);

                randsField.TypeText(randValue);
                centsField.TypeText(centsValue);
            }
            else
            {
                randsField.TypeText(sAmount);
            }
        }
    }
}