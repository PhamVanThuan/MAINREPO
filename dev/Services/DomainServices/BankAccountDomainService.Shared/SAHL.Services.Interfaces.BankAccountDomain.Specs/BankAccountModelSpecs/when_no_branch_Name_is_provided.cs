using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.BankAccountDomain.Models;
using System;

namespace SAHL.Services.Interfaces.BankAccountDomain.Specs.BankAccountModelSpecs
{
    public class when_no_branch_Name_is_provided : WithFakes
    {
        private static string branchCode;
        private static string branchName;
        private static string accountNumber;
        private static ACBType accountType;
        private static string accountName;
        private static string userID;
        private static BankAccountModel model;
        private static Exception ex;

        private Establish context = () =>
        {
            branchCode = "1352";
            branchName = string.Empty;
            accountNumber = "1352087464";
            accountType = Core.BusinessModel.Enums.ACBType.Current;
            accountName = "CT Speed";
            userID = "System";
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() =>
            {
                model = new BankAccountModel(branchCode, branchName, accountNumber, accountType, accountName, userID);
            });
        };

        private It should_throw_a_validation_exception = () =>
        {
            ex.ShouldBeOfExactType(typeof(System.ComponentModel.DataAnnotations.ValidationException));
        };

        private It should_contain_an_message = () =>
        {
            ex.Message.ShouldEqual("The BranchName field is required.");
        };
    }
}