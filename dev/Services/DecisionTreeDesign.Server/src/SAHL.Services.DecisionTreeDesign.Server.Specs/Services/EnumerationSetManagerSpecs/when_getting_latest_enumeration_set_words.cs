using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.DecisionTree;
using SAHL.Services.DecisionTreeDesign.Managers.EnumerationSet;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.DecisionTreeDesign.Server.Specs.Services.EnumerationSetManagerSpecs
{
    [Subject("SAHL.Services.DecisionTree.Services.EnumerationSetManager.GetLatestEnumerationSetWords")]
    public class when_getting_latest_enumeration_set_words : WithFakes
    {
        private static IEnumerationSetDataManager dataService;
        private static IEnumerationSetManager dataManager;
        private static IEnumerable<string> result;
        private static IUnitOfWorkFactory unitOfWorkFactory;

        private Establish context = () =>
        {
            dataService = An<IEnumerationSetDataManager>();
            EnumerationSetDataModel model = new EnumerationSetDataModel(1, "{\"groups\":[{\"id\":1,\"name\":\"SA Home Loans\",\"groups\":[{\"id\":1,\"name\":\"Credit\",\"groups\":[],\"enumerations\":[{\"name\":\"Credit Matrix Category\",\"value\":[\"Salaried Category 0\",\"Salaried Category 1\",\"Salaried Category 3\",\"Salaried Category 4\",\"Salaried Category 5\",\"Salaried with Deduction Category 0\",\"Salaried with Deduction Category 1\",\"Salaried with Deduction Category 3\",\"Salaried with Deduction Category 4\",\"Salaried with Deduction Category 5\",\"Salaried with Deduction Category 10\",\"Self Employed Category 0\",\"Self Employed Category 1\",\"Self Employed Category 3\",\"Alpha Category 6\",\"Alpha Category 7\",\"Alpha Category 8\",\"Alpha Category 9\"],\"definition\":\"These are the distinct categories which provide a combination of Employment Type, Loan Purpose, Loan Size, Property Value, Loan to Value, Payment to Income, Household Income and Household Empirica to assign a specific base Link Rate to an application during Origination.\"}]}],\"enumerations\":[{\"name\":\"Property Occupancy Type\",\"value\":[\"Owner Occupied\",\"Investment Property\"],\"definition\":\"This is the manner in which a property to financed is occupied by the borrower.\"},{\"name\":\"Household Income Type\",\"value\":[\"Salaried\",\"Self Employed\",\"Salaried with Deduction\",\"Unemployed\"],\"definition\":\"This is the income type assumed for the household and is based on the majority income contributors current employment type.\"},{\"name\":\"Mortgage Loan Application Type\",\"value\":[\"Switch\",\"New Purchase\"],\"definition\":\"This is the type of application used when applying for a Mortgage Loan\"},{\"name\":\"DefaultEnumValue\",\"value\":[\"Unknown\"],\"definition\":\"This is the enum value all enumerations are initialized to\"}]}]}");
            dataService.WhenToldTo(x => x.GetLatestEnumerationSet()).Return(model);
            unitOfWorkFactory = An<IUnitOfWorkFactory>();
            dataManager = new EnumerationSetManager(dataService, unitOfWorkFactory);
        };

        private Because of = () =>
        {
            result = dataManager.GetLatestEnumerationSetWords();
        };
        
        //terrible test i know, will make them more meaningful
        private It should_return_results = () =>
        {
            result.Count().ShouldNotEqual(0);
        };
    }
}