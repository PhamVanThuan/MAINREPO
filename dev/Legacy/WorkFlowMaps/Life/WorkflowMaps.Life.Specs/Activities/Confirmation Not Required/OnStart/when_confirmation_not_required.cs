﻿using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.Activities.Confirmation_Not_Required.OnStart
{
    [Subject("Activity => Confirmation_Not_Required => OnStart")]
    internal class when_confirmation_not_required : WorkflowSpecLife
    {
        private static bool result;

        private Establish context = () =>
        {
            workflowData.ConfirmationRequired = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Confirmation_Not_Required(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_the_inverse_of_the_confirmation_required_data_property = () =>
        {
            result.ShouldBeTrue();
        };
    }
}