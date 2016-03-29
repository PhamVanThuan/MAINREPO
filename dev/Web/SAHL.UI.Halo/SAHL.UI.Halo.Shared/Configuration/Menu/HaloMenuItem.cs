using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public abstract class HaloMenuItem : IHaloMenuItem
    {
        protected HaloMenuItem(string name, string moduleName, int sequence = 0, bool isTileBased = true, string nonTilePageState = "")
        {
            if (string.IsNullOrWhiteSpace(name)) { throw new ArgumentNullException("name"); }
            if (string.IsNullOrWhiteSpace(moduleName)) { throw new ArgumentNullException("moduleName"); }

            this.Name             = name;
            this.ModuleName       = moduleName;
            this.Sequence         = sequence;
            this.IsTileBased      = isTileBased;
            this.NonTilePageState = nonTilePageState;
        }

        public string Name { get; private set; }
        public int Sequence { get; private set; }
        public string ModuleName { get; private set; }
        public bool IsTileBased { get; private set; }
        public string NonTilePageState { get; private set; }

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

            var roleAttribute = roleAttributes.Any(attribute => attribute.Capabilities.Any(capability => capabilities.Contains(capability)));
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
