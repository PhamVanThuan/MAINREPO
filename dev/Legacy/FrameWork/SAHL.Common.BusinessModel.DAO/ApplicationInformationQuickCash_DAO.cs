using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// ApplicationInformationQuickCash_DAO is instantiated in order to retrieve information regarding the Quick Cash Application
    /// associated to the Mortgage Loan Application.
    /// </summary>
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("OfferInformationQuickCash", Schema = "dbo", Lazy = true)]
    public partial class ApplicationInformationQuickCash_DAO : DB_2AM<ApplicationInformationQuickCash_DAO>
    {
        private double _creditApprovedAmount;

        private int _term;

        private double _creditUpfrontApprovedAmount;

        private IList<ApplicationInformationQuickCashDetail_DAO> _applicationInformationQuickCashDetails;

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

        /// <summary>
        /// The total amount of Quick Cash which Credit has approved the client for.
        /// </summary>
        [Property("CreditApprovedAmount", ColumnType = "Double")]
        public virtual double CreditApprovedAmount
        {
            get
            {
                return this._creditApprovedAmount;
            }
            set
            {
                this._creditApprovedAmount = value;
            }
        }

        /// <summary>
        /// The Term of the Quick Cash loan.
        /// </summary>
        [Property("Term", ColumnType = "Int32")]
        public virtual int Term
        {
            get
            {
                return this._term;
            }
            set
            {
                this._term = value;
            }
        }

        /// <summary>
        /// The amount that Credit has approved as the maximum allowed for the Up Front payment to the Client.
        /// </summary>
        [Property("CreditUpfrontApprovedAmount", ColumnType = "Double")]
        public virtual double CreditUpfrontApprovedAmount
        {
            get
            {
                return this._creditUpfrontApprovedAmount;
            }
            set
            {
                this._creditUpfrontApprovedAmount = value;
            }
        }

        /// <summary>
        /// A OfferInformationQuickCash record has many records associated to it in the OfferInformationQuickCashDetails table.
        /// </summary>
        [HasMany(typeof(ApplicationInformationQuickCashDetail_DAO), ColumnKey = "OfferInformationKey", Table = "OfferInformationQuickCashDetail", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<ApplicationInformationQuickCashDetail_DAO> ApplicationInformationQuickCashDetails
        {
            get
            {
                return this._applicationInformationQuickCashDetails;
            }
            set
            {
                this._applicationInformationQuickCashDetails = value;
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

        public virtual void Clone(ApplicationInformationQuickCash_DAO QC)
        {
            //QC.ApplicationInformationQuickCashDetails;
            QC.ApplicationInformationQuickCashDetails = new List<ApplicationInformationQuickCashDetail_DAO>(this.ApplicationInformationQuickCashDetails);
            QC.CreditApprovedAmount = this.CreditApprovedAmount;
            QC.CreditUpfrontApprovedAmount = this.CreditUpfrontApprovedAmount;
            QC.Term = this.Term;
        }
    }
}