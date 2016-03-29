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
    public class NewApplicant : CapitecBase
    {
        /// <summary>
        /// This is to test that we don't save non-mandatory client contact details as zero when nothing has been captured.
        /// </summary>
        [Test]
        public void NewApplicant_WhenNoMandatoryContactInfo_ShouldNotInsertZero()
        {
            //Setup Test Pack
            var idnumber = IDNumbers.GetNextIDNumber();

            //Assert Precondition

            //Execute Test
            base.CreateApplication(MortgageLoanPurposeEnum.Newpurchase,
                                    firstNames:"CapitecNonMandatoryContactInfoNotZero", 
                                    includeITC: false, 
                                    workPhoneNumber: "", 
                                    idNumber: idnumber);

            //Assert
            base.AssertApplication();
        }
        [Test]
        public void NewApplicant_WhenContactInfoContainsWhitespace_ShouldTrimWhitespace()
        {
            //Setup Test Pack
            var idnumber = IDNumbers.GetNextIDNumber();

            //Assert Precondition

            //Execute Test
            base.CreateApplication(MortgageLoanPurposeEnum.Newpurchase,
                                    firstNames: "CapitecTrimWhitespaceContactNumbers",
                                    includeITC: false,
                                    workPhoneNumber: "0 1 3 1 2 3 4 5 6 7",
                                    homePhoneNumber: "0 1 2 7 6 5 4 3 2 1",
                                    idNumber: idnumber);

            //Assert
            base.AssertApplication();
        }
                
    }
}