namespace SAHL.Core.DataStructures
{
    public class NestedTernaryNode
    {
        internal NestedTernaryNode left, center, right, modules, properties, parent;

        [Newtonsoft.Json.JsonProperty(PropertyName = "Char")]
        public char Value { get; protected set; }

        [Newtonsoft.Json.JsonProperty(PropertyName = "Last")]
        public bool IsLast { get; set; }

        [Newtonsoft.Json.JsonProperty(PropertyName = "L")]
        public NestedTernaryNode Left
        {
            get { return this.left; }
            set { this.left = value; }
        }

        [Newtonsoft.Json.JsonProperty(PropertyName = "C")]
        public NestedTernaryNode Center
        {
            get { return this.center; }
            set { this.center = value; }
        }

        [Newtonsoft.Json.JsonProperty(PropertyName = "R")]
        public NestedTernaryNode Right
        {
            get { return this.right; }
            set { this.right = value; }
        }

        [Newtonsoft.Json.JsonProperty(PropertyName = "M")]
        public NestedTernaryNode Modules
        {
            get { return this.modules; }
            set { this.modules = value; }
        }

        [Newtonsoft.Json.JsonProperty(PropertyName = "P")]
        public NestedTernaryNode Properties
        {
            get { return this.properties; }
            set { this.properties = value; }
        }

        public NestedTernaryNode(char value)
        {
            this.Value = value;
            this.IsLast = false;
        }

        public override string ToString()
        {
            if (parent != null)
            {
                return parent.ToString() + this.Value.ToString();
            }
            return this.Value.ToString();
        }
    }
}