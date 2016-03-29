using Common.Enums;
using ObjectMaps.FloboControls;

namespace BuildingBlocks.Navigation
{
    public class DocumentCheckListNode : DocumentCheckListNodeControls
    {
        public void DocumentChecklist()
        {
            base.DocumentChecklist.Click();
        }

        public void ViewDocumentChecklist(NodeTypeEnum n)
        {
            base.ViewDocumentChecklist.Click();
            switch (n)
            {
                case NodeTypeEnum.View:
                    break;

                case NodeTypeEnum.Update:
                    base.UpdateDocumentChecklist.Click();
                    break;
            }
        }
    }
}