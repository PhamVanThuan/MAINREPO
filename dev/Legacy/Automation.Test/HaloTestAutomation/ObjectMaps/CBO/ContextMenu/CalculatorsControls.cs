using WatiN.Core;

namespace ObjectMaps.FloboControls
{
    public partial class CboControls
    {
        public class Calculators
        {
            private Browser b;
            private Frame frame;

            public Calculators(Browser browser)
            {
                b = browser;
                frame = b.Frame(Find.ByIndex(0));
            }
        }
    }
}