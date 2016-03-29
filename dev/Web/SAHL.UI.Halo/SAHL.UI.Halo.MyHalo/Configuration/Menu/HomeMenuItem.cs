using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.MyHalo.Configuration.Menu
{
    public class HomeMenuItem : HaloMenuItem, 
                                IHaloApplicationMenuItem<MyHaloHaloApplicationConfiguration>
    {
        public HomeMenuItem() : base("Home", "Home", 1)
        {
        }
    }
}
