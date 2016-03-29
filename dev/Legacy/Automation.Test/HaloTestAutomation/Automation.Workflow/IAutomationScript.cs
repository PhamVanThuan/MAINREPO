using Automation.Framework;

using Common.Enums;

namespace WorkflowAutomation.Harness
{
    public interface IAutomationScript
    {
        WorkflowEnum Workflow { get; }
        WorkflowScript Create();
    }
}