using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Rules.HOC;

namespace SAHL.Common.BusinessModel.Specs.Rules.HOC.HOCRoofDescriptionConventionalSpecs
{
	[Subject(typeof(HOCRoofDescriptionConventional))]
	public class when_no_valuation_exists : RulesBaseWithFakes<HOCRoofDescriptionConventional>
	{
		protected static IPropertyRepository propertyRepository;
		protected static IProperty property;
		protected static IHOC hoc;
		Establish context = () =>
		{
			hoc = An<IHOC>();
			property = An<IProperty>();

			property.WhenToldTo(x => x.LatestCompleteValuation).Return((IValuation)null);

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
