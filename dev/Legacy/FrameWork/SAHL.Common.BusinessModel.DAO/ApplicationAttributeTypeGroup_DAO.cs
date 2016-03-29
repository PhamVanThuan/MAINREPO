using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("OfferAttributeTypeGroup", Schema = "dbo")]
    public class ApplicationAttributeTypeGroup_DAO : DB_2AM<ApplicationAttributeTypeGroup_DAO>
    {
        private int _key;

        private string _description;

        private IList<ApplicationAttributeType_DAO> _applicationAttributeTypes;

        [PrimaryKey(PrimaryKeyType.Assigned, "OfferAttributeTypeGroupKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._key;
            }
            set
            {
                this._key = value;
            }
        }

        [Property("Description", ColumnType = "String")]
        public virtual string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }
    }
}