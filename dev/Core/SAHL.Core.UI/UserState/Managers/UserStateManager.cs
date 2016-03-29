using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Principal;
using SAHL.Core.BusinessModel;
using SAHL.Core.Caching;
using SAHL.Core.Data;
using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Context;
using SAHL.Core.UI.Data.Models;
using SAHL.Core.UI.Elements.Areas;
using SAHL.Core.UI.Elements.Editors;
using SAHL.Core.UI.Elements.Menus;
using SAHL.Core.UI.Enums;
using SAHL.Core.UI.Models;
using SAHL.Core.UI.UserState.Models;

namespace SAHL.Core.UI.UserState.Managers
{
    public class UserStateManager : IUserStateManager
    {
        private const string ldapAdBaseQuery = "LDAP://DC=SAHL,DC=com";
        private const string ldapUserFilter = "(&(SAMAccountName={0}))";
        private readonly ICache cache;
        private readonly ICacheKeyGenerator cacheKeyGenerator;
        private readonly IDbFactory dbFactory;
        private readonly IEditorManager editorManager;
        private readonly IMenuManager menuManager;
        private readonly ITileManager tileManager;

        public UserStateManager(ICache cache, ICacheKeyGenerator cacheKeyGenerator, IMenuManager menuManager, ITileManager tileManager, IEditorManager editorManager, IDbFactory dbFactory)
        {
            this.cache = cache;
            this.cacheKeyGenerator = cacheKeyGenerator;
            this.menuManager = menuManager;
            this.tileManager = tileManager;
            this.editorManager = editorManager;
            this.dbFactory = dbFactory;
        }

        public MenuElementArea GetOrCreateMenuForUser(IPrincipal user)
        {
            string cacheKey = cacheKeyGenerator.GetKey<IPrincipal, MenuElementArea>(user);
            var menu = this.cache.GetItem<MenuElementArea>(cacheKey);

            if (menu != null)
            {
                return menu;
            }
            menu = this.menuManager.CreateMenuForUser(user);
            this.cache.AddItem(cacheKey, menu);

            return menu;
        }

        public IUserDetails GetUserDetailsForUser(IPrincipal user)
        {
            string cacheKey = cacheKeyGenerator.GetKey<IPrincipal, IUserDetails>(user);
            var userDetails = this.cache.GetItem<IUserDetails>(cacheKey);

            if (userDetails != null)
            {
                return userDetails;
            }
            var directoryEntry = new DirectoryEntry(ldapAdBaseQuery, "sahl\\Halouser", "Natal123");
            var directorySearcher = new DirectorySearcher(directoryEntry);
            directorySearcher.PropertiesToLoad.Add("displayname");
            directorySearcher.PropertiesToLoad.Add("mail");
            directorySearcher.PropertiesToLoad.Add("thumbnailPhoto");

            int start = user.Identity.Name.LastIndexOf("\\");
            string adName = user.Identity.Name.Substring(start + 1, user.Identity.Name.Length - start - 1);

            directorySearcher.Filter = string.Format(ldapUserFilter, adName);
            var userData = directorySearcher.FindOne();

            string displayName = userData.Properties["displayname"][0].ToString();
            string emailAddress = userData.Properties["mail"][0].ToString();

            Image image = null;
            if (userData.Properties.Contains("thumbnailPhoto"))
            {
                var bytes = userData.Properties["thumbnailPhoto"][0] as byte[];
                if (bytes != null)
                {
                    using (var ms = new MemoryStream(bytes))
                    {
                        image = Image.FromStream(ms);
                    }
                }
            }

            userDetails = new UserDetails(user.Identity.Name, displayName, emailAddress, image);

            // get the users organisation roles
            IEnumerable<Data.Models.UserRole> userRoles;
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var usrRoleStatement = new UserRoleStatement(user.Identity.Name);
                userRoles = db.Select(usrRoleStatement);
            }
            if (userRoles != null)
            {
                UrlConfiguration urlConfig = new UrlConfiguration(UrlNames.CommonController, UrlNames.ChangeUserRole, UrlAction.LinkNavigation);
                foreach (var userRole in userRoles)
                {
                    userDetails.AddUserRole(userRole.OrganisationArea, userRole.RoleName, urlConfig.GenerateActionUrl());
                }
            }

