using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ReportFormatType", Schema = "dbo")]
    public partial class ReportFormatType_DAO : DB_2AM<ReportFormatType_DAO>
    {
        private int _Key;
        private string _description;
        private string _reportServicesFormatType;
        private string _fileExtension;
		private string _contentType;
		private int _displayOrder;

        [PrimaryKey(PrimaryKeyType.Assigned, "ReportFormatTypeKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._Key;
            }
            set
            {
                this._Key = value;
            }
        }

        [Property("Description", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Description is a mandatory field")]
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

        [Property("ReportServicesFormatType", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("ReportServicesFormatType is a mandatory field")]
        public virtual string ReportServicesFormatType
        {
            get
            {
                return this._reportServicesFormatType;
            }
            set
            {
                this._reportServicesFormatType = value;
            }
        }

        [Property("FileExtension", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("FileExtension is a mandatory field")]
        public virtual string FileExtension
        {
            get
            {
                return this._fileExtension;
            }
            set
            {
                this._fileExtension = value;
            }
        }

        [Property("ContentType", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("ContentType is a mandatory field")]
        public virtual string ContentType
        {
            get
            {
                return this._contentType;
            }
            set
            {
                this._contentType = value;
            }
        }

        [Property("DisplayOrder", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("DisplayOrder is a mandatory field")]
        public virtual int DisplayOrder
        {
            get
            {
                return this._displayOrder;
            }
            set
            {
                this._displayOrder = value;
            }
        }


    }
}
