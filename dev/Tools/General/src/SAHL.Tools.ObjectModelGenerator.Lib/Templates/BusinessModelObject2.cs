using System;
using System.Data;
using System.Linq;
using System.Text;

namespace SAHL.Tools.ObjectModelGenerator.Lib.Templates
{
    public partial class BusinessModelObject
    {
        public BusinessModelObject(BusinessModelDescription businessModelDescription, string defaultNameSpace)
        {
            this.BusinessModelDescription = businessModelDescription;
            this.Namespace = defaultNameSpace;
        }

        public BusinessModelDescription BusinessModelDescription { get; protected set; }

        public string Namespace { get; protected set; }

        public string PropertiesToParametersWithoutKey()
        {
            StringBuilder sb = new StringBuilder();
            int iteratorCount = 0;
            int totalCount = this.BusinessModelDescription.Properties.Count();
            foreach (var property in this.BusinessModelDescription.Properties)
            {
                iteratorCount++;
                if (!property.IsIdentitySeed)
                {
                    if (property.PropertyName.ToLower() != "id")
                    {
                        sb.Append(string.Format("{0} {1}", property.TypeName, this.ToPascalCase(property.PropertyName)));

                        if (iteratorCount < totalCount)
                        {
                            sb.Append(", ");
                        }
                    }
                }
            }

            string properties = sb.ToString();
            if (properties.EndsWith(", "))
            {
                properties = properties.Substring(0, properties.Length - 2);
            }
            return properties;
        }

        public string PropertiesToParameters()
        {
            StringBuilder sb = new StringBuilder();
            int iteratorCount = 0;
            int totalCount = this.BusinessModelDescription.Properties.Count();
            foreach (var property in this.BusinessModelDescription.Properties)
            {
                iteratorCount++;

                sb.Append(string.Format("{0} {1}", property.TypeName, this.ToPascalCase(property.PropertyName)));

                if (iteratorCount < totalCount)
                {
                    sb.Append(", ");
                }
            }

            return sb.ToString();
        }

        public string ToPascalCase(string original)
        {
            if (string.IsNullOrEmpty(original))
            {
                throw new ArgumentNullException("original");
            }
            original = original.Replace(" ", "_");
            return string.Format("{0}{1}", original.Substring(0, 1).ToLower(), original.Substring(1, original.Length - 1));
        }

        public string EscapeName(string name)
        {
            return name.ToUpper().Replace(" ", "_");
        }

    }
}