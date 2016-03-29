using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.MyHalo.Configuration.Menu
{
    public class TeamMenuItem : HaloMenuItem, IHaloApplicationMenuItem<MyHaloHaloApplicationConfiguration>
    {
        public TeamMenuItem()
            : base("Team", "Team", 2)
        {
        }
    }
}