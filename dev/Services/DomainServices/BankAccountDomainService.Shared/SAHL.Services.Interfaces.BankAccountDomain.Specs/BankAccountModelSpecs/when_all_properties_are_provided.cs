using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.BankAccountDomain.Models;
using System;

namespace SAHL.Services.Interfaces.BankAccountDomain.Specs.BankAccountModelSpecs
{
    public class when_all_properties_are_provided : WithFakes
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
            branchName = "BranchName";
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

        private It should_not_throw_a_validation_exception = () =>
        {
            ex.ShouldBeNull();
        };

        private It should_construct_a_model = () =>
        {
            model.ShouldNotBeNull();
        };
    }
}