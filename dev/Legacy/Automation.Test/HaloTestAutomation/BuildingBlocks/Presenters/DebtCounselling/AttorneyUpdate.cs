using BuildingBlocks.Services.Contracts;
using ObjectMaps.Pages;
using System.Linq;

namespace BuildingBlocks.Admin
{
    public class AttorneyUpdate : AttorneyUpdateControls
    {
        private readonly IAttorneyService attorneyService;

        public AttorneyUpdate()
        {
            attorneyService = ServiceLocator.Instance.GetService<IAttorneyService>();
        }

        /// <summary>
        /// Selects a litigation attorney
        /// </summary>
        /// <param name="b"></param>
        public int SelectLitigationAttorney()
        {
            //we need a litigation attorney
            var attorney = attorneyService.GetLitigationAttorney();
            var attorneyToSelect = (from a in attorney select a).FirstOrDefault();
            //select the attorney
            base.AttorneySelect.Option(attorneyToSelect.Value).Select();
            //update
            base.btnUpdate.Click();
            return attorneyToSelect.Key;
        }
    }
}