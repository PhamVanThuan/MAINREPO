using System;
using WatiN.Core;

namespace ObjectMaps
{
    internal static class ElementTimers
    {
        internal static void WaitForBrowserFrame(Browser browser)
        {
            int tryCount = 0;
            while (tryCount < Int32.MaxValue)
            {
                if (browser.Frames.Count > 0)
                    return;
                tryCount++;
            }
            throw new Exception("Timeout on finding browser frame");
        }
    }
}