using Common.Enums;
using ObjectMaps.FLoBO;

namespace BuildingBlocks.Navigation.FLOBO.Common
{
    public class ApplicantsNode : ApplicantsNodeControls
    {
        /// <summary>
        /// The applicants node in the flobo
        /// </summary>
        /// <param name="n"></param>
        public void Applicants(NodeTypeEnum n)
        {
            base.Applicants.Click();
            switch (n)
            {
                case NodeTypeEnum.View:
                    break;

                case NodeTypeEnum.Add:
                    base.AddLegalEntity.Click();
                    break;

                case NodeTypeEnum.Delete:
                    base.RemoveLegalEntity.Click();
                    break;
            }
        }

        /// <summary>
        /// The ITC details node in the flobo
        /// </summary>
        /// <param name="n"></param>
        public void ITCDetails(NodeTypeEnum n)
        {
            switch (n)
            {
                case NodeTypeEnum.View:
                    base.ITCDetails.Click();
                    break;
            }
        }
    }
}