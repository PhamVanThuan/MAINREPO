using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace SAHL.Common.BusinessModel.Specs.Rules.Property.LightStonePropertyIDExists
{
	public class when_no_id_exists : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Property.LightStonePropertyIDExists>
	{
		protected static IPropertyRepository propertyRepository;
		protected static IProperty property;
		protected static string propertyID;
		Establish context = () =>
		{
			propertyID = null;
			property = An<IProperty>();
			propertyRepository = An<IPropertyRepository>();
			propertyRepository.WhenToldTo(x => x.GetLightStonePropertyID(Param.IsAny<IProperty>())).Return(propertyID);
			businessRule = new BusinessModel.Rules.Property.LightStonePropertyIDExists(propertyRepository);
			RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Property.LightStonePropertyIDExists>.startrule.Invoke();
		};

		Because of = () =>
		{
			businessRule.ExecuteRule(messages, property);
		};

		It rule_should_fail = () =>
		{
			messages.Count().ShouldEqual(1);
		};
	}
}
