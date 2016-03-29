using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using SAHL.Core.BusinessModel;
using SAHL.Core.UI.ApplicationState.Managers;
using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Context;
using SAHL.Core.UI.Elements.Areas;
using SAHL.Core.UI.Elements.Tiles;
using SAHL.Core.UI.Providers.Tiles;
using SAHL.Core.UI.UserState.Models;

namespace SAHL.Core.UI.UserState.Managers
{
    public class TileManager : ITileManager
    {
        private readonly ITileConfigurationManager tileConfigurationManager;

        public TileManager(ITileConfigurationManager tileConfigurationManasger)
        {
            this.tileConfigurationManager = tileConfigurationManasger;
        }

        public TileElementArea LoadUserTilesForBusinessContext(IUserPrincipal user, TileBusinessContext tileBusinessContext)
        {
            IMajorTileConfiguration majorTileConfigurationToLoad;

            // check if we have been provided with an existing tile configuration
            if (tileBusinessContext.TileConfigurationType == null)
            {
                // get the root major tile for this context
                majorTileConfigurationToLoad = this.tileConfigurationManager.GetRootTileConfigurationForContext(new GenericPrincipal(new GenericIdentity(user.AdUserName), 
                                                                                                                                     new string[] {}), tileBusinessContext.Context);
            }
            else
            {
                majorTileConfigurationToLoad = this.tileConfigurationManager.GetTileConfigurationFromType(tileBusinessContext.TileModelType);
            }
            return majorTileConfigurationToLoad == null ? null : this.PopulateTileGrid(user, majorTileConfigurationToLoad, tileBusinessContext);
        }

        public dynamic GetTileContent(TileBusinessContext tileBusinessContext)
        {
            // get the tilecontentprovider from the tile type
            ITileContentProvider contentProvider = this.tileConfigurationManager.GetTileContentProviderForTileModel(tileBusinessContext.TileModelType);

            if (contentProvider == null)
            {
                return null;
            }
            dynamic contentModel = contentProvider.GetContent(tileBusinessContext.BusinessKey);
            return contentModel;
        }

        public TileElementArea DrillDownAndLoadUserTilesForBusinessContext(IUserPrincipal user, TileBusinessContext tileBusinessContext)
        {
            // use the tileConfiguration to find the drilldown configuration
            var majorTileConfig = this.tileConfigurationManager.GetDrillDownConfigurationForClickedTile(tileBusinessContext.TileConfigurationType);

            return this.PopulateTileGrid(user, majorTileConfig, tileBusinessContext);
        }

        public string GetContextDescriptionForMajorTileContext(TileBusinessContext tileBusinessContext)
        {
            var majorTileConfigurationToLoad = this.tileConfigurationManager.GetTileConfigurationFromType(tileBusinessContext.TileModelType);
            return majorTileConfigurationToLoad != null ? majorTileConfigurationToLoad.ContextDescription : string.Empty;
        }

        private TileElementArea PopulateTileGrid(IUserPrincipal user, IMajorTileConfiguration majorTileConfig, TileBusinessContext tileBusinessContext)
        {
            IEnumerable<IChildTileConfiguration> childTileConfigs = this.tileConfigurationManager.GetChildConfigurationsForMajorTile(majorTileConfig);

            List<ChildTileElement> childTiles = new List<ChildTileElement>();
            // now get the dataproviders for each childtileconfig and execute it
            foreach (var childTileConfig in childTileConfigs.OrderBy(x => x.Sequence))
            {
                // get the dataprovider for the child config
                Type tileModelType = childTileConfig.TileModelType;
                ITileDataProvider tileDataProvider = this.tileConfigurationManager.GetTileDataProviderForTileModel(tileModelType);
                if (tileDataProvider == null)
                {
                    continue;
                }
                IEnumerable<BusinessKey> tileInstanceKeys = tileDataProvider.GetTileInstanceKeys(tileBusinessContext.BusinessKey);
                foreach (BusinessKey businessKey in tileInstanceKeys)
                {
                    // check for an alternate before creating
                    Type alternateChildTileModel = this.tileConfigurationManager.GetAlternateTileModelIfConfiguredForUser(childTileConfig.TileModelType, user);
                    if (alternateChildTileModel != null)
                    {
                        childTileConfig.OverrideTileModelType(alternateChildTileModel);
                    }
                    childTiles.Add(childTileConfig.CreateElement(new BusinessContext(tileBusinessContext.Context, businessKey)));
                }
            }

            // get the current actiontiles
            IEnumerable<IActionTileConfiguration> actionTileConfigs = this.tileConfigurationManager.GetActionTileConfigurationsForMajorTile(majorTileConfig);
            List<ActionMiniTileElement> actionTiles = actionTileConfigs.Select(a => a.CreateElement(tileBusinessContext)).ToList();

            TileElementArea tileElementArea = new TileElementArea();

            // check for an alternate before creating
            Type alternateMajorTileModel = this.tileConfigurationManager.GetAlternateTileModelIfConfiguredForUser(majorTileConfig.TileModelType, user);
            if (alternateMajorTileModel != null)
            {
                majorTileConfig.OverrideTileModelType(alternateMajorTileModel);
            }
            tileElementArea.MajorTileArea.MajorTile = majorTileConfig.CreateElement(tileBusinessContext);
            tileElementArea.ChildTileArea.AddChildElements(childTiles);
            tileElementArea.ActionTileArea.AddChildElements(actionTiles);

            return tileElementArea;
        }
    }
}