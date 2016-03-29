using BuildingBlocks.Assertions;
using BuildingBlocks.Services.Contracts;
using Common.Enums;
using NUnit.Framework;
using ObjectMaps.Pages;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading;
using WatiN.Core;
using WatiN.Core.UtilityClasses;

namespace BuildingBlocks.Presenters.CommonPresenters
{
    public class CommonReasonCommonDecline : CommonReasonCommonDeclineControls
    {
        private readonly IReasonService reasonService;
        private readonly IWatiNService watinService;

        public CommonReasonCommonDecline()
        {
            reasonService = ServiceLocator.Instance.GetService<IReasonService>();
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        /// <summary>
        /// This method will retrieve a list of Reasons when provided with a Reason Type.
        /// It currently selects the first reason in the list, adds it to the Selected Reasons
        /// list and the commits the change on the CommonReasonCommonDecline screen.
        /// </summary>
        /// <param name="b">The IE TestBrowser Object</param>
        /// <param name="ReasonType">A ReasonType.Description i.e. Branch Decline</param>
        public void SelectReasonAndSubmit(string ReasonType, int reasonCount = 1)
        {
            //get a reason to select
            var results = reasonService.GetActiveReasonsByReasonType(ReasonType);
            int reasonsAdded = 0;
            while (reasonsAdded < reasonCount)
            {
                for (int i = 0; i < results.RowList.Count(); i++)
                {
                    ReasonType = results.Rows(i).Column(0).Value;
                    var reasonExists = (from r in base.ReasonList.Options where r.Text == ReasonType select r).FirstOrDefault();
                    if (results.Rows(i).Column("EnforceComment").GetValueAs<bool>() != true
                        && ReasonType != "Miscellaneous Reason"
                        && reasonExists != null
                        )
                        break;
                }

                //select the reason
                base.ReasonList.Option(ReasonType).WaitUntilExists();
                base.ReasonList.Select(ReasonType);
                //click the add button
                base.AddButton.Click();
                reasonsAdded++;
            }
            //need to find the dialogue box after the submit button
            watinService.HandleConfirmationPopup(base.SubmitButton);
        }

        /// <summary>
        /// This method will retrieve a list of Reasons when provided with a Reason Type.
        /// It currently selects the first reason in the list, adds it to the Selected Reasons
        /// list and the commits the change on the CommonReasonCommonDecline screen. The overloaded method returns the
        /// Selected Reason to the test for assertion purposes
        /// </summary>
        /// <param name="b">The IE TestBrowser Object</param>
        /// <param name="reasonType">A ReasonType.Description i.e. Branch Decline</param>
        /// <param name="SelectedReason">The Reason that was added to the Application/Account</param>
        public string SelectReasonAndSubmit(string reasonType)
        {
            //get a reason to select
            var results = reasonService.GetActiveReasonsByReasonType(reasonType);

            for (int i = 0; i < results.RowList.Count(); i++)
            {
                reasonType = results.Rows(i).Column(0).Value;
                if (results.Rows(i).Column("EnforceComment").Value != "1" && reasonType != "Miscellaneous Reason")
                    break;
            }

            //select the reason
            base.ReasonList.Option(reasonType).WaitUntilExists();
            base.ReasonList.Select(reasonType);
            //click the add button
            base.AddButton.Click();
            //need to find the dialogue box after the submit button
            watinService.HandleConfirmationPopup(base.SubmitButton);
            results.Dispose();
            Thread.Sleep(2500);
            return reasonType;
        }

        /// <summary>
        /// Allows the user to complete the action without supplying a reason.
        /// </summary>
        /// <param name="b">IE TestBrowser Object</param>
        public void SubmitWithoutReason()
        {
            //click the submit button
            base.SubmitButton.Click();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="b"></param>
        /// <param name="reasonType"></param>
        /// <param name="reasonDescription"></param>
        public void SelectReasonAndSubmit(string reasonType, string reasonDescription)
        {
            //get a reason to select
            var Results = reasonService.GetActiveReasonsByReasonType(reasonType);

            for (int i = 0; i < Results.RowList.Count(); i++)
            {
                reasonType = Results.Rows(i).Column(0).Value;
                if (Results.Rows(i).Column("EnforceComment").Value != "1" && reasonType != "Miscellaneous Reason" && reasonType == reasonDescription)
                    break;
            }
            base.ReasonList.Option(reasonType).WaitUntilExists();
            base.ReasonList.Select(reasonType);
            //click the add button
            base.AddButton.Click();
            //need to find the dialogue box after the submit button
            //need to find the dialogue box after the submit button
            watinService.HandleConfirmationPopup(base.SubmitButton);
            Results.Dispose();
        }

        /// <summary>
        /// Selects multiple reasons and returns a list of reasons selected.
        /// </summary>
        /// <param name="_b">IE TestBrowser</param>
        /// <param name="reasonType">Reason Type</param>
        /// <param name="numberOfReasons">number of reasons to select</param>
        /// <param name="selectedReasons">out parameter; a list of the reasons selected</param>
        public List<string> SelectMultipleReasons(string reasonType, int numberOfReasons)
        {
            //get a reason to select
            var Results = reasonService.GetActiveReasonsByReasonType(reasonType);
            var count = 0;
            var selectedReasons = new List<string>();
            for (int i = 0; i < Results.RowList.Count(); i++)
            {
                if (count == numberOfReasons)
                    break;
                reasonType = Results.Rows(i).Column(0).Value;
                if (Results.Rows(i).Column("EnforceComment").Value != "1" && reasonType != "Miscellaneous Reason")
                {
                    //select the reason
                    base.ReasonList.Option(reasonType).WaitUntilExists();
                    base.ReasonList.Select(reasonType);
                    //click the add button
                    base.AddButton.Click();
                    selectedReasons.Add(reasonType);
                    count++;
                }
            }
            //need to find the dialogue box after the submit button
            watinService.HandleConfirmationPopup(base.SubmitButton);
            Results.Dispose();
            return selectedReasons;
        }

        public void DeselectReason(string reasonDescription, ButtonTypeEnum buttonLabel)
        {
            base.SelectedReasons.Select(reasonDescription);
            //click the add button
            base.RemoveButton.Click();
            //need to find the dialogue box after the submit button
            ClickButton(buttonLabel, true);
        }

        public void ClickButton(ButtonTypeEnum buttonLabel, bool handlePopup)
        {
            switch (buttonLabel)
            {
                case ButtonTypeEnum.Cancel:
                    base.CancelButton.Click();
                    break;

                case ButtonTypeEnum.Submit:
                    if (handlePopup)
                    {
                        watinService.HandleConfirmationPopup(base.SubmitButton);
                        Thread.Sleep(2500);
                        if (base.divValidationSummaryBody.Exists)
                        {
                            if (base.divContinueButton.Exists)
                            {
                                watinService.HandleConfirmationPopup(base.divContinueButton);
                            }
                        }
                    }
                    else
                        base.SubmitButton.Click();
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// This method will select the first reason in the reason list that allows comments and add a comment
        /// </summary>
        /// <param name="b">The IE TestBrowser Object</param>
        /// <param name="reasonType">A ReasonType.Description i.e. Branch Decline</param>
        /// <param name="comment">A comment</param>
        /// <param name="buttonLabel">Name of button to click</param>
        public void SelectReasonAddCommentAndSubmit(string reasonType, string comment, ButtonTypeEnum buttonLabel)
        {
            //get a reason to select
            //get the first reason that allows comments
            var results = reasonService.GetReasonDescriptionsByReasonType(reasonType, true);
            string reason = (from r in results select r.Value).FirstOrDefault();

            base.ReasonList.Select(reason);
            base.AddButton.Click();
            base.SelectedReasons.Select(reason);
            base.SelectedReasons.Click();

            SimpleTimer timer = new SimpleTimer(TimeSpan.FromSeconds(30));
            while (true)
            {
                if (timer.Elapsed)
                {
                    throw new WatiN.Core.Exceptions.WatiNException("Could not find element");
                }
                else if (!base.CommentBox.ReadOnly)
                {
                    base.CommentBox.TypeText(comment);
                    break;
                }
            }

            ClickButton(buttonLabel, true);
        }

        /// <summary>
        /// This method will select a reason by reason description and attempt to add a reason comment
        /// </summary>
        /// <param name="b">The IE TestBrowser Object</param>
        /// <param name="reasonType">A ReasonType.Description i.e. Branch Decline</param>
        /// <param name="comment">A comment</param>
        /// <param name="buttonLabel">Name of button to click</param>
        public void SelectReasonAddCommentAndSubmit(string reasonType, string reasonDescription, string comment, ButtonTypeEnum buttonLabel)
        {
            base.ReasonType.Select(reasonType);
            base.ReasonList.Select(reasonDescription);
            base.AddButton.Click();
            base.SelectedReasons.Select(reasonDescription);
            base.SelectedReasons.Click();

            SimpleTimer timer = new SimpleTimer(TimeSpan.FromSeconds(30));
            while (true)
            {
                if (timer.Elapsed)
                {
                    throw new WatiN.Core.Exceptions.WatiNException("Could not find element");
                }
                else if (!base.CommentBox.ReadOnly)
                {
                    base.CommentBox.TypeText(comment);
                    break;
                }
            }

            ClickButton(buttonLabel, true);
        }

        /// <summary>
        /// This method will select a reason by reason description
        /// </summary>
        /// <param name="b">The IE TestBrowser Object</param>
        /// <param name="reasonType">A ReasonType.Description i.e. Branch Decline</param>
        /// <param name="comment">A comment</param>
        /// <param name="buttonLabel">Name of button to click</param>
        public void SelectReasonAndSubmit(string reasonDescription, ButtonTypeEnum buttonLabel)
        {
            base.ReasonList.Select(reasonDescription);
            base.AddButton.Click();

            ClickButton(buttonLabel, true);
        }

        public Button GetButton(TestBrowser b, ButtonTypeEnum buttonLabel)
        {
            switch (buttonLabel)
            {
                case ButtonTypeEnum.Cancel:
                    return base.CancelButton;
                    break;

                case ButtonTypeEnum.Submit:
                    return base.SubmitButton;
                    break;

                default:
                    throw new WatiN.Core.Exceptions.WatiNException(string.Format("No button with the button label: {0} should exist on this view",
                        ButtonTypeEnum.Submit));
                    break;
            }
        }

        public void AssertReasonListContents(List<string> reasonList, bool checkCountOfItems)
        {
            Thread.Sleep(1500);
            WatiNAssertions.AssertSelectListContents(base.ReasonList, reasonList, checkCountOfItems);
        }

        public void AssertViewDisplayed()
        {
            var expectedView = "WF_PersonalLoanAdminDecline";
            Assert.AreEqual(expectedView, base.ViewName.Text, "Expected {0} to be displayed.", expectedView);
        }
    }
}