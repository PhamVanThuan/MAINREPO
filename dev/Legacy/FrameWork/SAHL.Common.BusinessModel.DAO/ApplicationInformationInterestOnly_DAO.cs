using System;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// ApplicationInformationInterestOnly_DAO is instantiated in order to retrieve those details specific to an Interest Only
    /// Application.
    /// </summary>
    //[GenericTest(Globals.TestType.Find)]
    [ActiveRecord("OfferInformationInterestOnly", Schema = "dbo", Lazy = true)]
    public partial class ApplicationInformationInterestOnly_DAO : DB_2AM<ApplicationInformationInterestOnly_DAO>
    {
        private double? _installment;

        private DateTime? _maturityDate;

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
        /// The Interest Only Instalment which does not take into account the client repaying any capital as part of their
        /// monthly instalment.
        /// </summary>
        [Property("Installment", ColumnType = "Double")]
        public virtual double? Installment
        {
            get
            {
                return this._installment;
            }
            set
            {
                this._installment = value;
            }
        }

        /// <summary>
        /// The Interest Only Maturity Date is the date at which the client is required to repay any outstanding capital.
        /// </summary>
        [Property("MaturityDate")]
        public virtual DateTime? MaturityDate
        {
            get
            {
                return this._maturityDate;
            }
            set
            {
                this._maturityDate = value;
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

        public virtual void Clone(ApplicationInformationInterestOnly_DAO IO)
        {
            IO.Installment = this.Installment;
            IO.MaturityDate = this.MaturityDate;
        }
    }
}