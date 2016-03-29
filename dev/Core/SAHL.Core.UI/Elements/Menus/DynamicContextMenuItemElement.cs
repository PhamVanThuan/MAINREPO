using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Elements.Parts;
using SAHL.Core.UI.Enums;
using System;

namespace SAHL.Core.UI.Elements.Menus
{
    public class DynamicContextMenuItemElement : AbstractContextMenuItemElement
    {
        public DynamicContextMenuItemElement(string menuText, BusinessContext businessContext, string url, Type tileModelType)
            : base(businessContext, url, UrlAction.TileChangeContext)
        {
            this.TileModelType = tileModelType;

            this.AddPart(new StaticTextPart(menuText));
            this.AddPart(new StaticTextPart(" / "));
        }

        public Type TileModelType { get; protected set; }
    }
}