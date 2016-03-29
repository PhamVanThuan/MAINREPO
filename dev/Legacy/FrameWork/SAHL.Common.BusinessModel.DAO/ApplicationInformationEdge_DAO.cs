using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("OfferInformationEdge", Schema = "dbo")]
    public partial class ApplicationInformationEdge_DAO : DB_2AM<ApplicationInformationEdge_DAO>
    {
        private double _fullTermInstalment;

        private double _amortisationTermInstalment;

        private double _interestOnlyInstalment;

        private int _interestOnlyTerm;

        private int _applicationInformationKey;

        private ApplicationInformation_DAO _applicationInformation;

        /// <summary>
        /// Primary Key. This is also a foreign key reference to the OfferInformation table.
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Foreign, Column = "OfferInformationKey")]
        public virtual int Key
        {
            get { return _applicationInformationKey; }
            set { _applicationInformationKey = value; }
        }

        [Property("FullTermInstalment", ColumnType = "Double", NotNull = true)]
        public virtual double FullTermInstalment
        {
            get
            {
                return this._fullTermInstalment;
            }
            set
            {
                this._fullTermInstalment = value;
            }
        }

        [Property("AmortisationTermInstalment", ColumnType = "Double", NotNull = true)]
        public virtual double AmortisationTermInstalment
        {
            get
            {
                return this._amortisationTermInstalment;
            }
            set
            {
                this._amortisationTermInstalment = value;
            }
        }

        [Property("InterestOnlyInstalment", ColumnType = "Double", NotNull = true)]
        public virtual double InterestOnlyInstalment
        {
            get
            {
                return this._interestOnlyInstalment;
            }
            set
            {
                this._interestOnlyInstalment = value;
            }
        }

        [Property("InterestOnlyTerm", ColumnType = "Int32", NotNull = true)]
        public virtual int InterestOnlyTerm
        {
            get
            {
                return this._interestOnlyTerm;
            }
            set
            {
                this._interestOnlyTerm = value;
            }
        }

        [OneToOne]
        public virtual ApplicationInformation_DAO ApplicationInformation
        {
            get
            {
                return this._applicationInformation;
            }
            set
            {
                this._applicationInformation = value;
            }
        }

        public virtual void Clone(ApplicationInformationEdge_DAO Edge)
        {
            Edge.AmortisationTermInstalment = this.AmortisationTermInstalment;
            Edge.FullTermInstalment = this.FullTermInstalment;
            Edge.InterestOnlyInstalment = this.InterestOnlyInstalment;
            Edge.InterestOnlyTerm = this.InterestOnlyTerm;
        }
    }
}