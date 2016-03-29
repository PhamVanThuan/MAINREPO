using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.FinancialDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.FinancialDomain.Specs.ModelSpecs.OriginationFeesModelSpecs
{
    public class when_all_properties_are_set : WithFakes
    {
        static OriginationFeesModel model;
        static Exception ex;

        static decimal InterimInterest = 2;
        static decimal CancellationFee = 3;
        static decimal InitiationFee = 4;
        static decimal BondToRegister = 5;
        static decimal RegistrationFee = 6;
        static decimal InitiationFeeDiscount = 1;
        static bool CapitaliseFees = true;
        static bool CapitaliseInitiationFees = false;
        
        Establish context = () =>
        {
            ex = Catch.Exception(() =>
            {
                model = new OriginationFeesModel(InterimInterest, CancellationFee, InitiationFee, BondToRegister, RegistrationFee, InitiationFeeDiscount, CapitaliseFees, CapitaliseInitiationFees);
            });
        };

        It should_not_throw_a_validation_exception = () =>
        {
            ex.ShouldBeNull();
        };

        It should_construct_the_model = () =>
        {
            model.ShouldNotBeNull();
        };

        It should_calculate_the_total_fees = () =>
        {
            model.TotalFees().ShouldEqual((InitiationFee + RegistrationFee + CancellationFee));
        };
    }
}
