using SAHL.Core.BusinessModel;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.UI.Elements.Menus;
using SAHL.Core.UI.Elements.Parts;
using SAHL.Core.UI.UserState.Managers;
using SAHL.Services.Interfaces.Halo.Commands;
using System;
using System.Linq;
using System.Security.Principal;

namespace SAHL.Services.Halo.CommandHandlers
{
    public class AddRibbonMeniItemCommandHandler : IServiceCommandHandler<AddRibbonMenuItemCommand>
    {
        private IUserStateManager userStateManager;

        public AddRibbonMeniItemCommandHandler(IUserStateManager userStateManager)
        {
            this.userStateManager = userStateManager;
        }

        public ISystemMessageCollection HandleCommand(AddRibbonMenuItemCommand command)
        {
            var usermenu = this.userStateManager.GetOrCreateMenuForUser(new GenericPrincipal(new GenericIdentity(command.UserName), new string[] { }));

            var ribbonMenuItem = GetRibbonMenuItem(command.BusinessKey, command.BusinessKeyType, command.DisplayName, command.Url, command.Context);

            if (!usermenu.RibbonBar.RibbonMenuItemElements.Where(x => x.BusinessContext == ribbonMenuItem.BusinessContext).Any())
            {
                usermenu.RibbonBar.AddRibbonMenuElement(ribbonMenuItem);
            }
            usermenu.RibbonBar.Select(ribbonMenuItem);
            usermenu.ContextBar.ClearItems();
            command.Result = ribbonMenuItem.BusinessContext;
            return new SystemMessageCollection();
        }

        private DynamicRibbonMenuItemElement GetRibbonMenuItem(long businessKey, BusinessKeyType businessKeyType, string description, string url, string context)
        {
            return CreateRibbonMenuItem(new BusinessContext(context, businessKeyType, businessKey), description, url);
        }

        private DynamicRibbonMenuItemElement CreateRibbonMenuItem(BusinessContext businessContext, string description, string url)
        {
            var ribbonItem = new DynamicRibbonMenuItemElement(businessContext, url);
            ribbonItem.AddPart(new StaticTextPart(description));
            ribbonItem.AddPart(new IconPart("remove"));
            return ribbonItem;
        }
    }
}