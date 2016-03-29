using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using NHibernate.Criterion;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// LegalEntityNaturalPerson_DAO is derived from LegalEntity_DAO and is used to instantiate a Legal Entity of type "Natural Person."
    /// </summary>
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord(DiscriminatorValue = "2", Lazy = true)]
    public class LegalEntityNaturalPerson_DAO : LegalEntity_DAO
    {
        private MaritalStatus_DAO _maritalStatus;

        private Gender_DAO _gender;

        private PopulationGroup_DAO _populationGroup;

        private Salutation_DAO _salutation;

        private CitizenType_DAO _citizenType;

        private string _firstNames;

        private string _surname;

        private string _initials;

        private string _preferredName;

        private string _iDNumber;

        private string _passportNumber;

        private DateTime? _dateOfBirth;

        private Education_DAO _education;

        private Language_DAO _homeLanguage;

        private IList<ITC_DAO> _iTCs;

        /// <summary>
        /// The foreign key reference to the MaritalStatus table. A Natural Person Legal Entity belongs to a single Marital Status type
        /// </summary>
        [Lurker]
        [BelongsTo("MaritalStatusKey")]
        public virtual MaritalStatus_DAO MaritalStatus
        {
            get
            {
                return this._maritalStatus;
            }
            set
            {
                this._maritalStatus = value;
            }
        }

        /// <summary>
        /// The foreign key reference to the Gender table.  A Natural Person Legal Entity belongs to a single Gender type.
        /// </summary>
        [Lurker]
        [BelongsTo("GenderKey")]
        public virtual Gender_DAO Gender
        {
            get
            {
                return this._gender;
            }
            set
            {
                this._gender = value;
            }
        }

        /// <summary>
        /// The foreign key reference to the PopulationGroup table. A Natural Person Legal Entity belongs to a single Population Group type.
        /// </summary>
        [BelongsTo("PopulationGroupKey")]
        public virtual PopulationGroup_DAO PopulationGroup
        {
            get
            {
                return this._populationGroup;
            }
            set
            {
                this._populationGroup = value;
            }
        }

        /// <summary>
        /// The foreign key reference to the Salutation table. A Natural Person Legal Entity belongs to a single Saluation type.
        /// </summary>
        [Lurker]
        [BelongsTo("Salutationkey")]
        public virtual Salutation_DAO Salutation
        {
            get
            {
                return this._salutation;
            }
            set
            {
                this._salutation = value;
            }
        }

        /// <summary>
        /// The First Names of the Natural Person.
        /// </summary>
        [Lurker]
        [Property("FirstNames", ColumnType = "String")]
        public virtual string FirstNames
        {
            get
            {
                return this._firstNames;
            }
            set
            {
                this._firstNames = value;
            }
        }

        /// <summary>
        /// The Initials of the Natural Person.
        /// </summary>
        [Lurker]
        [Property("Initials", ColumnType = "String", Length = 5)]
        public virtual string Initials
        {
            get
            {
                return this._initials;
            }
            set
            {
                this._initials = value;
            }
        }

        /// <summary>
        /// The Preferred Name of the Natural Person.
        /// </summary>
        [Property("PreferredName", ColumnType = "String")]
        public virtual string PreferredName
        {
            get
            {
                return this._preferredName;
            }
            set
            {
                this._preferredName = value;
            }
        }

        [Lurker]
        [Property("IDNumber", ColumnType = "String", Length = 20)]
        public virtual string IDNumber
        {
            get
            {
                return this._iDNumber;
            }
            set
            {
                this._iDNumber = value;
            }
        }

        [Lurker]
        [Property("PassportNumber", ColumnType = "String")]
        public virtual string PassportNumber
        {
            get
            {
                return this._passportNumber;
            }
            set
            {
                this._passportNumber = value;
            }
        }

        /// <summary>
        /// The Date of Birth of the Natural Person.
        /// </summary>
        [Property("DateOfBirth")]
        public virtual DateTime? DateOfBirth
        {
            get
            {
                return this._dateOfBirth;
            }
            set
            {
                this._dateOfBirth = value;
            }
        }

        /// <summary>
        /// The Surname of the Natural Person.
        /// </summary>
        [Lurker]
        [Property("Surname", ColumnType = "String")]
        public virtual string Surname
        {
            get
            {
                return this._surname;
            }
            set
            {
                this._surname = value;
            }
        }

        /// <summary>
        /// The foreign key reference to the CitizenType table. A Natural Person Legal Entity belongs to a single Citizen Type.
        /// </summary>
        [Lurker]
        [BelongsTo("CitizenTypeKey")]
        public virtual CitizenType_DAO CitizenType
        {
            get
            {
                return this._citizenType;
            }
            set
            {
                this._citizenType = value;
            }
        }

        /// <summary>
        /// Gets a list of all legal entities with an ID number starting with the supplied <c>prefix</c>.
        /// </summary>
        public static IList<LegalEntityNaturalPerson_DAO> FindByIDNumber(string prefix, int maxRowCount)
        {
            SimpleQuery q = new SimpleQuery(typeof(LegalEntityNaturalPerson_DAO), @"
                        from LegalEntityNaturalPerson_DAO le
                        where le.IDNumber LIKE ?
                        ",
                prefix + "%"
            );
            q.SetQueryRange(maxRowCount);

            object result = ExecuteQuery(q);

            return new List<LegalEntityNaturalPerson_DAO>((LegalEntityNaturalPerson_DAO[])result);
        }

        /// <summary>
        /// Gets a list of all legal entities with a Passport number starting with the supplied <c>prefix</c>.
        /// </summary>
        public static IList<LegalEntityNaturalPerson_DAO> FindByPassportNumber(string prefix, int maxRowCount)
        {
            SimpleQuery q = new SimpleQuery(typeof(LegalEntityNaturalPerson_DAO), @"
                        from LegalEntityNaturalPerson_DAO le
                        where le.PassportNumber LIKE ?
                        ",
                prefix + "%"
            );
            q.SetQueryRange(maxRowCount);
            return new List<LegalEntityNaturalPerson_DAO>((LegalEntityNaturalPerson_DAO[])ExecuteQuery(q));
        }

        /// <summary>
        /// The highest education level that the Legal Entity has achieved.
        /// </summary>
        [BelongsTo("EducationKey")]
        public virtual Education_DAO Education
        {
            get
            {
                return this._education;
            }
            set
            {
                this._education = value;
            }
        }

        /// <summary>
        /// The Legal Entity's Home Language.
        /// </summary>
        [BelongsTo("HomeLanguageKey")]
        public virtual Language_DAO HomeLanguage
        {
            get
            {
                return this._homeLanguage;
            }
            set
            {
                this._homeLanguage = value;
            }
        }

        [HasMany(typeof(ITC_DAO), ColumnKey = "LegalEntityKey", Table = "ITC", Lazy = true, Cascade = ManyRelationCascadeEnum.None)]
        public virtual IList<ITC_DAO> ITCs
        {
            get
            {
                return this._iTCs;
            }
            set
            {
                this._iTCs = value;
            }
        }

        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static LegalEntityNaturalPerson_DAO Find(int id)
        {
            return ActiveRecordBase<LegalEntityNaturalPerson_DAO>.Find(id).As<LegalEntityNaturalPerson_DAO>();
        }

        public new static LegalEntityNaturalPerson_DAO Find(object id)
        {
            return ActiveRecordBase<LegalEntityNaturalPerson_DAO>.Find(id).As<LegalEntityNaturalPerson_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static LegalEntityNaturalPerson_DAO FindFirst()
        {
            return ActiveRecordBase<LegalEntityNaturalPerson_DAO>.FindFirst().As<LegalEntityNaturalPerson_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static LegalEntityNaturalPerson_DAO FindFirst(params ICriterion[] criteria)
        {
            return ActiveRecordBase<LegalEntityNaturalPerson_DAO>.FindFirst(criteria).As<LegalEntityNaturalPerson_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static LegalEntityNaturalPerson_DAO FindOne()
        {
            return ActiveRecordBase<LegalEntityNaturalPerson_DAO>.FindOne().As<LegalEntityNaturalPerson_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static LegalEntityNaturalPerson_DAO[] FindAllByProperty(string property, object value)
        {
            return ActiveRecordBase<LegalEntityNaturalPerson_DAO>.FindAllByProperty(property, value);
        }

        #endregion Static Overrides
    }
}