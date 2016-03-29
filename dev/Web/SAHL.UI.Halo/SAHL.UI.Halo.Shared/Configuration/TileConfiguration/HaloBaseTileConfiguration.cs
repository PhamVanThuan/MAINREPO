using System;
using System.Linq;
using System.Collections.Generic;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public abstract class HaloBaseTileConfiguration  : IHaloTileConfiguration
    {
        protected HaloBaseTileConfiguration(string tileName, string tileType, int sequence = 0, bool isTileBased = true)
        {
            if (string.IsNullOrWhiteSpace(tileName)) { throw new ArgumentNullException("tileName"); }
            if (string.IsNullOrWhiteSpace(tileType)) { throw new ArgumentNullException("tileType"); }

            this.Name        = tileName;
            this.TileType    = tileType;
            this.Sequence    = sequence;
            this.IsTileBased = isTileBased;
        }

        public string Name { get; protected set; }
        public string TileType { get; protected set; }
        public int Sequence { get; protected set; }
        public bool IsTileBased { get; protected set; }
        public string NonTilePageState { get; set; }

        public Type GetTileModelType()
        {
            var tileModelInterface = this.GetType().GetInterfaces().SingleOrDefault(x => x.Name.StartsWith("IHaloTileModel`1"));
            return tileModelInterface == null 
                        ? null 
                        : tileModelInterface.GenericTypeArguments[0];
        }

        public bool IsDynamicTile()
        {
            var interfaces       = this.GetType().GetInterfaces();
            var dynamicInterface = interfaces.FirstOrDefault(type => type.Name.StartsWith("IHaloDynamicRootTileConfiguration"));

            return dynamicInterface != null;
        }

        public IEnumerable<string> GetAllRoleNames()
        {
            var roleAttributes = this.GetAllRoleAttributes();
            if (roleAttributes == null) { return null; }

            var allRoleNames = roleAttributes.Cast<HaloRoleAttribute>()
                                             .Select(roleAttribute => roleAttribute.RoleName)
                                             .ToList();
            return allRoleNames;
        }

        public bool IsInRole(string roleName)
        {
            var roleAttributes = this.GetAllRoleAttributes();
            if (roleAttributes == null) { return true; }

            var roleAttribute = roleAttributes.FirstOrDefault(attribute => attribute.RoleName == roleName);
            return roleAttribute != null;
        }

        public bool IsInCapability(string[] capabilities)
        {
            var roleAttributes = this.GetAllRoleAttributes();
            if (roleAttributes == null) { return true; }

            var roleAttribute = roleAttributes.Any(attribute => attribute.Capabilities.Any(capability=>capabilities.Contains(capability)));
            return roleAttribute;
        }

        private IEnumerable<HaloRoleAttribute> GetAllRoleAttributes()
        {
            var roleAttributes = this.GetType().GetCustomAttributes(typeof(HaloRoleAttribute), true);
            return !roleAttributes.Any() 
                ? null 
                : roleAttributes.Cast<HaloRoleAttribute>();
        }
    }
}
