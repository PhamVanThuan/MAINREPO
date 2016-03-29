using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("OfferDeclarationQuestion", Schema = "dbo", Lazy = true)]
    public partial class ApplicationDeclarationQuestion_DAO : DB_2AM<ApplicationDeclarationQuestion_DAO>
    {
        private string _description;

        private bool _displayQuestionDate;

        private int _displaySequence;

        private int _key;

        // private IList<ApplicationDeclaration_DAO> _applicationDeclarations;

        private IList<ApplicationDeclarationQuestionAnswer_DAO> _applicationDeclarationQuestionAnswers;

        private IList<ApplicationDeclarationQuestionAnswerConfiguration_DAO> _applicationDeclarationQuestionAnswerConfigurations;

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

        [Property("DisplayQuestionDate", ColumnType = "Boolean", NotNull = true)]
        [ValidateNonEmpty("Display Question Date is a mandatory field")]
        public virtual bool DisplayQuestionDate
        {
            get
            {
                return this._displayQuestionDate;
            }
            set
            {
                this._displayQuestionDate = value;
            }
        }

        [Property("DisplaySequence", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("Display Sequence is a mandatory field")]
        public virtual int DisplaySequence
        {
            get
            {
                return this._displaySequence;
            }
            set
            {
                this._displaySequence = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Assigned, "OfferDeclarationQuestionKey", ColumnType = "Int32")]
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

        //[HasMany(typeof(ApplicationDeclaration_DAO), ColumnKey = "OfferDeclarationQuestionKey", Table = "OfferDeclaration", Lazy = true)]
        //public virtual IList<ApplicationDeclaration_DAO> ApplicationDeclarations
        //{
        //    get
        //    {
        //        return this._applicationDeclarations;
        //    }
        //    set
        //    {
        //        this._applicationDeclarations = value;
        //    }
        //}

        [HasMany(typeof(ApplicationDeclarationQuestionAnswer_DAO), ColumnKey = "OfferDeclarationQuestionKey", Table = "OfferDeclarationQuestionAnswer", Lazy = true)]
        public virtual IList<ApplicationDeclarationQuestionAnswer_DAO> ApplicationDeclarationQuestionAnswers
        {
            get
            {
                return this._applicationDeclarationQuestionAnswers;
            }
            set
            {
                this._applicationDeclarationQuestionAnswers = value;
            }
        }

        [HasMany(typeof(ApplicationDeclarationQuestionAnswerConfiguration_DAO), ColumnKey = "OfferDeclarationQuestionKey", Table = "OfferDeclarationQuestionAnswerConfiguration", Lazy = true)]
        public virtual IList<ApplicationDeclarationQuestionAnswerConfiguration_DAO> ApplicationDeclarationQuestionAnswerConfigurations
        {
            get
            {
                return this._applicationDeclarationQuestionAnswerConfigurations;
            }
            set
            {
                this._applicationDeclarationQuestionAnswerConfigurations = value;
            }
        }
    }
}