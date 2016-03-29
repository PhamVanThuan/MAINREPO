using Machine.Specifications;
using Machine.Fakes;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FinancialDomain.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Services.FinancialDomain.Managers.Models;
using SAHL.Services.FinancialDomain.Managers.Statements;
using NSubstitute;

namespace SAHL.Services.FinancialDomain.Specs.ManagerSpecs.FinancialData
{
    public class when_getting_valid_SPV :  WithCoreFakes
    {
        private static FinancialDataManager financialDataManager;
        private static FakeDbFactory dbFactory;

        private static decimal LTV;
        private static string offerAttributesCSV;

        private static string XML;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            financialDataManager = new FinancialDataManager(dbFactory);
            
            LTV = 0.9m;
            offerAttributesCSV = "26,56";
            XML = Functions.GenerateGetValidSPVxml(LTV, offerAttributesCSV);
        };

        private Because of = () =>
        {
            financialDataManager.GetValidSPV(LTV, offerAttributesCSV);
        };

        private It should_query_to_determine_an_SPV = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.SelectOne<GetValidSPVResultModel>(Arg.Is<GetValidSPVStatement>(y => y.XML == XML && y.SPVDetermineSource == 3)));
        };

    }
}
