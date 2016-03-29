using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("EmploymentType", Schema = "dbo")]
    public partial class EmploymentType_DAO : DB_2AM<EmploymentType_DAO>
    {
        private string _description;

        private int _key;

        // Commented, this is a lookup.
        /* private IList<CreditCriteria_DAO> _creditCriterias;

         private IList<Employment> _employments;

         private IList<OfferInformation_DAO> _applicationInformations;

         private IList<OfferInformationVariableLoan_DAO> _applicationInformationVariableLoans;
         */

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

        [PrimaryKey(PrimaryKeyType.Assigned, "EmploymentTypeKey", ColumnType = "Int32")]
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

        /*
        [HasMany(typeof(CreditCriteria_DAO), ColumnKey = "EmploymentTypeKey", Table = "CreditCriteria")]
        public virtual IList<CreditCriteria_DAO> CreditCriterias
        {
            get
            {
                return this._creditCriterias;
            }
            set
            {
                this._creditCriterias = value;
            }
        }

        [HasMany(typeof(Employment), ColumnKey = "EmploymentTypeKey", Table = "Employment")]
        public virtual IList<Employment> Employments
        {
            get
            {
                return this._employments;
            }
            set
            {
                this._employments = value;
            }
        }

        [HasMany(typeof(OfferInformation_DAO), ColumnKey = "EmploymentTypeKey", Table = "OfferInformation")]
        public virtual IList<OfferInformation_DAO> ApplicationInformations
        {
            get
            {
                return this._applicationInformations;
            }
            set
            {
                this._applicationInformations = value;
            }
        }

        [HasMany(typeof(OfferInformationVariableLoan_DAO), ColumnKey = "EmploymentTypeKey", Table = "OfferInformationVariableLoan")]
        public virtual IList<OfferInformationVariableLoan_DAO> ApplicationInformationVariableLoans
        {
            get
            {
                return this._applicationInformationVariableLoans;
            }
            set
            {
                this._applicationInformationVariableLoans = value;
            }
        }
         */
    }
}