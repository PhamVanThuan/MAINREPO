﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.ApplicationDomain.Models;

namespace SAHL.Services.Interfaces.ApplicationDomain.Specs.ModelSpecs.NewPurchaseApplication
{
    public class when_constructing_newpurchase_application_given_purchase_price_is_0 : WithFakes
    {
        private static System.Exception ex;

        private Because of = () =>
        {
            ex = Catch.Exception(() =>
            {
                new NewPurchaseApplicationModel(OfferStatus.Open, 1, OriginationSource.SAHomeLoans, 0, 0, 0, 0, Product.NewVariableLoan, "reference1", 1, null);
            });
        };

        private It should_throw_a_validation_exception = () =>
        {
            ex.ShouldBeOfExactType(typeof(System.ComponentModel.DataAnnotations.ValidationException));
        };
    }
}