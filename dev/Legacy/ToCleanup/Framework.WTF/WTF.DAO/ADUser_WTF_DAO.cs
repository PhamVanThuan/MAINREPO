
using System;
using System.Collections.Generic;
using System.Collections;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;


namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ADUser", Schema = "dbo")]
    public partial class ADUser_WTF_DAO : DB_Test_WTF<ADUser_WTF_DAO>
    {

        private string _aDUserName;

        private string _password;

        private string _passwordQuestion;

        private string _passwordAnswer;

        private int _key;

        private IList<UserOrganisationStructure_WTF_DAO> _userOrganisationStructures;

        private IList<StageTransition_WTF_DAO> _stageTransitions;

        private IList<StageTransitionComposite_WTF_DAO> _stageTransitionComposites;

        private GeneralStatus_WTF_DAO _generalStatus;

        private LegalEntity_WTF_DAO _legalEntity;

        [Property("ADUserName", ColumnType = "String", NotNull = true)]
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

        [HasMany(typeof(UserOrganisationStructure_WTF_DAO), ColumnKey = "ADUserKey", Table = "UserOrganisationStructure")]
        public virtual IList<UserOrganisationStructure_WTF_DAO> UserOrganisationStructures
        {
            get
            {
                return this._userOrganisationStructures;
            }
            set
            {
                this._userOrganisationStructures = value;
            }
        }

        [HasMany(typeof(StageTransition_WTF_DAO), ColumnKey = "ADUserKey", Table = "StageTransition")]
        public virtual IList<StageTransition_WTF_DAO> StageTransitions
        {
            get
            {
                return this._stageTransitions;
            }
            set
            {
                this._stageTransitions = value;
            }
        }

        [HasMany(typeof(StageTransitionComposite_WTF_DAO), ColumnKey = "ADUserKey", Table = "StageTransitionComposite")]
        public virtual IList<StageTransitionComposite_WTF_DAO> StageTransitionComposites
        {
            get
            {
                return this._stageTransitionComposites;
            }
            set
            {
                this._stageTransitionComposites = value;
            }
        }

        [BelongsTo("GeneralStatusKey", NotNull = true)]
        public virtual GeneralStatus_WTF_DAO GeneralStatus
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

        [BelongsTo("LegalEntityKey")]
        public virtual LegalEntity_WTF_DAO LegalEntity
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
    }

}

