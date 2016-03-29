using System;
using WatiN.Core;
using ObjectMaps;
using WatiN.Core.UtilityClasses;

namespace BuildingBlocks
{
    public static partial class Views
    {
        public sealed class InitiateCase : ObjectMaps.InitiateCaseControls
        {
            public void PopulateView(DateTime _17pt1Date, string Comment)
            {
                base.Comments.Value = Comment;
                base._17Pt1Date.Value = _17pt1Date.ToShortDateString();
            }
            public void ClickNext()
            {
                base.Next.Click();
            }
        }
    }
}
