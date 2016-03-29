using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.SaveApplicantDeclarations
{
    public class when_saving_an_application_declaration : WithFakes
    {
        private static ApplicantDataManager applicantDataManager;
        private static FakeDbFactory dbFactory;
        private static IEnumerable<OfferDeclarationDataModel> dataModel;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            applicantDataManager = new ApplicantDataManager(dbFactory);
            dataModel = new OfferDeclarationDataModel[] { new OfferDeclarationDataModel(1, 1, (int)OfferDeclarationAnswer.No, null),
                new OfferDeclarationDataModel(1,2,(int)OfferDeclarationAnswer.Yes, DateTime.Now)};

            dbFactory.FakedDb.DbContext.WhenToldTo<IReadWriteSqlRepository>(x => x.Insert<OfferDeclarationDataModel>(Param.IsAny<IEnumerable<OfferDeclarationDataModel>>()));
        };

        private Because of = () =>
        {
            applicantDataManager.SaveApplicantDeclarations(dataModel);
        };

        private It should_insert_the_correct_number_of_declarations = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert<OfferDeclarationDataModel>(Arg.Is<IEnumerable<OfferDeclarationDataModel>>(y => y.Count() == dataModel.Count())));
        };
    }
}