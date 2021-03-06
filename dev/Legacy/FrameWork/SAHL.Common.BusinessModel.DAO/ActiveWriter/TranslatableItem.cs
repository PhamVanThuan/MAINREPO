﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.42
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SAHL.Common.BusinessModel {
    using System;
    using System.Collections.Generic;
    using System.Collections;
    using Castle.ActiveRecord;
    
    
    [ActiveRecord("TranslatableItem", Schema="dbo")]
    public partial class TranslatableItem : ActiveRecordBase<TranslatableItem> {
        
        private string _description;
        
        private int _translatableItemKey;
        
        private IList<TranslatedText> _translatedTexts;
        
        [Property("Description", ColumnType="String", NotNull=true)]
        public virtual string Description {
            get {
                return this._description;
            }
            set {
                this._description = value;
            }
        }
        
        [PrimaryKey(PrimaryKeyType.Native, "TranslatableItemKey", ColumnType="Int32")]
        public virtual int TranslatableItemKey {
            get {
                return this._translatableItemKey;
            }
            set {
                this._translatableItemKey = value;
            }
        }
        
        [HasMany(typeof(TranslatedText), ColumnKey="TranslatableItemKey", Table="TranslatedText")]
        public virtual IList<TranslatedText> TranslatedTexts {
            get {
                return this._translatedTexts;
            }
            set {
                this._translatedTexts = value;
            }
        }
    }
    
    [ActiveRecord("TranslatedText", Schema="dbo")]
    public partial class TranslatedText : ActiveRecordBase<TranslatedText> {
        
        private string _translatedText;
        
        private int _translatedTextKey;
        
        private TranslatableItem _translatableItem;
        
        private Language _language;
        
        [Property("TranslatedText", ColumnType="String")]
        public virtual string TranslatedText {
            get {
                return this._translatedText;
            }
            set {
                this._translatedText = value;
            }
        }
        
        [PrimaryKey(PrimaryKeyType.Native, "TranslatedTextKey", ColumnType="Int32")]
        public virtual int TranslatedTextKey {
            get {
                return this._translatedTextKey;
            }
            set {
                this._translatedTextKey = value;
            }
        }
        
        [BelongsTo("TranslatableItemKey", NotNull=false)]
        public virtual TranslatableItem TranslatableItem {
            get {
                return this._translatableItem;
            }
            set {
                this._translatableItem = value;
            }
        }
        
        [BelongsTo("LanguageKey", NotNull=false)]
        public virtual Language Language {
            get {
                return this._language;
            }
            set {
                this._language = value;
            }
        }
    }
    
    [ActiveRecord("Language", Schema="dbo")]
    public partial class Language : ActiveRecordBase<Language> {
        
        private string _description;
        
        private bool _translatable;
        
        private int _languageKey;
        
        private IList<TranslatedText> _translatedTexts;
        
        [Property("Description", ColumnType="String", NotNull=true)]
        public virtual string Description {
            get {
                return this._description;
            }
            set {
                this._description = value;
            }
        }
        
        [Property("Translatable", ColumnType="Boolean", NotNull=true)]
        public virtual bool Translatable {
            get {
                return this._translatable;
            }
            set {
                this._translatable = value;
            }
        }
        
        [PrimaryKey(PrimaryKeyType.Native, "LanguageKey", ColumnType="Int32")]
        public virtual int LanguageKey {
            get {
                return this._languageKey;
            }
            set {
                this._languageKey = value;
            }
        }
        
        [HasMany(typeof(TranslatedText), ColumnKey="LanguageKey", Table="TranslatedText")]
        public virtual IList<TranslatedText> TranslatedTexts {
            get {
                return this._translatedTexts;
            }
            set {
                this._translatedTexts = value;
            }
        }
    }
}
