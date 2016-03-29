using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.FinancialDomain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.FinancialDomain.Specs.ModelSpecs.OriginationFeesModelSpecs
{
    public class when_registration_fee_is_negative : WithFakes
    {
        static OriginationFeesModel model;
        static Exception ex;

        static decimal InterimInterest = 2;
        static decimal CancellationFee = 3;
        static decimal InitiationFee = 4;
        static decimal BondToRegister = 5;
        static decimal RegistrationFee = -6;
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

        It should_throw_a_validation_exception = () =>
        {
            ex.ShouldBeOfExactType(typeof(ValidationException));
        };

        It should_not_construct_the_model = () =>
        {
            model.ShouldBeNull();
        };

        It should_return_a_message = () =>
        {
            ex.Message.ShouldEqual("RegistrationFee must be 0 or greater.");
        };

    }
}
