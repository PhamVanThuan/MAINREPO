using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// ApplicationInformation_DAO is used to link the Application with the various Application Information tables which
    /// hold product specific information regarding the Application.
    /// </summary>
    /// <seealso cref="ApplicationInformationInterestOnly_DAO"/>
    /// <seealso cref="ApplicationInformationQuickCash_DAO"/>
    /// <seealso cref="ApplicationInformationFinancialAdjustment_DAO"/>
    /// <seealso cref="ApplicationInformationSuperLoLoan_DAO"/>
    /// <seealso cref="ApplicationInformationVariableLoan_DAO"/>
    /// <seealso cref="ApplicationInformationVarifixLoan_DAO"/>
    /// <seealso cref="ApplicationInformationPersonalLoan_DAO"/>
    [ActiveRecord("OfferInformation", Schema = "dbo", Lazy = true)]
    public partial class ApplicationInformation_DAO : DB_2AM<ApplicationInformation_DAO>
    {
        private DateTime _applicationInsertDate;

        private Application_DAO _application;

        private int _key;

        private Product_DAO _product;

        private ApplicationInformationPersonalLoan_DAO _applicationInformationPersonalLoan;

        private ApplicationInformationQuickCash_DAO _applicationInformationQuickCash;

        private IList<ApplicationInformationFinancialAdjustment_DAO> _applicationInformationFinancialAdjustments;

        private ApplicationInformationVariableLoan_DAO _applicationInformationVariableLoan;

        private ApplicationInformationVarifixLoan_DAO _applicationInformationVarifixLoan;

        private ApplicationInformationSuperLoLoan_DAO _applicationInformationSuperLoLoan;

        private ApplicationInformationType_DAO _applicationInformationType;

        private ApplicationInformationInterestOnly_DAO _applicationInformationInterestOnly;

        private ApplicationInformationEdge_DAO _applicationInformationEdge;

        /// <summary>
        /// Date when the Application Information records were inserted.
        /// </summary>
        [Property("OfferInsertDate")] //, ColumnType = "Timestamp")]
        public virtual DateTime ApplicationInsertDate
        {
            get
            {
                return this._applicationInsertDate;
            }
            set
            {
                this._applicationInsertDate = value;
            }
        }

        /// <summary>
        /// The Application to which the ApplicationInformation belongs.
        /// </summary>
        [BelongsTo("OfferKey", NotNull = true)]
        [ValidateNonEmpty("Application is a mandatory field")]
        public virtual Application_DAO Application
        {
            get
            {
                return this._application;
            }
            set
            {
                this._application = value;
            }
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native, "OfferInformationKey", ColumnType = "Int32")]
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

        /// <summary>
        /// Each Application Information record belongs to a particular product.
        /// </summary>
        [BelongsTo("ProductKey", NotNull = true)]
        [ValidateNonEmpty("Product is a mandatory field")]
        public virtual Product_DAO Product
        {
            get
            {
                return this._product;
            }
            set
            {
                this._product = value;
            }
        }

        [Lurker]
        [OneToOne(Cascade = CascadeEnum.All)]
        public virtual ApplicationInformationPersonalLoan_DAO ApplicationInformationPersoanlLoan
        {
            get
            {
                return _applicationInformationPersonalLoan;
            }
            set
            {
                _applicationInformationPersonalLoan = value;
            }
        }

        [Lurker]
        [OneToOne(Cascade = CascadeEnum.All)]
        public virtual ApplicationInformationQuickCash_DAO ApplicationInformationQuickCash
        {
            get
            {
                return this._applicationInformationQuickCash;
            }
            set
            {
                this._applicationInformationQuickCash = value;
            }
        }

        /// <summary>
        /// An Application Information record can have many FinancialAdjustmentTypeSources associated to it.
        /// These FinancialAdjustmentTypeSources can be found in the OfferInformationFinancialAdjustmentTypeSource table.
        /// </summary>
        [HasMany(typeof(ApplicationInformationFinancialAdjustment_DAO), ColumnKey = "OfferInformationKey", Table = "OfferInformationFinancialAdjustment", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<ApplicationInformationFinancialAdjustment_DAO> ApplicationInformationFinancialAdjustments
        {
            get
            {
                if (_applicationInformationFinancialAdjustments == null)
                    _applicationInformationFinancialAdjustments = new List<ApplicationInformationFinancialAdjustment_DAO>();
                return this._applicationInformationFinancialAdjustments;
            }
            set
            {
                this._applicationInformationFinancialAdjustments = value;
            }
        }

        [Lurker]
        [OneToOne(Cascade = CascadeEnum.All)]
        public virtual ApplicationInformationVariableLoan_DAO ApplicationInformationVariableLoan
        {
            get
            {
                return this._applicationInformationVariableLoan;
            }
            set
            {
                this._applicationInformationVariableLoan = value;
            }
        }

        [Lurker]
        [OneToOne(Cascade = CascadeEnum.All)]
        public virtual ApplicationInformationEdge_DAO ApplicationInformationEdge
        {
            get
            {
                return this._applicationInformationEdge;
            }
            set
            {
                this._applicationInformationEdge = value;
            }
        }

        [Lurker]
        [OneToOne(Cascade = CascadeEnum.All)]
        public virtual ApplicationInformationVarifixLoan_DAO ApplicationInformationVarifixLoan
        {
            get
            {
                return this._applicationInformationVarifixLoan;
            }
            set
            {
                this._applicationInformationVarifixLoan = value;
            }
        }

        [Lurker]
        [OneToOne(Cascade = CascadeEnum.All)]
        public virtual ApplicationInformationSuperLoLoan_DAO ApplicationInformationSuperLoLoan
        {
            get
            {
                return this._applicationInformationSuperLoLoan;
            }
            set
            {
                this._applicationInformationSuperLoLoan = value;
            }
        }

        [Lurker]
        [OneToOne(Cascade = CascadeEnum.All)]
        public virtual ApplicationInformationInterestOnly_DAO ApplicationInformationInterestOnly
        {
            get { return _applicationInformationInterestOnly; }
            set { _applicationInformationInterestOnly = value; }
        }

        /// <summary>
        /// Each Application Information record has a particular type, which is used to control the revisions which the Application
        /// Information records will undergo. This is defined in the OfferInformationType table and holds values such as Original Offer
        /// /Revised Offer/Accepted Offer
        /// </summary>
        [BelongsTo("OfferInformationTypeKey", NotNull = true)]
        [ValidateNonEmpty("Application Information Type is a mandatory field")]
        public virtual ApplicationInformationType_DAO ApplicationInformationType
        {
            get
            {
                return this._applicationInformationType;
            }
            set
            {
                this._applicationInformationType = value;
            }
        }
    }
}