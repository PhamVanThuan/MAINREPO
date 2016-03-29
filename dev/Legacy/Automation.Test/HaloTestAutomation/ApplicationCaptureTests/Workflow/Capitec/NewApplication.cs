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

namespace ApplicationCaptureTests.Workflow.Capitec
{
    [RequiresSTA]
    public class NewApplication : CapitecBase
    {
        /// <summary>
        /// Asserts that when creating a new purchase application, a new application is created in Halo
        /// </summary>
        /// <param name="includeITC"></param>
        /// <param name="mortgageLoanPurposeEnum"></param>
        [TestCase(IncludeITC.IncludeITC, MortgageLoanPurposeEnum.Newpurchase)]
        [TestCase(IncludeITC.ExcludeITC, MortgageLoanPurposeEnum.Newpurchase)]
        public void NewApplication_WhenCreatingNewPurchaseApplication_ShouldCreateApplicationInHalo(IncludeITC includeITC, MortgageLoanPurposeEnum mortgageLoanPurposeEnum)
        {
            //Setup Test Pack
            var idnumber = IDNumbers.GetNextIDNumber();

            //Assert Precondition

            //Execute Test
            base.CreateApplication(mortgageLoanPurposeEnum, 
                                includeITC == IncludeITC.IncludeITC ? true : false, 
                                idNumber: idnumber, 
                                firstNames: "CapitecNewPurchase");

            //Assert
            base.AssertApplication();
        }

        /// <summary>
        /// Asserts that when creating a switch application, a new application is created in Halo
        /// </summary>
        /// <param name="includeITC"></param>
        /// <param name="mortgageLoanPurposeEnum"></param>
        [TestCase(IncludeITC.IncludeITC, MortgageLoanPurposeEnum.Switchloan)]
        [TestCase(IncludeITC.ExcludeITC, MortgageLoanPurposeEnum.Switchloan)]
        public void NewApplication_WhenCreatingSwitchApplication_ShouldCreateApplicationInHalo(IncludeITC includeITC, MortgageLoanPurposeEnum mortgageLoanPurposeEnum)
        {
            //Setup Test Pack
            var idnumber = IDNumbers.GetNextIDNumber();

            //Assert Precondition

            //Execute Test
            base.CreateApplication(mortgageLoanPurposeEnum, includeITC == IncludeITC.IncludeITC ? true : false, idNumber: idnumber, firstNames: "CapitecSwitch");

            //Assert
            base.AssertApplication();
        }

    }
}