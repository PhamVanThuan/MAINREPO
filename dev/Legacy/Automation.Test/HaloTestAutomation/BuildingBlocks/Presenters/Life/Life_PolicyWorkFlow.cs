using NUnit.Framework;
using ObjectMaps;
using ObjectMaps.Pages;
using System;
using WatiN.Core;

namespace BuildingBlocks.Presenters.Life
{
    public class Life_PolicyWorkFlow : LifePolicyWorkFlowControls
    {
        /// <summary>
        /// Click the Next button on the Life_PolicyWorkFlow view.
        /// </summary>
        public void AcceptPlan()
        {
            base.Document.DomContainer.WaitForComplete();
            base.btnAccept.Click();
            base.Document.DomContainer.WaitForComplete();
        }

        /// <summary>
        /// Click the Add Life button on this view.
        /// </summary>
        public void AddLife()
        {
            base.MainbtnAdd.Click();
            base.Document.DomContainer.WaitForComplete();
        }

        /// <summary>
        /// Click the Remove Life button on this view.
        /// </summary>
        public void RemoveLife()
        {
            CommonIEDialogHandler.SelectOK(base.btnRemove);
            base.Document.DomContainer.WaitForComplete();
        }

        /// <summary>
        /// Select Assured life and click the Remove Life button on this view.
        /// </summary>
        /// <param name="assuredLifeIDNumber">id number of the assured life that needs to be removed.</param>
        public void RemoveLife(string assuredLifeIDNumber)
        {
            bool assuredLifeFound = false;
            foreach (TableRow row in base.MainLegalEntityGrid.TableRows)
            {
                foreach (TableCell cell in row.TableCells)
                {
                    if (cell.Text.ToLower() == assuredLifeIDNumber)
                    {
                        assuredLifeFound = true;
                        row.Click();
                    }
                }
            }
            if (!assuredLifeFound)
            {
                string message =
                    string.Format(@"Could not locate the assured life row for ID: {0} in the assured life table.",
                    assuredLifeIDNumber);
                Console.WriteLine(message);
                throw new AssertionException(message);
            }
            CommonIEDialogHandler.SelectOK(base.btnRemove);
            base.Document.DomContainer.WaitForComplete();
        }
    }
}