using BuildingBlocks.Services.Contracts;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.Life
{
    public class Life_Declaration : LifeDeclarationControls
    {
        private readonly IWatiNService watinService;

        public Life_Declaration()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        /// <summary>
        /// Click the Next button on the Life_Declaration view.
        /// </summary>
        /// <param name="b">TestBrowser instance that is being used</param>
        public void AcceptDeclarationGoNext()
        {
            watinService.GenericCheckAllCheckBoxes(base.Document.DomContainer);
            base.ctl00MainbtnNext.Click();
            base.Document.DomContainer.WaitForComplete();
        }

        /// <summary>
        ///  This will test that the "All points must be accepted before you can continue." rule fires.
        /// </summary>
        /// <param name="b"></param>
        public void AllPointsMustBeAcceptedRule()
        {
            watinService.GenericCheckAllCheckBoxes(base.Document.DomContainer, base.ctl00Maincbx0);
            base.ctl00MainbtnNext.Click();
            System.Threading.Thread.SpinWait(2000000);
            base.Document.DomContainer.WaitForComplete();
        }
    }
}