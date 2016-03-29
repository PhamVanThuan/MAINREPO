using System;
using System.Linq;
using System.Collections.Generic;

namespace SAHL.Tools.ObjectModelGenerator.Lib.Templates
{
    public class BusinessModelDescription
    {
        private List<BusinessModelDescriptionProperty> properties;

        private List<EnumerationPropertyValue> enumerationValues;

        private BusinessModelDescriptionProperty primaryKeyModel;

        public BusinessModelDescription(string className, IEnumerable<BusinessModelDescriptionProperty> properties)
            : this(className, "", "", properties,Enumerable.Empty<EnumerationPropertyValue>())
        {
        }
        
        public BusinessModelDescription(string className, string databaseName, string schemaName, IEnumerable<BusinessModelDescriptionProperty> properties,IEnumerable<EnumerationPropertyValue> enumerationValues)
        {
            this.ClassName = this.ToTitleCase(className);
            this.TableName = className;
            this.DatabaseName = databaseName;
            this.SchemaName = schemaName;
            this.properties = new List<BusinessModelDescriptionProperty>(properties);
            this.enumerationValues = new List<EnumerationPropertyValue>(enumerationValues);


            if (HasPrimaryKeyColumn)
            {
                primaryKeyModel = this.Properties.Where(x => x.IsPrimaryKey == true).SingleOrDefault();
            }
            else if (HasNonPrimaryKeyIdColumn)
            {
                primaryKeyModel = this.Properties.Where(x => x.PropertyName.Equals("ID", StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault();
            }
            else if (HasIdentitySeed)
            {
                primaryKeyModel = this.Properties.Where(x => x.IsIdentitySeed).SingleOrDefault();
            }
        }

        public string ClassName { get; protected set; }

        public string TableName { get; protected set; }

        public string DatabaseName { get; protected set; }

        public string SchemaName { get; protected set; }

        public IEnumerable<BusinessModelDescriptionProperty> Properties { get { return this.properties; } }

        public IEnumerable<EnumerationPropertyValue> EnumerationValues { get { return this.enumerationValues; } }

        public int KeyCount
        {
            get
            {
                return this.Properties.Where(x => x.IsPrimaryKey == true).Select(y => y.PropertyName).Count();
            }
        }

        public string PrimaryKeyProperty
        {
            get
            {
                if (this.primaryKeyModel != null)
                {
                    return primaryKeyModel.PropertyName;
                }
                return string.Empty;
            }
        }

        public string PrimaryKeyPropertyType
        {
            get
            {
                if (this.primaryKeyModel != null)
                {
                    return primaryKeyModel.TypeName;
                }
                return string.Empty;
            }
        }
        
        public string PrimaryKeyType
        {
            get
            {
                return this.Properties.Where(x => x.IsPrimaryKey == true).Select(y => y.TypeName).SingleOrDefault();
            }
        }

        public bool HasIdentitySeed
        {
            get
            {
                return this.Properties.Where(x => x.IsIdentitySeed == true).Count() == 1;
            }
        }

        public bool HasIdentitySeedId
        {
            get
            {
                return this.Properties.Where(x => ((x.IsPrimaryKey == true) && (x.PropertyName == "Id") && (x.TypeName == "int"))).Count() == 1;
            }
        }

        public bool HasPrimaryKeyColumn
        {
            get
            {
                return this.Properties.Where(x => x.IsPrimaryKey == true).Select(y => y.PropertyName).Count() == 1;
            }
        }

        public bool HasNonPrimaryKeyIdColumn
        {
            get
            {
                return this.Properties.Where(x => x.PropertyName.Equals("ID", StringComparison.InvariantCultureIgnoreCase)).Select(y => y.PropertyName).SingleOrDefault() != null;
            }
        }

        public string ToTitleCase(string original)
        {
            if (string.IsNullOrEmpty(original))
            {
                throw new ArgumentNullException("original");
            }
            original = original.Replace(" ", "_");
            return string.Format("{0}{1}", original.Substring(0, 1).ToUpper(), original.Substring(1, original.Length - 1));
        }
    }
}