            this.cache.AddOrSetItem(cacheKey, userDetails);

            return userDetails;
        }

        public IEnumerable<FeatureAccess> GetFeatureAccessForUser(IPrincipal user)
        {
            string cacheKey = cacheKeyGenerator.GetKey<IPrincipal, IEnumerable<FeatureAccess>>(user);
            var featureAccess = this.cache.GetItem<IEnumerable<FeatureAccess>>(cacheKey);

            if (featureAccess != null)
            {
                return featureAccess;
            }
            var directoryEntry = new DirectoryEntry(ldapAdBaseQuery, "sahl\\Halouser", "Natal123");
            var directorySearcher = new DirectorySearcher(directoryEntry);

            int start = user.Identity.Name.LastIndexOf("\\");
            string adName = user.Identity.Name.Substring(start + 1, user.Identity.Name.Length - start - 1);

            directorySearcher.Filter = string.Format(ldapUserFilter, adName);
            var userData = directorySearcher.FindOne();

            IEnumerable<string> groups = ExtractGroups(userData.Properties["memberof"]);

            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var featureAccessStatement = new FeatureAccessStatement(groups);
                featureAccess = db.Select(featureAccessStatement);
            }

            return featureAccess;
        }

        public TileElementArea GetUsersTilesForContext(IPrincipal user, TileBusinessContext tileBusinessContext)
        {
            // no tiles currently exist for the user in this business context so go load them
            IUserPrincipal userPrincipal = this.GetUserDetailsForUser(user);
            var tileGrid = this.tileManager.LoadUserTilesForBusinessContext(userPrincipal, tileBusinessContext);

            var updatedTileBusinessContext = UpdateTileBusinessContext(tileBusinessContext, tileGrid.MajorTileArea.MajorTile.TileModelType, tileGrid.MajorTileArea.MajorTile.TileConfigurationType);

            // before returning update the context menu elements to reflect the newly loaded tile business context
            this.AddUserContextMenuItemForCurrentMajorTile(user, updatedTileBusinessContext);

            // remove items from the context that come after the selected context
            this.StripUserContextMenuItemsForCurrentMajorTile(user, updatedTileBusinessContext);

            return tileGrid;
        }

        public TileElementArea DrillDownAndGetUsersTilesForContext(IPrincipal user, TileBusinessContext tileBusinessContext)
        {
            IUserPrincipal userPrincipal = this.GetUserDetailsForUser(user);
            var tileGrid = this.tileManager.DrillDownAndLoadUserTilesForBusinessContext(userPrincipal, tileBusinessContext);
            TileBusinessContext updatedTileBusinessContext;
            if (tileGrid != null)
            {
                updatedTileBusinessContext = UpdateTileBusinessContext(tileBusinessContext, tileGrid.MajorTileArea.MajorTile.TileModelType, tileGrid.MajorTileArea.MajorTile.TileConfigurationType);
            }
            else
            {
                updatedTileBusinessContext = tileBusinessContext;
            }

            // before returning the grid update the context menu elements
            this.AddUserContextMenuItemForCurrentMajorTile(user, updatedTileBusinessContext);

            return tileGrid;
        }

        public EditorElement ShowEditorForUser(IPrincipal user, TileBusinessContext tileBusinessContext)
        {
            // ask the editor manager for the editor element for the passed in tile
            EditorElement editorElement = this.editorManager.GetEditor(tileBusinessContext);

            EditorBusinessContext editorBusinessContext = new EditorBusinessContext(tileBusinessContext, editorElement.EditorModelType, editorElement.EditorModelConfigurationType);

            // create an instance of editorstate to manage this editors lifetime
            EditorState editorState = this.editorManager.CreateEditorState(editorBusinessContext);

            // store the editor against the user
            string cacheKey = cacheKeyGenerator.GetKey<IPrincipal, EditorState>(user);
            this.cache.AddOrSetItem(cacheKey, editorState);

            return editorElement;
        }

        public IUIEditorPageModel GetEditorPageContentForUser(IPrincipal user, EditorBusinessContext editorBusinessContext)
        {
            string cacheKey = cacheKeyGenerator.GetKey<IPrincipal, EditorState>(user);
            EditorState editorState = this.cache.GetItem<EditorState>(cacheKey);

            if (editorState.CurrentPage == null)
            {
                // instantiate the first page
                this.editorManager.SetFirstPageModelOnEditorState(editorState);
            }

            return editorState.CurrentPage;
        }

        public IUIEditorPageModel GetPreviousEditorPageContentForUser(IPrincipal user, EditorBusinessContext editorBusinessContext)
        {
            string cacheKey = cacheKeyGenerator.GetKey<IPrincipal, EditorState>(user);
            EditorState editorState = this.cache.GetItem<EditorState>(cacheKey);

            this.editorManager.SetPreviousPageModelOnEditorState(editorState);

            return editorState.CurrentPage;
        }

        public IEnumerable<IUIValidationResult> SubmitEditorPageForUser(IPrincipal user, EditorBusinessContext editorBusinessContext, IEditorPageModel pageModelToValidate)
        {
            string cacheKey = cacheKeyGenerator.GetKey<IPrincipal, EditorState>(user);
            EditorState editorState = this.cache.GetItem<EditorState>(cacheKey);

            return this.editorManager.SubmitPageModelOnEditorState(editorState, pageModelToValidate);
        }

        public IEnumerable<IUIValidationResult> SubmitEditorForUser(IPrincipal user, EditorBusinessContext editorBusinessContext, IEditorPageModel finalPageModel)
        {
            string editorStateCacheKey = cacheKeyGenerator.GetKey<IPrincipal, EditorState>(user);
            EditorState editorState = this.cache.GetItem<EditorState>(editorStateCacheKey);

            // perform the action on editor
            var submitResults = this.editorManager.SubmitPageModelOnEditorState(editorState, finalPageModel);

            // if there are no errors remove the editor from cache
            if (!submitResults.Any())
            {
                this.cache.RemoveItem(editorStateCacheKey);
            }
            return submitResults;
        }

        private IEnumerable<string> ExtractGroups(ResultPropertyValueCollection groups)
        {
            foreach (string groupQuery in groups)
            {
                var groupDirectoryEntry = new DirectoryEntry("LDAP://" + groupQuery);
                var groupDirectorySearcher = new DirectorySearcher(groupDirectoryEntry);
                groupDirectorySearcher.PropertiesToLoad.Add("cn");
                var result = groupDirectorySearcher.FindOne();
                string group = result.Properties["cn"][0].ToString();
                yield return group;
            }
        }

        private void AddUserContextMenuItemForCurrentMajorTile(IPrincipal user, TileBusinessContext tileBusinessContext)
        {
            var menu = this.GetOrCreateMenuForUser(user);

            var contextMenuItemElements = menu.ContextBar.MenuItemElements.OfType<TileDynamicContextMenuItemElement>();
            if (contextMenuItemElements.Any(x => x.TileBusinessContext == tileBusinessContext))
            {
                return;
            }
            UrlConfiguration urlConfig = new UrlConfiguration(UrlNames.TileController, UrlNames.GetUsersTilesForContextAction, UrlAction.TileDrillDown);
            // get the major tile configuration for the context
            string contextDescription = this.tileManager.GetContextDescriptionForMajorTileContext(tileBusinessContext);
            menu.ContextBar.AddContextMenuElement(new TileDynamicContextMenuItemElement(contextDescription, tileBusinessContext, urlConfig.GenerateActionUrl()));
        }

        private void StripUserContextMenuItemsForCurrentMajorTile(IPrincipal user, TileBusinessContext tileBusinessContext)
        {
            var menu = this.GetOrCreateMenuForUser(user);

            var menuElements = menu.ContextBar.MenuItemElements.OfType<TileDynamicContextMenuItemElement>().Where(x => x.BusinessContext != null);

            var indexElement = menuElements.SingleOrDefault(x => x.TileBusinessContext == tileBusinessContext);
            if (indexElement != null)
            {
                menu.ContextBar.RemoveItemsAfterMenuElement(indexElement);
            }
        }

        private TileBusinessContext UpdateTileBusinessContext(BusinessContext businessContext, Type tileModelType, Type tileConfigurationType)
        {
            return new TileBusinessContext(businessContext, tileModelType, tileConfigurationType);
        }
    }
}