using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ClientDomain.Managers.AssetLiability;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;

namespace SAHL.Services.ClientDomain.Specs.Managers.AssetLiabilityDataManagerSpecs
{
    public class when_saving_a_liability_loan : WithCoreFakes
    {
        private static IAssetLiabilityDataManager assetliabilityDataManager;
        private static FakeDbFactory dbFactory;
        private static LiabilityLoanModel model;
        private static int result, assetLiabilityKey;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            assetliabilityDataManager = new AssetLiabilityDataManager(dbFactory);
            model = new LiabilityLoanModel(AssetLiabilitySubType.PersonalLoan, "Financial institution", DateTime.Now, 0d, 1d);

            assetLiabilityKey = 1111;
            dbFactory.FakedDb.DbContext.WhenToldTo<IReadWriteSqlRepository>
             (x => x.Insert<AssetLiabilityDataModel>(Param.IsAny<AssetLiabilityDataModel>()))
              .Callback<AssetLiabilityDataModel>(y => y.AssetLiabilityKey = assetLiabilityKey);
        };

        Because of = () =>
        {
            result = assetliabilityDataManager.SaveLiabilityLoan(model);
        };

        private It should_insert_liability_loan_into_system = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert<AssetLiabilityDataModel>
             (Arg.Is<AssetLiabilityDataModel>
               (y => y.AssetLiabilityTypeKey == (int)AssetLiabilityType.LiabilityLoan &&
                 y.AssetLiabilitySubTypeKey == (int)model.LoanType &&
                  y.LiabilityValue == model.LiabilityValue &&
                   y.CompanyName == model.FinancialInstitution &&
                    y.Cost == model.InstalmentValue &&
                     y.Date == model.DateRepayable)));
        };

        private It should_return_an_assetLiability_key = () =>
        {
            result.ShouldEqual(assetLiabilityKey);
        };


    }
}
