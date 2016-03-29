using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Rules.HOC;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Specs.Rules.HOC.HOCRoofDescriptionConventionalSpecs
{
	[Subject(typeof(HOCRoofDescriptionConventional))]
	public class when_insurer_is_not_sahomeloans : RulesBaseWithFakes<HOCRoofDescriptionConventional>
	{
		protected static IPropertyRepository propertyRepository;
		protected static IProperty property;
		protected static IHOC hoc;
		protected static IHOCRoof hocRoof;
		protected static IHOCInsurer hocInsurer;
		protected static IValuation valuation;
		Establish context = () =>
		{
			hoc = An<IHOC>();
			property = An<IProperty>();
			valuation = An<IValuation>();
			hocRoof = An<IHOCRoof>();
			hocInsurer = An<IHOCInsurer>();

			hocInsurer.WhenToldTo(x => x.Key).Return((int)HOCInsurers.SantamBeperk);
			hocRoof.WhenToldTo(x => x.Key).Return((int)HOCRoofs.Conventional);

			valuation.WhenToldTo(x => x.IsActive).Return(true);
			property.WhenToldTo(x => x.LatestCompleteValuation).Return(valuation);

			hoc.WhenToldTo(x => x.HOCConventionalAmount).Return(0);
			hoc.WhenToldTo(x => x.HOCInsurer).Return(hocInsurer);
			hoc.WhenToldTo(x => x.HOCRoof).Return(hocRoof);

			propertyRepository = An<IPropertyRepository>();
			propertyRepository.WhenToldTo(x => x.GetPropertyByHOC(Param.IsAny<IHOC>())).Return(property);

			businessRule = new HOCRoofDescriptionConventional(propertyRepository);
			RulesBaseWithFakes<HOCRoofDescriptionConventional>.startrule.Invoke();
		};

		Because of = () =>
		{
			businessRule.ExecuteRule(messages, hoc);
		};

		It rule_should_pass = () =>
		{
			messages.Count.ShouldEqual(0);
		};
	}
}
