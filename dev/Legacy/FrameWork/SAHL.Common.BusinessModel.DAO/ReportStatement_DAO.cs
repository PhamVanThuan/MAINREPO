using System.Collections.Generic;

using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

//using SAHL.Common.WebServices;
//using SAHL.Common.WebServices.ReportingServices2005;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("ReportStatement", Schema = "dbo", Lazy = true)]
    public partial class ReportStatement_DAO : DB_2AM<ReportStatement_DAO>
    {
        private string _reportName;

        private string _description;

        private string _statementName;

        private string _groupBy;

        private string _orderBy;

        private string _reportOutputPath;

        private int _key;

        // commented, this is a lookup.
        //private IList<Correspondence_DAO> _correspondences;

        // commented, this is a lookup.
        //private IList<CorrespondenceMediumReportStatement> _correspondenceMediumReportStatements;

        private IList<CorrespondenceMedium_DAO> _correspondenceMediums;

        //private IList<ReportParameter_DAO> _reportParameters;

        private Feature_DAO _feature;

        private OriginationSourceProduct_DAO _originationSourceProduct;

        private ReportGroup_DAO _reportGroup;

        private ReportType_DAO _reportType;

        [Property("ReportName", ColumnType = "String")]
        public virtual string ReportName
        {
            get
            {
                return this._reportName;
            }
            set
            {
                this._reportName = value;
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

        [Property("StatementName", ColumnType = "String")]
        public virtual string StatementName
        {
            get
            {
                return this._statementName;
            }
            set
            {
                this._statementName = value;
            }
        }

        [Property("GroupBy", ColumnType = "String")]
        public virtual string GroupBy
        {
            get
            {
                return this._groupBy;
            }
            set
            {
                this._groupBy = value;
            }
        }

        [Property("OrderBy", ColumnType = "String")]
        public virtual string OrderBy
        {
            get
            {
                return this._orderBy;
            }
            set
            {
                this._orderBy = value;
            }
        }

        [Property("ReportOutputPath", ColumnType = "String")]
        public virtual string ReportOutputPath
        {
            get
            {
                return this._reportOutputPath;
            }
            set
            {
                this._reportOutputPath = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Assigned, "ReportStatementKey", ColumnType = "Int32")]
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

        // commented, this is a lookup.
        //[HasMany(typeof(Correspondence_DAO), ColumnKey = "ReportStatementKey", Table = "Correspondence")]
        //public virtual IList<Correspondence_DAO> Correspondences
        //{
        //    get
        //    {
        //        return this._correspondences;
        //    }
        //    set
        //    {
        //        this._correspondences = value;
        //    }
        //}

        // commented, this is a lookup.
        //[HasMany(typeof(CorrespondenceMediumReportStatement), ColumnKey = "ReportStatementKey", Table = "CorrespondenceMediumReportStatement")]
        //public virtual IList<CorrespondenceMediumReportStatement> CorrespondenceMediumReportStatements
        //{
        //    get
        //    {
        //        return this._correspondenceMediumReportStatements;
        //    }
        //    set
        //    {
        //        this._correspondenceMediumReportStatements = value;
        //    }
        //}

        //[HasMany(typeof(ReportParameter_DAO), ColumnKey = "ReportStatementKey", Table = "ReportParameter")]
        //private IList<ReportParameter_DAO> ReportParameters_PVT
        //{
        //    get
        //    {
        //        return this._reportParameters;
        //    }
        //    set
        //    {
        //        this._reportParameters = value;
        //    }
        //}

        [BelongsTo("FeatureKey", NotNull = false)]
        public virtual Feature_DAO Feature
        {
            get
            {
                return this._feature;
            }
            set
            {
                this._feature = value;
            }
        }

        /// <summary>
        /// The OriginationSourceProduct for which this Report is defined.
        /// </summary>
        /// <seealso cref="OriginationSourceProduct_DAO"/>
        [BelongsTo("OriginationSourceProductKey", NotNull = false)]
        public virtual OriginationSourceProduct_DAO OriginationSourceProduct
        {
            get
            {
                return this._originationSourceProduct;
            }
            set
            {
                this._originationSourceProduct = value;
            }
        }

        /// <summary>
        /// a grouping of reports
        /// </summary>
        [BelongsTo("ReportGroupKey", NotNull = false)]
        public virtual ReportGroup_DAO ReportGroup
        {
            get
            {
                return this._reportGroup;
            }
            set
            {
                this._reportGroup = value;
            }
        }

        /// <summary>
        /// The type of this report.
        /// </summary>
        [BelongsTo("ReportTypeKey", NotNull = false)]
        public virtual ReportType_DAO ReportType
        {
            get
            {
                return this._reportType;
            }
            set
            {
                this._reportType = value;
            }
        }

        [HasAndBelongsToMany(typeof(CorrespondenceMedium_DAO), Table = "CorrespondenceMediumReportStatement", ColumnKey = "ReportStatementKey", ColumnRef = "CorrespondenceMediumKey", Lazy = true)]
        public virtual IList<CorrespondenceMedium_DAO> CorrespondenceMediums
        {
            get
            {
                return _correspondenceMediums;
            }
            set
            {
                _correspondenceMediums = value;
            }
        }
    }
}