using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SAHL.Core;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Shared
{
    public class TileDataRepository : ITileDataRepository
    {
        private readonly IIocContainer iocContainer;

        public TileDataRepository(IIocContainer iocContainer)
        {
            if (iocContainer == null) { throw new ArgumentNullException("iocContainer"); }
            this.iocContainer = iocContainer;
        }

        public IHaloTileModel FindTileDataModel<T>(T tileConfiguration) where T : class, IHaloTileConfiguration
        {
            var tileModelInterface = tileConfiguration.GetType().GetInterfaces().SingleOrDefault(x => x.Name.StartsWith("IHaloTileModel"));
            if (tileModelInterface == null) { return null; }

            var dataModelType = tileModelInterface.GenericTypeArguments[0];
            return Activator.CreateInstance(dataModelType) as IHaloTileModel;
        }

        public IHaloTileState FindTilePageState<T>(T tileConfiguration) where T : class, IHaloTileConfiguration
        {
            var tilePageState = tileConfiguration.GetType().GetInterfaces().SingleOrDefault(x => x.Name.StartsWith("IHaloTilePageState"));
            if (tilePageState == null) { return null; }

            var tileStateType = tilePageState.GenericTypeArguments[0];
            return Activator.CreateInstance(tileStateType) as IHaloTileState;
        }

        public IHaloTileChildDataProvider FindTileChildDataProvider<T>(T tileConfiguration) where T : IHaloTileConfiguration
        {
            var tileType = tileConfiguration.GetType();
            var contentProvider = typeof(IHaloTileChildDataProvider<>);
            var genericType = contentProvider.MakeGenericType(tileType);

            var tileContentProvider = iocContainer.GetInstance(genericType);
            return tileContentProvider as IHaloTileChildDataProvider;
        }

        public IHaloTileDynamicDataProvider FindTileDynamicDataProvider<T>(T tileConfiguration) where T : IHaloTileConfiguration
        {
            var tileType    = tileConfiguration.GetType();
            var provider    = typeof(IHaloTileDynamicDataProvider<>);
            var genericType = provider.MakeGenericType(tileType);

            var dynamicProvider = iocContainer.GetInstance(genericType);
            return dynamicProvider as IHaloTileDynamicDataProvider;
        }

        public IHaloTileContentDataProvider FindTileContentDataProvider<T>(T tileConfiguration) where T : IHaloTileConfiguration
        {
            var tileType = tileConfiguration.GetTileModelType();
            if (tileType == null) { return null; }

            var contentProvider = typeof(IHaloTileContentDataProvider<>);
            var genericType = contentProvider.MakeGenericType(tileType);

            var tileContentProvider = iocContainer.GetInstance(genericType);
            return tileContentProvider as IHaloTileContentDataProvider;
        }

        public IHaloTileContentMultipleDataProvider FindTileContentMultipleDataProvider<T>(T tileConfiguration) where T : IHaloTileConfiguration
        {
            var tileType = tileConfiguration.GetTileModelType();
            if (tileType == null) { return null; }

            var contentProvider = typeof(IHaloTileContentMultipleDataProvider<>);
            var genericType = contentProvider.MakeGenericType(tileType);

            var tileContentProvider = iocContainer.GetInstance(genericType);
            return tileContentProvider as IHaloTileContentMultipleDataProvider;
        }

        public IHaloTileEditorDataProvider FindTileEditorDataProvider<T>(T tileConfiguration) where T : IHaloTileConfiguration
        {
            var tileType = tileConfiguration.GetTileModelType();
            if (tileType == null) { return null; }

            var contentProvider = typeof(IHaloTileEditorDataProvider<>);
            var genericType = contentProvider.MakeGenericType(tileType);

            var tileEditorProvider = iocContainer.GetInstance(genericType);
            return tileEditorProvider as IHaloTileEditorDataProvider;
        }
    }
}
