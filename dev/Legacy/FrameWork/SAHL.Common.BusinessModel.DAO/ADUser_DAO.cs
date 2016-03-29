using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType = TestType.Find)]
    [ActiveRecord("ADUser", Schema = "dbo", Lazy = true)]
    public partial class ADUser_DAO : DB_2AM<ADUser_DAO>
    {
        private string _aDUserName;

        private GeneralStatus_DAO _generalStatus;

        private string _password;

        private string _passwordQuestion;

        private string _passwordAnswer;

        private int _key;

        private IList<UserProfileSetting_DAO> _userProfileSettings;
        private IList<UserOrganisationStructure_DAO> _UserOrganisationStructure;
        private LegalEntityNaturalPerson_DAO _legalEntity;

        [BelongsTo("LegalEntityKey", NotNull = false)]
        public virtual LegalEntityNaturalPerson_DAO LegalEntity
        {
            get
            {
                return this._legalEntity;
            }
            set
            {
                this._legalEntity = value;
            }
        }

        [Property("ADUserName", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("AD User Name is a mandatory field")]
        public virtual string ADUserName
        {
            get
            {
                return this._aDUserName;
            }
            set
            {
                this._aDUserName = value;
            }
        }

        [BelongsTo(Column = "GeneralStatusKey", NotNull = true)]
        [ValidateNonEmpty("General Status is a mandatory field")]
        public virtual GeneralStatus_DAO GeneralStatusKey
        {
            get
            {
                return this._generalStatus;
            }
            set
            {
                this._generalStatus = value;
            }
        }

        [Property("Password", ColumnType = "String")]
        public virtual string Password
        {
            get
            {
                return this._password;
            }
            set
            {
                this._password = value;
            }
        }

        [Property("PasswordQuestion", ColumnType = "String")]
        public virtual string PasswordQuestion
        {
            get
            {
                return this._passwordQuestion;
            }
            set
            {
                this._passwordQuestion = value;
            }
        }

        [Property("PasswordAnswer", ColumnType = "String")]
        public virtual string PasswordAnswer
        {
            get
            {
                return this._passwordAnswer;
            }
            set
            {
                this._passwordAnswer = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "ADUserKey", ColumnType = "Int32")]
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

        [HasMany(typeof(UserProfileSetting_DAO), ColumnKey = "ADUserKey", Table = "UserProfileSetting", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true, Lazy = true)]
        public virtual IList<UserProfileSetting_DAO> UserProfileSettings
        {
            get
            {
                return _userProfileSettings;
            }
            set
            {
                _userProfileSettings = value;
            }
        }

        [HasMany(typeof(UserOrganisationStructure_DAO), ColumnKey = "ADUserKey", Table = "UserOrganisationStructure", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true, Lazy = true)]
        public virtual IList<UserOrganisationStructure_DAO> UserOrganisationStructure
        {
            get
            {
                return _UserOrganisationStructure;
            }
            set
            {
                _UserOrganisationStructure = value;
            }
        }
    }
}