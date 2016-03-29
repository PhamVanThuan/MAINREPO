using System.Linq;
using Automation.Framework;
using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using BuildingBlocks.Services.Contracts;
using Automation.DataModels;
using System;
using System.Xml.Linq;
using SAHL.Services.Capitec.Models.Shared;
using System.Collections.Generic;
using ApplicationCaptureTests.Workflow.Capitec;

namespace ApplicationCaptureTests.Workflow.Capitec
{
    [RequiresSTA]
    public class DeclinedApplication : CapitecBase
    {

        [TestCase(MortgageLoanPurposeEnum.Newpurchase)]
        [TestCase(MortgageLoanPurposeEnum.Switchloan)]
        public void DeclinedApplication_WhenDeclineReasonsNotExist_ShouldBeAtCapitecBranchDecline(MortgageLoanPurposeEnum mortgageLoanPurposeEnum)
        {
            //Setup Test Pack
            var idnumber = IDNumbers.GetNextIDNumber();

            //Assert Precondition

            //Execute Test
            base.CreateApplication(mortgageLoanPurposeEnum, false, idNumber: idnumber,
                                        firstNames: "CapitecDeclined1",
                                        offerStatus: OfferStatusEnum.Declined);//have no decline reasons

            //Assert
            base.AssertApplication();
        }


        [TestCase(MortgageLoanPurposeEnum.Newpurchase)]
        [TestCase(MortgageLoanPurposeEnum.Switchloan)]
        public void DeclinedApplication_WhenDeclineReasonsExist_ShouldBeAtCapitecBranchDecline(MortgageLoanPurposeEnum mortgageLoanPurposeEnum)
        {
            //Setup Test Pack
            var idnumber = IDNumbers.GetNextIDNumber();

            //Assert Precondition

            //Execute Test
            base.CreateApplication(mortgageLoanPurposeEnum, false, idNumber: idnumber,
                                        firstNames: "CapitecDeclined2",
                                        offerStatus: OfferStatusEnum.Declined,
                                        reasonType: Common.Constants.ReasonType.CapitecBranchDecline,
                                        reasonDescription: ReasonDescription.CapitecBranchDeclineReason);//have decline reasons

            //Assert
            base.AssertApplication();
        }
    }
}