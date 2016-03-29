namespace SAHL.Core.UI.Halo.Common
{
    public class NamedKey
    {
        public NamedKey(string name, int key)
        {
            this.Name = name;
            this.Key = key;
        }

        public int Key { get; set; }

        public string Name { get; set; }
    }
}