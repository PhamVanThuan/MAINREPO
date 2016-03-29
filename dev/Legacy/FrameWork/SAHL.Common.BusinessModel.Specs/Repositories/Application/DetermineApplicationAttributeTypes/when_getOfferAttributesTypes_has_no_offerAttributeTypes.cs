using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.DataAccess;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using System;
using System.Data;
using System.Linq;
using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;

namespace SAHL.Common.BusinessModel.Specs.Repositories.Application.DetermineApplicationAttributeTypes
{
	[Subject(typeof(when_getOfferAttributesTypes_has_no_offerAttributeTypes))]
	internal class when_getOfferAttributesTypes_has_no_offerAttributeTypes : WithFakes
	{
		static IApplicationRepository applicationRepository;
		static ILookupRepository lookupRepository;
		static IUIStatementService uiStatementService;
		static ICastleTransactionsService castleTransactionsService;
        static ILegalEntityRepository legalEntityRepository;
        static IAddressRepository addressRepository;
        static IITCRepository itcRepository;
        static IX2Repository x2Repository;
        static IReasonRepository reasonRepository;
		static IApplication application;
		static DataTable applicationAttributesFromSource;
		static DataSet dataSet;
		static IList<ApplicationAttributeToApply> applicationAttributeTypes;
		Establish context = () =>
		{
			uiStatementService = An<IUIStatementService>();
			lookupRepository = An<ILookupRepository>();
			castleTransactionsService = An<ICastleTransactionsService>();
            x2Repository = An<IX2Repository>();
			application = An<IApplication>();
            reasonRepository = An<IReasonRepository>();


			application.WhenToldTo(x => x.Key).Return(Param.IsAny<int>());
            applicationRepository = new ApplicationRepository(uiStatementService, castleTransactionsService, lookupRepository, x2Repository, legalEntityRepository, addressRepository, itcRepository, reasonRepository, An<IMemoRepository>(), An<IOrganisationStructureRepository>());

			applicationAttributesFromSource = new DataTable();
			applicationAttributesFromSource.Columns.Add("OfferAttributeTypeKey");
			applicationAttributesFromSource.Columns.Add("Remove");

			dataSet = new DataSet();
			dataSet.Tables.Add(applicationAttributesFromSource.Clone());
			castleTransactionsService.WhenToldTo(x => x.ExecuteQueryOnCastleTran(Param.IsAny<string>(), Param.IsAny<Databases>(), Param.IsAny<ParameterCollection>())).Return(dataSet);
			uiStatementService.WhenToldTo(x => x.GetStatement(Param.IsAny<string>(), Param.IsAny<string>())).Return(Param.IsAny<string>());
		};

		Because of = () =>
		{
			applicationAttributeTypes = applicationRepository.DetermineApplicationAttributeTypes(application);
		};

		It should_return_an_empty_list_of_offerAttributeTypeKeys = () =>
		{
			applicationAttributesFromSource.Rows.Count.ShouldEqual(applicationAttributeTypes.Count);
		};

		It should_retrieve_the_application_getofferattributetypes_ui_statement = () =>
		{
			uiStatementService.WasToldTo(x=>x.GetStatement("Application", "GetOfferAttributeTypes"));
		};

		It should_match_the_keys_from_the_source = () =>
		{
			var keysFromSource = applicationAttributesFromSource.Rows.Cast<DataRow>().Select(row => int.Parse(row[0].ToString()));
			var keys = applicationAttributeTypes.Select(x => x.ApplicationAttributeTypeKey);
			keysFromSource.Except(keys).ShouldBeEmpty();
		};
	}
}
